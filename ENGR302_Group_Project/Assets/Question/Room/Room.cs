using System.Collections.Generic;
using DefaultNamespace;


public class Room
{
    private int _numTasks;
    private QuestionPool _questionPool;
    private List<Question> _questions;
    
    public Room(int numtasks, QuestionPool questionPool)
    {
        _numTasks = numtasks;
        _questionPool = questionPool;
    }

    public void Setup()
    {
        for (int i = 0; i < _numTasks; i++)
        {
            Question q = new Question(_questionPool);
            _questions.Add(q);
        }
       
    }

    public Question GetQuestion(int index)
    {
        return _questions[index];
    }


}