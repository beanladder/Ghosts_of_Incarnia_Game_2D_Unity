using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]float movespeed = 22f;
    [SerializeField]private GameObject HitVfx;
    private WeaponInfo weaponInfo;
    //[SerializeField] AudioSource SoundFX;
    private Vector3 startPos;
    private void Start(){
        startPos = transform.position;
    }
    private void Update(){
        MoveProjectile();
        DetectFireDistance();
    }
    public void UpdateWeaponInfo(WeaponInfo weaponInfo){
        this.weaponInfo = weaponInfo;
    }
    void OnTriggerEnter2D(Collider2D other){
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        if (!other.isTrigger && (enemyHealth || indestructible)) {
            enemyHealth?.TakeDamage(1);
            Instantiate(HitVfx, transform.position, transform.rotation);
            //SoundFX.Play();
            Destroy(gameObject);
        }
        else if(!other.isTrigger&& playerHealth){
            playerHealth.TakeDamage(1);
            Instantiate(HitVfx, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void DetectFireDistance(){
        if(Vector3.Distance(transform.position,startPos)>weaponInfo.weaponRange){
            Destroy(gameObject);
        }
    }
    void MoveProjectile(){
        transform.Translate(Vector3.right*Time.deltaTime*movespeed);
    }
}
