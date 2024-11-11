using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if(this is Player)
        {
            maxHealth = 100;
        } else if (this is XBOXBoss)
        {
            maxHealth = 300;
        }
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    virtual protected void Die()
    {
        animator.ResetTrigger(animationStates[Data.AnimationState.GettingHit]);
        animator.SetTrigger(animationStates[Data.AnimationState.Dying]);

        if (this is Player)
        {
            SceneController.Instance.SetGameResult(false); // Gracz przegrywa
            StartCoroutine(LoadEndSceneAfterDelay("EndScene"));
        }
        else if (this is XBOXBoss)
        {
            SceneController.Instance.SetGameResult(true); // Gracz wygrywa
            StartCoroutine(LoadEndSceneAfterDelay("WinTextScene"));
        }
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

    private IEnumerator LoadEndSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(delayBeforeSceneLoad);
        SceneController.Instance.LoadScene(sceneName);
    }
}
