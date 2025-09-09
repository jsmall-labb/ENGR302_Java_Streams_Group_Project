using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentRoom = 1;
    public int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Add this method
    public void ResetProgress()
    {
        currentRoom = 1;
        score = 0;
        Debug.Log("GameManager: Progress has been reset.");
    }
}
