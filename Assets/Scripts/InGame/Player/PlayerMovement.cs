using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float sneakMultiplier = 0.5f;
    public float jumpForce = 8f;
    public float rotationMultiplier = 20f;

    public float maxStamina = 5f;
    public float staminaDrainRate = 1f;
    public float staminaRegenRate = 0.5f;

    public Slider staminaBar;
    public Canvas staminaCanvas;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float currentStamina;
    private float horizontalInput;
    private bool isSprinting;
    private bool isSneaking;

    public bool useKeyboardInput = true;

    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip landingSFX;
    private AudioSource audioSource;

    // New public boolean for controlling SFX mute state
    public bool sfxEnabled = true;
    
    private string settingsFilePath;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        currentStamina = maxStamina;

        // Initialize AudioSource component
        audioSource = GetComponent<AudioSource>();
        
        // Set the path for UserSettings file
        settingsFilePath = Application.dataPath + "/Scenes/UserSettings.txt";

        // Load SFX setting from the UserSettings file
        LoadSFXSetting();

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }

        if (staminaCanvas != null)
        {
            staminaCanvas.enabled = false;
        }
    }

    private void Update()
    {
        if (useKeyboardInput)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            isSneaking = Input.GetKey(KeyCode.LeftControl);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryJump();
            }
        }

        HandleStamina();
        Move(horizontalInput);

        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
            if (staminaCanvas != null)
            {
                staminaCanvas.enabled = isSprinting || currentStamina < maxStamina;
            }
        }
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);

            if (currentStamina == 0)
            {
                isSprinting = false;
            }
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    private void Move(float horizontalInput)
    {
        float speed = moveSpeed * (isSprinting ? sprintMultiplier : isSneaking ? sneakMultiplier : 1f);
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        float rotationAmount = -horizontalInput * 5 * speed * rotationMultiplier * Time.deltaTime;
        transform.Rotate(0, 0, rotationAmount);
    }

    public void TryJump()
{
    if (isGrounded)
    {
        // Check the current gravity scale of the Rigidbody2D
        float gravityScale = rb.gravityScale;

        // Calculate jump direction based on gravity scale
        float jumpDirection = gravityScale < 0 ? -jumpForce : jumpForce;

        // Apply the jump force in the calculated direction
        rb.velocity = new Vector2(rb.velocity.x, jumpDirection);

        // Set isGrounded to false to indicate the player is in the air
        isGrounded = false;

        // Play jump sound effect if SFX is enabled
        PlaySFX(jumpSFX);
    }
}




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            // Play landing sound effect if SFX is enabled
            PlaySFX(landingSFX);
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null && sfxEnabled)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Load SFX setting from the UserSettings.txt file
    private void LoadSFXSetting()
    {
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("SFX:"))
                {
                    sfxEnabled = line.Contains("ON");
                    UpdateSFXMuteState();
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("UserSettings.txt file not found at path: " + settingsFilePath);
        }
    }

    // Save the current SFX setting to UserSettings.txt
    public void SaveSFXSetting()
    {
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("SFX:"))
                {
                    lines[i] = "SFX: " + (sfxEnabled ? "ON" : "OFF");
                    File.WriteAllLines(settingsFilePath, lines);
                    return;
                }
            }
        }
    }

    // Update the audio source's mute status based on sfxEnabled
    public void UpdateSFXMuteState()
    {
        if (audioSource != null)
        {
            audioSource.enabled = sfxEnabled;
        }
    }

    // Public method to toggle SFX mute state from external controls
    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        UpdateSFXMuteState();
        SaveSFXSetting();
    }

    // Methods to set input from UI controls
    public void SetHorizontalInput(float input)
    {
        if (!useKeyboardInput)
        {
            horizontalInput = input;
        }
    }

    public void SetSprinting(bool sprinting)
    {
        if (!useKeyboardInput)
        {
            isSprinting = sprinting;
        }
    }

    public void SetSneaking(bool sneaking)
    {
        if (!useKeyboardInput)
        {
            isSneaking = sneaking;
        }
    }
}
