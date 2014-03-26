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

 //public List<Effect> EffectsOnHit;
 
	public RPGArmor()
	{
		preffix = "ARMOR";
		Name = string.Empty;
		FBXName = "Armor/";
		AbilityIconPath = "AbilityIcon/";
		AbilityString = "Use this";
		IsEquippable = true;
		IsUsable = false;
	//EffectsOnHit = new List<Effect>();
	}
}
