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

    [SerializeField] private float immunityDuration = 5f; 
    [SerializeField, ReadOnly] private float remainingImmunityTime = 0f;
    [SerializeField, ReadOnly] private float remainingFireballTime = 0f;
    [SerializeField, ReadOnly] private float remainingLightningTime = 0f;
    private bool isImmune = false;

    [SerializeField] private float fireballCooldown = 10f;  
    [SerializeField] private float lightningCooldown = 15f;

    private bool canFire = true;
    private bool canLight = true;

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
        animator.SetTrigger(animationStates[Data.AnimationState.Defending]);
        ActivateImmunity();
    }

    public void DrinkPotion()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.DrinkingPotion]);
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
