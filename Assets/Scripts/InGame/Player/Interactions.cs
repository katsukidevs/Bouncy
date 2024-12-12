using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    private Vector3 originalScale;          // Original size of the object
    private Rigidbody2D rb;                 // Rigidbody to manage gravity
    private bool gravityReversed = false;   // Status of gravity

    public int Coins { get; private set; }                  // Total coins collected
    public int CollectablesRemaining { get; private set; }  // Total collectables collected
    public int TotalCollectables { get; private set; }      // Total collectables in the stage

    
    public AudioClip GravityResetSFX;
    public AudioClip ReverseGravitySFX;
    public AudioClip COllectCoinSFX;
    public AudioClip GrowSFX;
    public AudioClip ShrinkSFX;
    public AudioClip COllectableSFX;
    public AudioClip FinishSFX;
    private AudioSource audioSource; 

    [Header("UI Text")]
    public TMP_Text coinText;  // UI Text to display the coin count

    [Header("Camera Switch")]
    public Camera mainCamera;    // The main camera
    public Camera specialCamera; // The camera to switch to when all collectables are collected

    [Header("Sprite to Move")]
    public GameObject movingSprite;  // Sprite to move 10 units above

    private void Start()
{
    originalScale = transform.localScale;    // Store original scale
    rb = GetComponent<Rigidbody2D>();        // Get Rigidbody component
    
    // Initialize the total number of collectables by counting objects tagged as "collectable"
    TotalCollectables = GameObject.FindGameObjectsWithTag("collectable").Length;
    CollectablesRemaining = 0; // Start with no collectables collected
    Debug.Log("Total collectables in the stage: " + TotalCollectables);
    
    // Ensure the initial coin count is updated
    LoadCoinCountFromFile();

    // Initialize AudioSource
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        Debug.LogError("AudioSource component is missing on this GameObject!");
    }
}


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check tag and trigger respective behavior
        switch (collider.tag)
        {
            case "growIO":
                Grow();
                break;

            case "shrinkIO":
                Shrink();
                break;

            case "coin":
                CollectCoin(collider.gameObject);  // Pass the coin gameObject to destroy it after collection
                break;

            case "collectable":
                CollectCollectable(collider.gameObject);  // Collect the collectable and destroy it
                break;

            case "reverseGravity":
                ReverseGravity();
                break;

            case "resetgravity":
                ResetGravity();
                break;

            case "Finish":
                UpdateUserSettings();  // Update the file when player reaches finish line
                PlaySound(FinishSFX);
                break;

            default:
                break;
        }
    }

    private void Grow()
    {
        transform.localScale = originalScale * 2;    // Double the scale
        Debug.Log("Grew in size! Current scale: " + transform.localScale);
                PlaySound(GrowSFX); 
    }

    private void Shrink()
    {
        transform.localScale = originalScale;         // Reset to original scale
        Debug.Log("Shrunk back to original size!");
                PlaySound(ShrinkSFX); 
    }

    private void CollectCoin(GameObject coin)
    {
        Coins++;   // Increment coin count
        coinText.text = "" + Coins;  // Update the coin UI text
        Debug.Log("Coins collected: " + Coins);
        Destroy(coin);  // Destroy coin after collecting
        UpdateUserSettings();  // Update the file with the new coin count
        PlaySound(COllectCoinSFX); 
    }

    private void CollectCollectable(GameObject collectable)
    {
        CollectablesRemaining++;   // Increment collectables count
        Debug.Log("Collectables collected: " + CollectablesRemaining + " / " + TotalCollectables);
            PlaySound(COllectableSFX); 

        // If all collectables are collected, switch to the special camera and start the sequence
        if (CollectablesRemaining == TotalCollectables)
        {
            StartCoroutine(SwitchCameraAndMoveSprite());
        }

        Destroy(collectable);  // Destroy collectable after collecting
    }

    private void ReverseGravity()
    {
        if (!gravityReversed)
        {
            gravityReversed = true;
            rb.gravityScale = -1;   // Set gravity to -1
            PlaySound(ReverseGravitySFX); 
            Debug.Log("Gravity reversed!");
        }
    }

    private void ResetGravity()
    {
        if (gravityReversed)
        {
            gravityReversed = false;
            rb.gravityScale = 1;   // Reset gravity to normal
            PlaySound(GravityResetSFX); 
            Debug.Log("Gravity reset to normal!");
        }
    }

    // Method to load the coin count from the UserSettings.txt file
    private void LoadCoinCountFromFile()
    {
        string filePath = "Assets/Scenes/UserSettings.txt";

        if (File.Exists(filePath))
        {
            string fileContent = File.ReadAllText(filePath);
            string[] splitContent = fileContent.Split(':');
            if (splitContent.Length > 1 && int.TryParse(splitContent[1].Trim(), out int savedCoins))
            {
                Coins = savedCoins;  // Load saved coin count from file
                coinText.text = "" + Coins;  // Update UI text
                Debug.Log("Loaded coins: " + Coins);
            }
            else
            {
                Debug.LogError("Failed to load coins from file.");
            }
        }
        else
        {
            Debug.Log("UserSettings.txt not found, starting with 0 coins.");
        }
    }

    // Method to update the UserSettings.txt file with the coins collected
    private void UpdateUserSettings()
    {
        string filePath = "Assets/Scenes/UserSettings.txt"; 

        // Check if the file exists, create if not
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Coins: 0"); // Initialize file with default value
        }

        // Update the file with the current coin count
        string fileContent = "Coins: " + Coins;
        File.WriteAllText(filePath, fileContent);
        Debug.Log("UserSettings.txt updated with coins: " + Coins);
    }

    // Coroutine to switch camera for 5 seconds and move a sprite at the last 4 seconds
    private IEnumerator SwitchCameraAndMoveSprite()
    {
        mainCamera.gameObject.SetActive(false); // Deactivate main camera
        specialCamera.gameObject.SetActive(true); // Activate special camera

        float timeElapsed = 0f;
        float duration = 5f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 1f)  // During last 4 seconds, move sprite
            {
                if (timeElapsed <= duration - 1f) 
                {
                    movingSprite.transform.position += new Vector3(0, 0.1f, 0); // Move sprite 10 units above
                }
            }
            yield return null;
        }

        specialCamera.gameObject.SetActive(false); // Deactivate special camera
        mainCamera.gameObject.SetActive(true); // Reactivate main camera
    }
    private void PlaySound(AudioClip clip)
{
    if (audioSource != null && clip != null)
    {
        audioSource.PlayOneShot(clip);
    }
}

}