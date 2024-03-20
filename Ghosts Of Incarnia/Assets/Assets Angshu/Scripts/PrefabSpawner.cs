using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrefabSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject prefabToSpawn;
    public int numberOfPrefabs = 10;
    public float spawnRadius = 0.5f;
    public GameObject parentContainer; // New variable to hold the parent container

    void Start()
    {
        SpawnPrefabs();
    }

    void SpawnPrefabs()
    {
        if (parentContainer == null)
        {
            parentContainer = new GameObject("PrefabContainer"); // Create a new parent container if not assigned
        }

        BoundsInt bounds = tilemap.cellBounds;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Generate random positions within the Tilemap bounds
            Vector3Int randomTilePosition = new Vector3Int(
                Random.Range(bounds.x, bounds.x + bounds.size.x),
                Random.Range(bounds.y, bounds.y + bounds.size.y),
                0
            );

            // Check for overlaps (you may need a custom method based on your game logic)
            if (!IsTileOccupied(randomTilePosition))
            {
                // Convert tile position to world position
                Vector3 spawnPosition = tilemap.GetCellCenterWorld(randomTilePosition);

                // Randomize position within spawnRadius
                spawnPosition += (Vector3)(Random.insideUnitCircle * spawnRadius);


                // Instantiate the prefab at the chosen position as a child of the parent container
                GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, parentContainer.transform);

                // Decrease the radius of the spawned object (optional)
                spawnedPrefab.transform.localScale *= 0.5f;
            }
            else
            {
                // Retry or handle overlap as needed
                i--;
            }
        }
    }

    bool IsTileOccupied(Vector3Int tilePosition)
    {
        // Implement your own logic to check if a tile is occupied
        // For example, check if the tile is already filled with another prefab
        // You may need to consider other factors based on your game's requirements
        return false;
    }
}