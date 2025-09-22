using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    public GameObject pauseCanvas;
    public GameObject questionCanvas;
    public GameObject summaryCanvas;

    [Header("Summary UI Texts")]
    public TextMeshProUGUI accuracyText; // assign in Inspector
    public TextMeshProUGUI timeText;     // assign in Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowSummary()
    {
        if (summaryCanvas != null)
            summaryCanvas.SetActive(true);

        if (accuracyText != null)
        {
            float accuracy = GameStatsManager.Instance.GetAccuracyPercentage();
            accuracyText.text = $"Accuracy: {accuracy:F1}%";
        }

        // timeText can still be handled by your TimerHUD script
        if (questionCanvas != null) questionCanvas.SetActive(false);
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
    }
}
