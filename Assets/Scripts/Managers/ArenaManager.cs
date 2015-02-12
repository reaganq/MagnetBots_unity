using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Zone {

    public Transform[] arenaSpawnPos;
    public Transform enemySpawnPos;
	public RPGEnemy rpgEnemy;
	public int ownerViewID;
	public int ownerID;
    public List<SimpleFSM> enemyFSMs;
	public PhotonView myPhotonView;
	public int ID;
	public List<CharacterStatus> players;
	public List<int> playerIDs;
	public List<PhotonPlayer> admittedPlayers;
	//public List<ArenaPlayer> players;

	public void Start()
	{
		myPhotonView = GetComponent<PhotonView>();
		type = ZoneType.arena;
	}

	public void SpawnEnemyWave(int index)
	{
		if(rpgEnemy != null)
		{
			for (int i = 0; i < rpgEnemy.PrefabPaths.Count; i++) 
			{
				Debug.Log("spawning enemy wave" + index);
				GameObject enemyUnit = Instantiate(Resources.Load(rpgEnemy.PrefabPaths[index]), enemySpawnPos.position, enemySpawnPos.rotation) as GameObject;
				PhotonView nView = enemyUnit.GetComponent<PhotonView>();
				nView.viewID = ownerViewID;
				SimpleFSM fsm = enemyUnit.GetComponent<SimpleFSM>();
				enemyFSMs.Add(fsm);
				fsm.arena = this;
			}
		}
	}

	public void UpdateNewOwner()
	{
		for (int i = 0; i < enemyFSMs.Count; i++) 
		{
			enemyFSMs[i].myPhotonView.viewID = ownerViewID;
		}
	}

	//Master
	/*[RPC]
	public void Initialise(int enemyID, int id, int viewid)
    {
		myPhotonView.RPC("AddPlayer", PhotonTargets.All, id);

		if(PhotonNetwork.isMasterClient)
		{
			if(enemyFSM != null)
				return;

			int index = enemies.IndexOf(name);
			Debug.Log(index);
			if(index >=0)
			{
				GameObject newEnemy = PhotonNetwork.InstantiateSceneObject(enemyPrefabStrings[index], enemySpawnPos.position, enemySpawnPos.rotation, 0, null) as GameObject;
				PhotonNetwork.RemoveRPCs(newEnemy.GetComponent<PhotonView>());
				newEnemy.GetComponent<PhotonView>().RPC("SetupArena", PhotonTargets.AllBuffered, ID, viewid);
			}
		}
    }*/

	//target player
	/*[RPC]
	public void ChangeEnemyOwner()
	{
		if(PlayerManager.Instance.ActiveArena == this && enemyFSMs.Count > 0)
		{
			enemyFSMs[0].myPhotonView.RPC("ChangeOwner", PhotonTargets.AllBuffered, PhotonNetwork.AllocateViewID());
			Debug.Log("change owner");
		}
	}*/

	//all




	/*public void RefreshPlayerList(int id)
	{
		for (int i = 0; i < players.Count; i++) 
		{
			if(players[i] == null)
				players.RemoveAt(i);
		}
	}*/



	public void CleanUp()
	{
		Debug.Log("clean up");
		/*int id = enemyFSM.InitViewID;
		PhotonNetwork.RemoveRPCs(enemyFSM.myPhotonView);
		enemyFSM.myPhotonView.RPC("RevertOwner", PhotonTargets.All);

		PhotonNetwork.Destroy(enemyFSM.gameObject);
		PhotonNetwork.UnAllocateViewID(id);

		Debug.Log("end session arena");*/
		for (int i = 0; i < enemyFSMs.Count; i++) 
		{
			Destroy(enemyFSMs[i].gameObject);
		}
		rpgEnemy = null;
		ownerID = 0;
		ownerViewID = 0;
		players.Clear();
		playerIDs.Clear();
		admittedPlayers.Clear();
		enemyFSMs.Clear();
	}

	public void CheckDeathStatus()
	{
		for (int i = 0; i < enemyFSMs.Count; i++) 
		{
			if(enemyFSMs[i].myStatus.isAlive())
				return;
		}

		if(PhotonNetwork.player.ID == ownerID)
		{
			for (int i = 0; i < players.Count; i++) {
				PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("GiveArenaRewards", players[i].myPhotonView.owner, ID);
				Debug.Log("give rewards to: " + players[i].myPhotonView.ownerId);
			}

		}
	}

	public void GiveRewards()
	{
		PlayerManager.Instance.GiveRewards(rpgEnemy.Loots);
	}
}

