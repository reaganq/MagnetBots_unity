using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EquipedItem : ItemInWorld
{
 public EquipmentSlots Slot;
 
 public EquipedItem() : base()
 {
     //Slots = new RPGEquipmentSlot();
 }
}
