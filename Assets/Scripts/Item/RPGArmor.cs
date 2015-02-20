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

 //public List<Effect> EffectsOnHit;
 
	public RPGArmor()
	{
		preffix = "ARMOR";
		Name = string.Empty;
		FBXName = new List<string>();
		AbilityIconPath = "AbilityIcon/";
		AbilityString = "Use this";
		IsEquippable = true;
		IsUsable = false;
		IsUpgradeable = true;
		AtlasName = "Atlases/Armor/";
		Stackable = true;
	//EffectsOnHit = new List<Effect>();
	}

	public override void Use()
	{
		base.Use();
	}
}
