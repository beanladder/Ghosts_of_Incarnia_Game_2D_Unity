using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon{get;private set;}

    private BobbyDeol playerControls;
    float timeBetweenAttacks;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake() {
        base.Awake();

        playerControls = new BobbyDeol();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        AttackCoolDown();
    }

    private void Update() {
        Attack();
    }
    public void NewWeapon(MonoBehaviour newWeapon){
        CurrentActiveWeapon = newWeapon;
        AttackCoolDown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }
    public void WeaponNull(){
        CurrentActiveWeapon = null;
    }

    private void AttackCoolDown(){
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(timeBetweenAttacksRoutine());
    }
    private IEnumerator timeBetweenAttacksRoutine(){
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack() {
        if (attackButtonDown && !isAttacking) {
            AttackCoolDown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
