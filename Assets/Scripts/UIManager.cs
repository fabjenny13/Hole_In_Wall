using TMPro;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public GameObject ReplayMenu;
    public TextMeshProUGUI[] ScoreText;
    public TextMeshProUGUI[] HighScoreText;
    WallSpawner wallSpawner;

    int score;
    int highScore;


    private void Start()
    {
        Menu.SetActive(true);
        ReplayMenu.SetActive(false);
        score = 0;
        highScore = 0;
        wallSpawner = GetComponent<WallSpawner>();
    }

    private void SetScoreText(int score)
    {
        for (int i = 0; i < ScoreText.Length; i++)
        {
            ScoreText[i].text = score.ToString();
        }
    }


    public void StartGame()
    {
        score = 0;
        SetScoreText(score);
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

        SetScoreText(score);


        if (score > highScore)
        {
            highScore = score;

            for (int i = 0; i < HighScoreText.Length; i++)
            {
                HighScoreText[i].text = highScore.ToString();
            }
        }
    }

    public void DecreaseScore()
    {
        if (score >= 5)
            score -= 5;
        else
            score = 0;

        SetScoreText(score);

    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }
}
