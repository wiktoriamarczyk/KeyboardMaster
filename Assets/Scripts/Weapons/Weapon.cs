using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float currentDamage = 0;
    protected float baseDamage = 0;
    protected float speed = 0;
    protected float lifeTime = 0;
    protected float cooldown = 0;
    protected float comboScaler = 0.5f;

    const float animationDuration = 0.2f;

    public float Damage => currentDamage;
    public float Cooldown => cooldown;

    protected virtual void Start()
    {
        currentDamage = baseDamage;
        StartCoroutine(DestroyAfterTime());
        Combo.onComboChanged += OnComboChanged;
    }

    protected void OnDestroy()
    {
        Combo.onComboChanged -= OnComboChanged;
        StopAllCoroutines();
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        transform.DOScale(0, animationDuration).OnComplete(() => Destroy(gameObject));
    }

    protected void OnComboChanged(int combo)
    {
        currentDamage = baseDamage + combo * comboScaler;
    }

    public abstract void Init(GameObject from, GameObject target);
}
