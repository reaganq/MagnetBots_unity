using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile{

	public string name;
	public int level;
	public int exp;
	public int baseVitalityLevel;
	public int bonusVitalityLevel;
	public int totalVitalityLevel{get{return baseVitalityLevel+bonusVitalityLevel;}}
	//public int curVitalityExp;
	public int baseStrengthLevel;
	public int bonusStrengthLevel;
	public int totalStrengthLevel{get{return baseStrengthLevel+bonusStrengthLevel;}}
	//public int curStrengthExp; 
	public int bonusDefenseLevel;
	public int baseDefenseLevel;
	public int totalDefenseLevel{get{return baseDefenseLevel+bonusDefenseLevel;}}
	//public int curDefenseExp;
	public int energyState;

	public List<RPGBadge> badges;

	public void AddExp(int amount)
	{
		exp += amount;
		if(exp >= 100)
		{
			level ++;
			exp -= 100;
		}
	}

	public void AddEnergy(int amount)
	{
		energyState += amount;
		if(energyState >= 100)
			energyState = 100;
	}

	public void AddArmorStats(ArmorStatsSet set)
	{
		for (int i = 0; i < set.armorStats.Count; i++) {
			if(set.armorStats[i].armorStatsType == ArmorStatsType.vitality)
				bonusVitalityLevel += set.armorStats[i].armorStatsValue;
			else if(set.armorStats[i].armorStatsType == ArmorStatsType.defence)
				bonusDefenseLevel += set.armorStats[i].armorStatsValue;
			else if(set.armorStats[i].armorStatsType == ArmorStatsType.strength)
				bonusStrengthLevel += set.armorStats[i].armorStatsValue;
		}
		Debug.Log("vit: " + bonusVitalityLevel + " str: " + bonusStrengthLevel + " def: " + bonusDefenseLevel);
		Debug.Log("Tvit: " + totalVitalityLevel + " Tstr: " + totalStrengthLevel + " Tdef: " + totalDefenseLevel);
	}

	public void RemoveArmorStats(ArmorStatsSet set)
	{
		for (int i = 0; i < set.armorStats.Count; i++) {
			if(set.armorStats[i].armorStatsType == ArmorStatsType.vitality)
				bonusVitalityLevel -= set.armorStats[i].armorStatsValue;
			else if(set.armorStats[i].armorStatsType == ArmorStatsType.defence)
				bonusDefenseLevel -= set.armorStats[i].armorStatsValue;
			else if(set.armorStats[i].armorStatsType == ArmorStatsType.strength)
				bonusStrengthLevel -= set.armorStats[i].armorStatsValue;
		}
		Debug.Log("vit: " + bonusVitalityLevel + " str: " + bonusStrengthLevel + " def: " + bonusDefenseLevel);
		Debug.Log("Tvit: " + totalVitalityLevel + " Tstr: " + totalStrengthLevel + " Tdef: " + totalDefenseLevel);
	}
	//public List<Achievement> achievements;
}

[System.Serializable]
public class ParsePlayerProfileData{
	public string name;
	public int level;
	public int exp;
	public int baseVitalityLevel;
	//public int curVitalityExp;
	public int baseStrengthLevel;
	//public int curStrengthExp;
	public int baseDefenseLevel;
	//public int curDefenseExp;
	public int energyState;
}

public enum PlayerEnergyState
{
	empty,
	low,
	enough,
	full,
}