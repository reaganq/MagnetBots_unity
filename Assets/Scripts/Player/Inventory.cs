using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory  : BasicInventory
{
	public List<InventoryItem> _headItems = new List<InventoryItem>();
	public List<InventoryItem> HeadItems
	{
		get
		{
			Debug.Log("getting head items");
			_headItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor && Items[i].rpgItem.EquipmentSlotIndex == EquipmentSlots.Head)
				{
					_headItems.Add(Items[i]);
					Debug.Log(i);
				}
            }
			return _headItems;
        }
    }
    public List<InventoryItem> _BodyItems = new List<InventoryItem>();
	public List<InventoryItem> BodyItems
	{
		get
		{
			_BodyItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor && Items[i].rpgItem.EquipmentSlotIndex == EquipmentSlots.Body)
				{
					_BodyItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _BodyItems;
        }
    }
	public List<InventoryItem> _ArmLItems = new List<InventoryItem>();
	public List<InventoryItem> ArmLItems
	{
		get
		{
			_ArmLItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor && Items[i].rpgItem.EquipmentSlotIndex == EquipmentSlots.ArmL)
				{
					_ArmLItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _ArmLItems;
        }
    }
    public List<InventoryItem> _ArmRItems = new List<InventoryItem>();
	public List<InventoryItem> ArmRItems
	{
		get
		{
			_ArmRItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor && Items[i].rpgItem.EquipmentSlotIndex == EquipmentSlots.ArmR)
				{
					_ArmRItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _ArmRItems;
        }
    }
    public List<InventoryItem> _LegsItems = new List<InventoryItem>();
	public List<InventoryItem> LegsItems
	{
		get
		{
			_LegsItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor && Items[i].rpgItem.EquipmentSlotIndex == EquipmentSlots.Legs)
				{
					_LegsItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _LegsItems;
        }
    }
    public List<InventoryItem> _UpgradeableItems = new List<InventoryItem>();
	public List<InventoryItem> UpgradeableItems
	{
		get
		{
			_UpgradeableItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].IsItemUpgradeable)
				{
					_UpgradeableItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _UpgradeableItems;
        }
    }
    public List<InventoryItem> _UsableItems = new List<InventoryItem>();
	public List<InventoryItem> UsableItems
	{
		get
		{
			_UsableItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].IsItemUsable)
				{
					_UsableItems.Add(Items[i]);
					Debug.Log(i);
                }
            }
			return _UsableItems;
        }
    }
    public List<InventoryItem> _QuestItems = new List<InventoryItem>();
    public List<InventoryItem> QuestItems
	{
		get
		{
			_QuestItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Quest)
					_QuestItems.Add(Items[i]);
			}
			return _QuestItems;
		}
	}
	public List<InventoryItem> ArmorItems;

	public Inventory() : base()
	{
		maximumItems = 50;
	}
	
	public void AddItem(RPGItem itemToAdd, int level)
	{
		AddItem(itemToAdd, level, 1);
	}
	
	public void AddItem(RPGItem itemToAdd, int level, int amount)
	{
		if (!DoYouHaveSpaceForThisItem(itemToAdd, level, amount))
			return;
		
		if (itemToAdd.Stackable)
		{
			foreach(InventoryItem inventoryItem in Items)
			{
				if (inventoryItem.rpgItem.UniqueId == itemToAdd.UniqueId && inventoryItem.Level == level)
				{
					inventoryItem.CurrentAmount += amount;
					return;
				}
			}
			InventoryItem item = new InventoryItem();
			item.rpgItem = itemToAdd;
			item.Level = level;
			item.UniqueItemId = itemToAdd.UniqueId;
			item.CurrentAmount = amount;
			Items.Add(item);
		}
		else
		{
			for(int index = 1; index <= amount; index++)
			{
				InventoryItem item = new InventoryItem();
				item.rpgItem = itemToAdd;
				item.Level = level;
				item.UniqueItemId = itemToAdd.UniqueId;
				item.CurrentAmount = 1;
				Items.Add(item);
			}
		}
	}

	public bool RemoveItem(InventoryItem item)
	{
		Debug.Log(item.UniqueItemId);
		for (int i = 0; i < Items.Count; i++) 
		{
			if(Items[i].UniqueItemId == item.UniqueItemId && Items[i].Level == item.Level)
			{
				Debug.Log("unique id" + Items[i].UniqueItemId);
				Debug.Log("removed" + Items[i].rpgItem.Name);
				Items.Remove(Items[i]);
				Debug.Log(_headItems.Count);
				for (int j = 0; j < _headItems.Count; j++) {
					Debug.Log(_headItems[j].rpgItem.Name);
				}

			}

		}
		return true;
	}
	
	public bool RemoveItem(RPGItem itemToRemove, int level)
	{
		return RemoveItem(itemToRemove, level, 1);
	} 
	
	public bool RemoveItem(RPGItem itemToRemove, int level , int amountToRemove)
	{
		if (itemToRemove.ID == 0)
			return true;
		
		if (!DoYouHaveThisItem(itemToRemove.UniqueId, level, amountToRemove))
			return false;
		
		if (itemToRemove.Stackable)
		{
			foreach(InventoryItem item in Items)
			{
				if (item.rpgItem.UniqueId == itemToRemove.UniqueId && item.Level == level)
				{
					if (item.CurrentAmount > amountToRemove)
					{
						item.CurrentAmount -= amountToRemove;
					}
					else
					{
						Items.Remove(item);
						return true;
					}
				}
			}
		}
		else
		{
			for(int x = 1; x <= amountToRemove; x ++)
			{
				foreach(InventoryItem item in Items)
                {
                    if (itemToRemove.UniqueId == item.rpgItem.UniqueId && item.CurrentAmount == 1)
                    {
                        Items.Remove(item);
                        break;
                    }
                }
            }
        }
        //FinalizeInventoryOperation(player);
        
        return true;
    }
    
    public bool EquipItem(InventoryItem item)
    {
        if (!item.IsItemEquippable)
            return false;
        if(PlayerManager.Instance.Hero.Equip.EquipItem((RPGArmor)item.rpgItem, item.Level))
        {
            item.IsItemEquipped = true;
            return true;
        }
        else
            return false;
    }
    
    //TODO add in unequip functionality
    public bool UnequipItem(string itemID, int level)
    {
        for (int i = 0; i < Items.Count; i++) 
        {
            if(Items[i].UniqueItemId == itemID && Items[i].Level == level)
			{
				Items[i].IsItemEquipped = false;
			}
		}
		return true;
	}
	
}

