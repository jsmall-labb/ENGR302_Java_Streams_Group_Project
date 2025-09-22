using System;
using System.Collections.Generic;

public static class QuestionDatabase
{
    private static List<Question> questions;
    
    public static List<Question> All
    {
        get
        {
            if (questions == null)
            {
                JsonReader jr = new JsonReader();
                questions = jr.GetAllQuestions();
            }
            return questions;
        }
    }
}
