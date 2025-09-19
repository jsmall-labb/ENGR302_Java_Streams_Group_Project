using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PersistentTimer : MonoBehaviour
{
    public static PersistentTimer Instance;

    [SerializeField] private TextMeshProUGUI timeText; // assign in GameScene

    private float elapsedTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign TextMeshProUGUI if needed
        if (timeText == null)
        {
            timeText = FindObjectOfType<TextMeshProUGUI>();
        }

        // Optionally reset timer when starting GameScene
        if (scene.name == "GameScene")
        {
            elapsedTime = 0f;
        }
    }

    void Update()
    {
        if (Time.timeScale > 0f)
            elapsedTime += Time.deltaTime;

        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }
}
