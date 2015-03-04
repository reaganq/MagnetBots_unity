using UnityEngine;
using System.Collections;

public class AICharacter : CharacterStatus {

	public SimpleFSM fsm;

	// Use this for initialization
	void Start () {
		characterType = CharacterType.AI;
		enemyCharacterType = CharacterType.Playable;
	}
	
	// Update is called once per frame
	public override void Die()
	{
		fsm.EnterAIState(AIState.death);
	}
}
