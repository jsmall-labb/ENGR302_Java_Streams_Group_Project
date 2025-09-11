
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// class QuestionList
// {
//     public List<Question> questions;
// }

[System.Serializable]
public class JSONQuestion
{
    public string room;
    public string text;
    public List<string> answer;
    public string completed_text;
}

[System.Serializable]
public class JSONLoadedQuestions
{
    public List<JSONQuestion> questions;
}

public class JsonReader
{
    private static TextAsset json = Resources.Load<TextAsset>("questions");
    private static int currentID = 0;
    private static List<Question> returnQuestions = new();

    public static List<Question> GetAllQuestions()
    {
        if (returnQuestions.Count != 0) { return returnQuestions; }
        if (json == null) { Debug.Log("JSON file could not be loaded."); }

        JSONLoadedQuestions jsonLoadedQuestions = JsonUtility.FromJson<JSONLoadedQuestions>(json.text);

        foreach (JSONQuestion jq in jsonLoadedQuestions.questions)
        {

            List<string> answers = new();
            foreach (string answer in jq.answer)
            {
                answers.Add(answer);
            }

            returnQuestions.Add(new Question(currentID, jq.room, jq.text, answers, jq.completed_text));
            currentID++;
        }

        return returnQuestions;

    }
}
