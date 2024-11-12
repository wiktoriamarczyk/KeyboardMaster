using System.Collections.Generic;

public static class Data
{
    public const string lossSceneName = "EndScene";
    public const string winSceneName = "WinTextScene";

    public const float playerHealth = 100f;
    public const float xboxBossHealth = 800f;

    public const float xboxWeaponDmg = 10f;
    public const float basicWeaponDmg = 1f;
    public const float fireballDmg = 3f;
    public const float lightningDmg = 5f;

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
