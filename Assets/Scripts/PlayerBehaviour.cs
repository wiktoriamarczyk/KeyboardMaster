using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextFollower textFollower;

    enum PlayerState
    {
        Idle,
        AttackFireball,
        AttackLightning,
        Defending,
        DrinkingPotion,
        GettingHit,
        Dying
    }

    Dictionary<PlayerState, string> states = new()
    {
        { PlayerState.Idle, "Idle" },
        { PlayerState.AttackFireball, "AttackFireball" },
        { PlayerState.AttackLightning, "AttackLightning" },
        { PlayerState.Defending, "Defend" },
        { PlayerState.DrinkingPotion, "DrinkPotion" },
        { PlayerState.GettingHit, "GetHit" },
        { PlayerState.Dying, "Dead" }
    };

    void Start()
    {
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
    }

    void OnTextFollowerScoreChanged(int score)
    {
        if (score > 0)
        {
            return;
        }

        GetHit();
    }

    // TODO: Implement all aniamtions based on CommandSystem / TextFollower
    public void AttackFireball()
    {
        animator.SetTrigger(states[PlayerState.AttackFireball]);
    }

    public void AttackLightning()
    {
        animator.SetTrigger(states[PlayerState.AttackLightning]);
    }

    public void Defend()
    {
        animator.SetTrigger(states[PlayerState.Defending]);
    }

    public void DrinkPotion()
    {
        animator.SetTrigger(states[PlayerState.DrinkingPotion]);
    }

    public void GetHit()
    {
        animator.SetTrigger(states[PlayerState.GettingHit]);
    }

    public void Die()
    {
        animator.SetTrigger(states[PlayerState.Dying]);
    }
}
