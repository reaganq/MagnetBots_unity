using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerProfile{

	public string name;
	public int level;
	public int exp;
	public int questPoints;
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

	public PlayerProfile()
	{
		level = 1;
		baseVitalityLevel = 10;
		baseDefenseLevel = 10;
		baseStrengthLevel = 10;
		questPoints = 0;
		badges = new List<RPGBadge>();
	}

	public void UpdateProfileFromParse(ParsePlayerProfileData data)
	{
		name = data.name;
		level = data.level;
		exp = data.exp;
		baseVitalityLevel = data.baseVitalityLevel;
		baseStrengthLevel = data.baseStrengthLevel;
		baseDefenseLevel = data.baseDefenseLevel;
		energyState = data.energyState;
		badges.Clear();
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
		//Debug.Log("vit: " + bonusVitalityLevel + " str: " + bonusStrengthLevel + " def: " + bonusDefenseLevel);
		//Debug.Log("Tvit: " + totalVitalityLevel + " Tstr: " + totalStrengthLevel + " Tdef: " + totalDefenseLevel);
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
		//Debug.Log("vit: " + bonusVitalityLevel + " str: " + bonusStrengthLevel + " def: " + bonusDefenseLevel);
		//Debug.Log("Tvit: " + totalVitalityLevel + " Tstr: " + totalStrengthLevel + " Tdef: " + totalDefenseLevel);
	}

	public void UpdateEquippedItems()
	{
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
	//public List<ParseEquippedItem> equipItems;
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
		badgeIDs = new List<int>();
		for (int i = 0; i < profile.badges.Count; i++) {
			badgeIDs.Add(profile.badges[i].ID);
		}
	}
}

[System.Serializable]
public class PlayerProfileDisplayData
{
	public string name;
	public List<ParseEquippedItem> equips;
	public int baseHealth;
	public int bonusHealth;
	public int baseStrength;
	public int bonusStrength;
	public int baseDefense;
	public int bonusDefense;
	public List<int> badgeIDs;
	public int questPoints;
	public int citizenPoints;
	public PlayerProfileDisplayData()
	{
		equips = new List<ParseEquippedItem>();
		badgeIDs = new List<int>();
	}

	public void CreatePlayerProfileDisplayData()
	{
		name = PlayerManager.Instance.Hero.profile.name;
		equips.Clear();
		if(PlayerManager.Instance.Hero.Equip.EquippedFace != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedFace));
		if(PlayerManager.Instance.Hero.Equip.EquippedHead != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedHead));
		if(PlayerManager.Instance.Hero.Equip.EquippedBody != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedBody));
		if(PlayerManager.Instance.Hero.Equip.EquippedArmL != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedArmL));
		if(PlayerManager.Instance.Hero.Equip.EquippedArmR != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedArmR));
		if(PlayerManager.Instance.Hero.Equip.EquippedLegs != null)
			equips.Add(new ParseEquippedItem(PlayerManager.Instance.Hero.Equip.EquippedLegs));

		Debug.Log("equips has: "+ equips.Count);
		baseHealth = PlayerManager.Instance.Hero.profile.baseVitalityLevel;
		baseStrength = PlayerManager.Instance.Hero.profile.baseStrengthLevel;
		baseDefense = PlayerManager.Instance.Hero.profile.baseDefenseLevel;
		bonusHealth = PlayerManager.Instance.Hero.profile.bonusVitalityLevel;
		bonusDefense = PlayerManager.Instance.Hero.profile.bonusDefenseLevel;
		bonusStrength = PlayerManager.Instance.Hero.profile.bonusStrengthLevel;
		badgeIDs.Clear();
		for (int i = 0; i < PlayerManager.Instance.Hero.profile.badges.Count; i++) {
			badgeIDs.Add(PlayerManager.Instance.Hero.profile.badges[i].ID);
		}
		questPoints = PlayerManager.Instance.Hero.profile.questPoints;
		citizenPoints = PlayerManager.Instance.Hero.CitizenPoints;
	}
}

public enum PlayerEnergyState
{
	empty,
	low,
	enough,
	full,
}