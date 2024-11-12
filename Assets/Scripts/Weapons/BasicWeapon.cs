using UnityEngine;

public class BasicWeapon : Weapon
{
    protected override void Start()
    {
        baseDamage = 1f;
        speed = 5f;
        lifeTime = 5f;
        base.Start();
    }

    public override void Init(GameObject from, GameObject target)
    {
        //Vector3 direction = (target.transform.position - transform.position).normalized;
        //transform.position += direction * speed * Time.deltaTime;
    }
}
