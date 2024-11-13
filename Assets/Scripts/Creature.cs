using System.Collections;
using UnityEngine;
using static Data;

public class Creature : MonoBehaviour
{
    [SerializeField] protected HealthBar    healthBar;
    [SerializeField] protected ScoreIndicator scoreIndicator;
    [SerializeField] protected Animator     animator;

    [SerializeField] protected float currentHealth;

    protected float maxHealth;

    private const float delayBeforeSceneLoad = 5f;

    virtual protected void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    virtual public void UpdateHealth(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth;
        healthBar.SetHealth(healthPercentage);
        scoreIndicator.UpdateScoreIndicator(value);

        if (currentHealth <= 0)
        {
            animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
            Die();
        }
    }

    virtual protected void Die()
    {
        animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
        animator.SetTrigger(animationStates[Data.AnimationState.Dying]);
    }

    virtual protected void GetHit()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.GettingHit]);
    }

    protected IEnumerator LoadEndSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneController.Instance.LoadScene(sceneName);
    }
}
