using UnityEngine;
using TMPro;

public class SummaryDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI accuracyText; // assign in Inspector
    public TextMeshProUGUI timeText;     // assign in Inspector

    void Start()
    {
        UpdateSummary();
    }

    void UpdateSummary()
    {
        if (GameStatsManager.Instance != null && accuracyText != null)
        {
            float accuracy = GameStatsManager.Instance.GetAttemptBasedAccuracy();
            accuracyText.text = $"{accuracy:F1}%";
        }

        TimerHUD timer = FindObjectOfType<TimerHUD>();
        if (timer != null && timeText != null)
        {
            int minutes = Mathf.FloorToInt(timer.ElapsedTime / 60f);
            int seconds = Mathf.FloorToInt(timer.ElapsedTime % 60f);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
