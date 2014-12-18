using UnityEngine;
using System.Collections;

public class AICharacter : CharacterStatus {

	public SimpleFSM fsm;

	// Use this for initialization
	void Start () {
		characterType = CharacterType.AI;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
