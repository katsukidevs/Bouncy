using UnityEngine;

public class PausePlayButton : MonoBehaviour
{
    [Header("Objects to Enable When Paused")]
    public GameObject[] objectsToEnableWhenPaused;   // Objects that start disabled, shown during pause

    [Header("Objects to Disable When Paused")]
    public GameObject[] objectsToDisableWhenPaused;  // Objects that start enabled, hidden during pause

    private bool isPaused = false;

    void Start()
    {
        // Set initial state for objects that should only be active during pause
        foreach (GameObject obj in objectsToEnableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(false);  // Start disabled
                Debug.Log($"Start: {obj.name} is disabled");
            }
        }

        // Set initial state for objects that should be active during gameplay
        foreach (GameObject obj in objectsToDisableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(true);   // Start enabled
                Debug.Log($"Start: {obj.name} is enabled");
            }
        }
    }

    void Update()
    {
        // Check for ESC or Back button press to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePausePlay();
        }
    }

    public void TogglePausePlay()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Stop time to pause the game
        isPaused = true;

        Debug.Log("Game Paused");

        // Enable objects that should only be shown during pause
        foreach (GameObject obj in objectsToEnableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"{obj.name} enabled during pause");
            }
        }

        // Disable objects that should only be shown during gameplay
        foreach (GameObject obj in objectsToDisableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"{obj.name} disabled during pause");
            }
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume time
        isPaused = false;

        Debug.Log("Game Resumed");

        // Disable objects that should only be active during pause
        foreach (GameObject obj in objectsToEnableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log($"{obj.name} disabled after resume");
            }
        }

        // Enable objects that should only be active during gameplay
        foreach (GameObject obj in objectsToDisableWhenPaused)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"{obj.name} enabled after resume");
            }
        }
    }
}
