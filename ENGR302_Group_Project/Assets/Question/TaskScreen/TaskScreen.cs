using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class TaskScreen : MonoBehaviour
{
    public GameObject buttonPrefab;  // Prefab of the button to be used
    public GameObject textPrefab;
    public Transform buttonPanel;    // Parent object to hold the buttons (e.g. a UI Panel)
    private List<Button> buttons = new();   // List to hold buttons

    private List<string> answers = new();

    private List<string> allAnswers = new();

    private Text mainText;
    private Question question;

    private Action onComplete;

    private int incorrectAttempts = 0;

    // Prefab runs this method and passes relevant question and action to complete when correct.
    // _question is the displayed question.
    // _onComplete is a lambda that gets run when correctly completed.
    public void Execute(Question _question, Action _onComplete)
    {

        onComplete = _onComplete;
        question = _question;
        ClearButtons();

        GameObject textObject = Instantiate(textPrefab, GetComponent<RectTransform>());
        mainText = textObject.GetComponent<Text>();
        textObject.transform.SetAsFirstSibling();
        mainText.text = question.GetContext();

        mainText.text = Regex.Replace(mainText.text, "___", "<color=blue>___</color>");


        textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2, 20);

        CreateButtons();

    }


    private void ClearButtons()
    {
        // Clear any existing buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateButtons()
    {
        allAnswers.AddRange(question.GetAnswer());
        allAnswers.AddRange(question.GetDecoyAnswer());

        System.Random rand = new();

        allAnswers = allAnswers.OrderBy(x => rand.Next()).ToList();


        for (int i = 0; i < allAnswers.Count; i++)
        {
            // Create a new button from the prefab
            Button newButton = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
            Text bt = newButton.GetComponentInChildren<Text>();
            bt.text = allAnswers[i]; // Set button text
            bt.fontSize = 20;

            int index = i;  // Capture the index to avoid closure issues
            newButton.onClick.AddListener(() => ButtonClicked(index, newButton));  // Add listener for button click
            buttons.Add(newButton);
        }



        // Position the buttonPanel at the bottom-center of the canvas
        RectTransform panelRect = buttonPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0);   // Anchor to bottom-center of the canvas
        panelRect.anchorMax = new Vector2(0.5f, 0);   // Anchor to bottom-center of the canvas
        panelRect.pivot = new Vector2(0.5f, 0);       // Set pivot to center at the bottom
        // panelRect.anchoredPosition = new Vector2(0, 50);  // Small offset above the bottom (optional)


        // Instantiate the button and get the necessary components
        Button complete_button = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
        Text button_text = complete_button.GetComponentInChildren<Text>();

        // Set the button text and font size
        button_text.text = "<color=red>Submit</color>";
        button_text.fontSize = 20;
        // Add listener to the button's onClick event
        complete_button.onClick.AddListener(() =>
        {
            checkComplete();
        });


        Button exit_button = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
        Text exit_text = exit_button.GetComponentInChildren<Text>();
        exit_text.text = "<color=red>Close</color>";
        exit_text.fontSize = 20;

        exit_button.onClick.AddListener(() =>
        {
            onComplete();
        });


    }

    void checkComplete()
    {
        bool isCorrect = answers.Count == question.GetAnswer().Count && question.IsCorrect(answers);


        if (answers.Count == question.GetAnswer().Count && question.IsCorrect(answers))
        {
            Debug.Log("Correct");
            ClearButtons();
            mainText.text = question.GetCompletion();
            int totalAttempts = incorrectAttempts + 1; // Adding 1 attempt for the correct attempt
            GameStatsManager.Instance.RecordQuestionResult(
                question.GetContext(),    // Question text
                true,                     // Was correct
                totalAttempts           // Total attempts needed
            );

            if (onComplete == null) { return; }
            onComplete();
        }
        else
        {
            Debug.Log("Incorrect");
            incorrectAttempts++;
        }
    }

    void ButtonClicked(int index, Button b)
    {

        string answerText = allAnswers[index];
        answerText = answerText.Substring(0, answerText.Length - 2);

        // Debug.Log("Button : " + answerText);
        if (mainText.text.Contains(answerText))
        { // Remove inserted statement
            bool replaced = false;
            mainText.text = Regex.Replace(mainText.text, answerText, m =>
            {
                if (!replaced)
                {
                    replaced = true;
                    return "___";
                }
                return m.Value; // keep original
            });
            Image i = b.GetComponent<Image>();
            Color c = i.color;
            c.a = 1f;
            i.color = c;
            answers.Remove(allAnswers[index]);
        }
        else
        {
            bool replaced = false;
            mainText.text = Regex.Replace(mainText.text, "___", m =>
            { // Insert statement
                if (!replaced)
                {
                    replaced = true;
                    return "<color=blue>" + answerText + "</color>";
                }
                return m.Value; // keep original
            });
            Image i = b.GetComponent<Image>();
            Color c = i.color;
            c.a = 0.25f;
            i.color = c;
            answers.Add(allAnswers[index]);
        }

    }
}