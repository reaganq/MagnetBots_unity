using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile{

	public string name;
	public int level;
	public int questPoints;
	public PlayerEnergyState energyState;
	public List<Achievement> achievements;

}

public class Achievement
{
}
