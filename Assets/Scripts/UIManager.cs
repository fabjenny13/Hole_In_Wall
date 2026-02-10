using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;

    WallSpawner wallSpawner;

    private void Start()
    {
        Menu.SetActive(true);
        wallSpawner = GetComponent<WallSpawner>();
    }
    public void StartGame()
    {
        Menu.SetActive(false);
        wallSpawner.StartGame();
    }
    
    public void StopGame()
    {

        wallSpawner.StopGame();

        MoveTowardsPlayer[] walls = FindObjectsByType<MoveTowardsPlayer>(
    FindObjectsSortMode.None
);

        foreach (var wall in walls)
        {
            Destroy(wall.gameObject);
        }


        Menu.SetActive(true);
    }

    public void ResetGame()
    {

        wallSpawner.StopGame();

        MoveTowardsPlayer[] walls = FindObjectsByType<MoveTowardsPlayer>(
    FindObjectsSortMode.None
);

        foreach (var wall in walls)
        {
            Destroy(wall.gameObject);
        }


        wallSpawner.StartGame();
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }
}
