using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    void Start()
    {
        ResetScore();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + score;
    }
}
