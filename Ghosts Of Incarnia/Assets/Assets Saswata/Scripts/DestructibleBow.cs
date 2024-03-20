using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBow : MonoBehaviour
{
    [SerializeField]GameObject destoryVFX;
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.GetComponent<DamegeSource>()||other.gameObject.GetComponent<Projectile>()){
            //GetComponent<PickupSpawner>().DropItems();
            Instantiate(destoryVFX,transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
