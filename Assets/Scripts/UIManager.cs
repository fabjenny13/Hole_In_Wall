using TMPro;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject ReplayMenu;
    public TextMeshProUGUI ScoreText;
    WallSpawner wallSpawner;

    int score;


    private void Start()
    {
        Menu.SetActive(true);
        ReplayMenu.SetActive(false);
        score = 0;
        wallSpawner = GetComponent<WallSpawner>();
    }
    public void StartGame()
    {
        score = 0;
        Menu.SetActive(false);
        ReplayMenu.SetActive(false);
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


        ReplayMenu.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        ReplayMenu.SetActive(false);
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

        score = 0;

        wallSpawner.StartGame();
    }

    public void IncreaseScore()
    {
        score += 10;
        ScoreText.text = score.ToString();
    }

    public void DecreaseScore()
    {
        if (score >= 5)
            score -= 5;
        else
            score = 0;
        ScoreText.text = score.ToString();
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }
}
