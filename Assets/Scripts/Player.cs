using UnityEngine;
using static Data;

public class Player : Creature
{
    [SerializeField] TextFollower textFollower;

    const float onTextMissedPenalty = 2;

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
        animator.SetTrigger(animationStates[Data.AnimationState.AttackFireball]);
    }

    public void AttackLightning()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.AttackLightning]);
    }

    public void Defend()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.Defending]);
    }

    public void DrinkPotion()
    {
        animator.SetTrigger(animationStates[Data.AnimationState.DrinkingPotion]);
    }
    #endregion

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
        var weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null)
        {
            GetHit();
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
