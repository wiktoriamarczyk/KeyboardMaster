using System.Collections;
using UnityEngine;
using static Data;

public class XBOXBoss : Creature
{
    [SerializeField] XBOXWeapon     weapon;
    [SerializeField] GameObject     weaponHolder;
    [SerializeField] LookAtTarget   lookAtTarget;
    [SerializeField] AudioManager   audioManager;

    GameObject weaponObject;

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
        if (weaponObject != null)
        {
            weaponObject.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    protected override void GetHit()
    {
        base.GetHit();
        audioManager.PlayEnemyHitSound();
    }

    public override void UpdateHealth(float value)
    {
        if (value < 0)
        {
            GetHit();
        }
        base.UpdateHealth(value);
    }

    protected override void Die()
    {
        base.Die();
        SceneController.Instance.SetGameResult(true);
        StartCoroutine(LoadEndSceneAfterDelay(winSceneName));
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToAttack);
            animator.SetTrigger(animationStates[Data.AnimationState.RegularAttack]);
            audioManager.PlayEnemyAttackSound();

            yield return new WaitForSeconds(weaponGrabDuration);
            weaponObject = Instantiate(weapon.gameObject, weaponHolder.transform);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;
            weaponObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            yield return new WaitForSeconds(weaponThrowDuration);
            if (weaponObject != null)
            {
                weaponObject.transform.parent = transform.parent;
                var weaponComponent = weaponObject.GetComponent<Weapon>();
                weaponObject = null;
                weaponComponent.Init(gameObject, lookAtTarget.Target);
            }
        }
    }
}
