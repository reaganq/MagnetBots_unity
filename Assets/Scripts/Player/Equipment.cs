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

	public List<EquipedItem> equippedItems
	{
		get
		{
			List<EquipedItem> allItems = new List<EquipedItem>();
			allItems.Add(EquippedFace);
			allItems.Add(EquippedHead);
			allItems.Add(EquippedBody);
			allItems.Add(EquippedArmL);
			allItems.Add(EquippedArmR);
			allItems.Add(EquippedLegs);
			return allItems;
		}
	}

	public delegate void OnEquipItem(string uniqueItemID, int itemLevel, int slotIndex);
	public static event OnEquipItem onEquipItem;
    //public List<EquipedItem> Items;
 
 //[XmlIgnore]
 //public RPGWeapon Weapon;

 public Equipment()
 {
        //Items = new List<EquipedItem>();
		/*EquippedHead = new EquipedItem();
		EquippedBody = new EquipedItem();
		EquippedArmL = new EquipedItem();
		EquippedArmR = new EquipedItem();
		EquippedLegs = new EquipedItem();
		EquippedFace = new EquipedItem();*/
		EquippedHead = null;
		EquippedBody = null;
		EquippedArmL = null;
		EquippedArmR = null;
		EquippedLegs = null;
		EquippedFace = null;
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
		case EquipmentSlots.Face:
			if(EquippedFace != null)
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
 
 public bool EquipItem(RPGArmor armor, int level)
 {
     /*EquipedItem equiped = new EquipedItem() ;
     //equiped.CurrentAmount = item.CurrentAmount;
     //equiped.CurrentDurability = item.CurrentDurability;
     equiped.UniqueItemId = item.UniqueItemId;
     equiped.rpgItem = (RPGArmor)item.rpgItem;
     //equiped.rpgItem.LoadIcon();
     //get equipment slots
     RPGArmor e = (RPGArmor)equiped.rpgItem;
     equiped.Slot = e.EquipmentSlotIndex;
     */
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

		EquipedItem e = new EquipedItem(armor.UniqueId, level, armor);
		if(armor.armorStatsSets.Count >0)
		{
			PlayerManager.Instance.Hero.profile.AddArmorStats(armor.armorStatsSets[Mathf.Min(armor.armorStatsSets.Count, e.Level)-1]);
		}
		//e.UniqueItemId = armor.UniqueId;
		//e.rpgArmor = armor;
		//e.Level = level;
	    switch (e.rpgArmor.EquipmentSlotIndex)
	    {
			case EquipmentSlots.Face:
				if(IsEquipmentSlotUsed(EquipmentSlots.Face))
				UnequipItem(EquippedFace);
					//PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedFace.UniqueItemId, EquippedFace.Level);
				EquippedFace = e;
				if(PlayerManager.Instance.avatar != null)
					PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.Face);
				break;
	        case EquipmentSlots.Head:
	            if(IsEquipmentSlotUsed(EquipmentSlots.Head))
				UnequipItem(EquippedHead);
	                //PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedHead.UniqueItemId, EquippedHead.Level);
	            EquippedHead = e;
	            if(PlayerManager.Instance.avatar != null)
	                PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.Head);
	            break;
	        case EquipmentSlots.Body:
	            if(IsEquipmentSlotUsed(EquipmentSlots.Body))
				UnequipItem(EquippedBody);
				//PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedBody.UniqueItemId, EquippedBody.Level);
	            EquippedBody = e;
	            if(PlayerManager.Instance.avatar != null)
				PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.Body);
	            break;
	        case EquipmentSlots.ArmL:
	            if(IsEquipmentSlotUsed(EquipmentSlots.ArmL))
				UnequipItem(EquippedArmL);
				//PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedArmL.UniqueItemId, EquippedArmL.Level);
	            EquippedArmL = e;
	            if(PlayerManager.Instance.avatar != null)
				PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.ArmL);
	            break;
	        case EquipmentSlots.ArmR:
	            if(IsEquipmentSlotUsed(EquipmentSlots.ArmR))
					UnequipItem(EquippedArmR);
					//PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedArmR.UniqueItemId, EquippedArmR.Level);
	            EquippedArmR = e;
	            if(PlayerManager.Instance.avatar != null)
				PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.ArmR);
	            break;
	        case EquipmentSlots.Legs:
	            if(IsEquipmentSlotUsed(EquipmentSlots.Legs))
				//PlayerManager.Instance.Hero.ArmoryInventory.UnequipItem(EquippedLegs.UniqueItemId, EquippedLegs.Level);
					UnequipItem(EquippedLegs);
	            EquippedLegs = e;
	            if(PlayerManager.Instance.avatar != null)
				PlayerManager.Instance.avatar.EquipBodyPart(e.rpgArmor.FBXName[Mathf.Min(e.Level, e.rpgArmor.FBXName.Count) - 1], EquipmentSlots.Legs);
	            break;
	    }

		//onEquipItem(e.UniqueItemId, e.Level, (int)e.rpgArmor.EquipmentSlotIndex);
		PlayerManager.Instance.Hero.profile.UpdateEquippedItems(equippedItems);
     //Items.Add(equiped);
     return true;
 }

	public void UnequipItem(EquipedItem item)
	{
		PlayerManager.Instance.Hero.UnequipItem(item.UniqueItemId, item.Level);
		if(item.rpgArmor.armorStatsSets.Count >0)
		{
			PlayerManager.Instance.Hero.profile.RemoveArmorStats(item.rpgArmor.armorStatsSets[Mathf.Min(item.rpgArmor.armorStatsSets.Count, item.Level)-1]);
		}
	}

 // Loading all items after loading game
    public void LoadItems()
    {
        int id = Convert.ToInt32(EquippedHead.UniqueItemId.Replace("ARMOR", string.Empty));
        EquippedHead.rpgArmor = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedBody.UniqueItemId.Replace("ARMOR", string.Empty));
		EquippedBody.rpgArmor = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedArmL.UniqueItemId.Replace("ARMOR", string.Empty));
		EquippedArmL.rpgArmor = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedArmR.UniqueItemId.Replace("ARMOR", string.Empty));
		EquippedArmR.rpgArmor = Storage.LoadById<RPGArmor>(id, new RPGArmor());
        id = Convert.ToInt32(EquippedLegs.UniqueItemId.Replace("ARMOR", string.Empty));
		EquippedLegs.rpgArmor = Storage.LoadById<RPGArmor>(id, new RPGArmor());
    }
}
