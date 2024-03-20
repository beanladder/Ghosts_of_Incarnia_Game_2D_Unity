using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickUpType{
        HealthGlobe,
        Keys,
    }
    [SerializeField]PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField]private float acclerationRate = 0.2f;
    [SerializeField] private float moveSpeed = 3f;
    private Vector3 moveDir;
    private Rigidbody2D rb;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start(){
        StartCoroutine(AnimCurveSpawnRoutine());
    }
    private void Update(){
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if(Vector3.Distance(transform.position,playerPos)< pickUpDistance){
            moveDir = (playerPos-transform.position).normalized;
            moveSpeed+=acclerationRate;
        } 
        else{
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }
    private void FixedUpdate(){
        rb.velocity = moveDir*moveSpeed*Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.GetComponent<PlayerController>()){
            DetectPickUpType();
            Destroy(gameObject);
        }
    }
    private IEnumerator AnimCurveSpawnRoutine(){
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x+Random.Range(-2f,2f);
        float randomY = transform.position.y+Random.Range(-1f,1f);
        Vector2 endPoint = new Vector2(randomX,randomY);
        float timePassed= 0f;
        while(timePassed<popDuration){
            timePassed+=Time.deltaTime;
            float linearT = timePassed/popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f,heightY,heightT);
            transform.position = Vector2.Lerp(startPoint,endPoint,linearT)+new Vector2(0f,height);
            yield return null;

        }
    }
    void DetectPickUpType(){
        switch(pickUpType){
            case PickUpType.HealthGlobe:
            PlayerHealth.Instance.HealPlayer();
            Debug.Log("Health PickUp");
            break;
            case PickUpType.Keys:
            UpdateKeyPickupText();
            Debug.Log("Keys Pickup");
            break;
        }
    }
    void UpdateKeyPickupText(){
        KeyPickupCounter.Instance.IncrementKeyPickupCount();
    }
}
