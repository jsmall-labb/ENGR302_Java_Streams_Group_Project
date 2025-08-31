using System;
using System.Collections.Generic;


[ System.Serializable]
public class Map
{
    //TODO will need to be a dictionary of String, Room with String being Room name
    private List<Room> _rooms;
    private static QuestionPool _questionPool;
    //TODO going to need to be a dictionary of rooms with int, String for num of tasks matching the room name.
    private static List<String> _roomNames = new List<String>
     {
          "Room 1", "Room 2", "Room 3", "Room 4", "Room 5",
         "Room 6", "Room 7", "Room 8", "Room 9", "Room 10"
     };

    /// <summary>
    /// Initializes the question pool and room list.
    /// </summary>
    public Map()
    {
        _questionPool = new QuestionPool();
        _rooms = new List<Room>();
    }
    
    /// <summary>
    /// Loads question pool from JsonReader and sets up rooms
    /// saving them in a list of rooms.
    /// </summary>
    public void Setup()
    {
        
        _questionPool.LoadQuestions();
        
        //TODO Will need to change
        foreach (var name in  _roomNames)
        {
            _rooms.Add(new Room(name, 5, _questionPool));
        }
        
    }
    
    /// <summary>
    /// Cycles through all rooms invoking their setup method.
    /// Which will load questions and answers from the question pool to the amount specified
    /// in the rooms number of tasks.
    /// </summary>
    public void LoadRoom()
    {
        foreach (var room in _rooms)
        {
            room.Setup();
        }
    }
    
    /// <summary>
    /// Return a queried room from a list of rooms.
    /// </summary>
    /// <param name="index">The index value of room Will be changed to String being Room
    /// name in the future</param>
    /// <returns>Room</returns>
    public Room GetRoom(int index)
    {
        return _rooms[index];
    }
    

    /// <returns>Full list of Rooms</returns>
    public List<Room> GetRooms()
    {
        return _rooms;
    }
}