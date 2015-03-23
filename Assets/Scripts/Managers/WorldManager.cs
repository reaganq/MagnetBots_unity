using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ArenaLists
{
	public List<ArenaManager> arenaInstances;
	public List<bool> arenaStates;
	public string arenaName;
}

[Serializable]
public class ArenaStateData
{
	public string arenaName;
	public List<bool> arenaStates = new List<bool>();
}

public class WorldManager : Photon.MonoBehaviour {

	public List<Zone> allZones;
	public GameObject introCutscene;
	public ArenaLists Arenas;
	public Zone DefaultZone;
	public PhotonView myPhotonView;
	public List<ArenaManager> ArenaManagers;
	public AudioClip soundtrack;
	public List<int> allAvatars;
	public List<NPC> allNPCs = new List<NPC>();

	void Awake()
	{
		var tmp = new EnterArenaData();
		var ttmp = new ArenaStateData ();
		var tttmp = new PartyMemberData ();
		var ttttmp = new List<ArenaStateData> ();
		var tttttmp = new List<bool> ();
	}

	void Start()
	{
		myPhotonView = GetComponent<PhotonView>();
		for (int i = 0; i < Arenas.arenaInstances.Count; i++) 
		{
			ArenaManagers.Add(Arenas.arenaInstances[i]);
		}

		for (int i = 0; i < ArenaManagers.Count; i++) 
		{
			ArenaManagers[i].arenaID = ArenaManagers.IndexOf(ArenaManagers[i]);
		}

		if(!PhotonNetwork.isMasterClient)
		{
			myPhotonView.RPC("RequestArenaStateDataFromMaster", PhotonTargets.MasterClient);
			Debug.Log("sent request data to master");
		}
		//request arena data
		//ArenaManagers.Clear();
	}

	public void AddNPC(NPC npc)
	{
		if(!allNPCs.Contains(npc))
		{
			Debug.Log("added npc");
			allNPCs.Add(npc);
		}
	}

	public NPC GetNPCByID(int id)
	{
		for (int i = 0; i < allNPCs.Count; i++) {
			if(allNPCs[i].ID == id)
				return allNPCs[i];
				}
		return null;
	}

	#region social

	public void talk(string text, string playerName, float timeStamp)
	{
		myPhotonView.RPC("NetworkTalk", PhotonTargets.All, text, playerName, timeStamp);
		PlayerManager.Instance.avatarActionManager.Talk(text);

	}

	[RPC]
	public void NetworkTalk(string text, string playerName, float timeStamp)
	{
		ChatMessage cm = new ChatMessage();
		cm.playerName = playerName;
		cm.message = text;
		cm.timeStamp = timeStamp;
		GUIManager.Instance.chatGUI.AddChatBox(cm);
	}

	public void AddFriend(PhotonPlayer targetPlayer)
	{
		myPhotonView.RPC("NetworkAddFriend", targetPlayer, PlayerManager.Instance.Hero.PlayerName);
	}

	[RPC]
	public void NetworkAddFriend(string playerName, PhotonMessageInfo info)
	{
		GUIManager.Instance.NotificationGUI.DisplayAddFriendNotification(playerName, info.sender);
	}

	public void AcceptFriendRequest(PhotonPlayer target, string myPlayerName)
	{
		myPhotonView.RPC("NetworkAcceptFriendRequest", target, myPlayerName);
	}

	[RPC]
	public void NetworkAcceptFriendRequest(string friendName)
	{
		SocialManager.Instance.AddFriend(friendName);
	}


	#endregion

	#region player shop

	public void BuyItemFromPlayer(string uniqueItemId, int level, int amount, PhotonPlayer targetPlayer)
	{
		myPhotonView.RPC("NetworkBuyItemFromPlayer", targetPlayer, uniqueItemId, level, amount);
	}

	[RPC]
	public void NetworkBuyItemFromPlayer(string uniqueItemId, int level, int amount)
	{
		PlayerManager.Instance.Hero.SoldItem(uniqueItemId, level, amount);
	}
		
	#endregion

