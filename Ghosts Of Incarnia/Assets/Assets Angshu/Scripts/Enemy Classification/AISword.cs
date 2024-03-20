using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    public Transform slashAnimSpawnPoint;

    //[SerializeField] private float swordAttackCD = 0.5f;
    [SerializeField] private WeaponInfo weaponInfo;
    //private Transform weaponCollider;
    private Animator myAnimator;

    [SerializeField]private AudioSource AISoundFX;
    private GameObject slashAnim;
    //[SerializeField] private GameObject WeaponColliderobj;
    //[SerializeField]private PolygonCollider2D SwordCollider;
    PlayerHealth playerHealth;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        
        //SwordCollider = GetComponent<PolygonCollider2D>();
        
    }

    private void Start()
    {
        //WeaponColliderobj.SetActive(false);
        playerHealth = GetComponent<PlayerHealth>();
        //weaponCollider = GameObject.Find("WeaponCollid").transform;
        // Get the weapon collider from the child objects
       // weaponCollider = Roamer.Instance.GetWeaponCollider();
       // if (weaponCollider == null)
       // {
       //     Debug.LogError("Weapon collider not found! Make sure GetWeaponCollider() returns a valid Transform.");
        //}
        //if (colliderComponent != null)
        //{
        //    weaponCollider = colliderComponent.transform;
        //}
        //else
        //{
        //    Debug.LogError("Collider not found! Make sure it's set up correctly in your hierarchy.");
        //}

        // Adjust the name based on your hierarchy
        //slashAnimSpawnPoint = transform.Find("SlashSpawnPoint");
        //if (slashAnimSpawnPoint == null)
        //{
        //    Debug.LogError("SlashSpawnPoint not found! Make sure it's set up correctly in your hierarchy.");
        //}
    }

    // You might want to replace this with AI-specific logic
    private void Update()
    {
        // Add AI-specific logic here if needed
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        
        myAnimator.SetTrigger("Attack");
        //StartCoroutine(WeaponColliderControler());
        // Instantiate slashAnim if it's null
        if (slashAnim == null && slashAnimPrefab != null)
        {

            AISoundFX.Play();
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = transform.parent;
            //StartCoroutine(SwordCollide());
        }
        
        //weaponCollider?.gameObject.SetActive(true); // Ensure weaponCollider is not null
        
    }
    void OnTriggerEnter2D(Collider2D other){
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if(!other.isTrigger&&playerHealth){
            playerHealth.TakeDamage(1);
            
        }
    }

    //private IEnumerator WeaponColliderControler()
    //{
        //WeaponColliderobj.SetActive(true);
        //yield return new WaitForSeconds(0.3f);
       // WeaponColliderobj.SetActive(false);
    //}
    //private IEnumerator SwordCollide(){
    //    SwordCollider.enabled = true;
    //    yield return new WaitForSeconds(0.5f);
    //    SwordCollider.enabled = false;
    //}

    public void DoneAttackingAnimEvent()
    {
        //weaponCollider?.gameObject.SetActive(false); // Ensure weaponCollider is not null
    }

    public void SwingUpFlipAnimEvent()
    {
        SetSlashSpawnUp();

        // Check if slashAnim is not null before using it
        if (slashAnim != null)
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

            // Determine the facing direction based on AI's rotation
            bool facingLeft = transform.rotation.eulerAngles.y > 90f && transform.rotation.eulerAngles.y < 270f;

            FlipSlashAnim(facingLeft);
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        SetSlashSpawnDown();

        // Check if slashAnim is not null before using it
        if (slashAnim != null)
        {
            slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Determine the facing direction based on AI's rotation
            bool facingLeft = transform.rotation.eulerAngles.y > 90f && transform.rotation.eulerAngles.y < 270f;

            FlipSlashAnim(facingLeft);
        }
    }

    private void FlipSlashAnim(bool facingLeft)
    {
        if (slashAnim != null)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = facingLeft;
        }
    }

    private void SetSlashSpawnUp()
    {
        slashAnimSpawnPoint.localPosition = new Vector3(0.786f, -0.64f, 0f);
    }

    private void SetSlashSpawnDown()
    {
        slashAnimSpawnPoint.localPosition = new Vector3(0.786f, 0.447f, 0f);
    }
}
