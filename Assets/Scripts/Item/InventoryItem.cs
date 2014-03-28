using UnityEngine;
using System;
using System.Collections;

public class InventoryItem : ItemInWorld {
	public bool IsItemEquipped = false;

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

/*public enum InventoryTypeEnum
{
 Inventory = 0,
 Bank = 1
}*/
