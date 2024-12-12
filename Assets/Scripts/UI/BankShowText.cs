using System.IO;
using UnityEngine;
using TMPro;

public class UpdateTextFromSettings : MonoBehaviour
{
    [Header("TextMeshPro UI Components")]
    public TextMeshProUGUI coinsText;  // UI text for displaying the number of coins
    public TextMeshProUGUI gemsText;   // UI text for displaying the number of gems

    private string settingsFilePath;

    void Start()
    {
        // Set the path to the settings file in the Assets/Scenes/ folder
        settingsFilePath = Path.Combine(Application.dataPath, "Scenes/UserSettings.txt");

        // Update the texts based on the file contents
        UpdateTextsFromFile();
    }

    void UpdateTextsFromFile()
    {
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            foreach (string line in lines)
            {
                // Check for the "Coins" line and extract the number after the colon
                if (line.StartsWith("Coins:"))
                {
                    string coinsValue = line.Substring("Coins:".Length).Trim();
                    coinsText.text = coinsValue;  // Update the coinsText UI element
                }
                // Check for the "Gems" line and extract the number after the colon
                else if (line.StartsWith("Gems:"))
                {
                    string gemsValue = line.Substring("Gems:".Length).Trim();
                    gemsText.text = gemsValue;  // Update the gemsText UI element
                }
            }
        }
        else
        {
            Debug.LogError("Settings file not found at: " + settingsFilePath);
        }
    }
}
