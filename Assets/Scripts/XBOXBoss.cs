using System.Collections;
using UnityEngine;
using static Data;
using static Timers;

public class XBOXBoss : Creature
{
    [SerializeField] GameObject     weaponPrefab;
    [SerializeField] GameObject     weaponHolder;
    [SerializeField] LookAtTarget   lookAtTarget;
    [SerializeField] AudioManager   audioManager;
    [SerializeField] Timers         timer;

    GameObject spawnedWeapon;

    const float timeToAttack = 4f;
    const float weaponGrabDuration = 1f;
    const float weaponThrowDuration = 0.5f;

    override protected void Start()
    {
        maxHealth = xboxBossHealth;
        base.Start();
        StartCoroutine(AttackCoroutine());
    }

    void Update()
    {
        if (spawnedWeapon != null)
        {
            spawnedWeapon.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null && weapon.GetType() != typeof(XBOXWeapon))
        {
            GetHit();
            UpdateHealth(weapon.Damage);
            audioManager.PlayEnemyHitSound();
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToAttack);
            animator.SetTrigger(animationStates[Data.AnimationState.RegularAttack]);
            audioManager.PlayEnemyAttackSound();

            yield return new WaitForSeconds(weaponGrabDuration);
            spawnedWeapon = Instantiate(weaponPrefab, weaponHolder.transform);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.identity;
            spawnedWeapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            yield return new WaitForSeconds(weaponThrowDuration);
            var weaponComponent = spawnedWeapon.GetComponent<XBOXWeapon>();
            spawnedWeapon.transform.parent = transform.parent;
            spawnedWeapon = null;
            weaponComponent.Init(lookAtTarget.Target);
        }
    }

    protected override void Die()
    {
        base.Die();
        SceneController.Instance.SetGameResult(true);
        StartCoroutine(LoadEndSceneAfterDelay(winSceneName));
    }
}
