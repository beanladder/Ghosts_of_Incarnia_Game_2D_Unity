using UnityEngine;

public class WaypointIndicator : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    void Start(){
        if(AssetWarmup.Instance!=null &&AssetWarmup.Instance.centerObject!=null){
            spawnPoint = AssetWarmup.Instance.centerObject.transform;
        }
    }

    void Update()
    {
        if (player != null && spawnPoint != null)
        {
            // Calculate the direction from the player to the spawn point
            Vector3 directionToSpawn = (spawnPoint.position - player.position).normalized;

            // Calculate the rotation to face the spawn point
            float angle = Mathf.Atan2(directionToSpawn.y, directionToSpawn.x) * Mathf.Rad2Deg;

            // Apply the rotation to the waypoint indicator
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // Flip the image horizontally
            if (directionToSpawn.x < 0)
            {
                transform.localScale = new Vector3(1, -1, 1); // Flip along the y-axis
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
