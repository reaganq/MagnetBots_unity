using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone : Photon.MonoBehaviour {

	public string Name;
	public PhotonView myPhotonView;
	public Transform spawnPoint;
	public ZoneType zoneType;
	public GameObject zoneObject;
	public List<CharacterStatus> playerCharacterStatuses;
	public List<PhotonPlayer> photonPlayers = new List<PhotonPlayer>();

	public virtual void EnterZone()
	{
		zoneObject.SetActive(true);
		PlayerManager.Instance.SpawnPoint = spawnPoint;
		AddPlayer();
		PlayerManager.Instance.avatarStatus.ResetHealth();
		if(zoneType == ZoneType.arena)
			GUIManager.Instance.MainGUI.EnterBattleMode(true);
		else
			GUIManager.Instance.MainGUI.EnterBattleMode(false);
		//avatarObject.transform.position = SpawnPoint.position;
		//move the player to spawnpoint
	}

	public virtual void LeaveZone()
	{
		zoneObject.SetActive(false);
		RemovePlayer();
	}

	public void AddPlayer()
	{
		myPhotonView.RPC("NetworkAddPlayer", PhotonTargets.All, PlayerManager.Instance.avatarPhotonView.viewID);
	}

	//master
	[RPC]
	public virtual void NetworkAddPlayer(int csViewID, PhotonMessageInfo info)
	{
		PhotonView view = PhotonView.Find(csViewID);
		CharacterStatus cs = view.GetComponent<CharacterStatus>();
		playerCharacterStatuses.Add(cs);
		photonPlayers.Add(info.sender);
	}

	public void RemovePlayer()
	{
		myPhotonView.RPC("NetworkRemovePlayer", PhotonTargets.All, PlayerManager.Instance.avatarPhotonView.viewID);
	}

	[RPC]
	public virtual void NetworkRemovePlayer(int csViewID, PhotonMessageInfo info)
	{
		PhotonView view = PhotonView.Find(csViewID);
		CharacterStatus cs = view.GetComponent<CharacterStatus>();
		playerCharacterStatuses.Remove(cs);
		photonPlayers.Remove(info.sender);
	}

	public void DisconnectPlayer()
	{
	}
}

public enum ZoneType
{
	town,
	arena
}