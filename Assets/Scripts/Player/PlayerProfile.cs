using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile{

	public string name;
	public int magneticLevel;
	public int vitalityLevel;
	public int curVitalityExp;
	public int strengthLevel;
	public int curStrengthExp;
	public int defenseLevel;
	public int curDefenseExp;
	public PlayerEnergyState energyState;
	//public List<Achievement> achievements;
}

public class Achievement
{
}

public enum PlayerEnergyState
{
	empty,
	low,
	enough,
	full,
}