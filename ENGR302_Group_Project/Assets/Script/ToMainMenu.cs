using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading

public class ToMainMenu : MonoBehaviour
{
    // Public so you can set the scene name in the Inspector
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
