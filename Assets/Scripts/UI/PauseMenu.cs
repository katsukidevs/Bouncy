using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI;       // The main pause menu UI panel
    public GameObject controlsUI;        // The controls UI element to enable/disable

    [Header("Buttons Texts")]
    public Text musicButtonText;         // Text component for the Music toggle button
    public Text soundFxButtonText;       // Text component for the Sound FX toggle button
    public Text controlsButtonText;      // Text component for the Controls toggle button

    [Header("Audio Sources")]
    public AudioSource musicAudioSource; // AudioSource for background music
    public AudioSource sfxAudioSource;   // AudioSource for sound effects

    [Header("Other References")]
    public PlayerMovementController playerMovementController; // Reference to the PlayerMovementController
    public MusicController musicController; // Reference to the MusicController

    private bool isPaused = false;
    private bool controlsEnabled = true;
    private float currentVolume = 1f;     // Default volume is 1 (full volume)

    private string settingsFilePath;

    void Start()
    {
        // Set the path for the UserSettings file in the Assets/Scenes folder
        settingsFilePath = Application.dataPath + "/Scenes/UserSettings.txt";

        // Load settings from the file
        LoadSettings();

        // Apply the loaded settings to the audio sources
        ApplyAudioSettings();
    }

    void Update()
    {
        // Toggling the pause menu when the player presses the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Ensure time scale is reset when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale in case it was paused
        SceneManager.LoadScene("MainMenu");
    }

    // Toggle Music (calls MusicController)
    public void ToggleMusic()
    {
        if (musicController != null)
        {
            musicController.ToggleMusic();
        }

        musicButtonText.text = musicController.musicOn ? "Music: On" : "Music: Off";
    }

    // Toggle Sound FX (controls PlayerMovementController)
    public void ToggleSoundFX()
    {
        if (playerMovementController != null)
        {
            playerMovementController.sfxEnabled = !playerMovementController.sfxEnabled;
        }

        soundFxButtonText.text = playerMovementController.sfxEnabled ? "Sound FX: On" : "Sound FX: Off";
    }

    public void ToggleControls()
    {
        controlsEnabled = !controlsEnabled;
        
        // Toggle control scheme
        if (playerMovementController != null)
        {
            playerMovementController.useKeyboardInput = !controlsEnabled;
        }

        // Update button text
        controlsButtonText.text = controlsEnabled ? "Controls: Buttons" : "Controls: Keyboard";
    }

    // Load settings from the UserSettings.txt file
    private void LoadSettings()
    {
        // Ensure the file exists
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("Music:"))
                {
                    bool musicOn = line.Contains("On");
                    if (musicController != null)
                    {
                        musicController.musicOn = musicOn;
                        musicButtonText.text = musicOn ? "Music: On" : "Music: Off";
                    }
                }
                else if (line.StartsWith("SoundFX:"))
                {
                    bool soundFxOn = line.Contains("On");
                    if (playerMovementController != null)
                    {
                        playerMovementController.sfxEnabled = soundFxOn;
                        soundFxButtonText.text = soundFxOn ? "Sound FX: On" : "Sound FX: Off";
                    }
                }
                else if (line.StartsWith("Controls:"))
                {
                    controlsEnabled = line.Contains("Buttons");
                    controlsButtonText.text = controlsEnabled ? "Controls: Buttons" : "Controls: Keyboard";

                    // Set the control mode in the PlayerMovementController
                    if (playerMovementController != null)
                    {
                        playerMovementController.useKeyboardInput = !controlsEnabled;
                    }
                }
                else if (line.StartsWith("Volume:"))
                {
                    // Parse and set volume from settings file
                    float volume = float.Parse(line.Split(':')[1]);
                    currentVolume = volume;
                }
            }
        }
        else
        {
            // If the file doesn't exist, create default settings and save them
            SaveSettings();
        }
    }

    // Save current settings to the UserSettings.txt file
    private void SaveSettings()
    {
        string[] settings = new string[] 
        {
            "Music: " + (musicController.musicOn ? "On" : "Off"),
            "SoundFX: " + (playerMovementController.sfxEnabled ? "On" : "Off"),
            "Controls: " + (controlsEnabled ? "Buttons" : "Keyboard"),
            "Volume: " + currentVolume.ToString("F2") // Save volume setting
        };

        File.WriteAllLines(settingsFilePath, settings);
    }

    // Apply the audio settings to the audio sources based on loaded settings
    private void ApplyAudioSettings()
    {
        if (musicController != null)
        {
            musicController.ApplyMusicSettings();
        }

        if (playerMovementController != null)
        {
            playerMovementController.UpdateSFXMuteState();
        }

        // Apply saved volume settings
        AdjustVolume(currentVolume);
    }

    // Adjust the volume directly without using a slider
    public void AdjustVolume(float volume)
    {
        currentVolume = volume; // Save current volume level

        // Apply volume to the audio sources
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = currentVolume;
        }

        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = currentVolume;
        }
    }
}
