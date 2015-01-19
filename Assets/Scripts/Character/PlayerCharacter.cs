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
			characterName = PlayerManager.Instance.Hero.profile.name;
			if(nameLabel != null)
				nameLabel.text = characterName;
			Debug.LogWarning(characterName);
		}
		else
		{
			this.tag = "OtherPlayer";
			//request name
			//request parts
		}
		DisplayInfoByZone();
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
