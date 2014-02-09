using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlInclude(typeof(Equiped))]
public class Equipment
{
 //public int MinimumRange;
 //public int MaximumRange;
 //private bool NeedAmmo;
    public EquipedItem EquippedHead;
    public EquipedItem EquippedBody;
    public EquipedItem EquippedArmL;
    public EquipedItem EquippedArmR;
    public EquipedItem EquippedLegs;
    public EquipedItem EquippedFace;
    
    //public List<EquipedItem> Items;
 
 //[XmlIgnore]
 //public RPGWeapon Weapon;

 public Equipment()
 {
        //Items = new List<EquipedItem>();
        EquippedHead = new EquipedItem();
        EquippedBody = new EquipedItem();
        EquippedArmL = new EquipedItem();
        EquippedArmR = new EquipedItem();
        EquippedLegs = new EquipedItem();
        EquippedFace = new EquipedItem();
     //Weapon = new RPGWeapon();
 }

 
 /*public List<Effect> GetEffects()
 {
     List<Effect> effects = new List<Effect>();
     
     foreach(EquipedItem equiped in Items)
     {
         if (equiped.rpgItem is Equiped)
         {
             Equiped equip = (Equiped)equiped.rpgItem;
             foreach(Effect e in equip.WornEffects)
             {
                 effects.Add(e);
             }
         }   
     }
     return effects;
 }*/

/*    public void OnHitEffects(Enemy enemy, Player player)
 {
     foreach(EquipedItem equiped in Items)
     {
         if (equiped.rpgItem is RPGArmor)
         {
             RPGArmor armor = (RPGArmor)equiped.rpgItem;
             
             foreach(Effect effect in armor.EffectsOnHit)
             {
                 if (effect.Target == TargetType.Self)
                        player.Hero.Buffs.AddEffect(effect);
             }
         }
     }
 }*/
 
 /*public void HitEffects(Enemy enemy, Player player)
 {
     foreach(EquipedItem equiped in Items)
     {
         if (equiped.rpgItem is RPGWeapon)
         {
             RPGWeapon armor = (RPGWeapon)equiped.rpgItem;
             
             foreach(Effect effect in armor.EffectsHit)
             {
                 if (effect.Target == TargetType.Self)
                        player.Hero.Buffs.AddEffect(effect);
             }
         }
     }
 }*/
 
 private bool IsEquipmentSlotUsed(EquipmentSlots slot)
 {
        switch (slot)
    {
        case EquipmentSlots.Head:
            if(EquippedHead != null)
                return true;
            break;
        case EquipmentSlots.Body:
            if(EquippedBody != null)
                return true;
            break;
        case EquipmentSlots.ArmL:
            if(EquippedArmL != null)
                return true;
            break;
        case EquipmentSlots.ArmR:
            if(EquippedArmR != null)
                return true;
            break;
        case EquipmentSlots.Legs:
            if(EquippedLegs != null)
                return true;
            break;
    }
        return false;
 }
 
 /*private List<EquipedItem> GetUnequipingItems(RPGEquipmentSlot slot)
 {
     List<RPGEquipmentSlot> slotsID = new List<RPGEquipmentSlot>();
     slotsID.Add(slot);
     return GetUnequipingItems(slotsID);
 }*/
 
 /*private EquipedItem GetUnequipingItems(EquipmentSlots slot)
 {
     EquipedItem item = new EquipedItem();
     
     foreach(EquipedItem equiped in Items)
         {
             foreach(RPGEquipmentSlot es in equiped.Slots)
             {
                 if (es.UniqueId == newEquipmentID.UniqueId)
                     items.Add(equiped);
             }
         }
     }
 }*/
 
 // Unequip one item
 /*public bool UnEquipItem(RPGEquipmentSlot equipmentSlot, Player player)
 {
     List<EquipedItem> dropingItems = GetUnequipingItems(equipmentSlot);
     PlayerEquip.itemsToUnequip = dropingItems;
     //only if items doest not remain in inventory
        if (!player.Hero.Settings.EquipedItemInInventory)
     {
         foreach(EquipedItem e in dropingItems)
         {
                player.Hero.Inventory.AddItem(e.rpgItem, player);
         }
     }
     RemoveItem(dropingItems);
     
     return true;
 }*/
 
 // Remove item from all collections
 /*private void RemoveItem(EquipedItem dropingItem)
 {
    foreach(EquipedItem e in Items)
             {
                 if (e == eq)
                 {
                     Items.Remove(e);
                     break;
                 }
             }
         }
    }
 }*/
 
