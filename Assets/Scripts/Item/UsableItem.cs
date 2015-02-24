using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

//[XmlInclude(typeof(Effect))]
//[XmlInclude(typeof(Condition))]
public class UsableItem : BasicItem
{
 //public List<Effect> Effects;
 //public List<Condition> UseConditions;
 	public bool IsUsable;
	public bool IsEquippable;
	public bool IsUpgradeable;
    public ItemType ItemCategory;
// public UsageSkillType UsageSkill;
 //public float Recharge; 
 	
	public EquipmentSlots EquipmentSlotIndex; 
	public bool isLimitedUse;

 //public Texture2D Icon;
 
 
 public UsableItem()
 {
     //Effects = new List<Effect>();
     //UseConditions = new List<Condition>();
     IconPath = string.Empty;
        AtlasName = string.Empty;
 }
 
    public bool CheckCondition()
 {
     /*foreach(Condition condition in UseConditions)
     {
            if (!condition.Validate(player))
             return false;
         
     }*/
     return true;
 }

public virtual bool CheckRequirements()
 {
        return CheckCondition();
 }
 
 /*public void LoadIcon()
 {
     if (Icon == null)
         Icon = (Texture2D)Resources.Load(IconPath, typeof(Texture2D));
 }*/
}

public enum ItemType
{
    Armor = 0,
    Food = 1,
    RewardChest = 2,
    Quest = 3,
	Normal = 4,
	Currency = 5,
	UpgradeMaterials = 6,
	Toys = 7,
	Other = 8,
	NakedArmor = 9,
	Badge = 10,
	DanceMove = 11,
}

public enum UsageSkillType
{
	 Combat = 0,
	 Spell = 1,
	 Item = 2
}

public enum EquipmentSlots
{
	Head = 0,
	Body = 1,
	ArmL = 2,
	ArmR = 3,
	Legs = 4,
	Face = 5,
	None = 6
}

