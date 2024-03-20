using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
//gg
public class EnemyHealth : MonoBehaviour
{
    [SerializeField]int StartingHealth = 3;
    [SerializeField]GameObject deathVFXPrefab;
    [SerializeField]float knockBackThurst = 15f;
    KnockBack knockBack;
    public bool hitCheck;

    public static EnemyHealth Inst;
    
    Flash flash;
    int currentHealth;
    private void Awake(){
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
        Inst = this;
    }
    private void Start(){
        currentHealth = StartingHealth;
        hitCheck = false;
    }
    public void TakeDamage(int damage){
        //Sword.Instance.HitEnemy();
        hitCheck = true;
        currentHealth-=damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform,knockBackThurst);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathCoroutine());
    }

    public IEnumerator HitChecker()
    {
        if (hitCheck)
        {
            yield return new WaitForSeconds(0.4f);
            hitCheck = false;
        }
    }
    private IEnumerator CheckDetectDeathCoroutine(){
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }
    public void DetectDeath(){
        if(currentHealth<=0){
            Instantiate(deathVFXPrefab,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
