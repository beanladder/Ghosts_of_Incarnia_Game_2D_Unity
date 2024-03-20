using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class AssetWarmup : MonoBehaviour
{

    public static AssetWarmup Instance;
    public GameObject[] WorldPrefab;
    public GameObject MainCamera;
    public GameObject CenterPrefab;
     public GameObject centerObject { get; private set; } // Prefab to spawn at the center
    public GameObject PointOfInterestPrefab;
    public GameObject PlayerPrefab;
    public GameObject GuardEnemyPrefab;
    public GameObject RangedEnemyPrefab;
    public GameObject RoamerEnemyPrefab;



    
    public int numberOfPoints = 10;
    public float spawnRadius = 5.0f;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
{
    // Check if the prefabs are assigned
    if (WorldPrefab != null && CenterPrefab != null && PointOfInterestPrefab != null && PlayerPrefab != null && GuardEnemyPrefab != null && RangedEnemyPrefab != null && RoamerEnemyPrefab != null)
    {
        StartCoroutine(CameraStop());
            int random = Random.Range(0, WorldPrefab.Length);
        // Instantiate the WorldPrefab
        GameObject instantiatedWorld = Instantiate(WorldPrefab[random]);

            Vector3 guardEnemyScale = new Vector3(0.65f, 0.65f, 0.65f);
            Vector3 rangedEnemyScale = new Vector3(0.65f, 0.65f, 0.65f);

            // Instantiate the CenterPrefab at the center of the instantiated WorldPrefab
            centerObject = Instantiate(CenterPrefab, transform.position, Quaternion.identity, instantiatedWorld.transform);

        // Spawn PointOfInterest prefabs around the center
        List<Transform> poiTransforms = SpawnPointsOfInterest(centerObject.transform.position, numberOfPoints, spawnRadius, instantiatedWorld.transform, 20f);

        // Instantiate PlayerPrefab at the center
        Instantiate(PlayerPrefab, centerObject.transform.position, Quaternion.identity);

            // Spawn EnemyPrefabs around each PointOfInterest
            foreach (Transform poiTransform in poiTransforms)
            {
                SpawnGuardEnemies(poiTransform, 2, 2.0f, guardEnemyScale);
                SpawnRangedEnemies(poiTransform, 1, 2.0f, rangedEnemyScale);
            }

            // Set the position of the main camera to the position of the instantiated WorldPrefab
            SetCameraPositionToTilemap(instantiatedWorld);
    }
    else
    {
        Debug.LogError("One or more prefabs are not assigned in the inspector.");
    }
}

    void SpawnGuardEnemies(Transform pointOfInterestTransform, int count, float radius, Vector3 scale)
    {
        for (int i = 0; i < count; i++)
        {
            // Calculate random position around the PointOfInterest within the specified radius
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = pointOfInterestTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0.0f);

            // Instantiate GuardEnemyPrefab at the calculated position, set its parent to the PointOfInterest, and apply the scale
            GameObject enemy = Instantiate(GuardEnemyPrefab, spawnPosition, Quaternion.identity, pointOfInterestTransform);
            enemy.transform.localScale = scale;
        }
    }

    void SpawnRangedEnemies(Transform pointOfInterestTransform, int count, float radius, Vector3 scale)
    {
        for (int i = 0; i < count; i++)
        {
            // Calculate random position around the PointOfInterest within the specified radius
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = pointOfInterestTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0.0f);

            // Instantiate RangedEnemyPrefab at the calculated position, set its parent to the PointOfInterest, and apply the scale
            GameObject enemy = Instantiate(RangedEnemyPrefab, spawnPosition, Quaternion.identity, pointOfInterestTransform);
            enemy.transform.localScale = scale;
        }
    }


    List<Transform> SpawnPointsOfInterest(Vector3 center, int count, float radius, Transform parent, float exclusionRadius)
    {
        List<Transform> poiTransforms = new List<Transform>();

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = Vector3.zero;

            // Keep trying to find a position until it is outside the exclusionRadius from the center
            do
            {
                // Calculate random position around the center within the specified radius
                Vector2 randomOffset = Random.insideUnitCircle * radius;
                spawnPosition = center + new Vector3(randomOffset.x, randomOffset.y, 0.0f);

            } while (Vector3.Distance(spawnPosition, center) < exclusionRadius);

            // Instantiate PointOfInterestPrefab at the calculated position
            GameObject poi = Instantiate(PointOfInterestPrefab, spawnPosition, Quaternion.identity, parent);
            GameObject roam = Instantiate(RoamerEnemyPrefab, spawnPosition, Quaternion.identity, parent);

            // Add the transform of the spawned PointOfInterest to the list
            poiTransforms.Add(poi.transform);
        }

        return poiTransforms;
    }


    private Bounds GetBounds(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds;
        }

        Debug.LogError("Object has no Renderer or Collider component.");
        return new Bounds();
    }

    public IEnumerator CameraStop()
    {
        yield return new WaitForSeconds(0.2f);
        MainCamera.SetActive(false);
    }

    void SetCameraPositionToTilemap(GameObject tilemapObject)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // Set the position of the main camera to the position of the tilemapObject
            mainCamera.transform.position = tilemapObject.transform.position;

            // Optionally, adjust camera settings like rotation and other properties if needed
        }
        else
        {
            Debug.LogError("Main camera not found in the scene.");
        }
    }
}
