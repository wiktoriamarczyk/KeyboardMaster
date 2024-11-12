using UnityEngine;
using static Data;

public class Lightning : Weapon
{
    protected override void Start()
    {
        baseDamage = lightningDmg;
        speed = 20f;
        lifeTime = 5f;
        cooldown = 15f;
        base.Start();
    }

    public override void Init(GameObject from, GameObject target)
    {

    }

}
