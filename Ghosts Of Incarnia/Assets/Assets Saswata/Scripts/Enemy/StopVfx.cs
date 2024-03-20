using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gg
public class StopVfx : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake(){
        ps = GetComponent<ParticleSystem>();
    }
    private void Update(){
        if(ps&& !ps.IsAlive()){
            Destroy(gameObject);
        }
    }
}
