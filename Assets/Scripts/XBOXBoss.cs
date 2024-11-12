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

    void OnCollisionEnter(Collision collision)
    {
        var weaponComponent = collision.gameObject.GetComponent<Weapon>();
        if (weaponComponent != null && weaponComponent.Source != gameObject)
        {
            GetHit();
            UpdateHealth(weaponComponent.Damage);
            audioManager.PlayEnemyHitSound();

            Debug.Log(string.Format("<color=#{0:X2}{1:X2}{2:X2}>{3}</color>",
              (byte)(Color.red.r * 255f), (byte)(Color.red.g * 255f), (byte)(Color.red.b * 255f),
              $"DAMAGE to XBOX: {weaponComponent.Damage} - {weaponComponent.gameObject.name}"));
            Debug.Log($"SOURCE: {weaponComponent.Source?.name} | init: {weaponComponent.initialized} ME: {gameObject}");
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
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
            weaponObject.transform.parent = transform.parent;
            var weaponComponent = weaponObject.GetComponent<Weapon>();
            weaponObject = null;
            weaponComponent.Init(gameObject, lookAtTarget.Target);
        }
    }
}
