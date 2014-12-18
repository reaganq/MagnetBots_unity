using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager ActionManager;
	// Use this for initialization
	void Awake () {
		characterName = GenerateRandomString(6
		characterType = CharacterType.Playable;
		ActionManager = GetComponent<CharacterActionManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
