using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameSettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text musicToggleText;          // Text for the Music toggle button
    public Text sfxToggleText;            // Text for the Sound FX toggle button
    public Text controlToggleText;        // Text for the Controls toggle button

    private bool musicEnabled = true;     // Tracks if music is enabled
    private bool sfxEnabled = true;       // Tracks if sound effects are enabled
    private bool buttonControls = true;   // Tracks if controls use buttons (true) or keyboard (false)

    private string settingsFilePath;

    void Start()
    {
        // Define the path to the settings file
        settingsFilePath = Application.dataPath + "/Scenes/UserSettings.txt";

        // Load settings from the file or create default settings
        LoadSettings();
    }

    public void ToggleMusicSetting()
    {
        // Toggle music setting
        musicEnabled = !musicEnabled;

        // Update the UI text
        musicToggleText.text = musicEnabled ? "Music: On" : "Music: Off";

        // Save the updated settings
        SaveSettings();
    }

    public void ToggleSFXSetting()
    {
        // Toggle sound effects setting
        sfxEnabled = !sfxEnabled;

        // Update the UI text
        sfxToggleText.text = sfxEnabled ? "Sound FX: On" : "Sound FX: Off";

        // Save the updated settings
        SaveSettings();
    }

    public void ToggleControlSetting()
    {
        // Toggle control scheme
        buttonControls = !buttonControls;

        // Update the UI text
        controlToggleText.text = buttonControls ? "Controls: Buttons" : "Controls: Keyboard";

        // Save the updated settings
        SaveSettings();
    }

    private void LoadSettings()
    {
        // Check if the settings file exists
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("Music:"))
                {
                    musicEnabled = line.Contains("On");
                    musicToggleText.text = musicEnabled ? "Music: On" : "Music: Off";
                }
                else if (line.StartsWith("SoundFX:"))
                {
                    sfxEnabled = line.Contains("On");
                    sfxToggleText.text = sfxEnabled ? "Sound FX: On" : "Sound FX: Off";
                }
                else if (line.StartsWith("Controls:"))
                {
                    buttonControls = line.Contains("Buttons");
                    controlToggleText.text = buttonControls ? "Controls: Buttons" : "Controls: Keyboard";
                }
            }
        }
        else
        {
            // If no settings file exists, save default settings
            SaveSettings();
        }
    }

    private void SaveSettings()
    {
        // Create an array of strings representing the settings
        string[] settings = new string[]
        {
            "Music: " + (musicEnabled ? "On" : "Off"),
            "SoundFX: " + (sfxEnabled ? "On" : "Off"),
            "Controls: " + (buttonControls ? "Buttons" : "Keyboard")
        };

        // Write the settings to the file
        File.WriteAllLines(settingsFilePath, settings);
    }
}
