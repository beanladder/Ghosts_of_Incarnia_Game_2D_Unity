using UnityEngine;

public class RainEffect : MonoBehaviour
{
    private Transform playerTransform;
    private Transform mainCameraTransform;

    void Start()
    {
        // Assuming the rain is a child of the player object.
        playerTransform = transform.parent;

        // Assuming the camera is also a child of the player object.
        mainCameraTransform = playerTransform.GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // Calculate the offset between the rain and the camera.
        Vector3 cameraOffset = mainCameraTransform.position - transform.position;

        // Update the local position of the rain based on the player's movement and camera offset.
        transform.localPosition = playerTransform.position - mainCameraTransform.position;
        transform.position += cameraOffset;
    }
}
