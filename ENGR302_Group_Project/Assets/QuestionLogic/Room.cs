using System.Collections.Generic;
using DefaultNamespace;


public class Room
{
    private int numTasks;
    private QuestionPool questionPool;
    private List<Question> questions;
    
    public Room(int numtasks, QuestionPool questionPool)
    {
        this.numTasks = numtasks;
        this.questionPool = questionPool;
    }

    public void Setup()
    {
        for (int i = 0; i < numTasks; i++)
        {
            Question q = new Question(questionPool);
            questions.Add(q);
        }
       
    }

    public Question GetQuestion(int index)
    {
        return questions.
    }


}