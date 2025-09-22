using System;
using System.Collections.Generic;

[System.Serializable]
public class QuestionPool
{
    private static Dictionary<int, Question> _questions;
    private static JsonReader _jsonReader = new JsonReader();
    
    public QuestionPool() {}
    
    /// <summary>
    /// Loads Questions from JsonReader
    /// </summary>
    public void LoadQuestions()
    {
        List<Question> questions = _jsonReader.GetAllQuestions();
        foreach (Question q in questions)
        {
            _questions.Add(q.GetId(), q);
        }
    }
    
    
    /// <summary>
    /// Returns a random question from the question pool.
    /// Then deletes the question from the pool.
    /// </summary>
    /// <returns>Question</returns>
    public Question GetRandomQuestion()
    {
        if (_questions.Count == 0)
        {
            return null;
            
        }
        var random = new Random();
        var randomIndex = random.Next(0, _questions.Count);
        var question = _questions[randomIndex];
        _questions.Remove(randomIndex);
        return question;
    }
}