using UnityEngine;
using UnityEngine.UI; // For UI Text and Button
using System.Collections;

public class BackToMainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public RectTransform MainMenuPanel;  // The Main Menu panel that will be shown when going back
    public RectTransform otherPanel1;   // Example other panel to hide

    [Header("UI Text (Back Button)")]
    public Text backToMainMenuText;  // UI Text that acts as a button to go back to the main menu

    // Off-screen position above the screen for hiding panels
    public Vector2 offScreenPosition = new Vector2(0, 1500); // Adjust Y value for screen height
    public float transitionDuration = 0.5f; // Duration of the transition in seconds

    private void Start()
    {
        // Set all panels to off-screen initially
        ResetPanelPositions();

        // Check if the text is assigned and is clickable
        if (backToMainMenuText != null)
        {
            // Add listener to the Text's Button (if it has one)
            Button button = backToMainMenuText.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(ShowMenuPanel);  // Set the OnClick listener for the back-to-main-menu action
            }
            else
            {
                Debug.LogError("The Text does not have a Button component. Add a Button to make it clickable.");
            }
        }
        else
        {
            Debug.LogError("The backToMainMenuText is not assigned!");
        }
    }

    // Method to show the Main Menu panel when called
    public void ShowMenuPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(MainMenuPanel, Vector2.zero)); // Moves to center
    }

    // Method to hide all panels
    private void HideAllPanels()
    {
        StopAllCoroutines(); // Stop any ongoing panel movements
        otherPanel1.anchoredPosition = offScreenPosition;
    }

    // Method to reset panel positions to off-screen
    private void ResetPanelPositions()
    {
        otherPanel1.anchoredPosition = offScreenPosition;
    }

    // Coroutine to move the panel into view with a transition
    private IEnumerator MovePanel(RectTransform panel, Vector2 targetPosition)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel is exactly at the target position at the end
        panel.anchoredPosition = targetPosition;
    }
}
