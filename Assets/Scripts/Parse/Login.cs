using UnityEngine;
using System.Collections;
using System;
using Parse;

public class Login : MonoBehaviour {
	private string userName = string.Empty;
	private string password = string.Empty;
	private bool isAuthenticated = false;
	private bool isMainMenu = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnGUI () {
		
		if (ParseUser.CurrentUser != null)
		{
		    // Once user has been authenticated do stuff
			ShowMainMenuGUI();
		}
		else
		{
		    // show the signup or login screen
			ShowLoginGUI();
		}
	}
	
	void ShowLoginGUI(){
		// Make a group on the center of the screen
		GUI.BeginGroup (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 132, 300, 400));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (new Rect (0,0,300,200), "Login Window Example:");
		//Create a label and textfield for the username:
		GUI.Label (new Rect (25, 35, 100, 30), "Username:");
		userName = GUI.TextField (new Rect (125, 35, 140, 30), userName);
		//Create a label and textfield for the password:
		GUI.Label (new Rect (25, 75, 100, 30), "Password:");
		password = GUI.PasswordField (new Rect (125, 75, 140, 30), password, "*"[0], 25);
		
		if (GUI.Button (new Rect (185,115,80,30), "Login"))
		{
			authenticateUser(userName, password);
		}
		
		if(GUI.Button ( new Rect (135, 155, 130, 30), "Login with Facebook"))
		{
			authenticateUser(userName, password);
			var expiration = DateTime.Now.AddHours(6);
			var logInTask = ParseFacebookUtils.LogInAsync("373295066079975", ParseUser.CurrentUser.ObjectId, expiration);
			Debug.Log(logInTask.Result);
		}
		
		if (GUI.Button (new Rect (25,115,120,30), "Create Account")){
 			Application.LoadLevel("CreateNewAccount");
		}
		
		//GUI.Label (new Rect (25, 120, 100, 30), "LoggedIn: " + isAuthenticated.ToString());

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
	
	void ShowMainMenuGUI(){
		
		GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (new Rect (0,0,200,200), "Main Menu");
		if (GUI.Button (new Rect (30,40,140,30), "High Score Demo"))
		{
			Application.LoadLevel("HighScore");
		}
		if (GUI.Button (new Rect (30,75,140,30), "Log Out"))
		{
			ParseUser.LogOut();
		}

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
	
	void authenticateUser(string username, string password){
		
		ParseUser.LogInAsync(username, password).ContinueWith(t =>
		{
		    if (t.IsFaulted || t.IsCanceled)
		    {
		        // The login failed. Check t.Exception to see why.
				isAuthenticated = false;
		    }
		    else
		    {
		        // Login was successful.
				isAuthenticated = true;
		    }
		});
	}
		
}
