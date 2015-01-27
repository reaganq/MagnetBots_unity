using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Jukebox{

	public List<DanceMove> danceMoves;

	public Jukebox()
	{
		danceMoves = new List<DanceMove>();
	}

	public void AddDanceMove(int id)
	{
		danceMoves.Add(Storage.LoadById<DanceMove>(id, new DanceMove()));
	}
}

