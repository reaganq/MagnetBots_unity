using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager playerActionManager;
	public Avatar avatar;

	// Use this for initialization
	public override void Awake () {
		base.Awake();
		characterName = GenerateRandomString(6);
		characterType = CharacterType.Playable;
		enemyCharacterType = CharacterType.AI;
		playerActionManager = GetComponent<CharacterActionManager>();
		if(myPhotonView.isMine)
		{
			this.tag = "Player";
		}
		else
			this.tag = "OtherPlayer";
	}
	
	public override void ChangeMovementSpeed(float change)
	{
		base.ChangeMovementSpeed(change);
		if(playerActionManager != null)
		{
			playerActionManager.motor.AnimationUpdate();
		}
	}
}
