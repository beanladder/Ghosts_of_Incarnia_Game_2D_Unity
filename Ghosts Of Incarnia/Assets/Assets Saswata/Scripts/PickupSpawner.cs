using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject HealthPickup;
    [SerializeField] private GameObject KeyPickup;

    private int totalKeyPickups = 15; // Updated total number of KeyPickups
    private int totalHealthPickups = 15; // Updated total number of HealthPickups
    private int keyPickupCount = 0;
    private int healthPickupCount = 0;

    public void DropItems()
    {
        // Check if both KeyPickups and HealthPickups are below their respective target counts
        if (keyPickupCount < totalKeyPickups || healthPickupCount < totalHealthPickups)
        {
            // Randomly choose whether to spawn a KeyPickup or HealthPickup
            if (Random.Range(0f, 1f) < 0.5f)
            {
                // Spawn KeyPickup
                Instantiate(KeyPickup, transform.position, Quaternion.identity);
                keyPickupCount++;
            }
            else
            {
                // Spawn HealthPickup
                Instantiate(HealthPickup, transform.position, Quaternion.identity);
                healthPickupCount++;
            }
        }
    }
}
