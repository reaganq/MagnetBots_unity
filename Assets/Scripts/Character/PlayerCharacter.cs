using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager playerActionManager;
	public Avatar avatar;
	public Zone parentZone;
	public int zoneViewID;

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

	}

	[RPC]
	public override void NetworkDie ()
	{
		base.NetworkDie ();
		if(PlayerManager.Instance.ActiveArena != null)
		{
			PlayerManager.Instance.ActiveArena.CheckPlayerDeathStatus();
		}
	}

	public void DisplayInfoByZone()
	{
		myPhotonView.RPC("NetworkDisplayInfoByZone", PhotonTargets.All, PlayerManager.Instance.ActiveZone.myPhotonView.viewID);
	}

	[RPC]
	public void NetworkDisplayInfoByZone(int zoneID)
	{
		zoneViewID = zoneID;
		PhotonView zoneView = PhotonView.Find(zoneViewID);
		Zone newZone = zoneView.GetComponent<Zone>();
		if(newZone != null)
			parentZone = newZone;

		DisplayName(true);
		if(parentZone.zoneType == ZoneType.arena)
		{
			DisplayHpBar(true);
		}
		else
			DisplayHpBar(false);
		Debug.Log("i now belong to new zoneid: " + parentZone.Name + zoneViewID);
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
