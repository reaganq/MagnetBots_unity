using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

//[XmlInclude(typeof(RPGEquipmentSlot))]
//[XmlInclude(typeof(Effect))]
//[XmlInclude(typeof(Condition))]
public class Equiped : RPGItem {
  
 //public int Durability;
 //public List<Condition> Conditions;
 //public List<Effect> WornEffects;
 public EquipmentSlots EquipmentSlotIndex;
 public string FBXName;
 
 [XmlIgnore]
 public int CurrentAmount;
 
 public Equiped()
 {
     //EquipmentSlot = new RPGEquipmentSlot();
     //WornEffects = new List<Effect>();
     //Conditions = new List<Condition>();
     FBXName = string.Empty;
 }
 
 //[XmlIgnore]
 
 public bool CanYouEquip()
 {
        /*foreach (Condition condition in Conditions)
        {
            if (!condition.Validate(player))
                return false;
        }*/
        return true;
 }
    

}
public enum EquipmentSlots
{
    Head = 0,
    Body = 1,
    ArmL = 2,
    ArmR = 3,
    Legs = 4,
    Face = 5
}

