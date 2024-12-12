using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadSceneOnTrigger : MonoBehaviour
{
    [Header("Scene to Load")]
    public string sceneName = "Level1";

    [Header("UI Elements")]
    public RectTransform popDownObject; // UI RectTransform to animate
    public Text messageText;            // Congratulatory message text

    [Header("Animation Settings")]
    public float popDownDuration = 0.5f; // Duration of the pop-down animation

    private Vector2 offScreenPosition = new Vector2(0, 800); // Off-screen position (UI)
    private Vector2 onScreenPosition = new Vector2(0, 0);    // On-screen position (UI)

    private void Start()
    {
        // Ensure the pop-down object starts off-screen (UI space)
        if (popDownObject != null)
        {
            popDownObject.anchoredPosition = offScreenPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Start the congratulatory sequence
            StartCoroutine(ShowCongratsAndLoadScene());
        }
    }

    private IEnumerator ShowCongratsAndLoadScene()
    {
        // Pause the game
        PauseGame();

        // Animate the pop-down object
        float elapsedTime = 0f;
        while (elapsedTime < popDownDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Use unscaled time since the game is paused
            float t = Mathf.Clamp01(elapsedTime / popDownDuration);

            if (popDownObject != null)
            {
                popDownObject.anchoredPosition = Vector2.Lerp(offScreenPosition, onScreenPosition, t);
            }
            yield return null;
        }

        // Ensure the object is at the exact on-screen position
        if (popDownObject != null)
        {
            popDownObject.anchoredPosition = onScreenPosition;
        }

        // Show congratulatory message
        if (messageText != null)
        {
            messageText.text = "Congratulations! You finished the level!";
        }

        // Wait for 3 seconds in real-time
        yield return new WaitForSecondsRealtime(2f);

        // Resume the game and load the next scene
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Freeze game time
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game time
    }
}