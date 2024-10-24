using System.Collections;
using UnityEngine;
using static Data;

public class XBOXBoss : Creature
{
    [SerializeField] GameObject     weaponPrefab;
    [SerializeField] GameObject     weaponHolder;
    [SerializeField] LookAtTarget   lookAtTarget;

    GameObject spawnedWeapon;

    const float fireballDamage = 10;
    const float lightningDamage = 20;
    const float timeToAttack = 4f;

    override protected void Start()
    {
        base.Start();
        InitializeCommands();
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToAttack);
            Debug.Log("Attacking");
            animator.SetTrigger(animationStates[Data.AnimationState.RegularAttack]);

            yield return new WaitForSeconds(1f);
            spawnedWeapon = Instantiate(weaponPrefab, weaponHolder.transform);
            spawnedWeapon.transform.localPosition = Vector3.zero;
            spawnedWeapon.transform.localRotation = Quaternion.identity;
            spawnedWeapon.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            yield return new WaitForSeconds(0.5f);
            Debug.Log("Throwing");
            var weaponComponent = spawnedWeapon.GetComponent<XBOXWeapon>();
            spawnedWeapon.transform.parent = transform.parent;
            spawnedWeapon = null;
            weaponComponent.Throw(lookAtTarget.Target.transform.position);
        }
    }

    void Update()
    {
        if (spawnedWeapon != null)
        {
            spawnedWeapon.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    void InitializeCommands()
    {
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackFireball], new() { GetHit, UpdateHealthFireball });
        CommandSet.AddCommand(animationStates[Data.AnimationState.AttackLightning], new() { GetHit, UpdateHealthLightning });
    }

    void UpdateHealthFireball()
    {
        base.UpdateHealth(fireballDamage);
    }

    void UpdateHealthLightning()
    {
        base.UpdateHealth(lightningDamage);
    }

    void OnDestroy()
    {
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackFireball]);
        CommandSet.RemoveCommand(animationStates[Data.AnimationState.AttackLightning]);
        StopAllCoroutines();
    }
}
