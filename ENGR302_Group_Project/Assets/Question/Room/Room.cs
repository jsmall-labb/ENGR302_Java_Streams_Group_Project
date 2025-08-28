using System.Collections.Generic;

public class Room
{
    private int _numTasks;
    private QuestionPool _questionPool;
    private Dictionary<int,Question> _questions;
    
    public Room(int numtasks, QuestionPool questionPool)
    {
        _numTasks = numtasks;
        _questionPool = questionPool;
    }

    public void Setup()
    {
        for (int i = 0; i < _numTasks; i++)
        {
            Question q = _questionPool.GetRandomQuestion();
            _questions.Add(q.getId(), q);
        }
       
    }

    public Question GetQuestion(int id)
    {
        return _questions[id];
    }
    
}