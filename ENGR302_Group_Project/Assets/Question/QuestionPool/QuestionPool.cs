using System;
using System.Collections.Generic;
using System.Diagnostics;

[System.Serializable]
public class QuestionPool
{
    private static Dictionary<int, Question> _questions = new();
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
 
        var keys = new List<int>(_questions.Keys);
        Random rand = new Random();

        int index = keys[rand.Next(keys.Count)];

        Question question = _questions[index];

        _questions.Remove(index);
        return question;
    }
}