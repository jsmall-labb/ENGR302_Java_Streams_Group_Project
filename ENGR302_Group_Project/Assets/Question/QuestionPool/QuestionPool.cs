using System;
using System.Collections.Generic;

public class QuestionPool
{
    private Dictionary<int, Question> _questions;
    private String SomeJSONStuffHERE; // Unsure of format at current state will change later. This will hold json objects 
    //When load questions called will get json objects.
    //When setup questions called will transform from json objects into actual dictionary of questions
    
    public QuestionPool()
    {
        
    }

    public Question SetupQuestion()
    {
        return null;
    }

    public void LoadQuestions()
    {
        
    }

    public Question GetRandomQuestion()
    {
        return null;
    }
}