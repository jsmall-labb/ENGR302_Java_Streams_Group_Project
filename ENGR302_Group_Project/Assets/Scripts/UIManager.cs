using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject pauseCanvas;
    public GameObject questionCanvas;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseCanvas != null)
            pauseCanvas.SetActive(show);

        if (show)
            HideQuestion(); // auto-hide questions when pausing
    }

    public void ShowQuestion()
    {
        if (questionCanvas != null)
            questionCanvas.SetActive(true);

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false); // hide pause while question is up
    }

    public void HideQuestion()
    {
        if (questionCanvas != null)
            questionCanvas.SetActive(false);
    }
}
