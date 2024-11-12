using UnityEngine;
using static Data;

public class Fireball : Weapon
{
    protected override void Start()
    {
        baseDamage = fireballDmg;
        speed = 8f;
        lifeTime = 5f;
        cooldown = 10f;
        base.Start();
    }

    public override void Init(GameObject from, GameObject target)
    {
        //Vector3 direction = (target.transform.position - transform.position).normalized;
        //transform.position += direction * speed * Time.deltaTime;
    }
}
