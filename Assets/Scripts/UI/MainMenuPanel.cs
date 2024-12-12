using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIPanel : MonoBehaviour
{
    // UI objects to bring into view
    public RectTransform MainMenuPanel;
    public RectTransform playPanel;
    public RectTransform creditsPanel;
    public RectTransform settingsPanel;
    public RectTransform quitConfirmPanel;

    // Off-screen position above the screen for hiding panels
    public Vector2 offScreenPosition = new Vector2(0, 1500); // Adjust Y value for screen height
    public float transitionDuration = 0.5f; // Duration of the transition in seconds

    private void Start()
    {
        // Set all panels to off-screen initially
        ResetPanelPositions();
    }

    private void Update()
    {
        // Check if the device back button was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowQuitConfirmPanel();
        }
    }

    private void ResetPanelPositions()
    {
        playPanel.anchoredPosition = offScreenPosition;
        creditsPanel.anchoredPosition = offScreenPosition;
        settingsPanel.anchoredPosition = offScreenPosition;
        quitConfirmPanel.anchoredPosition = offScreenPosition;
    }

    // Method to show the Play panel with a bounce transition
    public void ShowPlayPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(playPanel, Vector2.zero)); // Moves to center
    }

    // Method to show the MainMenu panel with a bounce transition
    public void ShowMenuPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(MainMenuPanel, Vector2.zero)); // Moves to center
    }

    // Method to show the Credits panel with a bounce transition
    public void ShowCreditsPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(creditsPanel, Vector2.zero));
    }

    // Method to show the Settings panel with a bounce transition
    public void ShowSettingsPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(settingsPanel, Vector2.zero));
    }

    // Method to open the Themes scene
    public void OpenThemesScene()
    {
        SceneManager.LoadScene("Themes");
    }

    // Method to show the Quit confirmation panel with a bounce transition
    public void ShowQuitConfirmPanel()
    {
        HideAllPanels();
        StartCoroutine(MovePanel(quitConfirmPanel, Vector2.zero));
    }

    // Method to close the Quit confirmation panel and return to the main menu
    public void CloseQuitConfirmPanel()
    {
        HideAllPanels();
        // Bring back main menu panel here (e.g., playPanel or any other main menu panel)
        StartCoroutine(MovePanel(playPanel, Vector2.zero));
    }

    // Method to hide all panels by moving them off-screen
    private void HideAllPanels()
    {
        StopAllCoroutines(); // Stop any ongoing panel movements
        playPanel.anchoredPosition = offScreenPosition;
        creditsPanel.anchoredPosition = offScreenPosition;
        settingsPanel.anchoredPosition = offScreenPosition;
        quitConfirmPanel.anchoredPosition = offScreenPosition;
    }

    // Coroutine to move the panel into view with a bouncy transition
    private IEnumerator MovePanel(RectTransform panel, Vector2 targetPosition)
    {
        Vector2 startPosition = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = BounceEaseOut(t);  // Apply the bounce easing function
            panel.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the panel is exactly at the target position at the end
        panel.anchoredPosition = targetPosition;
    }

    // Custom easing function for a bouncy ease-out effect
    private float BounceEaseOut(float t)
    {
        if (t < (1 / 2.75f))
        {
            return 7.5625f * t * t;
        }
        else if (t < (2 / 2.75f))
        {
            t -= (1.5f / 2.75f);
            return 7.5625f * t * t + 0.75f;
        }
        else if (t < (2.5f / 2.75f))
        {
            t -= (2.25f / 2.75f);
            return 7.5625f * t * t + 0.9375f;
        }
        else
        {
            t -= (2.625f / 2.75f);
            return 7.5625f * t * t + 0.984375f;
        }
    }

    // Method to exit the application
    public void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
