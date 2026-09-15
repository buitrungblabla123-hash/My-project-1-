using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;

    private int score = 0;

    void Start()
    {
        RectTransform rect = scoreText.rectTransform;
        
        rect.sizeDelta = new Vector2(400, 80);

        scoreText.fontSize = 40;
        scoreText.text = "Score: 0";
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}
