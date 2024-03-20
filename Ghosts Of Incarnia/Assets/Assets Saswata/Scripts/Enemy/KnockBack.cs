using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gg
public class KnockBack : MonoBehaviour
{
    public bool GettingKnockedback{get;private set;}
    [SerializeField]float KnockBackTime = 0.2f;
    Rigidbody2D rb;
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    public void GetKnockedBack(Transform damageSource,float knockBackThurst){
        GettingKnockedback = true;
        Vector2 difference = (transform.position-damageSource.position).normalized*knockBackThurst*rb.mass;
        rb.AddForce(difference,ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }
    private IEnumerator KnockRoutine(){
        yield return new WaitForSeconds(KnockBackTime);
        rb.velocity = Vector2.zero;
        GettingKnockedback = false;
    }

    
}
