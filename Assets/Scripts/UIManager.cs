using TMPro;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Menu;
    public TextMeshProUGUI ScoreText;
    WallSpawner wallSpawner;

    int score;


    private void Start()
    {
        Menu.SetActive(true);
        score = 0;
        wallSpawner = GetComponent<WallSpawner>();
    }
    public void StartGame()
    {
        score = 0;
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
        score -= 5;
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
