using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneNavigationPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button level1Button;    // Button to load Level1 scene
    public Button level2Button;    // Button to load Level2 scene
    public Button level3Button;    // Button to load Level3 scene
    public Button level4Button;    // Button to load Level4 scene
    public Button level5Button;    // Button to load Level5 scene
    public Button level6Button;    // Button to load Level6 scene
    public Button level7Button;    // Button to load Level7 scene
    public Button level8Button;    // Button to load Level8 scene
    public Button level9Button;    // Button to load Level8 scene

    void Start()
    {
        // Check if buttons are assigned in the inspector, then add listeners
        if (level1Button != null)
            level1Button.onClick.AddListener(OpenLevel1);
        if (level2Button != null)
            level2Button.onClick.AddListener(OpenLevel2);
        if (level3Button != null)
            level3Button.onClick.AddListener(OpenLevel3);
        if (level4Button != null)
            level4Button.onClick.AddListener(OpenLevel4);
        if (level5Button != null)
            level5Button.onClick.AddListener(OpenLevel5);
        if (level6Button != null)
            level6Button.onClick.AddListener(OpenLevel6);
        if (level7Button != null)
            level7Button.onClick.AddListener(OpenLevel7);
        if (level8Button != null)
            level8Button.onClick.AddListener(OpenLevel8);
        if (level8Button != null)
            level8Button.onClick.AddListener(OpenLevel9);
    }

    // Methods to load each scene
    public void OpenLevel1() { SceneManager.LoadScene("Level1"); }
    public void OpenLevel2() { SceneManager.LoadScene("Level2"); }
    public void OpenLevel3() { SceneManager.LoadScene("Level3"); }
    public void OpenLevel4() { SceneManager.LoadScene("Level4"); }
    public void OpenLevel5() { SceneManager.LoadScene("Level5"); }
    public void OpenLevel6() { SceneManager.LoadScene("Level6"); }
    public void OpenLevel7() { SceneManager.LoadScene("Level7"); }
    public void OpenLevel8() { SceneManager.LoadScene("Level8"); }
    public void OpenLevel9() { SceneManager.LoadScene("Level9"); }
}
