using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;

    [SerializeField] private AudioClip slashSound;
    
    
    private AudioSource audioSource;
    public AudioSource HitSoundFX;

    //[SerializeField] private float swordAttackCD = .5f;
    [SerializeField]private WeaponInfo weaponInfo;
    private Transform weaponCollider;
    private Animator myAnimator;


    public static Sword Instance;
    

    private GameObject slashAnim;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    private void Start(){
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("slashSpawnPointer").transform;
    }
    private void Update() {
        MouseFollowWithOffset();
    }
    public WeaponInfo GetWeaponInfo(){
        return weaponInfo;
    }

    public void Attack() {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;

        PlaySound(slashSound);
    }

    private void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
    }


    public void DoneAttackingAnimEvent() {
        weaponCollider.gameObject.SetActive(false);
    }

    



    public void SwingUpFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft) { 
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimEvent() {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x) {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        } else {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }


    private void SetSlashSpawnUp()
    {
        
        slashAnimSpawnPoint.localPosition = new Vector3(0.786f, -0.64f, 0f); 
    }

    private void SetSlashSpawnDown()
    {
       // Set the new slash spawn point for swing-down animation
        slashAnimSpawnPoint.localPosition = new Vector3(0.786f, 0.447f, 0f); 
    }
}

