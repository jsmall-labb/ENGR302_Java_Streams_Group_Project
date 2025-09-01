using System;


[ System.Serializable]
public class Question
{
 
private int _id;
 private String _room;
 private String _context;
 private String _answer;
 private String _completion;
 private bool _isAnswered;
 
 
 /// <summary>
 /// Initializes a new Question with the specified parameters.
 /// </summary>
 /// <param name="id">Unique identifier for the question</param>
 /// <param name="room">Room where the question is located</param>
 /// <param name="context">Context or description of the question</param>
 /// <param name="answer">Correct answer to the question</param>
 /// <param name="completion">Completion status or additional completion info</param>
 public Question(int id, String  room, String context, String answer, String completion)
 {
 _room = room;
 _context = context;
 _answer = answer;
 _completion = completion;
 }

 /// <summary>
 /// Gets the unique identifier of the question.
 /// </summary>
 /// <returns>The question ID as an integer</returns>
 public int getId()
 {
  return _id;
 }
 
 /// <summary>
 /// Gets the room where the question is located.
 /// </summary>
 /// <returns>The room name as a string</returns>
 public String GetRoom()
 {
  return _room;
 }

 /// <summary>
 /// Gets the context or description of the question.
 /// </summary>
 /// <returns>The question context as a string</returns>
 public String GetContext()
 {
  return _context;
 }

 /// <summary>
 /// Gets the correct answer to the question.
 /// </summary>
 /// <returns>The correct answer as a string</returns>
 public String GetAnswer()
 {
  return _answer;
 }

 /// <summary>
 /// Gets the completion status or additional completion information.
 /// </summary>
 /// <returns>The completion information as a string</returns>
 public String GetCompletion()
 {
  return _completion;
 }

 /// <summary>
 /// Checks if the provided input matches the correct answer.
 /// </summary>
 /// <param name="input">The input to check against the correct answer</param>
 /// <returns>True if the input matches the correct answer, false otherwise</returns>
 public bool IsCorrect(String input)
 {
  if (input == _answer)
  {
   _isAnswered = true;
   return true;
  }
  return false;
 }
 
 /// <summary>
 /// Returns if the Question has already been answered.
 /// </summary>
 /// <returns>True if the question has already been answered, false otherwise</returns>
 public bool IsAnswered()
 {
  return _isAnswered;
 }


}