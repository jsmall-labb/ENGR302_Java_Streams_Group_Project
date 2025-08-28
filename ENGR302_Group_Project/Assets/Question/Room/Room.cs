using System;
using System.Collections.Generic;

public class Room
{
    private int _numTasks;
    private QuestionPool _questionPool;
    private Dictionary<int,Question> _questions;
    private Dictionary<int, String> _questionAnswers;
    
    public Room(int numtasks, QuestionPool questionPool)
    {
        _numTasks = numtasks;
        _questionPool = questionPool;
    }

    public void Setup()
    {
        _questions = new Dictionary<int, Question>();
        for (int i = 0; i < _numTasks; i++)
        {
            Question q = _questionPool.GetRandomQuestion();
            _questions.Add(q.getId(), q);
        }
        GetAllAnswers();
       
    }

    private void GetAllAnswers()
    {
        _questionAnswers = new Dictionary<int, String>();
        foreach(Question q in _questions.Values)
        {
            _questionAnswers.Add(q.getId(), q.GetAnswer());
        }
    }

    public Question GetQuestion(int id)
    {
        return _questions[id];
    }
    
}