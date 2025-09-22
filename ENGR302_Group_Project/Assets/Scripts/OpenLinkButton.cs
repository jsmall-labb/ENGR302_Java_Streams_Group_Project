using UnityEngine;

public class OpenSurveyLink : MonoBehaviour
{
    [SerializeField] private string url = "https://docs.google.com/forms/d/e/1FAIpQLSf-lTboba-RxY1BlckH6WXWLMvuHGZX8TszZMITbruvjj1V1A/viewform?usp=sharing&ouid=114197202088013331987"; // Replace with your form link

    public void OpenLink()
    {
        Debug.Log("Opening survey link: " + url);
        Application.OpenURL(url);
    }
}
