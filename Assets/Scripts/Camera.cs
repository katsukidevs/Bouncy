using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;               // Object to follow, can be any GameObject
    public Vector3 offset = new Vector3(0, 1, -10); // Offset for camera positioning
    public float smoothSpeed = 0.125f;     // Overall smooth speed for following
    public float verticalDamping = 0.3f;   // Damping for vertical movement
    public float horizontalDamping = 0.15f; // Damping for horizontal movement

    public Transform leftBoundary;         // Left boundary object
    public Transform rightBoundary;        // Right boundary object
    public Transform topBoundary;          // Top boundary object
    public Transform bottomBoundary;       // Bottom boundary object

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        // Check if target and boundary objects are set
        if (target == null || leftBoundary == null || rightBoundary == null || topBoundary == null || bottomBoundary == null) 
            return;

        // Calculate the camera’s view extents (half of width and height based on orthographic size or perspective FOV)
        float camHeight = cam.orthographic ? cam.orthographicSize : Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * (transform.position.z - target.position.z);
        float camWidth = camHeight * cam.aspect;

        // Calculate the camera’s target position based on the target’s position and offset
        Vector3 targetPosition = target.position + offset;

        // Apply smooth dampening to move the camera horizontally and vertically
        float smoothX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocity.x, horizontalDamping);
        float smoothY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocity.y, verticalDamping);

        // Define clamped camera position based on boundary objects and camera size
        float clampedX = Mathf.Clamp(smoothX, leftBoundary.position.x + camWidth, rightBoundary.position.x - camWidth);
        float clampedY = Mathf.Clamp(smoothY, bottomBoundary.position.y + camHeight, topBoundary.position.y - camHeight);

        // Set the new camera position with the clamped values
        transform.position = new Vector3(clampedX, clampedY, targetPosition.z);

        // Adjust player offset if camera reaches boundary
        if (transform.position.x == clampedX && smoothX != clampedX)
        {
            // Allow player to offset within the boundary if camera is constrained horizontally
            targetPosition.x = clampedX;
        }
        if (transform.position.y == clampedY && smoothY != clampedY)
        {
            // Allow player to offset within the boundary if camera is constrained vertically
            targetPosition.y = clampedY;
        }
    }
}
