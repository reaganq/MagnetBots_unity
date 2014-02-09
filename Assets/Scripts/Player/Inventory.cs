using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Inventory  : BasicInventory
{
   

 public Inventory() : base()
 {
     maximumItems = 10;
 }

 /*public bool DoYouHaveSpecForTheseItems(List<Equiped> items)
 {
     int size = 0;
     foreach(Equiped equiped in items)
     {
         if (equiped.Stackable)
         {
             bool founded = false;
             foreach(InventoryItem item in Items)
             {
                 if (item.UniqueItemId == equiped.UniqueId && item.CurrentAmount < item.rpgItem.MaximumStack)
                     founded = true;
             }
             if (!founded)
                 size++;
         }
         else
             size++;
     }
     if (maximumItems < Items.Count + size)
         return false;
     else
         return true;
 }*/
 
 public void DropItem(InventoryItem item)
 {
     foreach(InventoryItem i in Items)
     {
         if (item.ItemInventoryIndex == i.ItemInventoryIndex)
         {
             Items.Remove(i);
             FinalizeInventoryOperation();
             break;
         }
     }
 }
 
 protected override void FinalizeInventoryOperation()
 {
        //player.Hero.Quest.CheckInventoryItem(player);
 }
 
 public bool EquipItem(InventoryItem item)
 {
     if (!item.IsItemEquippable)
         return false;
     /*if (!player.Hero.Settings.EquipedItemInInventory)
     {
         foreach(InventoryItem i in Items)
         {
             if (item == i)
             {
                 Items.Remove(i);
                 break;
             }
         }
     }*/
        if(PlayerManager.Instance.Hero.Equip.EquipItem(item))
        {
            //player.Hero.Equip.EquipItem(item, player);
            item.IsItemEquipped = true;
            return true;
        }
        else
            return false;
 }
    
    public bool UnequipItem(EquipedItem item)
    {
        foreach(InventoryItem i in Items)
        {
            if(i.UniqueItemId == item.UniqueItemId)
                i.IsItemEquipped = false;
        }
        
        return true;
    }
    
}

