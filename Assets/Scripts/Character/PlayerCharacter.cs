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

	public override void ChangeMovementSpeed(float change)
	{
		base.ChangeMovementSpeed(change);
		if(playerActionManager != null)
		{
			playerActionManager.motor.AnimationUpdate();
		}
	}

	public IEnumerator RequestInfo()
	{
		yield return new WaitForSeconds(1);
		avatar.LoadAllBodyParts(PlayerManager.Instance.Hero.profile.name,
		                        PlayerManager.Instance.Hero.Equip.EquippedFace.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedFace.Level, PlayerManager.Instance.Hero.Equip.EquippedFace.rpgArmor.FBXName.Count) - 1], 
		                        PlayerManager.Instance.Hero.Equip.EquippedHead.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedHead.Level, PlayerManager.Instance.Hero.Equip.EquippedHead.rpgArmor.FBXName.Count) - 1], 
		                        PlayerManager.Instance.Hero.Equip.EquippedBody.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedBody.Level, PlayerManager.Instance.Hero.Equip.EquippedBody.rpgArmor.FBXName.Count) - 1], 
		                        PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedArmL.Level, PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.FBXName.Count) - 1], 
		                        PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedArmR.Level, PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.FBXName.Count) - 1], 
		                        PlayerManager.Instance.Hero.Equip.EquippedLegs.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedLegs.Level, PlayerManager.Instance.Hero.Equip.EquippedLegs.rpgArmor.FBXName.Count) - 1]
		                        );
	}
}
