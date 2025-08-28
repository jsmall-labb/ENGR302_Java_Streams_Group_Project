using UnityEngine;

public class quitButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void doQuitAction(){
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
