using UnityEngine;
using System.Collections;
using Parse;

public class CreateAccount : MonoBehaviour {
	
	private string userName = string.Empty;
	private string password = string.Empty;
	private string password2 = string.Empty;
	private string email = string.Empty;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		// Make a group on the center of the screen
		GUI.BeginGroup (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 132, 300, 400));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (new Rect (0,0,300,200), "Create New Account:");
		//Create a label and textfield for the username:
		GUI.Label (new Rect (25, 35, 100, 30), "Username:");
		userName = GUI.TextField (new Rect (125, 35, 140, 30), userName);
		//Create a label and textfield for the password:
		GUI.Label (new Rect (25, 75, 100, 30), "Password:");
		password = GUI.PasswordField (new Rect (125, 75, 140, 30), password, "*"[0], 25);
		
		GUI.Label (new Rect (25, 115, 100, 30), "Email:");
		email = GUI.TextField (new Rect (125, 115, 140, 30), email);
		
		if (GUI.Button (new Rect (185,155,80,30), "Create"))
		{
			CreateNewUser(userName, password, email);
		}
		
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
		
	}
	void CreateNewUser(string userName, string password, string email)
	{
		var user = new ParseUser()
		{
		    Username = userName,
		    Password = password,
		    Email = email
		};
		
		user.SignUpAsync();
		Application.LoadLevel("Demo");
	}
}
