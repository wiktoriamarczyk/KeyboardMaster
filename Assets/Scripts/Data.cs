using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public const string lossSceneName = "EndScene";
    public const string winSceneName = "WinTextScene";
    public const string lightningEffectTag = "lightning";

    public const float playerHealth = 100f;
    public const float xboxBossHealth = 400f;

    public const float xboxWeaponDmg = 5f;
    public const float basicWeaponDmg = 1f;
    public const float fireballDmg = 5f;
    public const float lightningDmg = 10f;

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

   public static GameObject FindInactiveObjectWithTag(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
            {
                return obj;
            }
        }

        return null;
    }
}
