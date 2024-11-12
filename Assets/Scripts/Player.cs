using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using static Data;
using static Timers;

public class Player : Creature
{
    [Serializable]
    class AttackTypes
    {
        public BasicWeapon basicWeapon;
        public Fireball fireball;
        public Lightning lightning;
    }

    [SerializeField] TextFollower   textFollower;
    [SerializeField] Timers         timer;
    [SerializeField] AudioManager   audioManager;
    [SerializeField] AttackTypes    attackTypes;
    [SerializeField] LookAtTarget   lookAtTarget;
    [SerializeField] GameObject     weaponHolder;

    [SerializeField] float immunityDuration = 5f;
    [SerializeField, ReadOnly] float remainingImmunityTime = 0f;
    [SerializeField] float drinkingCooldown = 30f;

    GameObject spawnedWeapon;

    bool isImmune = false;

    const float onTextMissedPenalty = 2;
    const float potionHealingAmount = 50f;

    override protected void Start()
    {
        maxHealth = playerHealth;
        base.Start();
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
        InitializeCommands();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isImmune)
        {
            return;
        }

        var weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null && weapon.Source != gameObject)
        {
            GetHit();
            UpdateHealth(-weapon.Damage);
            audioManager.PlayPlayerHitSound();

            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
            (byte)(Color.green.r * 255f), (byte)(Color.green.g * 255f), (byte)(Color.green.b * 255f),
            $"DAMAGE to PLAYER: {weapon.Damage} - {weapon.gameObject.name}"));
        }
    }

    void OnDestroy()
    {
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.RegularAttack]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackFireball]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackLightning]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.Defending]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.DrinkingPotion]);

        StopAllCoroutines();
    }

    #region TriggerAnimations
    public void BasicAttack()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.RegularAttack]);
        InitWeapon(attackTypes.basicWeapon);
        audioManager.PlayBasicAttackSound();
    }

    public void AttackFireball()
    {
        if (timer.IsReady(TimerType.Fireball))
        {
            InitWeapon(attackTypes.fireball);
            animator.SetTrigger(animationStates[Data.AnimationState.AttackFireball]);
            audioManager.PlayFireballAttackSound();
            timer.StartTimer(attackTypes.fireball.Cooldown, TimerType.Fireball);
        }
    }

    public void AttackLightning()
    {
        if (timer.IsReady(TimerType.Light))
        {
            InitWeapon(attackTypes.lightning);
            animator.SetTrigger(animationStates[Data.AnimationState.AttackLightning]);
            audioManager.PlayLightningAttackSound();
            timer.StartTimer(attackTypes.lightning.Cooldown, TimerType.Light);
        }
    }

    public void Defend()
    {
        if (timer.IsReady(TimerType.Defend))
        {
            animator.SetTrigger(animationStates[Data.AnimationState.Defending]);
            ActivateImmunity();
            audioManager.PlayPlayerDefenseSound();
            timer.StartTimer(immunityDuration, TimerType.Defend);
        }
    }

    public void DrinkPotion()
    {
        if (timer.IsReady(TimerType.Drink))
        {
            animator.SetTrigger(animationStates[Data.AnimationState.DrinkingPotion]);
            audioManager.PlayPlayerDrinkSound();
            Heal(potionHealingAmount);
            timer.StartTimer(drinkingCooldown, TimerType.Drink);
        }
    }
    #endregion

    void InitWeapon(Weapon weapon)
    {
        spawnedWeapon = Instantiate(weapon.gameObject, weaponHolder.transform.position, Quaternion.identity);
        spawnedWeapon.transform.parent = transform.parent;
        spawnedWeapon.name = weapon.name;
        spawnedWeapon.GetComponent<Weapon>().Init(gameObject, lookAtTarget.Target);
    }

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
        if (score > 0 || isImmune)
        {
            return;
        }

        GetHit();
        UpdateHealth(-onTextMissedPenalty * Mathf.Abs(score));
    }

    void Heal(float amount)
    {
        amount = Mathf.Clamp(amount, 0, maxHealth);
        UpdateHealth(amount);
    }

    void InitializeCommands()
    {
        CommandSet.AddCommand(animationStates[Data.AnimationState.RegularAttack], new() { BasicAttack });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackFireball], new() { AttackFireball });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackLightning], new() { AttackLightning });
        CommandSet.AddCommand(animationStates[Data.AnimationState.Defending], new() { Defend });
        CommandSet.AddCommand(animationStates[Data.AnimationState.DrinkingPotion], new() { DrinkPotion });
    }

    protected override void Die()
    {
        base.Die();
        SceneController.Instance.SetGameResult(false);
        StartCoroutine(LoadEndSceneAfterDelay(lossSceneName));
    }
}
