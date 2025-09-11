using System;
using System.Collections.Generic;


[ System.Serializable]
public class Map
{

    private Dictionary<String, Room> _roomDictionary;
    
    private static QuestionPool _questionPool;
    //Dictionary of rooms with int (room number) and String (room name)
    private static Dictionary<int, String> _roomNamesAndTaskNum = new Dictionary<int, String>
    {
        {1, "Room 1"}, {2, "Room 2"}, {3, "Room 3"}, {4, "Room 4"}, {5, "Room 5"},
        {6, "Room 6"}, {7, "Room 7"}, {8, "Room 8"}, {9, "Room 9"}, {10, "Room 10"}
    };

    /// <summary>
    /// Initializes the question pool and room list.
    /// </summary>
    public Map()
    {
        _questionPool = new QuestionPool();
        _roomDictionary = new Dictionary<string, Room>();
    }
    
    /// <summary>
    /// Loads question pool from JsonReader and sets up rooms
    /// saving them in a list of rooms.
    /// </summary>
    public void Setup()
    {
        
        _questionPool.LoadQuestions();
        
        foreach (var task in _roomNamesAndTaskNum)
        {
            Room room = new Room(task.Value, task.Key, _questionPool);
            _roomDictionary.Add(task.Value, room);;
        }
        
    }
    
    /// <summary>
    /// Cycles through all rooms invoking their setup method.
    /// Which will load questions and answers from the question pool to the amount specified
    /// in the rooms number of tasks.
    /// </summary>
    public void LoadRoom()
    {
        foreach (var room in _roomDictionary.Values)
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
    public Room GetRoom(String name)
    {
        return _roomDictionary[name];
    }
    

    /// <returns>Full list of Rooms</returns>
    public Dictionary<String, Room> GetRooms()
    {
        return _roomDictionary;
    }
}