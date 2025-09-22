using UnityEngine;
using TMPro;

public class TimerHUD : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timeText;

    private float elapsedTime = 0f; // total seconds

    void Update()
    {
        // Only count time when game is not paused
        if (Time.timeScale > 0f)
            elapsedTime += Time.deltaTime;

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    // Optional: Reset timer (e.g., when starting a new game)
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}
