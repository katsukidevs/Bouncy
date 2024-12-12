using UnityEngine;

public class SideToSideMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementRange = 5f; // How far the object moves side to side
    public float movementSpeed = 2f; // Speed of the movement

    private float startPosition;

    void Start()
    {
        // Record the initial position of the object
        startPosition = transform.position.x;
    }

    void Update()
    {
        // Calculate the new position
        float newX = startPosition + Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
