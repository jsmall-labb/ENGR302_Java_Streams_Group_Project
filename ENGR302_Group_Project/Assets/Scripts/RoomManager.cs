using UnityEngine;
using UnityEngine.UI;
using TMPro; // if using TextMeshPro

public class RoomManager : MonoBehaviour
{
    public Room currentRoom;               // the room the player is in
    public GameObject buttonPrefab;        // your button prefab
    public Transform buttonPanel;          // panel where buttons will appear

    public Room[] allRooms;  // assign all your rooms here

    void Start()
    {
        Room roomA = new Room() { roomName = "Room A" };
        Room roomB = new Room() { roomName = "Room B" };
        Room roomC = new Room() { roomName = "Room C" };

        // set adjacent rooms
        roomA.adjacentRooms = new Room[] { roomB, roomC };
        roomB.adjacentRooms = new Room[] { roomA };
        roomC.adjacentRooms = new Room[] { roomA };

        currentRoom = roomA;
        UpdateMoveButtons();
    }



    // call this whenever the player enters a room
    public void UpdateMoveButtons()
    {
        // first, clear old buttons
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }

        // create a button for each adjacent room
        foreach (Room adj in currentRoom.adjacentRooms)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonPanel);
            buttonObj.SetActive(true); // make sure it's visible

            // set button text
            TextMeshProUGUI tmp = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = adj.roomName;
            else
                buttonObj.GetComponentInChildren<Text>().text = adj.roomName;

            // add click listener
            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                MoveToRoom(adj);
            });
        }
    }

    void MoveToRoom(Room newRoom)
    {
        currentRoom = newRoom;
        UpdateMoveButtons();
        // here you can also teleport the player or update the camera
        Debug.Log("Moved to: " + currentRoom.roomName);
    }
}