using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseCanvas;

    private CanvasGroup pauseCanvasGroup; // auto-detected CanvasGroup

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    private bool isPaused = false;

    void Start()
    {
        if (pauseCanvas == null)
        {
            Debug.LogError("PauseMenu: Pause Canvas is not assigned!");
            return;
        }

        // Try to get the CanvasGroup on pauseCanvas
        pauseCanvasGroup = pauseCanvas.GetComponent<CanvasGroup>();
        if (pauseCanvasGroup == null)
        {
            // Add one if it doesn't exist
            pauseCanvasGroup = pauseCanvas.AddComponent<CanvasGroup>();
        }

        // Hide menu initially
        pauseCanvas.SetActive(false);
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
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
        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        Time.timeScale = 0f;

        if (musicSource != null)
            musicSource.Pause();

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;
        pauseCanvas.SetActive(false);

        Time.timeScale = 1f;

        if (musicSource != null)
            musicSource.UnPause();

        isPaused = false;
    }

    public void QuitToMainMenu(string mainMenuSceneName)
    {
        Time.timeScale = 1f;
        if (musicSource != null)
            musicSource.Stop();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
