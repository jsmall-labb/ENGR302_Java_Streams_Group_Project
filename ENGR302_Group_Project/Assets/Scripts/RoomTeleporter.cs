using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RoomTeleporter : MonoBehaviour
{
    public Transform[] teleportPoints; // Room1 → Room10
    public Camera mainCamera;          // The camera with AudioListener
    private int currentRoom = 0;

    // Adjacency list: which rooms are connected
    private Dictionary<int, int[]> adjacency = new Dictionary<int, int[]>()
    {
        {0, new int[]{1,5}},      // Room1 → Room2 & Room6
        {1, new int[]{0,5}},      // Room2 → Room1 & Room6
        {2, new int[]{3,5}},      // Room3 → Room4 & Room6
        {3, new int[]{2,6,4}},    // Room4 → Room3, Room7, Room5
        {4, new int[]{3}},        // Room5 → Room4
        {5, new int[]{0,1,2,6,7}},// Room6 → Room1,2,3,7,8
        {6, new int[]{3,5,8}},    // Room7 → Room4,6,9
        {7, new int[]{5,8}},      // Room8 → Room6,9
        {8, new int[]{6,7,9}},    // Room9 → Room7,8,10
        {9, new int[]{8}}         // Room10 → Room9
    };

    void Start()
    {
        TeleportToRoom(0); // Start in Room1
    }

    void Update()
    {
        // Number key input (0-9)
        if (Keyboard.current.digit1Key.wasPressedThisFrame) TryTeleport(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) TryTeleport(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) TryTeleport(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) TryTeleport(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) TryTeleport(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) TryTeleport(5);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) TryTeleport(6);
        if (Keyboard.current.digit8Key.wasPressedThisFrame) TryTeleport(7);
        if (Keyboard.current.digit9Key.wasPressedThisFrame) TryTeleport(8);
        if (Keyboard.current.digit0Key.wasPressedThisFrame) TryTeleport(9);
    }

    // Attempt to teleport to a target room
    void TryTeleport(int targetRoom)
    {
        if (adjacency[currentRoom] != null &&
            System.Array.Exists(adjacency[currentRoom], r => r == targetRoom))
        {
            TeleportToRoom(targetRoom);
        }
        else
        {
            Debug.LogWarning($"Cannot teleport to Room {targetRoom+1} from Room {currentRoom+1}");
        }
    }

    // Actually move the camera
    void TeleportToRoom(int roomIndex)
    {
        mainCamera.transform.position = teleportPoints[roomIndex].position;
        mainCamera.transform.rotation = teleportPoints[roomIndex].rotation;
        currentRoom = roomIndex;

        Debug.Log($"Teleported to Room {roomIndex+1}");
    }
}
