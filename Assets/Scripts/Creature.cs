using System.Collections;
using UnityEngine;
using static Data;

public class Creature : MonoBehaviour
{
    [SerializeField] protected HealthBar    healthBar;
    [SerializeField] protected Animator     animator;

    [SerializeField] protected float currentHealth;

    protected float maxHealth;

    private const float delayBeforeSceneLoad = 5f;

    virtual protected void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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

    virtual protected void UpdateHealth(float value)
    {
        currentHealth -= value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float healthPercentage = currentHealth / maxHealth;
        healthBar.SetHealth(healthPercentage);

        if (currentHealth <= 0)
        {
            animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
            Die();
        }
    }

    protected IEnumerator LoadEndSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneController.Instance.LoadScene(sceneName);
    }
}
