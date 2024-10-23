using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] HealthBar      healthBar;
    [SerializeField] TextFollower   textFollower;
    [SerializeField] float          onTextMissedPenalty = 10;

    float maxHealth = 100;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
    }

    void OnTextFollowerScoreChanged(int score)
    {
        if (score > 0)
        {
            return;
        }

        float healthPenalty = onTextMissedPenalty * Mathf.Abs(score);
        currentHealth -= healthPenalty;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth;
        healthBar.SetHealth(healthPercentage);
    }
}
