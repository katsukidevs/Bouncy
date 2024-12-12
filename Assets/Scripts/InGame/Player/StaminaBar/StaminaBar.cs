using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCameraTransform;
    private Vector3 offsetFromPlayer;   // Fixed offset to maintain relative position
    private Transform playerTransform;  // Reference to the player

    private void Start()
    {
        // Find the main camera once at the start
        mainCameraTransform = Camera.main.transform;

        // Get the player transform (assumes this script is on the Canvas, which is a child of the player)
        playerTransform = transform.parent;

        // Set the initial offset from the player's position
        offsetFromPlayer = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        // Update the position to follow the player's position, keeping the offset constant
        transform.position = playerTransform.position + offsetFromPlayer;

        // Rotate to face the camera without tilting
        transform.rotation = Quaternion.Euler(0, mainCameraTransform.eulerAngles.y, 0);
    }
}
