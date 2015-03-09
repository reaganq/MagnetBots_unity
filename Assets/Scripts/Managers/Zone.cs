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
		myPhotonView.RPC("NetworkAddPlayer", PhotonTargets.MasterClient, PlayerManager.Instance.avatarPhotonView.viewID);
	}

	//master
	[RPC]
	public virtual void NetworkAddPlayer(int csViewID, PhotonMessageInfo info)
	{
		PhotonView view = PhotonView.Find(csViewID);
		CharacterStatus cs = view.GetComponent<CharacterStatus>();
		playerCharacterStatuses.Add(cs);
		Debug.Log(view.ownerId);
		photonPlayers.Add(info.sender);
		Debug.Log("base netwrok add player");
	}

	public void RemovePlayer()
	{
		myPhotonView.RPC("NetworkRemovePlayer", PhotonTargets.MasterClient, PlayerManager.Instance.avatarPhotonView.viewID);
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