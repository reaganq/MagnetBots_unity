using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[Serializable]
//[XmlInclude(typeof(Effect))]
public class RPGArmor : Equiped 
{
    public bool HasAbility;
    public string AbilityString;
    public string AbilityIconPath;
	public string AbilityAtlasPath;
	public string headPortraitPath;
	public List<ArmorStatsSet> armorStatsSets;

 //public List<Effect> EffectsOnHit;
 
	public RPGArmor()
	{
		preffix = "ARMOR";
		Name = string.Empty;
		AbilityAtlasPath = "Atlases/SkillIcons/SkillIconsAtlas";
		AbilityString = "Use this";
		IsEquippable = true;
		IsUsable = false;
		IsUpgradeable = true;
		AtlasName = "Atlases/Armor/ArmorAtlas";
		Stackable = true;
		maxLevel = 3;
		armorStatsSets = new List<ArmorStatsSet>();
	//EffectsOnHit = new List<Effect>();
	}

	public override void Use()
	{
		base.Use();
	}
}

[Serializable]
public class ArmorStat
{
	public ArmorStatsType armorStatsType;
	public int armorStatsValue;

	public ArmorStat()
	{
	}
}

public enum ArmorStatsType
{
	defence,
	strength,
	vitality
}

[Serializable]
public class ArmorStatsSet
{
	public List<ArmorStat> armorStats;

	public ArmorStatsSet()
	{
		armorStats = new List<ArmorStat>();
	}
}