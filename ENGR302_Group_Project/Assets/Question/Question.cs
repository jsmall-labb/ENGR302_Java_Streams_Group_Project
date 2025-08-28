using System;



public class Question
{
 
private int _id;
 private String _room;
 private String _context;
 private String _answer;
 private String _completion;
 
 public Question(int id, String  room, String context, String answer, String completion)
 {
 _room = room;
 _context = context;
 _answer = answer;
 _completion = completion;
 }

 public int getId()
 {
  return _id;
 }
 
 public String GetRoom()
 {
  return _room;
 }

 public String GetContext()
 {
  return _context;
 }

 public String GetAnswer()
 {
  return _answer;
 }

 public String GetCompletion()
 {
  return _completion;
 }

 public bool IsCompleted(String input)
 {
  return input == _answer;
 }
}