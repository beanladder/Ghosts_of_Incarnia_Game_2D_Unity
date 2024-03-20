using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemy : MonoBehaviour
{


    public float detectionRadius = 3f; // Radius for detecting the player
    public float moveSpeed = 2f; // Speed at which the enemy moves towards the player
    public string obstacleTag = "Tree"; // Tag for obstacle GameObjects
    [SerializeField] float dashSpeed = 7f;
    public static Roamer Instance;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AudioSource detectAudio;
    private bool hasDetectedPlayer = false;
    //[SerializeField] private Transform weaponCollider;

    private Transform player; // Reference to the player
    private bool isChasingPlayer = false;
    //private Animator anim;

    private bool isFallingBack = false;
    private float fallbackCooldown = 2.2f; // Time the AI will fall back before attacking again
    private float currentFallbackCooldown = 0f;

    public float fallbackSpeed = 3f;


    public float attackCooldown = 0.6f; // Cooldown duration between attacks
    private float currentCooldown = 0f; // Current cooldown timer

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        //centerPoint.position = AssetWarmup.Instance.CenterPrefab.transform.position; 
    }

    void Start()
    {

        // Assuming your player object has the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }
    //public Transform GetWeaponCollider()
    //{
       // return weaponCollider;
    //}

    void Update()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentFallbackCooldown > 0f)
        {
            currentFallbackCooldown -= Time.deltaTime;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < 1f && currentCooldown <= 0f)
        {
            // Face the player before attacking
            FacePlayer();
            AttackPlayer();
        }
        else
        {
            if (PlayerWithinDetectRadius())
            {
                isChasingPlayer = true;
                // Face the player when chasing
                FacePlayer();
            }

            if (isChasingPlayer)
            {
                ApproachPlayer();
            }
            

            if (isFallingBack)
            {
                FallBack();
            }
        }
    }

    void FacePlayer()
    {
        // Face the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        // Determine the Y rotation based on the angle
        float newYRotation = (angle < 0) ? 180f : 0.1f;

        // Set the rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, newYRotation, 0f));
    }


    void AttackPlayer()
    {
        // Trigger attack animation or perform attack logic
        AISword sword = GetComponentInChildren<AISword>();

        if (sword != null)
        {
            // Call a method to trigger the attack logic on the sword
            sword.Attack();

            // Set the cooldown timer
            currentCooldown = attackCooldown;

            // Set falling back state
            isFallingBack = true;
            currentFallbackCooldown = fallbackCooldown;
        }
    }

    void FallBack()
    {
        if (currentFallbackCooldown > 0f)
        {
            // Calculate the direction opposite to the player's position
            Vector3 directionToPlayer = (transform.position - player.position).normalized;

            // Move away from the player
            Vector3 fallbackPosition = transform.position + directionToPlayer * fallbackSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, fallbackPosition, fallbackSpeed * Time.deltaTime);

            // Decrease the fallback cooldown
            currentFallbackCooldown -= Time.deltaTime;
        }
        else
        {
            // Reset falling back state
            isFallingBack = false;
        }
    }

    void ApproachPlayer()
    {
        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        //anim.SetFloat("moveX", transform.eulerAngles.x);
        //anim.SetFloat("moveY", transform.eulerAngles.y);
    }

    bool PlayerWithinDetectRadius()
    {
        if (!hasDetectedPlayer)
        {
            // Check if the player is within the detect radius
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius)
            {
                // Play the detect audio sound
                detectAudio.Play();

                // Set the flag to true to indicate that the player has been detected
                hasDetectedPlayer = true;
            }
        }

        return hasDetectedPlayer;
    }
}
