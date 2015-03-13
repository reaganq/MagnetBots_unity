using UnityEngine;
using System.Collections;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager playerActionManager;
	public Avatar avatar;
	public Zone parentZone;
	public string headPortraitString = "headshot_default";
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

	public void UpdatePortrait(string path)
	{
		myPhotonView.RPC("NetworkUpdatePortrait", PhotonTargets.All, path);
	}

	[RPC]
	public void NetworkUpdatePortrait(string path)
	{
		headPortraitString = path;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].viewID == myPhotonView.viewID)
			{
				GUIManager.Instance.MainGUI.UpdatePartyMembers();
			}
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
		if(parentZone != null)
		{
			if(parentZone.zoneType == ZoneType.arena)
			{
				DisplayHpBar(true);
			}
			else
				DisplayHpBar(false);
		}
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
