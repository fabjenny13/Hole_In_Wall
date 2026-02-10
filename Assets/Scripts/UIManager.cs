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
        Menu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }
}
