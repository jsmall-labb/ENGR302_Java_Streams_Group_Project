using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RoomButtonManager : MonoBehaviour
{
    [Header("UI Setup")]
    public GameObject buttonPrefab;        // your button prefab
    public Transform buttonPanel;          // panel where buttons will appear
    public RoomTeleporter roomTeleporter;  // reference to your teleporter

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
        // Find RoomTeleporter if not assigned
        if (roomTeleporter == null) 
            roomTeleporter = FindObjectOfType<RoomTeleporter>();

        if (roomTeleporter == null)
        {
            Debug.LogError("RoomButtonManager: No RoomTeleporter found in scene!");
            return;
        }

        // Update buttons for starting room
        UpdateMoveButtons();
    }

    void OnEnable()
    {
        // Subscribe to room changes if you want to add events to RoomTeleporter later
        UpdateMoveButtons();
    }

    // Call this whenever the player moves to a new room
    public void UpdateMoveButtons()
    {
        if (roomTeleporter == null) return;

        // Clear existing buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }

        // Get current room index (0-based)
        int currentRoomIndex = GetCurrentRoomIndex();

        // Get adjacent rooms from teleporter's adjacency dictionary
        int[] adjacentRooms = GetAdjacentRooms(currentRoomIndex);

        if (adjacentRooms == null) return;

        // Create button for each adjacent room
        foreach (int adjRoomIndex in adjacentRooms)
        {
            CreateRoomButton(adjRoomIndex);
        }
    }

    void CreateRoomButton(int roomIndex)
    {
        GameObject buttonObj = Instantiate(buttonPrefab, buttonPanel);
        buttonObj.SetActive(true);

        // Set button text
        string roomDisplayName = GetRoomDisplayName(roomIndex);

        // Try TextMeshPro first, then regular Text
        TextMeshProUGUI tmpText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.text = roomDisplayName;
        }
        else
        {
            Text regularText = buttonObj.GetComponentInChildren<Text>();
            if (regularText != null)
                regularText.text = roomDisplayName;
        }

        // Add click listener - this uses your teleporter's existing method
        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            int targetRoom = roomIndex + 1; // Convert to 1-based room number
            button.onClick.AddListener(() =>
            {
                TeleportToRoom(targetRoom);
            });
        }
    }

    void TeleportToRoom(int roomNumber)
    {
        // Use your teleporter's TryTeleport method
        roomTeleporter.TryTeleport(roomNumber);

        // Update buttons after teleportation
        // Add a small delay to ensure teleportation completes
        Invoke(nameof(UpdateMoveButtons), 0.1f);
    }

    // Helper method to get current room index from teleporter
    public int GetCurrentRoomIndex()
    {
        // Access the private field using reflection or make it public/protected
        // For now, we'll use GameManager as your teleporter does
        if (GameManager.Instance != null)
            return GameManager.Instance.currentRoom - 1; // Convert 1-based to 0-based

        return 0; // Default to room 1 (index 0)
    }

    // Get adjacent rooms from teleporter's adjacency data
    int[] GetAdjacentRooms(int currentRoomIndex)
    {
        // Since the adjacency dictionary is private, we'll recreate it
        // Alternatively, you could make it public/protected in RoomTeleporter
        Dictionary<int, int[]> adjacency = new Dictionary<int, int[]>()
        {
            {0, new int[]{1,5}},        // Room1 → Room2 & Room6
            {1, new int[]{0,5}},        // Room2 → Room1 & Room6
            {2, new int[]{3,5}},        // Room3 → Room4 & Room6
            {3, new int[]{2,6,4}},      // Room4 → Room3, Room7, Room5
            {4, new int[]{3}},          // Room5 → Room4
            {5, new int[]{0,1,2,6,7}},  // Room6 → Room1,2,3,7,8
            {6, new int[]{3,5,8}},      // Room7 → Room4,6,9
            {7, new int[]{5,8}},        // Room8 → Room6,9
            {8, new int[]{6,7,9}},      // Room9 → Room7,8,10
            {9, new int[]{8}}           // Room10 → Room9
        };

        return adjacency.ContainsKey(currentRoomIndex) ? adjacency[currentRoomIndex] : null;
    }

    // Get display name for room
    string GetRoomDisplayName(int roomIndex)
    {
        if (roomNames != null && roomIndex < roomNames.Length)
            return roomNames[roomIndex];

        return "Room " + (roomIndex + 1); // Default naming
    }

    // Public method to manually refresh buttons (useful for external calls)
    public void RefreshButtons()
    {
        UpdateMoveButtons();
    }

    // Debug method to test button creation
    [ContextMenu("Test Current Room Buttons")]
    public void TestCurrentRoomButtons()
    {
        Debug.Log("=== TESTING ROOM BUTTONS ===");
        int currentRoom = GetCurrentRoomIndex();
        Debug.Log($"Current Room Index: {currentRoom} (Room {currentRoom + 1})");

        int[] adjacent = GetAdjacentRooms(currentRoom);
        Debug.Log($"Adjacent room count: {adjacent.Length}");

        for (int i = 0; i < adjacent.Length; i++)
        {
            Debug.Log($"Adjacent Room {i}: Index {adjacent[i]} (Room {adjacent[i] + 1})");
        }

        UpdateMoveButtons();
    }
}