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

	public List<EquipedItem> equippedItems = new List<EquipedItem>();
	public List<RPGBadge> badges = new List<RPGBadge>();

	public void UpdateProfileFromParse(ParsePlayerProfileData data)
	{
		name = data.name;
		level = data.level;
		exp = data.exp;
		baseVitalityLevel = data.baseVitalityLevel;
		baseStrengthLevel = data.baseStrengthLevel;
		baseDefenseLevel = data.baseDefenseLevel;
		energyState = data.energyState;
		equippedItems.Clear();
		badges.Clear();
		for (int i = 0; i < data.equipItems.Count; i++) {
			equippedItems.Add(new EquipedItem(data.equipItems[i].uniqueItemId, data.equipItems[i].level));
		}
		for (int i = 0; i < data.badgeIDs.Count; i++) {
			badges.Add(Storage.LoadById<RPGBadge>(data.badgeIDs[i], new RPGBadge()));
		}
	}

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

	public void UpdateEquippedItems(List<EquipedItem> items)
	{
		equippedItems = items;
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
	public List<ParseEquippedItem> equipItems;
	public List<int> badgeIDs;

	public ParsePlayerProfileData()
	{
	}

	public ParsePlayerProfileData(PlayerProfile profile)
	{
		name = profile.name;
		level = profile.level;
		exp = profile.exp;
		baseVitalityLevel = profile.baseVitalityLevel;
		baseStrengthLevel = profile.baseStrengthLevel;
		baseDefenseLevel = profile.baseDefenseLevel;
		energyState = profile.energyState;
		equipItems = new List<ParseEquippedItem>();
		for (int i = 0; i < profile.equippedItems.Count; i++) {
			equipItems.Add(new ParseEquippedItem(profile.equippedItems[i]));
		}
		badgeIDs = new List<int>();
		for (int i = 0; i < profile.badges.Count; i++) {
			badgeIDs.Add(profile.badges[i].ID);
		}
	}
}

public enum PlayerEnergyState
{
	empty,
	low,
	enough,
	full,
}