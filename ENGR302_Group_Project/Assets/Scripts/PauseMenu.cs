using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseCanvas;
    
    [Header("Audio")]
    [SerializeField] private AudioSource musicSource; // drag your background music here
    
    private bool isPaused = false;
    
    void Update()
    {
        // Toggle pause with Escape using new Input System
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        if (musicSource != null)
            musicSource.Pause(); // 🔇 stop music playback
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        if (musicSource != null)
            musicSource.UnPause(); // 🔊 resume music from same spot
        isPaused = false;
    }
    
    public void QuitToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1f; // reset in case you quit while paused
        if (musicSource != null)
            musicSource.Stop(); // optional: fully stop music if returning to menu
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
}