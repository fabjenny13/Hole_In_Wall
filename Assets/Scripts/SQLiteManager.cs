/*using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;

public class SQLiteManager : MonoBehaviour
{
    public static SQLiteManager Instance;
    GameManager gameManager;

    // ================= SQLITE LOW LEVEL =================

    public class SQLiteDB : IDisposable
    {
        private IntPtr _db = IntPtr.Zero;
        private const string DLL = "sqlite3";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_open(string filename, out IntPtr db);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_close(IntPtr db);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_exec(
            IntPtr db, string sql, IntPtr cb, IntPtr arg, out IntPtr err);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_prepare_v2(
            IntPtr db, string sql, int nByte, out IntPtr stmt, IntPtr tail);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_step(IntPtr stmt);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern int sqlite3_finalize(IntPtr stmt);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern long sqlite3_column_int64(IntPtr stmt, int col);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr sqlite3_errmsg(IntPtr db);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
        private static extern void sqlite3_free(IntPtr ptr);

        public SQLiteDB(string path)
        {
            if (sqlite3_open(path, out _db) != 0)
                throw new Exception("sqlite3_open failed");
        }

        public void Exec(string sql)
        {
            IntPtr err;
            if (sqlite3_exec(_db, sql, IntPtr.Zero, IntPtr.Zero, out err) != 0)
            {
                string msg = err != IntPtr.Zero ? Marshal.PtrToStringAnsi(err) : "unknown";
                if (err != IntPtr.Zero) sqlite3_free(err);
                throw new Exception(msg);
            }
        }

        public long ExecScalar(string sql)
        {
            IntPtr stmt;
            if (sqlite3_prepare_v2(_db, sql, -1, out stmt, IntPtr.Zero) != 0)
                throw new Exception(Marshal.PtrToStringAnsi(sqlite3_errmsg(_db)));

            long value = 0;
            if (sqlite3_step(stmt) == 100)
                value = sqlite3_column_int64(stmt, 0);

            sqlite3_finalize(stmt);
            return value;
        }

        public void Dispose()
        {
            if (_db != IntPtr.Zero)
            {
                sqlite3_close(_db);
                _db = IntPtr.Zero;
            }
        }
    }

    // ================= STATE =================

    private SQLiteDB mainDB;
    private SQLiteDB streamDB;

    private long currentSessionNo = -1;
    private bool dbAvailable = false;
    private bool sessionActive = false;

    [Header("Stream Logging")]
    public Transform cursorTransform;
    public float streamIntervalSeconds = 1f;

    private Coroutine streamCoroutine;
    private readonly object dbLock = new object();

    // ================= UNITY =================

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        gameManager = FindFirstObjectByType<GameManager>();

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("SQLiteManager: Disabled (non-Android)");
            return;
        }

        try
        {
            string mainPath = Path.Combine(Application.persistentDataPath, "main.db");
            string streamPath = Path.Combine(Application.persistentDataPath, "stream.db");

            mainDB = new SQLiteDB(mainPath);
            streamDB = new SQLiteDB(streamPath);

            // ================= MAIN SESSION TABLE =================
            // CHANGED: simplified main table
            mainDB.Exec(@"
                CREATE TABLE IF NOT EXISTS sessions (
                    session_no INTEGER PRIMARY KEY AUTOINCREMENT,
                    session_start_time TEXT NOT NULL,
                    session_end_time TEXT,
                    score INTEGER
                );
            ");

            dbAvailable = true;
            Debug.Log("SQLiteManager: DB initialized");
        }
        catch (Exception e)
        {
            Debug.LogError("SQLiteManager: DB init failed ? " + e.Message);
            dbAvailable = false;
        }
    }

    // ================= SESSION =================

    public void BeginSession()
    {
        if (!dbAvailable) return;

        lock (dbLock)
        {
            if (sessionActive)
                EndSession();

            mainDB.Exec(
                "INSERT INTO sessions (session_start_time) VALUES (datetime('now'));"
            );

            currentSessionNo = mainDB.ExecScalar("SELECT last_insert_rowid();");

            // ================= STREAM TABLE PER SESSION =================
            streamDB.Exec($@"
                CREATE TABLE IF NOT EXISTS {currentSessionNo} (
                    x FLOAT NOT NULL,
                    y FLOAT NOT NULL
                );
            ");

            sessionActive = true;
            StartStreamLogging();

            Debug.Log($"Session {currentSessionNo} started");
        }
    }

    public void EndSession()
    {
        if (!dbAvailable || !sessionActive) return;

        lock (dbLock)
        {
            mainDB.Exec(
                $"UPDATE sessions " +
                $"SET session_end_time = datetime('now'), score = {gameManager.GetCurrentScore()} " +
                $"WHERE session_no = {currentSessionNo};"
            );

            StopStreamLogging();
            sessionActive = false;

            Debug.Log($"Session {currentSessionNo} ended");
        }
    }

    // ================= STREAM =================

    private void InsertStream(float x, float y)
    {
        if (!dbAvailable || !sessionActive) return;

        streamDB.Exec(
            $"INSERT INTO {currentSessionNo} (x, y) " +
            $"VALUES ({x:F3}, {y:F3});"
        );
    }

    private void StartStreamLogging()
    {
        StopStreamLogging();
        streamCoroutine = StartCoroutine(StreamRoutine());
    }

    private void StopStreamLogging()
    {
        if (streamCoroutine != null)
        {
            StopCoroutine(streamCoroutine);
            streamCoroutine = null;
        }
    }

    private IEnumerator StreamRoutine()
    {
        Camera cam = Camera.main;

        while (true)
        {
            if (cam != null)
            {
                Vector3 mouse = Input.mousePosition;
                mouse.z = -cam.transform.position.z; // depth for 2D
                Vector3 world = cam.ScreenToWorldPoint(mouse);

                InsertStream(world.x, world.y);
            }

            yield return new WaitForSecondsRealtime(streamIntervalSeconds);
        }
    }

    // ================= CLEANUP =================

    void OnApplicationQuit()
    {
        EndSession();
    }

    void OnDestroy()
    {
        EndSession();
        mainDB?.Dispose();
        streamDB?.Dispose();
    }
}
*/