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
    public ItemType ItemCategory;
// public UsageSkillType UsageSkill;
 //public float Recharge; 
 public string IconPath;
    public string AtlasName;
 
 //public Texture2D Icon;
 
 
 public UsableItem()
 {
     //Effects = new List<Effect>();
     //UseConditions = new List<Condition>();
     IconPath = string.Empty;
        AtlasName = string.Empty;
 }
 
 public virtual bool UseItem()
 {
     return false;
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
    Chest = 2,
    Quest = 3,
}

public enum UsageSkillType
{
 Combat = 0,
 Spell = 1,
 Item = 2
}

