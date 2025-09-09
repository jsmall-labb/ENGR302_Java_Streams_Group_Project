using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <- New Input System

public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene names")]
    public string pauseMenuScene = "PauseMenu";

    void Update()
    {
        // Check for ESC key with new Input System
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(pauseMenuScene);
        }
    }
}
