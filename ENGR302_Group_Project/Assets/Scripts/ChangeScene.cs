using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public bool resetProgressOnLoad = false; // toggle in Inspector

    public void LoadScene()
    {
        if (resetProgressOnLoad && GameManager.Instance != null)
        {
            GameManager.Instance.ResetProgress();
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
