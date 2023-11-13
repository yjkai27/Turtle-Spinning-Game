using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyProjectGameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton; 
    public ScoreManager scoreManager;
    public HealthManager healthManager;

    void Start()
    {
        ResetGame();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        EnableRestartButton(true); 
    }

    public void RestartGame()
    {
        Debug.Log("Restart button clicked.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void ResetGame()
    {
        if (scoreManager != null)
            scoreManager.ResetScore();
        if (healthManager != null)
            healthManager.ResetHealth();

        gameOverPanel.SetActive(false);
        EnableRestartButton(false); 
    }

    private void EnableRestartButton(bool enable)
    {
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(enable);
        }
    }
}
