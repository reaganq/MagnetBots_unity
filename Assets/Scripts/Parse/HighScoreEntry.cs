using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class HighScoreEntry : MonoBehaviour {
	private string score = string.Empty;
	
	void OnGUI () {
		
		if (ParseUser.CurrentUser != null)
		{
		    // Once user has been authenticated do stuff
			// Make a group on the center of the screen
			GUI.BeginGroup (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 132, 300, 400));
			// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
	
			// We'll make a box so you can see where the group is on-screen.
			GUI.Box (new Rect (0,0,300,165), "Enter High Score Example:");
			//Create a label and textfield for the username:
			GUI.Label (new Rect (25, 35, 100, 30), "PlayerName:");
			GUI.Label (new Rect (125, 35, 140, 30), ParseUser.CurrentUser.Username);
			//Create a label and textfield for the password:
			GUI.Label (new Rect (25, 75, 100, 30), "Score:");
			score = GUI.TextField (new Rect (125, 75, 140, 30), score);
			
			if (GUI.Button (new Rect (25,115,80,30), "Save"))
			{
				SaveHighScore(score);
			}
			if (GUI.Button (new Rect (125,115,120,30), "Show High Scores"))
			{
				Application.LoadLevel("HighScores");
			}
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
		}
		else
		{
		    // show the signup or login screen
			 Application.LoadLevel("Demo");
		}
	}
			
	void SaveHighScore(string score)
	{
		ParseObject gameScore = new ParseObject("GameScore");
		gameScore["score"] = score;
		gameScore["playerName"] = ParseUser.CurrentUser.Username;
		gameScore.SaveAsync();
	}
}