	#region party challenge
	public void SendPartyChallenge(int enemyID)
	{
		PlayerManager.Instance.partyChallengeReplies.Clear();
		PlayerManager.Instance.partyChallengeReplies.Add(true);
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].playerID != PhotonNetwork.player.ID)
				myPhotonView.RPC("ReceivePartyChallenge", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i].playerID), enemyID);
		}
		PlayerManager.Instance.startedPartyChallenge = true;
		GUIManager.Instance.NotificationGUI.DisplayChallengerPartyWaiting();
	}

	[RPC]
	public void ReceivePartyChallenge(int enemyID, PhotonMessageInfo info)
	{
		GUIManager.Instance.NotificationGUI.DisplayPartyChallenge(enemyID, info.sender);
		PlayerManager.Instance.startedPartyChallenge = true;
	}

	public void PartyChallengeReply(PhotonPlayer challenger, bool accept)
	{
		myPhotonView.RPC("NetworkPartyChallengeReply", challenger, accept);
	}

	//challenger
	[RPC]
	public void NetworkPartyChallengeReply(bool accept, PhotonMessageInfo info)
	{
		if(!accept)
			CancelPartyChallenge();

		PlayerManager.Instance.partyChallengeReplies.Add(accept);
		if(!PlayerManager.Instance.haveAllTeamReplies())
			return;
		if(PlayerManager.Instance.shouldStartPartyChallenge())
		{
			Debug.Log("we should start this challenge");
			StartPartyChallenge();
		}
	}

	public void StartPartyChallenge()
	{
		GUIManager.Instance.NPCGUI.arenaGUI.LaunchArena(false);
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			myPhotonView.RPC("NetworkStartPartyChallenge", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i].playerID));
		}
	}

	[RPC]
	public void NetworkStartPartyChallenge()
	{
		GUIManager.Instance.NotificationGUI.StartTeamChallenge();
		PlayerManager.Instance.startedPartyChallenge = false;
	}

	public void CancelPartyChallenge()
	{
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].playerID != PhotonNetwork.player.ID)
				myPhotonView.RPC("NetworkCancelPartyChallenge", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i].playerID));	
		}
		NetworkCancelPartyChallenge();
	}

	[RPC]
	public void NetworkCancelPartyChallenge()
	{
		GUIManager.Instance.NotificationGUI.CancelPartyChallenge();
		PlayerManager.Instance.startedPartyChallenge = false;
	}
	#endregion

	#region arena logic

	//new loader to master client
	[RPC]
	public void RequestArenaStateDataFromMaster(PhotonMessageInfo info)
	{
		ArenaStateData data = new ArenaStateData();
		{
			data.arenaName = Arenas.arenaName;
			for (int i = 0; i < Arenas.arenaInstances.Count; i++) {
				data.arenaStates.Add(Arenas.arenaInstances[i]);
			}
		}
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, data);

		myPhotonView.RPC("NetworkUpdateArenaStateData", info.sender, m.GetBuffer());
		Debug.Log("sent reply back to new loader");
	}

	[RPC]
	public void NetworkUpdateArenaStateData(byte[] data)
	{
		ArenaStateData arenaStateData = new ArenaStateData();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(data);
		arenaStateData = (ArenaStateData)b.Deserialize(m);
		Debug.Log("receiving updates from master about arenas");
		for (int i = 0; i < arenaStateData.arenaStates.Count; i++) {
			Arenas.arenaStates[i] = arenaStateData.arenaStates[i];
		}
	}

	public void RequestArenaEntry(string arenaName, int enemyID, bool solo)
	{
		int newid = PhotonNetwork.AllocateViewID();
		EnterArenaData data = new EnterArenaData();
		data.EnemyID = enemyID;
		data.NewViewID = newid;
		Debug.Log(data.NewViewID);
		
		if(solo)
		{
			PartyMemberData pmd = new PartyMemberData();
			pmd.viewID = PlayerManager.Instance.avatarPhotonView.viewID;
			pmd.playerID = PhotonNetwork.player.ID;
			data.partyList.Add(pmd);
			data.PartyLeaderID = PhotonNetwork.player.ID;
		}
		else
		{
			if(PlayerManager.Instance.partyMembers.Count == 0)
			{
				PartyMemberData pmdd = new PartyMemberData();
				pmdd.viewID = PlayerManager.Instance.avatarPhotonView.viewID;
				pmdd.playerID = PhotonNetwork.player.ID;
				data.partyList.Add(pmdd);
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
		Debug.Log ("sent through request");
	}

	//from battle challenge initiator to Master
	[RPC]
	public void GetAvailableArena(string arenaname, byte[] data, PhotonMessageInfo info)
	{
		for (int i = 0; i < Arenas.arenaStates.Count; i++) 
		{
			if(Arenas.arenaStates[i] == false)
			{
				myPhotonView.RPC("SetupArena", PhotonTargets.All, i, data);
				return;
			}
		}
	}

	//all buffered
	//load up the arena with relevant data and then load in all players
	[RPC]
	public void SetupArena(int i, byte[] data)
	{
		EnterArenaData arenaData = new EnterArenaData();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(data);
		arenaData = (EnterArenaData)b.Deserialize(m);

		//change bool state of arenas across the board
		Arenas.arenaStates[i] = true;

		List<PhotonPlayer> pPlayers = new List<PhotonPlayer>();
		for (int x = 0; x < arenaData.partyList.Count; x++) {
			pPlayers.Add(PhotonPlayer.Find(arenaData.partyList[x].playerID));
		}

		Arenas.arenaInstances[i].PreloadArena(pPlayers, arenaData.EnemyID, arenaData.PartyLeaderID, arenaData.NewViewID );

		//instantiate the first enemy wave as scene objects TODO change scene object into custom loaded object
		if(Arenas.arenaInstances[i].admittedPlayers.Contains(PhotonNetwork.player))
		{
			Debug.Log("telling player to go to arena");
			PlayerManager.Instance.GoToZone(Arenas.arenaInstances[i]);
			//Arenas[i].arenaInstances[s].SpawnEnemyWave(0);
			//instantiate the enemy
		}
	}

	[RPC]
	public void AddPlayer(int zoneid, int id)
	{
		/*Debug.Log(zoneid + "adding player with id: "+id);
		PhotonView view = PhotonView.Find(id);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		//ArenaPlayer ap = new ArenaPlayer();
		//ap.playerCS = playerObject;
		//ap.playerID = view.ownerId;
		//players.Add(ap);
		ArenaManagers[zoneid].players.Add(playerObject);
		ArenaManagers[zoneid].playerIDs.Add(view.ownerId);*/
	}
	
	public void EndSession(ArenaManager zone)
	{
		myPhotonView.RPC("DecommissionArena", PhotonTargets.All, zone.arenaID);
	}

	[RPC]
	public void DecommissionArena(int zoneid)
	{
		//ArenaManager am = ArenaManagers[zoneid];
		for (int s = 0; s < Arenas.arenaInstances.Count; s++) 
		{
			if(Arenas.arenaInstances[s].arenaID == zoneid)
			{
				Arenas.arenaStates[s] = false;
				//Arenas[i].arenaInstances[s].CleanUp();
				//Arenas[i].arenaStates[s] = false;
				return;
			}
		}

	}

	//all buffered
	[RPC]
	public void RemovePlayer(int zoneid, int objectViewid)
	{
		PhotonView view = PhotonView.Find(objectViewid);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		ArenaManagers[zoneid].playerCharacterStatuses.Remove(playerObject);
		ArenaManagers[zoneid].photonPlayers.Remove(view.owner);

		if(PhotonNetwork.isMasterClient)
		{
			if(ArenaManagers[zoneid].playerCharacterStatuses.Count == 0)
			{
				Debug.Log("clear");
				myPhotonView.RPC("DecommissionArena", PhotonTargets.AllBuffered, zoneid);
			}
			else
			{
				if(ArenaManagers[zoneid].ownerID == view.ownerId)
				{
					Debug.Log("here alloc");
					//myPhotonView.RPC("ChangeEnemyOwner", PhotonPlayer.Find(ArenaManagers[zoneid].playerIDs[0]), zoneid);
				}
			}
		}
	}
	
	//all buffered
	/*[RPC]
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
	}*/

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


	//players within arena
	[RPC]
	public void GiveArenaRewards( int zoneid )
	{
		ArenaManagers[zoneid].GiveRewards();
	}

	#endregion

	#region profile logic

	public void RequestPlayerProfileData(PhotonPlayer targetPlayer)
	{
		myPhotonView.RPC("NetworkRequestPlayerProfileData", targetPlayer);
	}

	[RPC]
	public void NetworkRequestPlayerProfileData(PhotonMessageInfo info)
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		PlayerProfileDisplayData newData = new PlayerProfileDisplayData();
		newData.CreatePlayerProfileDisplayData();
		b.Serialize(m, newData);
		myPhotonView.RPC("SendBackPlayerProfileData", info.sender, m.GetBuffer());
	}

	[RPC]
	public void SendBackPlayerProfileData(byte[] profileData)
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(profileData);
		GUIManager.Instance.profileGUI.DisplayProfile((PlayerProfileDisplayData)b.Deserialize(m));
	}

	#endregion

	#region Party Logic

	[RPC]
	public void UpdatePartyList(byte[] partyList)
	{
		Debug.Log("received updated party list");
		PlayerManager.Instance._partyMembers.Clear();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(partyList);
		PlayerManager.Instance.partyMembers = (List<PartyMemberData>)b.Deserialize(m);
		Debug.Log(PlayerManager.Instance.partyMembers.Count);
		GUIManager.Instance.MainGUI.UpdatePartyMembers();
	}
	
	//party leader send to prospective party members
	[RPC]
	public void SendPartyInvite(int partyLeaderID,PhotonMessageInfo info)
	{
		if(PlayerManager.Instance.isInParty())
			myPhotonView.RPC("NetworkRejectPartyInvite", info.sender, PlayerManager.Instance.Hero.PlayerName + " is already in a party");
		else
		{
			GUIManager.Instance.DisplayPartyNotification(info.sender, partyLeaderID);
			Debug.Log("received party invite");
		}
	}
	
	public void RejectPartyInvite(PhotonPlayer prospectivePartyLeader)
	{
		myPhotonView.RPC("NetworkRejectPartyInvite", prospectivePartyLeader, PlayerManager.Instance.Hero.PlayerName + " has rejected your party invitation");
	}

	[RPC]
	public void NetworkRejectPartyInvite(string message, PhotonMessageInfo info)
	{
		//display rejection notice from 
		GUIManager.Instance.NotificationGUI.DisplayMessageBox(message);
	}

	
	//invitees reply back to prospective/party leader
	[RPC]
	public void AcceptPartyInvite(int viewID, PhotonMessageInfo info)
	{
		if(PlayerManager.Instance.partyMembers.Count >= GeneralData.maxPartySize)
		{
			return;
		}
		//failsafe
		if(PlayerManager.Instance.partyMembers.Count == 0)
		{
			PartyMemberData pmd = new PartyMemberData();
			pmd.viewID = PlayerManager.Instance.avatarPhotonView.viewID;
			pmd.playerID = PhotonNetwork.player.ID;
			PlayerManager.Instance.partyMembers.Add(pmd);
			Debug.Log("adding myself as party member number 1");
		}

		PartyMemberData pmdd = new PartyMemberData ();
		pmdd.viewID = viewID;
		pmdd.playerID = PhotonView.Find(viewID).ownerId;
		PlayerManager.Instance.partyMembers.Add(pmdd);
		Debug.Log("added new party member");
		UpdatePartyMemberList();
	}

	public void DisbandParty()
	{
		if(PlayerManager.Instance.partyMembers[0].playerID == PhotonNetwork.player.ID)
		{
			//find next party leader
			myPhotonView.RPC("QuitParty", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[1].playerID));
		}
		else
		{
			myPhotonView.RPC("QuitParty", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[0].playerID));
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
		int index = -1;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].playerID == info.sender.ID)
				index = i;
		}
		if(index > -1)
		{
			PlayerManager.Instance.partyMembers.RemoveAt(index);
			if(PlayerManager.Instance.partyMembers.Count <= 1)
				EndParty();
			else
				UpdatePartyMemberList();
		}
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
			if(PlayerManager.Instance.partyMembers[i].playerID != PhotonNetwork.player.ID)
				myPhotonView.RPC("UpdatePartyList", PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i].playerID), m.GetBuffer());
		}
		GUIManager.Instance.MainGUI.UpdatePartyMembers();
	}

	#endregion
	
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.LogError(player);
		bool isInMyParty = false;
		int index = 0;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].playerID == player.ID)
			{
				isInMyParty = true;
				index = i;
			}
		}

		if(isInMyParty)
		{
			PlayerManager.Instance.partyMembers.RemoveAt(index);
			if(PlayerManager.Instance.partyMembers[0].playerID == PhotonNetwork.player.ID)
			{
				if(PlayerManager.Instance.partyMembers.Count <= 1)
					EndParty();
				else
				{
					UpdatePartyMemberList();
				}
				if(PlayerManager.Instance.startedPartyChallenge)
					CancelPartyChallenge();
			}
		}

		/*if(PhotonNetwork.isMasterClient)
		{
			for (int j = 0; j < ArenaManagers.Count; j++) 
			{
				for (int i = 0; i < ArenaManagers[j].photonPlayers.Count; i++) 
				{
					if(ArenaManagers[j].photonPlayers[i] == player)
					{
						Debug.Log("disconnecting player" + player);
						//players.RemoveAt(i);
						myPhotonView.RPC("RemovePlayerAt", PhotonTargets.AllBuffered, j, player.ID);
						return;
					}
				}
			}
		}*/
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