 /*private void SetWeapon(RPGWeapon weapon)
 {
     if (weapon.IsAmmo)
         return;
     Weapon = weapon;
     NeedAmmo = weapon.NeedAmmo;
 }*/
 
 public bool EquipItem(InventoryItem item)
 {
     EquipedItem equiped = new EquipedItem() ;
     //equiped.CurrentAmount = item.CurrentAmount;
     //equiped.CurrentDurability = item.CurrentDurability;
     equiped.UniqueItemId = item.UniqueItemId;
     equiped.rpgItem = (RPGArmor)item.rpgItem;
     //equiped.rpgItem.LoadIcon();
     //get equipment slots
     RPGArmor e = (RPGArmor)equiped.rpgItem;
     equiped.Slot = e.EquipmentSlotIndex;
     
     if (!e.CanYouEquip())
     {
         //TODO display error message
         return false;
     }
     //if equipment slot is used
     /*if (IsEquipmentSlotUsed(equiped.Slot))
     {
         List<EquipedItem> dropingItems = GetUnequipingItems(equiped.Slot);
         //only if items does not remain in inventory
            if (!player.Hero.Settings.EquipedItemInInventory)
         {
             foreach(EquipedItem equip in dropingItems)
             {
                    player.Hero.Inventory.AddItem(equip.rpgItem, player);    
             }
         }
         //remove dropping items from equip
         RemoveItem(dropingItems);
         PlayerEquip.itemsToUnequip = dropingItems;
     }*/
    //if(IsEquipmentSlotUsed(equiped.Slot))
            
        
    switch (equiped.Slot)
    {
        case EquipmentSlots.Head:
            if(IsEquipmentSlotUsed(EquipmentSlots.Head))
                PlayerManager.Instance.Hero.HeadInventory.UnequipItem(EquippedHead);
            EquippedHead = equiped;
            if(PlayerManager.Instance.avatar != null)
                PlayerManager.Instance.avatar.EquipBodyPart(e.FBXName, EquipmentSlots.Head);
            break;
        case EquipmentSlots.Body:
            if(IsEquipmentSlotUsed(EquipmentSlots.Body))
                PlayerManager.Instance.Hero.BodyInventory.UnequipItem(EquippedBody);
            EquippedBody = equiped;
            if(PlayerManager.Instance.avatar != null)
                PlayerManager.Instance.avatar.EquipBodyPart(e.FBXName, EquipmentSlots.Body);
            break;
        case EquipmentSlots.ArmL:
            if(IsEquipmentSlotUsed(EquipmentSlots.ArmL))
                PlayerManager.Instance.Hero.ArmLInventory.UnequipItem(EquippedArmL);
            EquippedArmL = equiped;
            if(PlayerManager.Instance.avatar != null)
                PlayerManager.Instance.avatar.EquipBodyPart(e.FBXName, EquipmentSlots.ArmL);
            break;
        case EquipmentSlots.ArmR:
            if(IsEquipmentSlotUsed(EquipmentSlots.ArmR))
                PlayerManager.Instance.Hero.ArmRInventory.UnequipItem(EquippedArmR);
            EquippedArmR = equiped;
            if(PlayerManager.Instance.avatar != null)
                PlayerManager.Instance.avatar.EquipBodyPart(e.FBXName, EquipmentSlots.ArmR);
            break;
        case EquipmentSlots.Legs:
            if(IsEquipmentSlotUsed(EquipmentSlots.Legs))
                PlayerManager.Instance.Hero.LegsInventory.UnequipItem(EquippedLegs);
            EquippedLegs = equiped;
            if(PlayerManager.Instance.avatar != null)
                PlayerManager.Instance.avatar.EquipBodyPart(e.FBXName, EquipmentSlots.Legs);
            break;
    }
        
     //Items.Add(equiped);
     return true;
 }
 
 public void EquipAll()
 {
     /*foreach(EquipedItem ei in Items)
     {
         EquipedItem newItem = new EquipedItem();
         newItem.rpgItem = ei.rpgItem;
         //PlayerEquip.itemToEquip.Add(newItem);
     }*/
 }
 
 // Loading all items after loading game
    public void LoadItems()
    {
        int id = Convert.ToInt32(EquippedHead.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedHead.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedBody.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedBody.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedArmL.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedArmL.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedArmR.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedArmR.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedLegs.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedLegs.rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());

             //item.rpgItem.Icon = (Texture2D)Resources.Load(item.rpgItem.IconPath, typeof(Texture2D)); 

    }
 
 //check if it is possible attack, it will check if weapon needs ammo 
}
