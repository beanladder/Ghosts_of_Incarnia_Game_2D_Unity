using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeSource : MonoBehaviour
{
    [SerializeField]int damageAmount = 1;
    private void OnTriggerEnter2D(Collider2D other){
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        //Sword.Instance.HitSoundFX.Play();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
