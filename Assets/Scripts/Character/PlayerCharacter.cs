using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager playerActionManager;
	public Avatar avatar;
	public Zone parentZone;

	// Use this for initialization
	public override void Awake () {
		base.Awake();
		characterType = CharacterType.Playable;
		enemyCharacterType = CharacterType.AI;

		playerActionManager = GetComponent<CharacterActionManager>();

		if(myPhotonView.isMine)
		{
			this.tag = "Player";
			//StartCoroutine( RequestInfo());
		}
		else
		{
			this.tag = "OtherPlayer";
			avatar.RequestInitInfo();
			//request name
			//request parts
		}
		DisplayInfoByZone();
	}

	/*public override void ChangeMovementSpeed(float change)
	{
		base.ChangeMovementSpeed(change);
		if(playerActionManager != null)
		{
			playerActionManager.myMotor.AnimationUpdate();
		}
	}*/
}
