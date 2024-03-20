using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform arrowSpawn;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    [SerializeField] private AudioSource AISoundFX;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Attack()
    {
        AISoundFX.Play();
        SpawnArrow(arrowSpawn.position, arrowSpawn.rotation);

        // Spawn arrow slightly to the left
        //Quaternion leftRotation = Quaternion.Euler(0, 0, -15f);
        //SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * leftRotation);

        // Spawn arrow slightly to the right
        //Quaternion rightRotation = Quaternion.Euler(0, 0, 15f);
        //SpawnArrow(arrowSpawn.position, arrowSpawn.rotation * rightRotation);
    }
    private void SpawnArrow(Vector3 position, Quaternion rotation)
    {
        GameObject newArrow = Instantiate(arrowPrefab, position, rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
