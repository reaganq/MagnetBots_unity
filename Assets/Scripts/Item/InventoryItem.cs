using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InventoryItem : ItemInWorld {
	public bool IsItemEquipped = false;
	public bool isItemViewed;
	public bool IsItemTradeable
	{
		get
		{
			if(IsItemEquipped && CurrentAmount == 1 || rpgItem.ItemCategory == ItemType.Quest)
				return false;
			return true;
		}
	}

	public bool IsItemUpgradeable
	{
		get
		{
			if(rpgItem.IsUpgradeable && !(CurrentAmount == 1 && IsItemEquipped))
			   return true;
			else
			   return false;
		}
	}
}

[Serializable]
public class ParseInventoryItem
{
	public bool IsItemEquipped;
	public bool isItemViewed;
	public string UniqueItemId;
	public int Amount;
	public int ItemLevel;
	
}

/*public enum InventoryTypeEnum
{
 Inventory = 0,
 Bank = 1
}*/
