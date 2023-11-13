using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Text healthText;
    private int maxHealth = 15;
    private int currentHealth;

    void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            FindObjectOfType<MyProjectGameManager>().GameOver();
        }
    }

    void UpdateHealthDisplay()
    {
        healthText.text = "HP: " + currentHealth + "/" + maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }
}
