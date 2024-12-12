using UnityEngine;

public class PowerUpAnimation : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 2f; // Speed of floating
    public float floatAmplitude = 0.5f; // Amplitude of floating

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f; // Speed of rotation

    [Header("Player Tag")]
    public string playerTag = "Player"; // Tag to identify the player

    [Header("Behavior Settings")]
    public bool destroyOnCollect = true; // Determines if the object is destroyed on collection

    private Vector3 startPosition;

    private void Start()
    {
        // Save the starting position of the power-up
        startPosition = transform.position;
    }

    private void Update()
    {
        AnimateFloating();
        AnimateSpinning();
    }

    private void AnimateFloating()
    {
        // Create a floating effect using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void AnimateSpinning()
    {
        // Rotate the power-up around its Z-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is tagged as "Player"
        if (collision.CompareTag(playerTag))
        {
            // Add logic for what happens when the player collects the power-up
            Debug.Log("Power-up collected!");

            // Destroy the power-up if destroyOnCollect is true
            if (destroyOnCollect)
            {
                Destroy(gameObject);
            }
        }
    }
}
