using UnityEngine;
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
	public AudioClip soundtrack;
	public List<int> allAvatars;

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
		ArenaManagers.Clear();
	}

	#region register players


	public void RegisterNewAvatar()
	{
		//local player adds him/herself to the world manager's master list of all players

	}

	[RPC]
	public void NetworkRegisterNewAvatar(int id)
	{
		if(PhotonNetwork.isMasterClient)
		{
			allAvatars.Add(id);
		}
		else
		{
		}
	}

	[RPC]
	public void NetworkUpdateAvatarList()
	{
	}
	
	public void RequestAvatarInfo()
	{
		//request initialise info for avatar of other players
	}
		
	#endregion

	public void SendPartyChallenge(int enemyID)
	{
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i] != PhotonNetwork.player.ID)
				myPhotonView.RPC("ReceivePartyChallenge", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]), enemyID);
		}
	}

	[RPC]
	public void ReceivePartyChallenge(int enemyID, PhotonMessageInfo info)
	{
		GUIManager.Instance.NotificationGUI.DisplayPartyChallenge(enemyID, info.sender);
	}

	public void PartyChallengeReply(PhotonPlayer challenger, bool accept)
	{
		myPhotonView.RPC("NetworkPartyChallengeReply", challenger, accept);
	}

	[RPC]
	public void NetworkPartyChallengeReply(bool accept, PhotonMessageInfo info)
	{
		if(!accept)
		{
			for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
				if(PlayerManager.Instance.partyMembers[i] != PhotonNetwork.player.ID)
					myPhotonView.RPC("CancelPartyChallenge", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]));
			}
		}
	}

	[RPC]
	public void CancelPartyChallenge()
	{

	}

	#region arena logic

	public void RequestArenaEntry(string arenaName, int enemyID, bool solo)
	{
		int newid = PhotonNetwork.AllocateViewID();
		EnterArenaData data = new EnterArenaData();
		data.EnemyID = enemyID;
		data.NewViewID = newid;
		Debug.Log(data.NewViewID);
		
		if(solo)
		{
			data.partyList.Add(PhotonNetwork.player.ID);
			data.PartyLeaderID = PhotonNetwork.player.ID;
		}
		else
		{
			if(PlayerManager.Instance.partyMembers.Count == 0)
			{
				data.partyList.Add(PhotonNetwork.player.ID);
				data.PartyLeaderID = PhotonNetwork.player.ID;
			}
			else if(PlayerManager.Instance.partyMembers.Count > 0)
			{
				data.partyList = PlayerManager.Instance.partyMembers;
				data.PartyLeaderID = PhotonNetwork.player.ID;
			}
		}
		
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, data);
		
		myPhotonView.RPC("GetAvailableArena", PhotonTargets.MasterClient, arenaName, m.GetBuffer());
	}

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

	//all buffered
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

	//players within arena
	[RPC]
	public void GiveArenaRewards( int zoneid )
	{
		ArenaManagers[zoneid].GiveRewards();
	}

	#endregion

	#region Party Logic

	[RPC]
	public void UpdatePartyList(byte[] partyList)
	{
		PlayerManager.Instance._partyMembers.Clear();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(partyList);
		PlayerManager.Instance.partyMembers = (List<int>)b.Deserialize(m);
		GUIManager.Instance.MainGUI.UpdatePartyMembers();
	}
	
	//party leader send to prospective party members
	[RPC]
	public void SendPartyInvite(int partyLeaderID,PhotonMessageInfo info)
	{
		if(PlayerManager.Instance.isInParty())
			myPhotonView.RPC("NetworkRejectPartyInvite", info.sender, PlayerManager.Instance.Hero.profile.name + " is already in a party");
		else
		{
			GUIManager.Instance.DisplayPartyNotification(info.sender, partyLeaderID);
			Debug.Log("received party invite");
		}
	}
	
	public void RejectPartyInvite(PhotonPlayer prospectivePartyLeader)
	{
		myPhotonView.RPC("NetworkRejectPartyInvite", prospectivePartyLeader, PlayerManager.Instance.Hero.profile.name + " has rejected your party invitation");
	}

	[RPC]
	public void NetworkRejectPartyInvite(string message, PhotonMessageInfo info)
	{
		//display rejection notice from 
		GUIManager.Instance.NotificationGUI.DisplayMessageBox(message);
	}

	
	//invitees reply back to prospective/party leader
	[RPC]
	public void AcceptPartyInvite(PhotonMessageInfo info)
	{
		//failsafe
		if(PlayerManager.Instance.partyMembers.Count == 0)
		{
			PlayerManager.Instance.partyMembers.Add(PhotonNetwork.player.ID);
		}
		PlayerManager.Instance.partyMembers.Add(info.sender.ID);
		UpdatePartyMemberList();
	}

	public void DisbandParty()
	{
		if(PlayerManager.Instance.partyMembers[0] == PhotonNetwork.player.ID)
		{
			//find next party leader
			myPhotonView.RPC("QuitParty", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[1]));
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
		PlayerManager.Instance.partyMembers.Clear();
		GUIManager.Instance.MainGUI.UpdatePartyMembers();
	}

	//quitter sends to partyleader
	[RPC]
	public void QuitParty(PhotonMessageInfo info)
	{
		PlayerManager.Instance.partyMembers.Remove( info.sender.ID );
		if(PlayerManager.Instance.partyMembers.Count <= 1)
			EndParty();
		else
			UpdatePartyMemberList();
	}

	/*[RPC]
	public void TakeOverAsPartyLeader()
	{
		if(PlayerManager.Instance.partyMembers[0] == PhotonNetwork.player.ID)
			PlayerManager.Instance.isPartyLeader = true;
	}*/

	//used by party leader
	public void UpdatePartyMemberList()
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, PlayerManager.Instance.partyMembers);
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) 
		{
			if(PlayerManager.Instance.partyMembers[i] != PhotonNetwork.player.ID)
				myPhotonView.RPC("UpdatePartyList", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]), m.GetBuffer());
		}
		GUIManager.Instance.MainGUI.UpdatePartyMembers();
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
					UpdatePartyMemberList();
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
