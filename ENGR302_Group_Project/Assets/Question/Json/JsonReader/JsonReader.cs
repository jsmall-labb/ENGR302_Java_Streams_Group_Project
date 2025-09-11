
using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<Question> GetAllQuestions()
    {
        if (returnQuestions.Count != 0) { return returnQuestions; }
        if (json == null) { Debug.Log("JSON file could not be loaded."); }

        JSONLoadedQuestions jsonLoadedQuestions = JsonUtility.FromJson<JSONLoadedQuestions>(json.text);

        foreach (JSONQuestion jq in jsonLoadedQuestions.questions)
        {
            // Temporary solution until a different method is chosen.
            string answerText = "";
            foreach (string s in jq.answer)
            {
                answerText += s;
            }

            returnQuestions.Add(new Question(currentID, jq.room, jq.text, answerText, jq.completed_text));
            currentID++;
        }

        return returnQuestions;

    }
}
