using System.Collections;
using UnityEngine;
using DG.Tweening;

public class XBOXWeapon : Weapon
{
    [SerializeField] Rigidbody rb;

    const float lifeTime = 8f;
    const float speed = 11;
    const float animationDuration = 0.2f;
    const float bounceForce = 1f;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    public void Throw(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        transform.DOScale(0, animationDuration).OnComplete(() => Destroy(gameObject));
    }

    void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

}
