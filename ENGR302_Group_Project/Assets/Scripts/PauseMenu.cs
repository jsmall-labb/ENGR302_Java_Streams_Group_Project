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
<<<<<<< Updated upstream

    void Start()
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(false); // always start hidden
    }

=======
>>>>>>> Stashed changes
    
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        if (musicSource != null)
<<<<<<< Updated upstream
            musicSource.Pause(); // stop music playback
=======
            musicSource.Pause(); // 🔇 stop music playback
>>>>>>> Stashed changes
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        if (musicSource != null)
<<<<<<< Updated upstream
            musicSource.UnPause(); // resume music from same spot
=======
            musicSource.UnPause(); // 🔊 resume music from same spot
>>>>>>> Stashed changes
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