using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory  : BasicInventory
{
	private List<InventoryItem> _headItems = new List<InventoryItem>();
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
	private List<InventoryItem> _BodyItems = new List<InventoryItem>();
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
	private List<InventoryItem> _ArmLItems = new List<InventoryItem>();
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
	private List<InventoryItem> _ArmRItems = new List<InventoryItem>();
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
	private List<InventoryItem> _LegsItems = new List<InventoryItem>();
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
	private List<InventoryItem> _UpgradeableItems = new List<InventoryItem>();
	public List<InventoryItem> UpgradeableItems
	{
		get
		{
			_UpgradeableItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.IsUpgradeable)
				{
					_UpgradeableItems.Add(Items[i]);
                }
            }
			return _UpgradeableItems;
        }
    }
	private List<InventoryItem> _UsableItems = new List<InventoryItem>();
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
	private List<InventoryItem> _QuestItems = new List<InventoryItem>();
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
	private List<InventoryItem> _ArmorItems = new List<InventoryItem>();
	public List<InventoryItem> ArmorItems
	{
		get
		{
			_ArmorItems.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.Armor)
					_ArmorItems.Add(Items[i]);
			}
			return _ArmorItems;
		}
	}
	private List<InventoryItem> _UpgradeMaterials = new List<InventoryItem>();
	public List<InventoryItem> UpgradeMaterials
	{
		get
		{
			_UpgradeMaterials.Clear();
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].rpgItem.ItemCategory == ItemType.UpgradeMaterials)
					_UpgradeMaterials.Add(Items[i]);
			}
			return _UpgradeMaterials;
		}
	}

	public List<InventoryItem> FilteredItemByCategory(ItemType category)
	{
		List<InventoryItem> result = new List<InventoryItem>();
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].rpgItem.ItemCategory == category)
				result.Add(Items[i]);
		}
		return result;
	}

	public List<InventoryItem> FilteredArmorBySlot(EquipmentSlots slot)
	{
		List<InventoryItem> result = new List<InventoryItem>();
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].rpgItem.EquipmentSlotIndex == slot)
				result.Add(Items[i]);
        }
        return result;
    }

	public static List<InventoryItem> FilteredItemByLevel(List<InventoryItem> Items, int minLevel, int maxLevel)
	{
		List<InventoryItem> result = new List<InventoryItem>();
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].Level >= minLevel && Items[i].Level <= maxLevel)
				result.Add(Items[i]);
        }
        return result;
    }
    //types

	public Inventory() : base()
	{
		maximumItems = 50;
	}

	public void ReplaceNakedItem(RPGItem itemToAdd, int level, int amount)
	{
		int index = -1;
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].rpgItem.EquipmentSlotIndex == itemToAdd.EquipmentSlotIndex)
			{
				index = i;
			}
		}
		if(index > -1)
			Items.RemoveAt(index);
		InventoryItem item = new InventoryItem();
		item.rpgItem = itemToAdd;
		item.Level = level;
		item.UniqueItemId = itemToAdd.UniqueId;
		item.CurrentAmount = amount;
		item.isItemViewed = true;
		Items.Add(item);
	}

	public void AddItem(InventoryItem item, int amount)
	{
		if (!DoYouHaveSpaceForThisItem(item))
			return;
		
		if (item.rpgItem.Stackable)
		{
			foreach(InventoryItem inventoryItem in Items)
			{
				if (inventoryItem.rpgItem.UniqueId == item.rpgItem.UniqueId && inventoryItem.Level == item.Level)
				{
					inventoryItem.CurrentAmount += amount;
					inventoryItem.isItemViewed = false;
					return;
				}
			}
			item.isItemViewed = false;
			Items.Add(item);
			Debug.LogWarning("ADDING ITEM: " + amount + item.UniqueItemId + item.rpgItem.UniqueId);
		}
		else
		{
			for(int index = 1; index <= amount; index++)
			{
				InventoryItem newItem = new InventoryItem();
				newItem.rpgItem = item.rpgItem;
				newItem.Level = item.Level;
				newItem.UniqueItemId = item.rpgItem.UniqueId;
				newItem.CurrentAmount = 1;
				newItem.isItemViewed = false;
				Items.Add(item);
			}
		}
	}
	
	public void AddItem(RPGItem itemToAdd, int level)
	{
		AddItem(itemToAdd, level, 1);
	}
	
	public void AddItem(RPGItem itemToAdd, int level, int amount)
	{
		AddItem(itemToAdd, level, amount, false);
	}

	public void AddItem(RPGItem itemToAdd, int level, int amount, bool viewedState)
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
					inventoryItem.isItemViewed = viewedState;
					return;
				}
			}
			InventoryItem item = new InventoryItem();
			item.rpgItem = itemToAdd;
			item.Level = level;
			item.UniqueItemId = itemToAdd.UniqueId;
			item.CurrentAmount = amount;
			item.isItemViewed = viewedState;
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
				item.isItemViewed = viewedState;
				Items.Add(item);
			}
		}
	}

	public bool RemoveItem(InventoryItem item, int amount)
	{
		Debug.Log(item.UniqueItemId + amount);
		for (int i = 0; i < Items.Count; i++) 
		{
			if(Items[i].UniqueItemId == item.UniqueItemId && Items[i].Level == item.Level)
			{
				Debug.Log(Items[i].CurrentAmount);
				Items[i].CurrentAmount -= amount;
				Debug.Log(Items[i].CurrentAmount);
				if(Items[i].CurrentAmount <= 0)
					Items.RemoveAt(i);
				return true;
			}

		}
		return false;
	}

	public bool RemoveItemByUniqueID(string uniqueItemId, int level, int amount)
	{
		for (int i = 0; i < Items.Count; i++) {
			if(Items[i].UniqueItemId == uniqueItemId && Items[i].Level == level)
			{
				Items[i].CurrentAmount -= amount;
				if(Items[i].CurrentAmount <= 0)
					Items.RemoveAt(i);
				return true;
			}
		}
		return false;
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
    
    

	//used by parse
	public bool EquipItem(string itemID, int level)
	{
		for (int i = 0; i < Items.Count; i++) 
		{
			if(Items[i].UniqueItemId == itemID && Items[i].Level == level)
			{
				if(PlayerManager.Instance.Hero.Equip.EquipItem((RPGArmor)Items[i].rpgItem, Items[i].Level))
				{
					Items[i].IsItemEquipped = true;
					Items[i].isItemViewed = true;
					return true;
				}
			}
		}
		return false;
	}
    
    //TODO add in unequip functionality
    

	public bool DoYouHaveThisItem(InventoryItem item, bool ignoreLevel)
	{
		if(ignoreLevel)
		{
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].UniqueItemId == item.UniqueItemId && Items[i].CurrentAmount >= item.CurrentAmount)
					return true;
			}
		}
		else
		{
			for (int i = 0; i < Items.Count; i++) {
				if(Items[i].UniqueItemId == item.UniqueItemId && Items[i].CurrentAmount >= item.CurrentAmount && Items[i].Level == item.Level)
					return true;
			}
		}

		return false;
	}

	public int GetItemAmount(InventoryItem itemToHave, bool ignoreLevel)
	{
		int amount = 0;
		for (int i = 0; i < Items.Count; i++) {
			if(ignoreLevel)
			{
				if(Items[i].UniqueItemId == itemToHave.UniqueItemId)
					amount += Items[i].CurrentAmount;
			}
			else
			{
				if(Items[i].UniqueItemId == itemToHave.UniqueItemId && Items[i].Level == itemToHave.Level)
				{
					amount += Items[i].CurrentAmount;
					return amount;
				}
			}
		}
		return amount;
	}
	
}

