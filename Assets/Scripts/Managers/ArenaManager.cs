using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Zone {

	public bool isArenaActive;
    public Transform[] arenaSpawnPos;
    public Transform enemySpawnPos;
	public RPGEnemy rpgEnemy;
	public int ownerViewID;
	public int ownerID;
    public List<SimpleFSM> enemyFSMs;
	public int arenaID;
	public ArenaState arenaState;
	public List<PhotonPlayer> admittedPlayers;
	//public List<ArenaPlayer> players;

	public void Start()
	{
		myPhotonView = GetComponent<PhotonView>();
		zoneType = ZoneType.arena;
	}

	public override void EnterZone ()
	{
		base.EnterZone ();
		//set guimanager to battlemode
	}

	public void PreloadArena(List<PhotonPlayer> admissionPlayers, int enemyID, int newOwnerID, int newViewID )
	
	{
		arenaState = ArenaState.waitingForPlayersToLoad;
		Debug.Log("preloading Arena data");
		admittedPlayers = admissionPlayers;
		rpgEnemy = Storage.LoadById<RPGEnemy>(enemyID, new RPGEnemy());
		ownerViewID = newViewID;
		ownerID = newOwnerID;
		SpawnEnemyWave(0);

		//instantiate enemy
	}

	public void SpawnEnemyWave(int index)
	{
		if(rpgEnemy != null)
		{
			for (int i = 0; i < rpgEnemy.PrefabPaths.Count; i++) 
			{
				GameObject enemyUnit = Instantiate(Resources.Load(rpgEnemy.PrefabPaths[index]), enemySpawnPos.position, enemySpawnPos.rotation) as GameObject;
				enemyUnit.transform.parent = zoneObject.transform;
				SimpleFSM fsm = enemyUnit.GetComponent<SimpleFSM>();
				enemyFSMs.Add(fsm);
				fsm.Initialise(this, ownerViewID);
				//fsm.arena = this;
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

	[RPC]
	public override void NetworkAddPlayer(int csViewID, PhotonMessageInfo info)
	{
		base.NetworkAddPlayer(csViewID, info);
		if(PhotonNetwork.isMasterClient)
		{
			if(CheckIfAllPlayersArePresent())
			{
				//start arena
				BeginPreBattle();
			}
		}
		//check if the last player has loaded

	}

	public bool CheckIfAllPlayersArePresent()
	{
		if(admittedPlayers.Count > photonPlayers.Count)
			return false;
		return true;
	}

	public void BeginPreBattle()
	{
		myPhotonView.RPC("EnterArenaState", PhotonTargets.All, (int)ArenaState.preBattle);
		//chceck for quest messages
		//if... else begin battle
		for (int i = 0; i < photonPlayers.Count; i++) {
			myPhotonView.RPC("BeginBattle", photonPlayers[i] );
		}
	}

	[RPC]
	public void EnterArenaState(int stateID)
	{
		arenaState = (ArenaState)stateID;
	}

	[RPC]
	public void BeginBattle()
	{
		StartCoroutine(StartBattleSequence());
		//play countdown
	}

	public IEnumerator StartBattleSequence()
	{
		Debug.Log("ready");
		yield return new WaitForSeconds(1);
		Debug.Log("3,2,1");
		yield return new WaitForSeconds(1);
		Debug.Log("start!");
		arenaState = ArenaState.battle;
		for (int i = 0; i < enemyFSMs.Count; i++) {
			enemyFSMs[i].StartBattle();
		}
	}

	public void CleanUp()
	{
		myPhotonView.RPC("NetworkCleanUp", PhotonTargets.All);
		PlayerManager.Instance.ActiveWorld.EndSession(this);
	}

	[RPC]
	public void NetworkCleanUp()
	{
		Debug.Log("clean up arena");
		for (int i = 0; i < enemyFSMs.Count; i++) 
		{
			Destroy(enemyFSMs[i].gameObject);
		}
		rpgEnemy = null;
		ownerID = 0;
		ownerViewID = 0;
		playerCharacterStatuses.Clear();
		photonPlayers.Clear();
		admittedPlayers.Clear();
		enemyFSMs.Clear();
		arenaState = ArenaState.inactive;
	}

	//owner of ai
	public void CheckDeathStatus()
	{
		if(arenaState != ArenaState.battle)
			return;

		Debug.Log("checking death status of arena");
		for (int i = 0; i < enemyFSMs.Count; i++) 
		{
			if(enemyFSMs[i].myStatus.isAlive())
				return;
		}

		myPhotonView.RPC("EnterArenaState", PhotonTargets.All, (int)ArenaState.battleEnd);
			/*for (int i = 0; i < players.Count; i++) {
				PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("GiveArenaRewards", players[i].myPhotonView.owner, arenaID);
				Debug.Log("give rewards to: " + players[i].myPhotonView.ownerId);
			}

		}*/
		//play death conversation
		for (int i = 0; i < photonPlayers.Count; i++) {
			myPhotonView.RPC("WinScenario", photonPlayers[i]);
				}
	}

	public void CheckPlayerDeathStatus()
	{
		if(arenaState != ArenaState.battle)
			return;

		for (int i = 0; i < playerCharacterStatuses.Count; i++) {
			if(playerCharacterStatuses[i].isAlive())
				return;
		}

		Debug.Log("all dead");
		myPhotonView.RPC("EnterArenaState", PhotonTargets.All, (int)ArenaState.battleEnd);
		for (int i = 0; i < photonPlayers.Count; i++) {
			myPhotonView.RPC("LoseScenario", photonPlayers[i]);
		}
	}



	[RPC]
	public void WinScenario()
	{
		StartCoroutine(WinSequence());
	}

	public IEnumerator WinSequence()
	{
		Debug.LogError("WIN ARENA");
		yield return null;
		GiveRewards();
	}

	[RPC]
	public void LoseScenario()
	{
		StartCoroutine(LoseSequence());
		for (int i = 0; i < enemyFSMs.Count; i++) {
			enemyFSMs[i].Win();
		}
	}

	public IEnumerator LoseSequence()
	{
		Debug.LogError("LOSE ARENA");
		yield return null;
		GUIManager.Instance.DisplayArenaFailure();
	}

	public void GiveRewards()
	{
		//if quest, give quest loot
		GUIManager.Instance.DisplayArenaRewards(PlayerManager.Instance.GiveRewards(rpgEnemy.Loots, 0));
	}

	[RPC]
	public override void NetworkRemovePlayer(int csViewID, PhotonMessageInfo info)
	{
		base.NetworkRemovePlayer(csViewID, info);
		Debug.Log("removed player " + csViewID + "from this arena");
		if(PhotonNetwork.isMasterClient)
		{
			for (int i = 0; i < playerCharacterStatuses.Count; i++) {
				if(playerCharacterStatuses[i] == null)
				{
					Debug.Log("found null cs");
					playerCharacterStatuses.RemoveAt(i);
					break;
				}
			}
			if(playerCharacterStatuses.Count <= 0)
				CleanUp();
		}
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		if(photonPlayerIDs.Contains(player.ID))
		{
			base.OnPhotonPlayerDisconnected(player);
			if(PhotonNetwork.isMasterClient)
			{
				Debug.Log("shutting down this arena because of disconnect");
				if(arenaState == ArenaState.battle || arenaState == ArenaState.preBattle)
				{
				for (int i = 0; i < photonPlayers.Count; i++) {
					myPhotonView.RPC("LoseScenario", photonPlayers[i]);
				}
				}
			}
		}
	}

	/*[RPC]
	public void NetworkForceEnd()
	{
		
	}*/
}

public enum ArenaState
{
	inactive,
	waitingForPlayersToLoad,
	preBattle,
	battle,
	battleEnd
}

