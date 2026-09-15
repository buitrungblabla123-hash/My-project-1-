using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Game Over")]
    public GameObject gameOverText;

    private bool gameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        Time.timeScale = 1f;
        
        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOver)
            return;

        gameOver = true;

        Debug.Log("GAME OVER!");
        
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        
        Time.timeScale = 0f;
    }
}

