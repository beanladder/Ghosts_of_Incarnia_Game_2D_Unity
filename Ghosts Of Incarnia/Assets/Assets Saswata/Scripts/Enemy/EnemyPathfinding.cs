using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gg
public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField]float Movespeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    KnockBack knockBack;
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }
    private void FixedUpdate(){
        if(knockBack.GettingKnockedback){return;}
        rb.MovePosition(rb.position+moveDir*(Movespeed*Time.fixedDeltaTime));
    }
    public void MoveTo(Vector2 targetPosition){
        moveDir = targetPosition;
    }
}
