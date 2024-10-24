using System.Collections.Generic;

public static class Data
{
    public enum AnimationState
    {
        Idle,
        RegularAttack,
        AttackFireball,
        AttackLightning,
        Defending,
        DrinkingPotion,
        GettingHit,
        Dying
    }

    public static Dictionary<AnimationState, string> animationStates => states;

    static Dictionary<AnimationState, string> states = new()
    {
        { AnimationState.Idle, "idle" },
        { AnimationState.RegularAttack, "attack" },
        { AnimationState.AttackFireball, "fireball" },
        { AnimationState.AttackLightning, "light" },
        { AnimationState.Defending, "defend" },
        { AnimationState.DrinkingPotion, "drink potion" },
        { AnimationState.GettingHit, "get hit" },
        { AnimationState.Dying, "dead" }
    };
}
