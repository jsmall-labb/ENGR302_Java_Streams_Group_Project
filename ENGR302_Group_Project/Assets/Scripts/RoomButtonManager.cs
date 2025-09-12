using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RoomButtonManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject buttonPrefab;        
    public Transform buttonPanel;          
    public RoomTeleporter roomTeleporter;  
    
    [Header("Room Names (Optional)")]
    public string[] roomNames = new string[]
    {
        "Entry Hall",    // Room 1
        "Library",       // Room 2  
        "Kitchen",       // Room 3
        "Dining Room",   // Room 4
        "Storage",       // Room 5
        "Main Hall",     // Room 6
        "Bedroom",       // Room 7
        "Bathroom",      // Room 8
        "Attic",         // Room 9
        "Secret Room"    // Room 10
    };

    void Start()
    {
        if (roomTeleporter == null)
            roomTeleporter = FindObjectOfType<RoomTeleporter>();
            
        if (roomTeleporter == null)
        {
            Debug.LogError("RoomButtonManager: No RoomTeleporter found in scene!");
            return;
        }

        SetupButtonPanelLayout();
        UpdateMoveButtons();
    }

    void OnEnable()
    {
        UpdateMoveButtons();
    }

    public void UpdateMoveButtons()
    {
        if (roomTeleporter == null) return;

        // Clear existing buttons
        foreach (Transform child in buttonPanel)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        int currentRoomIndex = GetCurrentRoomIndex();
        int[] adjacentRooms = GetAdjacentRooms(currentRoomIndex);
        
        Debug.Log("Current room: " + (currentRoomIndex + 1) + ", Adjacent rooms: " + adjacentRooms.Length);
        
        if (adjacentRooms == null || adjacentRooms.Length == 0) 
        {
            Debug.LogWarning("No adjacent rooms found for room " + (currentRoomIndex + 1));
            return;
        }

        for (int i = 0; i < adjacentRooms.Length; i++)
        {
            CreateRoomButton(adjacentRooms[i]);
        }
        
        Debug.Log("Created " + adjacentRooms.Length + " buttons for adjacent rooms");
    }

    void CreateRoomButton(int roomIndex)
    {
        if (buttonPrefab == null || buttonPanel == null) return;

        GameObject buttonObj = Instantiate(buttonPrefab, buttonPanel);
        buttonObj.SetActive(true);

        // Manual positioning
        int buttonCount = buttonPanel.childCount - 1;
        float buttonWidth = 130f;
        float spacing = 20f;
        float startX = -(buttonCount * (buttonWidth + spacing)) / 2f;
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            buttonRect.sizeDelta = new Vector2(buttonWidth, 50f);
            buttonRect.anchoredPosition = new Vector2(startX + (buttonCount * (buttonWidth + spacing)), 0f);
        }

        // Set button text
        string roomDisplayName = GetRoomDisplayName(roomIndex);
        
        TextMeshProUGUI tmpText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = roomDisplayName;
            tmpText.fontSize = 16f;
        }
        else
        {
            Text regularText = buttonObj.GetComponentInChildren<Text>();
            if (regularText != null)
            {
                regularText.text = roomDisplayName;
                regularText.fontSize = 16;
            }
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            int targetRoom = roomIndex + 1;
            button.onClick.AddListener(() => { TeleportToRoom(targetRoom); });
        }
    }

    void TeleportToRoom(int roomNumber)
    {
        if (roomTeleporter != null)
        {
            roomTeleporter.TryTeleport(roomNumber);
            Invoke("UpdateMoveButtons", 0.1f);
        }
    }

    int GetCurrentRoomIndex()
    {
        if (GameManager.Instance != null)
            return GameManager.Instance.currentRoom - 1;
        
        return 0;
    }

    int[] GetAdjacentRooms(int currentRoomIndex)
    {
        if (roomTeleporter != null && roomTeleporter.adjacency != null && roomTeleporter.adjacency.ContainsKey(currentRoomIndex))
        {
            return roomTeleporter.adjacency[currentRoomIndex];
        }
        
        Debug.LogWarning("No adjacency data found for room index " + currentRoomIndex);
        return new int[0];
    }

    string GetRoomDisplayName(int roomIndex)
    {
        if (roomNames != null && roomIndex < roomNames.Length)
            return roomNames[roomIndex];
        
        return "Room " + (roomIndex + 1);
    }

    void SetupButtonPanelLayout()
    {
        if (buttonPanel == null) return;

        // Remove existing layout components
        HorizontalLayoutGroup hLayout = buttonPanel.GetComponent<HorizontalLayoutGroup>();
        if (hLayout != null) 
        {
            if (Application.isPlaying)
                Destroy(hLayout);
            else
                DestroyImmediate(hLayout);
        }
        
        VerticalLayoutGroup vLayout = buttonPanel.GetComponent<VerticalLayoutGroup>();
        if (vLayout != null) 
        {
            if (Application.isPlaying)
                Destroy(vLayout);
            else
                DestroyImmediate(vLayout);
        }
        
        GridLayoutGroup gLayout = buttonPanel.GetComponent<GridLayoutGroup>();
        if (gLayout != null) 
        {
            if (Application.isPlaying)
                Destroy(gLayout);
            else
                DestroyImmediate(gLayout);
        }

        ContentSizeFitter fitter = buttonPanel.GetComponent<ContentSizeFitter>();
        if (fitter != null) 
        {
            if (Application.isPlaying)
                Destroy(fitter);
            else
                DestroyImmediate(fitter);
        }

        Debug.Log("Removed layout groups - using manual positioning");
    }

    public void RefreshButtons()
    {
        UpdateMoveButtons();
    }

    [ContextMenu("Test Current Room Buttons")]
    public void TestCurrentRoomButtons()
    {
        Debug.Log("=== TESTING ROOM BUTTONS ===");
        int currentRoom = GetCurrentRoomIndex();
        Debug.Log("Current Room Index: " + currentRoom + " (Room " + (currentRoom + 1) + ")");
        
        int[] adjacent = GetAdjacentRooms(currentRoom);
        Debug.Log("Adjacent room count: " + adjacent.Length);
        
        for (int i = 0; i < adjacent.Length; i++)
        {
            Debug.Log("Adjacent Room " + i + ": Index " + adjacent[i] + " (Room " + (adjacent[i] + 1) + ")");
        }
        
        UpdateMoveButtons();
    }
}