using UnityEngine;
using static Data;

public class XBOXWeapon : Weapon
{
    [SerializeField] Rigidbody rb;

    const float bounceForce = 1f;

    protected override void Start()
    {
        baseDamage = xboxWeaponDmg;
        speed = 11f;
        lifeTime = 8f;

        base.Start();
    }

    void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    public override void Init(GameObject from, GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }
}
