using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;


[ System.Serializable]
public class Room
{
    private int _numTasks;
    private static QuestionPool _questionPool;
    private Dictionary<int,Question> _questions;
    private Dictionary<int, String> _questionAnswers;
    private String _name;
    
    private static List<String> _roomNames = new List<String>
    {
        "Room 1", "Room 2", "Room 3", "Room 4", "Room 5",
        "Room 6", "Room 7", "Room 8", "Room 9", "Room 10"
    };
    
    /// <summary>
    /// Initializes Room with name, number of tasks, and question pool.
    /// </summary>
    /// <param name="name">Name of the Room</param>
    /// <param name="numtasks">Number of tasks, matches the number of questions required from the question pool</param>
    /// <param name="questionPool">Holds all questions in the game</param>
    public Room(String name, int numtasks, QuestionPool questionPool)
    {
        _numTasks = numtasks;
        _questionPool = questionPool;
        _name = name;
    }
    
    /// <summary>
    /// Loads random questions from the question pool and adds them to the room.
    /// </summary>
    public void Setup()
    {
        _questions = new Dictionary<int, Question>();
        for (int i = 0; i < _numTasks; i++)
        {
            Question q = _questionPool.GetRandomQuestion();
            _questions.Add(q.GetId(), q);
        }
        GetAllAnswers();
       
    }
    
    /// <summary>
    /// Saves all answers from questions in the room to a dictionary.
    /// </summary>
    private void GetAllAnswers()
    {
        _questionAnswers = new Dictionary<int, String>();
        foreach(Question q in _questions.Values)
        {
            _questionAnswers.Add(q.GetId(), q.GetAnswer());
        }
    }
    
    /// <summary>
    /// Returns all answers from questions in the room.
    /// </summary>
    /// <returns>Collection of question answers</returns>
    public  Dictionary<int,string>.ValueCollection GetAllQuestionAnswers()
    {
        return _questionAnswers.Values;
    }
    
    /// <summary>
    /// Return queried question from the room.
    /// </summary>
    /// <param name="id">ID of question</param>
    /// <returns>Question</returns>
    public Question GetQuestion(int id)
    {
        return _questions[id];
    }
    
    /// <summary>
    /// Returns the name of the room.
    /// </summary>
    /// <returns>String</returns>
    public String GetName()
    {
        return _name;
    }
    
    
    public bool AllQuestionsAnswered()
    {
        foreach (Question q in _questions.Values)
        {
            if (!q.IsAnswered())
            {
                return false;
            }
        }
        return true;
    }
    
}