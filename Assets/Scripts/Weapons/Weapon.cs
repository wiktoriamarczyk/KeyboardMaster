using DG.Tweening;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected virtual float currentDamage { get; set; } = 0;
    protected virtual float baseDamage { get; set; } = 0;
    protected virtual float speed { get; set; } = 0;
    protected virtual float lifeTime { get; set; } = 0;
    protected virtual float cooldown { get; set; } = 0;
    protected virtual float comboScaler { get; set; } = 0.5f;
    protected virtual GameObject source { get; set; }
    protected virtual GameObject target { get; set; }

    public float Damage => currentDamage;
    public float Cooldown => cooldown;
    public GameObject Source => source;

    public bool initialized { get; private set; } = false;

    const float animationDuration = 0.2f;

    protected void Start()
    {
        currentDamage = baseDamage;
        StartCoroutine(DestroyAfterTime());
        Combo.onComboChanged += OnComboChanged;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target)
        {
            Destroy();
        }
    }

    protected void OnDestroy()
    {
        Combo.onComboChanged -= OnComboChanged;
        StopAllCoroutines();
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy();
    }

    void Destroy()
    {
        transform.DOScale(0, animationDuration).OnComplete(() => Destroy(gameObject));
    }

    protected void OnComboChanged(int combo)
    {
        currentDamage = baseDamage + combo * comboScaler;
    }

    public virtual void Init(GameObject source, GameObject target)
    {
        this.source = source;

        if (source == null)
        {
            Debug.Log("AAAAAAAAAAAAAAAA");
        }
        this.target = target;
        initialized = true;
    }
}
