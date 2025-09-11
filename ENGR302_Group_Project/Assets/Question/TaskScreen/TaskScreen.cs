using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System;

public class TaskScreen : MonoBehaviour
{
    public GameObject buttonPrefab;  // Prefab of the button to be used
    public Transform buttonPanel;    // Parent object to hold the buttons (e.g. a UI Panel)
    private List<Button> buttons = new();   // List to hold buttons

    private List<string> answers = new();

    private Text mainText;
    private Question question;

    private Action onComplete;

    // Needs to be removed for when called by prefab.
    // void Start()
    // {
    //     mainText = GetComponent<Text>();

    //     // Generate buttons based on the specified number
    //     // Question q = JsonReader.GetAllQuestions()[qindex];
    //     JsonReader jr = new();
    //     question = jr.GetAllQuestions()[7];

    //     mainText.text = question.GetContext();
    //     CreateButtons(question.GetAnswer().Count);
    // }


    // Prefab runs this method and passes relevant question and action to complete when correct.
    // _question is the displayed question.
    // _onComplete is a lambda that gets run when correctly completed.
    void Execute(Question _question, Action _onComplete)
    {
        onComplete = _onComplete;
        mainText = GetComponent<Text>();
        question = _question;
        mainText.text = question.GetContext();
        CreateButtons(question.GetAnswer().Count);
    }


    private void ClearButtons()
    {
        // Clear any existing buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }
    }

    void CreateButtons(int buttonCount)
    {

        ClearButtons();

        for (int i = 0; i < buttonCount; i++)
        {
            // Create a new button from the prefab
            Button newButton = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
            Text bt = newButton.GetComponentInChildren<Text>();
            bt.text = question.GetAnswer()[i]; // Set button text
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
        panelRect.anchoredPosition = new Vector2(0, 50);  // Small offset above the bottom (optional)


        // Instantiate the button and get the necessary components
        Button complete_button = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
        Text button_text = complete_button.GetComponentInChildren<Text>();
        
        // Set the button text and font size
        button_text.text = "Submit";
        button_text.fontSize = 20;

        // Add listener to the button's onClick event
        complete_button.onClick.AddListener(() =>
        {
            checkComplete();
        });

    }

    void checkComplete()
    {
        
        if (answers.Count == question.GetAnswer().Count && question.IsCorrect(answers))
        {
            Debug.Log("Correct");
            ClearButtons();
            mainText.text = question.GetCompletion();
            if (onComplete == null) { return; }
            onComplete();
        }
        else
        {
            Debug.Log("Incorrect");
        }
    }

    void ButtonClicked(int index, Button b)
    {

        string answerText = question.GetAnswer()[index];
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
            answers.Remove(question.GetAnswer()[index]);
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
            answers.Add(question.GetAnswer()[index]);
        }

    }
}