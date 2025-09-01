using System;
using System.Collections.Generic;

[System.Serializable]
public class QuestionPool
{
    private Dictionary<int, Question> _questions;
    private static JsonReader _jsonReader = new JsonReader();
    
    public QuestionPool() {}
    
    /// <summary>
    /// Loads Questions from JsonReader
    /// </summary>
    public void LoadQuestions()
    {
        List<Question> questions = JsonReader.GetAllQuestions();
        foreach (Question q in questions)
        {
            _questions.Add(q.getId(), q);
        }
    }
    
    //TODO going to need a condition that if no questions are left it will return null which will need to be handled.
    /// <summary>
    /// Returns a random question from the question pool.
    /// Then deletes the question from the pool.
    /// </summary>
    /// <returns>Question</returns>
    public Question GetRandomQuestion()
    {
        var random = new Random();
        var randomIndex = random.Next(0, _questions.Count);
        var question = _questions[randomIndex];
        _questions.Remove(randomIndex);
        return question;
    }
}