using UnityEngine;


public class MapManager : MonoBehaviour
{
    private static Map _map;
    public static MapManager Instance;
        
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

    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        _map = new Map();
        _map.Setup();
        _map.LoadRoom();
    }
        
    public Map GetMap()
    {
        return _map;
    }

}