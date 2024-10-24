using System.Collections;
using UnityEngine;

public class XBOXWeapon : Weapon
{
    [SerializeField] Rigidbody rb;

    const float lifeTime = 10f;
    const float speed = 10;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    public void Throw(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
