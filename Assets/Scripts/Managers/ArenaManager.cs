using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaManager : Photon.MonoBehaviour {

    public Transform[] arenaSpawnPos;
    public Transform enemySpawnPos;
    public string arenaName;
    public List<string> enemyPrefabStrings;
	public List<string> enemies;
    public SimpleFSM enemy;
	public PhotonView myPhotonView;
	public int ID;
	public List<CharacterStatus> players;
	public List<int> playerIDs;
	//public List<ArenaPlayer> players;

	public void Start()
	{
		myPhotonView = GetComponent<PhotonView>();
	}

	//Master
	[RPC]
	public void Initialise(string name, int id, int viewid)
    {
		myPhotonView.RPC("AddPlayer", PhotonTargets.All, id);

		if(PhotonNetwork.isMasterClient)
		{
			if(enemy != null)
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
    }

	//target player
	[RPC]
	public void ChangeEnemyOwner()
	{
		if(PlayerManager.Instance.ActiveArena == this && enemy!=null)
		{
			enemy.myPhotonView.RPC("ChangeOwner", PhotonTargets.AllBuffered, PhotonNetwork.AllocateViewID());
			Debug.Log("change owner");
		}
	}

	//all
	[RPC]
	public void AddPlayer(int id)
	{
		Debug.Log("adding player with id: "+id);
		PhotonView view = PhotonView.Find(id);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		//ArenaPlayer ap = new ArenaPlayer();
		//ap.playerCS = playerObject;
		//ap.playerID = view.ownerId;
		//players.Add(ap);
		players.Add(playerObject);
		playerIDs.Add(view.ownerId);
	}

	//all buffered
	[RPC]
	public void RemovePlayer(int id)
	{
		PhotonView view = PhotonView.Find(id);
		CharacterStatus playerObject = view.GetComponent<CharacterStatus>();
		//ArenaPlayer ap = new ArenaPlayer();
		//ap.playerCS = playerObject;
		//ap.playerID = view.ownerId;
		//players.Remove(ap);
		players.Remove(playerObject);
		playerIDs.Remove(view.ownerId);
		if(PhotonNetwork.isMasterClient)
		{
			if(players.Count == 0)
			{
				Debug.Log("clear");
				//PhotonView enemyView = enemy.GetComponent<PhotonView>();
				/*if(enemyView.owner != null)
				{
					myPhotonView.RPC("ClearEnemies", enemyView.owner);
				}
				else*/
					//myPhotonView.RPC("ClearEnemies", PhotonTargets.MasterClient);
				ClearEnemies();
				PlayerManager.Instance.ActiveWorld.DecommissionArena(GetComponent<Zone>());
				//ClearEnemies();
			}
			else
			{
				if(enemy.GetComponent<SimpleFSM>().ownerID == view.ownerId)
				{
					Debug.Log("here alloc");
					//PhotonNetwork.UnAllocateViewID(enemyView.viewID);
					myPhotonView.RPC("ChangeEnemyOwner", PhotonPlayer.Find(playerIDs[0]));
				}
			}
		}
	}

	//all buffered
	[RPC]
	public void RemovePlayerAt(int id)
	{
		for (int i = 0; i <playerIDs.Count; i++) 
		{
			if(playerIDs[i] == id)
			{
				playerIDs.RemoveAt(i);
				players.RemoveAt(i);
				Debug.Log("disconnected player" + id);
			}
		}
		if(PhotonNetwork.isMasterClient)
		{
			PhotonView enemyView = enemy.GetComponent<PhotonView>();

			if(players.Count == 0)
			{
				Debug.Log("clear");

				//if(enemyView.owner != null)
					//myPhotonView.RPC("ClearEnemies", enemyView.owner);
				//else
					//myPhotonView.RPC("ClearEnemies", PhotonTargets.MasterClient);
				ClearEnemies();
				PlayerManager.Instance.ActiveWorld.DecommissionArena(GetComponent<Zone>());
			}
			else
			{
				if(enemyView.owner == null && enemy.GetComponent<SimpleFSM>().ownerID == id)
				{
					Debug.Log("here alloc");
					//PhotonNetwork.UnAllocateViewID(enemyView.viewID);
					myPhotonView.RPC("ChangeEnemyOwner", PhotonPlayer.Find(playerIDs[0]));
				}
			}
		}
	}

	public void EndSession(int id)
	{
		myPhotonView.RPC("RemovePlayer", PhotonTargets.AllBuffered, id);
	}

	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("ONPHOtonplayerdisconnected");
		if(PhotonNetwork.isMasterClient)
		{
			for (int i = 0; i < playerIDs.Count; i++) 
			{
				if(playerIDs[i] == player.ID)
				{
					Debug.Log("disconnecting player" + player);
					//players.RemoveAt(i);
					myPhotonView.RPC("RemovePlayerAt", PhotonTargets.AllBuffered, player.ID);
				}
			}
		}
	}

	/*public void RefreshPlayerList(int id)
	{
		for (int i = 0; i < players.Count; i++) 
		{
			if(players[i] == null)
				players.RemoveAt(i);
		}
	}*/


	//[RPC]
	public void ClearEnemies()
	{
		int id = enemy.InitViewID;
		PhotonNetwork.RemoveRPCs(enemy.myPhotonView);
		enemy.myPhotonView.RPC("RevertOwner", PhotonTargets.All);

		PhotonNetwork.Destroy(enemy.gameObject);
		PhotonNetwork.UnAllocateViewID(id);

		Debug.Log("end session arena");
	}

}

/*public class ArenaPlayer
{
	public CharacterStatus playerCS;
	public int playerID;
}*/
