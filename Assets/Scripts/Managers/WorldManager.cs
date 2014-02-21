using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {

	public GameObject[] Arenas;
	public bool[] ArenaStates;
	public Zone DefaultZone;

	public GameObject GetAvailableArena()
	{
		for (int i = 0; i < ArenaStates.Length; i++) 
		{
			if(!ArenaStates[i])
			{
				return Arenas[i];
			}
		}
		return null;
	}

}
