[System.Serializable]  // allows you to see this class in the Inspector
public class Room
{
    public string roomName;
    public Room[] adjacentRooms;  // rooms you can move to
}