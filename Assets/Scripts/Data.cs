using System.Collections.Generic;

public static class Data
{
    public enum PlayerState
    {
        Idle,
        AttackFireball,
        AttackLightning,
        Defending,
        DrinkingPotion,
        GettingHit,
        Dying
    }

    public static Dictionary<PlayerState, string> playerStates => states;

    static Dictionary<PlayerState, string> states = new()
    {
        { PlayerState.Idle, "idle" },
        { PlayerState.AttackFireball, "attack fireball" },
        { PlayerState.AttackLightning, "attack lightning" },
        { PlayerState.Defending, "defend" },
        { PlayerState.DrinkingPotion, "drink potion" },
        { PlayerState.GettingHit, "get hit" },
        { PlayerState.Dying, "dead" }
    };
}
