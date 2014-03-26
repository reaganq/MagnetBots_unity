﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class WorldManager : Photon.MonoBehaviour {

	public List<ArenaLists> Arenas;
	public Zone DefaultZone;
	public PhotonView myPhotonView;
	public List<ArenaManager> ArenaManagers;

	void Start()
	{
		myPhotonView = GetComponent<PhotonView>();
		for (int i = 0; i < Arenas.Count; i++) 
		{
			for (int s = 0; s < Arenas[i].arenaInstances.Count; s++)
			{
				ArenaManagers.Add(Arenas[i].arenaInstances[s]);
			}
		}

		for (int i = 0; i < ArenaManagers.Count; i++) 
		{
			ArenaManagers[i].ID = ArenaManagers.IndexOf(ArenaManagers[i]);
		}
	}

	#region arena logic

	//from battle challenge initiator to Master
	[RPC]
	public void GetAvailableArena(string arenaname, byte[] data, PhotonMessageInfo info)
	{
		for (int i = 0; i < Arenas.Count; i++) 
		{
			if(Arenas[i].arenaName == arenaname)
			{
				for (int s = 0; s < Arenas[i].arenaStates.Count; s++) 
				{
					if(Arenas[i].arenaStates[s] == false)
					{
						myPhotonView.RPC("SetupArena", PhotonTargets.AllBuffered, i, s, data);
						return;
					}
				}
			}
		}
	}

	//all buffered
	[RPC]
	public void SetupArena(int i, int s, byte[] data)
	{
		EnterArenaData arenaData = new EnterArenaData();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(data);
		arenaData = (EnterArenaData)b.Deserialize(m);

		//change bool state of arenas across the board
		Arenas[i].arenaStates[s] = true;


		List<PhotonPlayer> pPlayers = new List<PhotonPlayer>();
		for (int x = 0; x < arenaData.partyList.Count; x++) {
			pPlayers.Add(PhotonPlayer.Find(arenaData.partyList[x]));
		}

		//pass through full admitted party list to all arenas
		Arenas[i].arenaInstances[s].admittedPlayers = pPlayers;

		//load the correct enemy wave on all arenas
		Arenas[i].arenaInstances[s].rpgEnemy = Storage.LoadById<RPGEnemy>(arenaData.EnemyID, new RPGEnemy());

		Arenas[i].arenaInstances[s].ownerViewID = arenaData.NewViewID;
		Arenas[i].arenaInstances[s].ownerID = arenaData.PartyLeaderID;

		//instantiate the first enemy wave as scene objects TODO change scene object into custom loaded object
		if(Arenas[i].arenaInstances[s].admittedPlayers.Contains(PhotonNetwork.player))
		{

			PlayerManager.Instance.GoToArena(Arenas[i].arenaInstances[s], arenaData.EnemyID);
			Arenas[i].arenaInstances[s].SpawnEnemyWave(0);
			//instantiate the enemy
		}
	}

	[RPC]
	public void AddPlayer(int zoneid, int id)
	{
		Debug.Log("adding player with id: "+id);
		PhotonView view = PhotonView.Find(id);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		//ArenaPlayer ap = new ArenaPlayer();
		//ap.playerCS = playerObject;
		//ap.playerID = view.ownerId;
		//players.Add(ap);
		ArenaManagers[zoneid].players.Add(playerObject);
		ArenaManagers[zoneid].playerIDs.Add(view.ownerId);
	}
	
	public void EndSession(ArenaManager zone)
	{
		int id = ArenaManagers.IndexOf(zone);
		myPhotonView.RPC("RemovePlayer", PhotonTargets.AllBuffered, id, PlayerManager.Instance.avatarPhotonView.viewID);
	}

	//all buffered
	[RPC]
	public void RemovePlayer(int zoneid, int objectViewid)
	{
		PhotonView view = PhotonView.Find(objectViewid);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		ArenaManagers[zoneid].players.Remove(playerObject);
		ArenaManagers[zoneid].playerIDs.Remove(view.ownerId);

		if(PhotonNetwork.isMasterClient)
		{
			if(ArenaManagers[zoneid].players.Count == 0)
			{
				Debug.Log("clear");
				//PhotonView enemyView = enemy.GetComponent<PhotonView>();
				/*if(enemyView.owner != null)
				{
					myPhotonView.RPC("ClearEnemies", enemyView.owner);
				}
				else*/
				//myPhotonView.RPC("ClearEnemies", PhotonTargets.MasterClient);

				myPhotonView.RPC("DecommissionArena", PhotonTargets.AllBuffered, zoneid);
				//ClearEnemies();
			}
			else
			{
				if(ArenaManagers[zoneid].ownerID == view.ownerId)
				{
					Debug.Log("here alloc");
					//PhotonNetwork.UnAllocateViewID(enemyView.viewID);
					myPhotonView.RPC("ChangeEnemyOwner", PhotonPlayer.Find(ArenaManagers[zoneid].playerIDs[0]), zoneid);
				}
			}
		}
	}
	
	//all buffered
	[RPC]
	public void RemovePlayerAt(int zoneid, int id)
	{
		for (int i = 0; i < ArenaManagers[zoneid].playerIDs.Count; i++) 
		{
			if(ArenaManagers[zoneid].playerIDs[i] == id)
			{
				ArenaManagers[zoneid].playerIDs.RemoveAt(i);
				ArenaManagers[zoneid].players.RemoveAt(i);
				Debug.Log("disconnected player" + id);
			}
		}
		if(PhotonNetwork.isMasterClient)
		{
			if(ArenaManagers[zoneid].players.Count == 0)
			{
				Debug.Log("clear");
				//if(enemyView.owner != null)
					//myPhotonView.RPC("ClearEnemies", enemyView.owner);
				//else
					//myPhotonView.RPC("ClearEnemies", PhotonTargets.MasterClient);
				myPhotonView.RPC("DecommissionArena", PhotonTargets.AllBuffered, zoneid);
			}
			else
			{
				if(ArenaManagers[zoneid].ownerID == id)
				{
					Debug.Log("here alloc");
					//PhotonNetwork.UnAllocateViewID(enemyView.viewID);
					myPhotonView.RPC("ChangeEnemyOwner", PhotonPlayer.Find(ArenaManagers[zoneid].playerIDs[0]), zoneid);
				}
			}
		}
	}

	//target player
	[RPC]
	public void ChangeEnemyOwner(int zoneid)
	{
		/*if(PlayerManager.Instance.ActiveArena == this && enemyFSMs.Count > 0)
		{
			enemyFSMs[0].myPhotonView.RPC("ChangeOwner", PhotonTargets.AllBuffered, PhotonNetwork.AllocateViewID());
			Debug.Log("change owner");
		}*/

		int newid = PhotonNetwork.AllocateViewID();
		myPhotonView.RPC("ChangeZoneOwner", PhotonTargets.AllBuffered, zoneid, newid, PhotonNetwork.player.ID);

	}

	//all buffered
	[RPC]
	public void ChangeZoneOwner(int zoneid, int viewid, int ownerid)
	{
		ArenaManagers[zoneid].ownerViewID = viewid;
		ArenaManagers[zoneid].ownerID = ownerid;
		ArenaManagers[zoneid].UpdateNewOwner();
	}
	
	[RPC]
	public void DecommissionArena(int zoneid)
	{
		//ArenaManager am = ArenaManagers[zoneid];

		for (int i = 0; i < Arenas.Count; i++) 
		{
			for (int s = 0; s < Arenas[i].arenaInstances.Count; s++) 
			{
				if(Arenas[i].arenaInstances[s].ID == zoneid)
				{
					Arenas[i].arenaStates[s] = false;
					Arenas[i].arenaInstances[s].CleanUp();
					//Arenas[i].arenaStates[s] = false;
					return;
				}
			}
		}
	}

	#endregion

	#region Party Logic

	[RPC]
	public void UpdatePartyList(byte[] partyList)
	{
		PlayerManager.Instance.partyMembers.Clear();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(partyList);
		PlayerManager.Instance.partyMembers = (List<int>)b.Deserialize(m);

		if(PlayerManager.Instance.partyMembers[0] == PhotonNetwork.player.ID)
			PlayerManager.Instance.isPartyLeader = true;
	}
	
	//party leader send to prospective party members
	[RPC]
	public void SendPartyInvite(PhotonMessageInfo info)
	{
		if(PlayerManager.Instance.partyMembers.Count == 0 && !PlayerManager.Instance.isPartyLeader)
		{
			GUIManager.Instance.DisplayPartyNotification(info.sender);
		}
	}
	
	//invitees reply back to prospective/party leader
	[RPC]
	public void AcceptPartyInvite(PhotonMessageInfo info)
	{
		if(PlayerManager.Instance.partyMembers.Count == 0)
		{
			PlayerManager.Instance.partyMembers.Add(PhotonNetwork.player.ID);
			PlayerManager.Instance.isPartyLeader = true;
		}
		PlayerManager.Instance.partyMembers.Add(info.sender.ID);
		UpdatePartyMemberList(1);
	}

	public void DisbandParty()
	{
		if(PlayerManager.Instance.isPartyLeader)
		{
			PlayerManager.Instance.partyMembers.Remove(PhotonNetwork.player.ID);
			if(PlayerManager.Instance.partyMembers.Count <= 1)
			{
				for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) 
				{
					myPhotonView.RPC("EndParty", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]));
				}
			}
			else
			{
				UpdatePartyMemberList(0);
				//myPhotonView.RPC("TakeOverAsPartyLeader", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[0]));
			}
		}
		else
		{
			myPhotonView.RPC("QuitParty", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[0]));
		}
		EndParty();
	}

	//party leader sends to all members
	[RPC]
	public void EndParty()
	{
		PlayerManager.Instance.isPartyLeader = false;
		PlayerManager.Instance.partyMembers.Clear();
	}

	//quitter sends to partyleader
	[RPC]
	public void QuitParty(PhotonMessageInfo info)
	{
		PlayerManager.Instance.partyMembers.Remove( info.sender.ID );
		if(PlayerManager.Instance.partyMembers.Count <= 1)
			EndParty();
		else
			UpdatePartyMemberList(1);
	}

	/*[RPC]
	public void TakeOverAsPartyLeader()
	{
		if(PlayerManager.Instance.partyMembers[0] == PhotonNetwork.player.ID)
			PlayerManager.Instance.isPartyLeader = true;
	}*/

	//used by party leader
	public void UpdatePartyMemberList(int index)
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, PlayerManager.Instance.partyMembers);
		for (int i = index; i < PlayerManager.Instance.partyMembers.Count; i++) 
		{
			myPhotonView.RPC("UpdatePartyList", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]), m.GetBuffer());
		}
	}

	#endregion

	#region challenge party logic

	public void IssueArenaChallenge()
	{
	}

	#endregion

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.LogError(player);
		if(PlayerManager.Instance.partyMembers.Contains(player.ID))
		{
			PlayerManager.Instance.partyMembers.Remove(player.ID);
			if(PlayerManager.Instance.partyMembers[0] == PhotonNetwork.player.ID)
			{
				if(PlayerManager.Instance.partyMembers.Count <= 1)
					EndParty();
				else
				{
					PlayerManager.Instance.isPartyLeader = true;
					UpdatePartyMemberList(1);
				}
			}
		}

		if(PhotonNetwork.isMasterClient)
		{
			for (int j = 0; j < ArenaManagers.Count; j++) 
			{
				for (int i = 0; i < ArenaManagers[j].playerIDs.Count; i++) 
				{
					if(ArenaManagers[j].playerIDs[i] == player.ID)
					{
						Debug.Log("disconnecting player" + player);
						//players.RemoveAt(i);
						myPhotonView.RPC("RemovePlayerAt", PhotonTargets.AllBuffered, j, player.ID);
						return;
					}
				}
			}
		}
	}

	/*public void CheckArenasForDisconnects(int id)
	{
		for (int i = 0; i < ArenaManagers.Count; i++) 
		{
			for (int s = 0; s < ArenaManagers[i].players; s++) 
			{
				if(ArenaManagers[i].players[s].playerID == id)
				{
					ArenaManagers[i].RefreshPlayerList(id);
					return;
				}
			}
		}
	}*/

}

[Serializable]
public class ArenaLists
{
	public List<ArenaManager> arenaInstances;
	public List<bool> arenaStates;
	public string arenaName;

}
