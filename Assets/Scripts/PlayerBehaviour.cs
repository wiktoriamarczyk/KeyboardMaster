using UnityEngine;
using static Data;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextFollower textFollower;

    void Start()
    {
        textFollower.onScoreChanged += OnTextFollowerScoreChanged;
        InitializeCommands();
    }

    // TODO: Implement all aniamtions based on CommandSystem / TextFollower
    public void AttackFireball()
    {
        animator.SetTrigger(playerStates[PlayerState.AttackFireball]);
    }

    public void AttackLightning()
    {
        animator.SetTrigger(playerStates[PlayerState.AttackLightning]);
    }

    public void Defend()
    {
        animator.SetTrigger(playerStates[PlayerState.Defending]);
    }

    public void DrinkPotion()
    {
        animator.SetTrigger(playerStates[PlayerState.DrinkingPotion]);
    }

    public void GetHit()
    {
        animator.SetTrigger(playerStates[PlayerState.GettingHit]);
    }

    public void Die()
    {
        animator.SetTrigger(playerStates[PlayerState.Dying]);
    }

    void OnTextFollowerScoreChanged(int score)
    {
        if (score > 0)
        {
            return;
        }

        GetHit();
    }

    void InitializeCommands()
    {
        CommandSet.AddCommand(playerStates[PlayerState.AttackFireball], AttackFireball);
        CommandSet.AddCommand(playerStates[PlayerState.AttackLightning], AttackLightning);
        CommandSet.AddCommand(playerStates[PlayerState.Defending], Defend);
        CommandSet.AddCommand(playerStates[PlayerState.DrinkingPotion], DrinkPotion);
    }
}
