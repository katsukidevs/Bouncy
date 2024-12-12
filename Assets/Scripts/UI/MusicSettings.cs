using UnityEngine;
using System.IO;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    // Public boolean to be controlled externally (from PauseMenu)
    public bool musicOn = true;

    private void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Apply music setting based on the boolean
        ApplyMusicSettings();
    }

    // Apply music setting based on the musicOn value
    public void ApplyMusicSettings()
    {
        if (audioSource != null)
        {
            audioSource.mute = !musicOn;
        }
    }

    // Public method to toggle music (called by PauseMenu)
    public void ToggleMusic()
    {
        musicOn = !musicOn;
        ApplyMusicSettings();
    }
}
