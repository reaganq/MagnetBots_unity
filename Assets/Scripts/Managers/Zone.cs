using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone : Photon.MonoBehaviour {

	public string Name;
	public PhotonView myPhotonView;
	public Transform spawnPoint;
	public ZoneType zoneType;
	public GameObject zoneObject;
	public List<CharacterStatus> players;
	public List<int> playerIDs;

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
		players.Add(cs);
		playerIDs.Add(info.sender.ID);
		Debug.Log("base netwrok add player");
	}

	public void RemovePlayer()
	{
	}

	[RPC]
	public virtual void NetworkRemovePlayer()
	{
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