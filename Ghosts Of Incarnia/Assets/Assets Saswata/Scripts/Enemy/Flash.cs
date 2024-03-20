using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//gg
public class Flash : MonoBehaviour
{
    [SerializeField]Material WhiteFlashMat;
    [SerializeField]private float restoreDefaultMat = 0.2f;
    Material defaultMat;
    SpriteRenderer spriteRenderer;
    private void Awake(){
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
        
    }
    public float GetRestoreMatTime(){
        return restoreDefaultMat;
    }
    public IEnumerator FlashRoutine(){
        spriteRenderer.material = WhiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMat);
        spriteRenderer.material = defaultMat;
        
    }
}
