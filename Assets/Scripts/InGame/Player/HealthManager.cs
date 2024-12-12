using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHP = 5;                // Maximum health points
    private int currentHP;               // Current health points
    public Transform respawnPoint;       // Transform for the respawn location
    public TextMeshProUGUI healthText;   // Reference to the TextMeshProUGUI component for health display
    public GameObject gameOverUI;        // Game Over UI panel
    public GameObject pauseButton;       // Pause button
    public Graphic gameOverText;         // Supports Text or TextMeshProUGUI

    // Audio clips for events
    public AudioClip loseHpSound;
    public AudioClip recoverHpSound;
    public AudioClip GameOverSound;
    private AudioSource audioSource;     // Audio source to play sounds

    private void Start()
    {
        currentHP = maxHP;               // Initialize HP to max
        UpdateHealthUI();                // Display initial health status
        audioSource = GetComponent<AudioSource>(); // Get AudioSource component

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Ensure Game Over UI is hidden at the start
        }
    }

    // Use OnTriggerEnter2D for trigger colliders
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Danger"))
        {
            LoseHP();
        }
        else if (collider.CompareTag("hpplus"))
        {
            RecoverHP();
        }
        else if (collider.CompareTag("respawnIO"))
        {
            RespawnPoint newRespawnPoint = collider.GetComponent<RespawnPoint>();
            if (newRespawnPoint != null)
            {
                SetRespawnPoint(collider.transform, newRespawnPoint);
            }
        }
    }

    private void LoseHP()
    {
        if (currentHP > 0)
        {
            currentHP--;
            PlaySound(loseHpSound);      // Play losing HP sound
            UpdateHealthUI();
            if (currentHP > 0)
            {
                Respawn();
            }
        }

        if (currentHP <= 0)
        {
            TriggerGameOver();
        }
    }

    private void RecoverHP()
    {
        if (currentHP < maxHP)
        {
            currentHP++;
            PlaySound(recoverHpSound);   // Play recovering HP sound
            UpdateHealthUI();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "" + currentHP;
        }
    }

    // Set the respawn point and activate the corresponding RespawnPoint object
    private void SetRespawnPoint(Transform respawnTransform, RespawnPoint newRespawnPoint)
    {
        if (respawnPoint != null && respawnPoint != respawnTransform)
        {
            Destroy(respawnPoint.gameObject);  // Destroy the old respawn point object
        }

        respawnPoint = respawnTransform;
        newRespawnPoint.SetActive();         // Call the new respawn object's SetActive() method
    }

    private void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }
    }

    private void TriggerGameOver()
    {
        // Display Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            
            PlaySound(GameOverSound);
        }
        
        if (gameOverUI != null)
        {
            pauseButton.SetActive(false);
        }

        // Update Game Over text
        if (gameOverText != null)
        {
            if (gameOverText is TextMeshProUGUI tmpText)
            {
                tmpText.text = "Game Over!";
            }
            else if (gameOverText is Text uiText)
            {
                uiText.text = "Game Over!";
            }
        }

        // Pause the game
        Time.timeScale = 0f;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Play the provided sound clip
        }
    }
}
