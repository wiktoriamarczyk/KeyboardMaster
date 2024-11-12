using UnityEngine;
using static Data;

public class Lightning : Weapon
{
    protected override float baseDamage => lightningDmg;
    protected override float speed => 20f;
    protected override float lifeTime => 5f;
    protected override float cooldown => 15f;

    public override void Init(GameObject source, GameObject target)
    {
        base.Init(source, target);
    }

}
