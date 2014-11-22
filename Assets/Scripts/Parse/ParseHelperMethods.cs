using UnityEngine;
using System.Collections;
using Parse;

public class ParseHelperMethods : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnApplicationQuit() {
        ParseUser.LogOut();
			var currentUser = ParseUser.CurrentUser; // this will now be null
    }
}
