using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
				ArenaManagers.Add(Arenas[i].arenaInstances[s].GetComponent<ArenaManager>());
			}
		}

		for (int i = 0; i < ArenaManagers.Count; i++) 
		{
			ArenaManagers[i].ID = ArenaManagers.IndexOf(ArenaManagers[i]);
		}
	}

	//master
	[RPC]
	public void GetAvailableArena(string name, PhotonMessageInfo info)
	{
		for (int i = 0; i < Arenas.Count; i++) 
		{
			if(Arenas[i].arenaName == name)
			{
				for (int s = 0; s < Arenas[i].arenaStates.Count; s++) 
				{
					if(Arenas[i].arenaStates[s] == false)
					{
						myPhotonView.RPC("SetArenaState", PhotonTargets.AllBuffered, i, s, true);
						//Arenas[i].arenaStates[s] = true;
						myPhotonView.RPC("SpawnNewArena", info.sender, i, s);
						return;
					}
					if(Arenas[i].arenaStates[s] == true)
					{
						myPhotonView.RPC("SpawnNewArena", info.sender, i, s);
						return;
					}
				}
			}
		}
	}


	//target player
	[RPC]
	public void SpawnNewArena(int i, int j)
	{
		Debug.LogWarning("making new arena");
		//Arenas[i].arenaInstances[j].gameObject.SetActive(true);
		PlayerManager.Instance.GoToArena(Arenas[i].arenaInstances[j]);
	}

	/*[RPC]
	public void InitialiseArena(Zone zone)
	{
		zone.gameObject.SetActive(true);
	}*/

	//all buffered
	[RPC]
	public void SetArenaState(int i, int s, bool state)
	{
		Arenas[i].arenaStates[s] = state;
	}

	public void DecommissionArena(Zone instance)
	{
		for (int i = 0; i < Arenas.Count; i++) 
		{
			for (int s = 0; s < Arenas[i].arenaInstances.Count; i++) 
			{
				if(Arenas[i].arenaInstances[s] == instance)
				{
					myPhotonView.RPC("SetArenaState", PhotonTargets.AllBuffered, i, s, false);
					//Arenas[i].arenaStates[s] = false;
					return;
				}
			}
		}
	}

	/*public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.LogError(player);
	}

	public void CheckArenasForDisconnects(int id)
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
	public List<Zone> arenaInstances;
	public List<bool> arenaStates;
	public string arenaName;

}
