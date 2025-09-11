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
    public int numberOfButtons = 5;  // Number of buttons you want to generate
    private List<Button> buttons = new List<Button>();   // List to hold buttons
    private List<string> clickOrder = new List<string>(); // List to track button click order
    private int qindex = 7;
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();

        // Generate buttons based on the specified number
        Question q = JsonReader.GetAllQuestions()[qindex];
        text.text = q.GetContext();
        CreateButtons(q.GetAnswer().Count);
    }

    void CreateButtons(int buttonCount)
    {   



    
        // Clear any existing buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }

        // float am = Screen.width / buttonCount;
        for (int i = 0; i < buttonCount; i++)
        {
            // Create a new button from the prefab
            Button newButton = Instantiate(buttonPrefab, buttonPanel).GetComponent<Button>();
            // newButton.GetComponentInChildren<Text>().text = "Button " + (i + 1); // Set button text
            newButton.GetComponentInChildren<Text>().text = JsonReader.GetAllQuestions()[qindex].GetAnswer()[i]; // Set button text
            Text tc = newButton.GetComponentInChildren<Text>();
            tc.fontSize = 20;   
            // newButton.transform.position = new Vector3(Screen.width / (i+1), Screen.height / 2, 0);

            int index = i;  // Capture the index to avoid closure issues
            newButton.onClick.AddListener(() => ButtonClicked(index, newButton));  // Add listener for button click
            buttons.Add(newButton);
        }
    }

    void ButtonClicked(int index, Button b)
    {
        // Track the order of clicks
        // clickOrder.Add("Button " + (index + 1));

        // Update the UI text to show the current order of clicks
        // orderText.text = "Click Order: " + string.Join(", ", clickOrder);
        string text_ = JsonReader.GetAllQuestions()[qindex].GetAnswer()[index];
        text_ = text_.Substring(0, text_.Length - 2);

        Debug.Log("Button : " + text_);
        if (text.text.Contains(text_))
        { // Remove inserted statement
            bool replaced = false;
            text.text = Regex.Replace(text.text, text_, m =>
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
        }
        else
        {
            bool replaced = false;
            text.text = Regex.Replace(text.text, "___", m =>
            { // Insert statement
                if (!replaced)
                {
                    replaced = true;
                    return "<color=blue>" + text_ + "</color>";
                }
                return m.Value; // keep original
            });
            Image i = b.GetComponent<Image>();
            Color c = i.color;
            c.a = 0.25f;
            i.color = c;
        }




    }
}