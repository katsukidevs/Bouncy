using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [Header("Sprites for State Change")]
    public SpriteRenderer baseSprite;

    [Header("Colors")]
    public Color activeBaseColor = Color.green;
    public Color inactiveBaseColor = Color.red;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.5f;  // How much it floats up and down
    public float floatSpeed = 1f;       // How fast it floats

    private Vector3 initialPosition;
    private bool isActive = false;

    void Start()
    {
        initialPosition = transform.position;
        SetInactive();
    }

    void Update()
    {
        // Apply floating animation
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    public void SetActive()
    {
        isActive = true;
        UpdateColors();
    }

    public void SetInactive()
    {
        isActive = false;
        UpdateColors();
    }

    private void UpdateColors()
    {
        // Change colors based on the active state
        baseSprite.color = isActive ? activeBaseColor : inactiveBaseColor;
    }

    // Optional: Method to toggle between active and inactive
    public void ToggleState()
    {
        if (isActive)
            SetInactive();
        else
            SetActive();
    }
}
