using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerCharacter : CharacterStatus {

	public CharacterActionManager playerActionManager;
	public Avatar avatar;
	public Zone parentZone;
	public string headPortraitString = "headshot_default";
	public int zoneViewID;
	public List<ParseInventoryItem> shopItems;
	// Use this for initialization
	public override void Awake () {
		base.Awake();
		characterType = CharacterType.Playable;
		enemyCharacterType = CharacterType.AI;
		shopItems = new List<ParseInventoryItem>();
		playerActionManager = GetComponent<CharacterActionManager>();

		if(myPhotonView.isMine)
		{
			this.tag = "Player";
		}
		else
		{
			this.tag = "OtherPlayer";
			avatar.RequestInitInfo();
			RequestShopItems();
		}
	}

	public void UpdateShopItems()
	{
		//Debug.Log(PlayerManager.Instance.Hero.playerShopInventory.Items[0].CurrentAmount);
		myPhotonView.RPC("NetworkUpdateShopItems", PhotonTargets.All, PlayerManager.Instance.Hero.ParseInventoryList(PlayerManager.Instance.Hero.playerShopInventory));
	}

	[RPC]
	public void NetworkUpdateShopItems(byte[] shopItemdsData)
	{
		shopItems.Clear();
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(shopItemdsData);
		shopItems = (List<ParseInventoryItem>)bb.Deserialize(mm);
		if(!myPhotonView.isMine && GUIManager.Instance.ShopGUI.playerShopKeeper == myPhotonView.owner)
			GUIManager.Instance.ShopGUI.UpdatePlayerShopitemList(shopItems);
	}

	public void RequestShopItems()
	{
		myPhotonView.RPC("NetworkRequestShopItems", myPhotonView.owner);
	}

	[RPC]
	public void NetworkRequestShopItems(PhotonMessageInfo info)
	{
		UpdateShopItems();
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
	public override void Die()
	{
		//base.NetworkDie ();
		base.Die();
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
		if(zoneView != null)
		{
			Zone newZone = zoneView.GetComponent<Zone>();
			if(newZone != null)
				parentZone = newZone;
			if(!myPhotonView.isMine)
				DisplayName(true);
			else
				DisplayName(false);
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
	}

	public void DisplayTeamSign(bool state)
	{
		HUD.teamIcon.SetActive(state);
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
