using UnityEngine;

public class OpenSurveyLink : MonoBehaviour
{
    [SerializeField] private string url = "https://forms.gle/yourGoogleFormLink"; // Replace with your form link

    public void OpenLink()
    {
        Debug.Log("Opening survey link: " + url);
        Application.OpenURL(url);
    }
}
