using UnityEngine;
using System.Collections.Generic;

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

    void Awake()
    {
        // Ensure only one instance exists
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

    // Call this when a question is completed
    public void RecordQuestionResult(string questionText, bool wasCorrect, int attemptsNeeded)
    {
        totalQuestions++;
        
        if (wasCorrect)
        {
            correctAnswers++;
        }
        
        incorrectAttempts += (attemptsNeeded - 1); // Subtract 1 because the final attempt was correct

        // Store individual question result
        QuestionResult result = new QuestionResult
        {
            questionText = questionText,
            wasCorrect = wasCorrect,
            attemptsNeeded = attemptsNeeded
        };
        questionResults.Add(result);

        Debug.Log($"Question completed: {wasCorrect} | Attempts: {attemptsNeeded}");
        Debug.Log($"Overall accuracy: {GetAttemptBasedAccuracy():F1}%");
    }

    // Calculate overall accuracy percentage
    public float GetAccuracyPercentage()
    {
        if (totalQuestions == 0) return 0f;
        return (correctAnswers / (float)totalQuestions) * 100f;
    }

    // Calculate accuracy based on attempts (fewer attempts = better accuracy)
    public float GetAttemptBasedAccuracy()
    {
        if (totalQuestions == 0) return 0f;
        int totalAttempts = correctAnswers + incorrectAttempts;
        return (correctAnswers / (float)totalAttempts) * 100f;
    }

    // Get a letter grade based on accuracy
    public string GetLetterGrade()
    {
        float accuracy = GetAccuracyPercentage();
        if (accuracy >= 90) return "A";
        if (accuracy >= 80) return "B";
        if (accuracy >= 70) return "C";
        if (accuracy >= 60) return "D";
        return "F";
    }

    // Reset all stats (call at start of new game)
    public void ResetStats()
    {
        totalQuestions = 0;
        correctAnswers = 0;
        incorrectAttempts = 0;
        questionResults.Clear();
        Debug.Log("Game stats reset");
    }

    // Display final results
    public void DisplayFinalResults()
    {
        Debug.Log("=== FINAL RESULTS ===");
        Debug.Log($"Questions Completed: {totalQuestions}");
        Debug.Log($"Correct Answers: {correctAnswers}");
        Debug.Log($"Accuracy: {GetAccuracyPercentage():F1}%");
        Debug.Log($"Letter Grade: {GetLetterGrade()}");
    }
}

[System.Serializable]
public class QuestionResult
{
    public string questionText;
    public bool wasCorrect;
    public int attemptsNeeded;
}