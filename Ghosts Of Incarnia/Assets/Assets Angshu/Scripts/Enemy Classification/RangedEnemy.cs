using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float detectionRadius = 3f;
    public float followDistance = 10f; // The distance at which the AI will stop following
    public float moveSpeed = 4f;
    public float attackCooldown = 0.6f;

    private float currentCooldown = 0f;
    private Transform player;
    //private Transform targetPoint; // Empty GameObject as the target point
    private AIBow bow;

    //private Animator anim;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }


        // Assuming AIBow script is on the bow object
        bow = GetComponentInChildren<AIBow>();
    }

    void Update()
    {
        HandleCooldowns();

        if (PlayerWithinDetectRadius())
        {
            FacePlayer();

            // Follow the player up to a certain distance
            FollowPlayer();

            if (CanAttack())
            {
                FaceBowToPlayer();
                AttackPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        // Calculate the distance between AI and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            
        }
    }

    void AttackPlayer()
    {
        if (bow != null)
        {
            bow.Attack();
            currentCooldown = attackCooldown;
        }
    }

    bool PlayerWithinDetectRadius()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= detectionRadius;
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;
        float newYRotation = (angle < 0) ? 180f : 0.1f;
        transform.rotation = Quaternion.Euler(new Vector3(0f, newYRotation, 0f));
    }

    void FaceBowToPlayer()
    {
        if (bow != null && player != null)
        {
            Vector3 currpos = player.position;
            Vector2 dir = bow.transform.position - currpos;
            bow.transform.right = -dir;
        }
    }

    

    void HandleCooldowns()
    {
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    bool CanAttack()
    {
        return currentCooldown <= 0f;
    }

   
}

