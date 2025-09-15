// PauseMenu.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseCanvas; // assign PauseCanvas here

    [Header("Input / Behavior")]
    public bool useTimeScalePause = true; // freeze time by default
    public MonoBehaviour[] disableOnPause; // list scripts (player controller, AI) to disable
    public bool unlockCursorOnPause = true;

    [Header("Optional")]
    public string mainMenuSceneName = "Main_Menu"; // if you want to load main menu

    bool isPaused = false;

    void Start()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        isPaused = false;
    }

    void Update()
    {
        // new Input System: toggle on Escape
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(true);

        if (useTimeScalePause) Time.timeScale = 0f;

        // disable specified scripts
        if (disableOnPause != null)
        {
            foreach (var m in disableOnPause)
            {
                if (m != null) m.enabled = false;
            }
        }

        if (unlockCursorOnPause)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseCanvas != null) pauseCanvas.SetActive(false);

        if (useTimeScalePause) Time.timeScale = 1f;

        if (disableOnPause != null)
        {
            foreach (var m in disableOnPause)
            {
                if (m != null) m.enabled = true;
            }
        }

        if (unlockCursorOnPause)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // or set your preferred state
        }

        isPaused = false;
    }

    // Hook this to "Main Menu" button OnClick if you want to leave to main menu
    public void GoToMainMenu()
    {
        // ensure game is unpaused before loading scene
        Time.timeScale = 1f;
        // optionally reset progress here or use your ToMainMenu/ChangeScene logic
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        // unpause to avoid frozen editor weirdness
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Safety: ensure timeScale restored if object destroyed
    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
