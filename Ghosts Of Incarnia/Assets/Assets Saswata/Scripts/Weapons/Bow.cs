using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField]private WeaponInfo weaponInfo;
    [SerializeField]GameObject arrowPrefab;
    [SerializeField]Transform arrowSpawn;
    [SerializeField]AudioSource BowFire;
    readonly int FIRE_HASH =   Animator.StringToHash("Fire");
    private Animator anim;
    private void Awake(){
        anim = GetComponent<Animator>();
    }
    public void Attack()
    {

        BowFire.Play();
        SpawnArrow(arrowSpawn.position, arrowSpawn.rotation);
        //BowFire.Stop();
        // Spawn arrow slightly to the left
        //Quaternion leftRotation = Quaternion.Euler(0, 0, -15f);
        //SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * leftRotation);

        // Spawn arrow slightly to the right
        //Quaternion rightRotation = Quaternion.Euler(0, 0, 15f);
        //SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * rightRotation);
    }

    public void Attack2()
    {
        SpawnArrow(arrowSpawn.position, arrowSpawn.rotation);

        // Spawn arrow slightly to the left
        Quaternion leftRotation = Quaternion.Euler(0, 0, -15f);
        SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * leftRotation);

        // Spawn arrow slightly to the right
        Quaternion rightRotation = Quaternion.Euler(0, 0, 15f);
        SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * rightRotation);
    }
    private void SpawnArrow(Vector3 position, Quaternion rotation)
    {
        GameObject newArrow = Instantiate(arrowPrefab, position, rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
    }

    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }
}
