using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    private static GameStatsManager instance;
    public static GameStatsManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameStatsManager");
                instance = go.AddComponent<GameStatsManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [Header("Game Statistics")]
    public int totalQuestions = 0;
    public int correctAnswers = 0;
    public int incorrectAttempts = 0;

    [Header("Individual Question Stats")]
    public List<QuestionResult> questionResults = new List<QuestionResult>();

    [Header("Timing")]
    public float elapsedTime = 0f; // seconds

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Update elapsed time continuously
        if (Time.timeScale > 0f)
            elapsedTime += Time.deltaTime;
    }

    public void RecordQuestionResult(string questionText, bool wasCorrect, int attemptsNeeded)
    {
        totalQuestions++;
        if (wasCorrect) correctAnswers++;
        incorrectAttempts += (attemptsNeeded - 1);

        questionResults.Add(new QuestionResult
        {
            questionText = questionText,
            wasCorrect = wasCorrect,
            attemptsNeeded = attemptsNeeded
        });
    }

    public float GetAccuracyPercentage()
    {
        if (totalQuestions == 0) return 0f;
        return (correctAnswers / (float)totalQuestions) * 100f;
    }

    public float GetAttemptBasedAccuracy()
    {
        if (totalQuestions == 0) return 0f;
        int totalAttempts = correctAnswers + incorrectAttempts;
        return (correctAnswers / (float)totalAttempts) * 100f;
    }

    public string GetLetterGrade()
    {
        float accuracy = GetAccuracyPercentage();
        if (accuracy >= 90) return "A";
        if (accuracy >= 80) return "B";
        if (accuracy >= 70) return "C";
        if (accuracy >= 60) return "D";
        return "F";
    }

    public void ResetStats()
    {
        totalQuestions = 0;
        correctAnswers = 0;
        incorrectAttempts = 0;
        questionResults.Clear();
        elapsedTime = 0f;
    }

    // Method to pass summary to UI
    public void FillSummaryUI(TextMeshProUGUI accuracyText, TextMeshProUGUI gradeText, TextMeshProUGUI timeText)
    {
        if (accuracyText != null)
            accuracyText.text = $"Accuracy: {GetAccuracyPercentage():F1}%";

        if (gradeText != null)
            gradeText.text = $"Grade: {GetLetterGrade()}";

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}

[System.Serializable]
public class QuestionResult
{
    public string questionText;
    public bool wasCorrect;
    public int attemptsNeeded;
}
