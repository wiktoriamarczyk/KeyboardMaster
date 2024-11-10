using System.Collections;
using Unity.Collections;
using UnityEngine;
using static Data;

public class Player : Creature
{
    [SerializeField] TextFollower textFollower;
    [SerializeField] Combo comboSystem;
    [SerializeField] private Timers timer;

    const float onTextMissedPenalty = 2;
    const float potionHealingAmount = 50f;

    [SerializeField] private float immunityDuration = 5f; 
    [SerializeField, ReadOnly] private float remainingImmunityTime = 0f;
    private bool isImmune = false;

    [SerializeField] private float fireballCooldown = 10f;  
    [SerializeField] private float lightningCooldown = 15f;
    [SerializeField] private float drinkingCooldown = 30f;

    override protected void Start()
    {
        base.Start();
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
        InitializeCommands();
    }

    // TODO: Implement all aniamtions based on CommandSystem / TextFollower

    #region TriggerAnimations
    public void AttackFireball()
    {
        if (timer.IsReady(0))
        {
            comboSystem.IncrementCombo();
            animator.SetTrigger(animationStates[Data.AnimationState.AttackFireball]);
            timer.StartTimer(fireballCooldown, 0);
        }
    }

    public void AttackLightning()
    {
        if (timer.IsReady(1))
        {
            comboSystem.IncrementCombo();
            comboSystem.IncrementCombo();
            animator.SetTrigger(animationStates[Data.AnimationState.AttackLightning]);
            timer.StartTimer(lightningCooldown, 1);
        }
    }

    public void Defend()
    {
        if (timer.IsReady(3))
        {
            animator.SetTrigger(animationStates[Data.AnimationState.Defending]);
            ActivateImmunity();
            timer.StartTimer(immunityDuration, 3);
        }

    }

    public void DrinkPotion()
    {
        if (timer.IsReady(2)) { 
            animator.SetTrigger(animationStates[Data.AnimationState.DrinkingPotion]);
            Heal(potionHealingAmount);
            timer.StartTimer(drinkingCooldown, 2);
        }
    }
    #endregion

    void ActivateImmunity()
    {
        if (!isImmune)
        {
            isImmune = true;
            remainingImmunityTime = immunityDuration;
            StartCoroutine(ImmunityCooldown());
        }
    }

    private IEnumerator ImmunityCooldown()
    {
        while (remainingImmunityTime > 0)
        {
            remainingImmunityTime -= Time.deltaTime;
            yield return null;
        }

        remainingImmunityTime = 0;
        isImmune = false;
    }

    void OnTextFollowerScoreChanged(int score)
    {
        if (score > 0)
        {
            return;
        }

        GetHit();
        UpdateHealth(onTextMissedPenalty * Mathf.Abs(score));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isImmune) return;

        var weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null)
        {
            GetHit();
            comboSystem.ResetCombo();
            UpdateHealth(weapon.Damage);
        }
    }

    void Heal(float amount)
    {
        // Przywróæ ¿ycie graczowi
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Upewnij siê, ¿e ¿ycie nie przekroczy maksymalnej wartoœci
        healthBar.SetHealth(currentHealth / maxHealth); // Zaktualizuj pasek zdrowia
    }

    void InitializeCommands()
    {
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackFireball], new() { AttackFireball });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackLightning], new() { AttackLightning });
        CommandSet.AddCommand(animationStates[Data.AnimationState.Defending], new() { Defend });
        CommandSet.AddCommand(animationStates[Data.AnimationState.DrinkingPotion], new() { DrinkPotion });
    }

    void OnDestroy()
    {
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackFireball]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackLightning]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.Defending]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.DrinkingPotion]);
    }
}
