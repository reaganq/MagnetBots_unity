using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

public class ItemInWorld 
{
 public string UniqueItemId;
 public int CurrentAmount;
 public int Level;
 //public float CurrentDurability;
 //public float PriceModifier;
 
 [XmlIgnore]
 public RPGItem rpgItem;
 
 public bool IsItemLoaded()
 {
     if (!string.IsNullOrEmpty(UniqueItemId) && rpgItem == null)
     {
         if (UniqueItemId.IndexOf("ITEM") != -1)
         {
             int id = Convert.ToInt32(UniqueItemId.Replace("ITEM", string.Empty));
             rpgItem = Storage.LoadById<RPGItem>(id, new RPGItem());
                
         }
         if (UniqueItemId.IndexOf("ARMOR") != -1)
         {
             int id = Convert.ToInt32(UniqueItemId.Replace("ARMOR", string.Empty));
             rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
         }
     }
     
     if (rpgItem != null && !string.IsNullOrEmpty(rpgItem.Name))
     {
         //rpgItem.LoadIcon();
         return true;
     }
     return false;
 }
 
 /*public bool IsItemLoaded(float priceModifier)
 {
     if (!string.IsNullOrEmpty(UniqueItemId) && rpgItem != null && !string.IsNullOrEmpty(rpgItem.Name))
     {
         rpgItem.LoadIcon();
         PriceModifier = priceModifier;
         return true;
     }
     
     return false;
 }*/
}
