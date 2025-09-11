using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RoomTeleporter : MonoBehaviour
{
    [Header("Setup")]
    public Transform[] teleportPoints; // Room1 → Room10
    public Camera mainCamera;          // Camera with AudioListener (optional in Inspector)

    private int currentRoomIndex = 0; // internal array index 0-9

    // Make adjacency public so RoomButtonManager can access it
    public Dictionary<int, int[]> adjacency = new Dictionary<int, int[]>()
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

    // Event for when room changes (optional - for button updates)
    public System.Action OnRoomChanged;

    // Public getter for current room
    public int CurrentRoomIndex => currentRoomIndex;
    public int CurrentRoomNumber => currentRoomIndex + 1;

    void Start()
    {
        // Auto-assign camera if missing
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("RoomTeleporter: No camera assigned and no Camera.main found in the scene!");
                return;
            }
        }

        // Restore current room from GameManager if exists
        if (GameManager.Instance != null && GameManager.Instance.currentRoom >= 1 && GameManager.Instance.currentRoom <= 10)
        {
            currentRoomIndex = GameManager.Instance.currentRoom - 1;
        }
        else
        {
            currentRoomIndex = 0; // default to Room1
        }

        TeleportToRoom(currentRoomIndex);
    }

    void Update()
    {
        // Number key input
        if (Keyboard.current.digit1Key.wasPressedThisFrame) TryTeleport(1);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) TryTeleport(2);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) TryTeleport(3);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) TryTeleport(4);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) TryTeleport(5);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) TryTeleport(6);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) TryTeleport(7);
        if (Keyboard.current.digit8Key.wasPressedThisFrame) TryTeleport(8);
        if (Keyboard.current.digit9Key.wasPressedThisFrame) TryTeleport(9);
        if (Keyboard.current.digit0Key.wasPressedThisFrame) TryTeleport(10); // 0 = Room10
    }

    // Make this public so buttons can call it
    public void TryTeleport(int targetRoomNumber)
    {
        int targetIndex = targetRoomNumber - 1;

        if (!adjacency.ContainsKey(currentRoomIndex))
            return;

        if (System.Array.Exists(adjacency[currentRoomIndex], r => r == targetIndex))
        {
            TeleportToRoom(targetIndex);
        }
        else
        {
            Debug.LogWarning($"Cannot teleport to Room {targetRoomNumber} from Room {currentRoomIndex + 1}");
        }
    }

    void TeleportToRoom(int roomIndex)
    {
        SetupTeleportPoints(); // ensure array is valid

        if (teleportPoints.Length <= roomIndex || teleportPoints[roomIndex] == null)
        {
            Debug.LogError($"RoomTeleporter: Teleport point for Room {roomIndex + 1} is missing!");
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        mainCamera.transform.position = teleportPoints[roomIndex].position;
        mainCamera.transform.rotation = teleportPoints[roomIndex].rotation;

        currentRoomIndex = roomIndex;

        if (GameManager.Instance != null)
            GameManager.Instance.currentRoom = roomIndex + 1;

        Debug.Log($"Teleported to Room {roomIndex + 1}");

        // Trigger room changed event
        OnRoomChanged?.Invoke();
    }

    private void SetupTeleportPoints()
    {
        if (teleportPoints != null && teleportPoints.Length > 0)
            return; // Already set

        GameObject parent = GameObject.Find("TeleportPos");
        if (parent == null)
        {
            Debug.LogError("RoomTeleporter: TeleportPos object not found!");
            return;
        }

        List<Transform> points = new List<Transform>();
        foreach (Transform child in parent.transform)
            points.Add(child);

        // Sort by name: Room1 → Room10
        points.Sort((a, b) => a.name.CompareTo(b.name));

        teleportPoints = points.ToArray();
    }

    void OnEnable()
    {
        // 1. Re-assign MainCamera if missing
        if (mainCamera == null)
            mainCamera = Camera.main;

        // 2. Re-assign teleport points if missing
        if (teleportPoints == null || teleportPoints.Length == 0)
        {
            GameObject parent = GameObject.Find("TeleportPos");
            if (parent != null)
            {
                List<Transform> tempList = new List<Transform>();
                foreach (Transform child in parent.transform)
                    tempList.Add(child);

                tempList.Sort((a, b) => a.name.CompareTo(b.name));
                teleportPoints = tempList.ToArray();
            }
            else
            {
                Debug.LogError("RoomTeleporter: TeleportPos object not found!");
            }
        }

        // 3. Move camera to current room
        if (GameManager.Instance != null)
            TeleportToRoom(GameManager.Instance.currentRoom - 1); // convert 1-based to 0-based
    }
}
