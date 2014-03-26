using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class BasicInventory
{
	public BasicInventory()
	{
		Items = new List<InventoryItem>();  
	}

	public int sizeXOfInventory
	{
		get
		{
			return maximumItems / sizeYOfInventory;
		}
	}

	public List<InventoryItem> Items;
	public int maximumItems;
	public int sizeYOfInventory = 2;

	public bool DoYouHaveThisItem(string itemToHave, int level, int amountToReach)
	{
		int numberItemsInInventory = 0;
		foreach(ItemInWorld itemInInventory in Items)
		{
			if (itemToHave == itemInInventory.rpgItem.UniqueId && itemInInventory.Level == level)
			{
				numberItemsInInventory += itemInInventory.CurrentAmount;
			}
		}
		if (numberItemsInInventory >= amountToReach)
			return true;

		return false;
	}

	public bool DoYouHaveThisItem(string itemToHave, int level)
	{
		return DoYouHaveThisItem(itemToHave, level, 1);
	}
 
	public bool DoYouHaveSpaceForThisItem(RPGItem itemToHave, int level, int amountToReach)
    {
        if (itemToHave == null)
            return false;
        
        if(itemToHave.Stackable)
        {
            foreach(InventoryItem item in Items)
            {
                if(item.rpgItem.UniqueId == itemToHave.UniqueId && item.Level == level )
                    return true;
            }
            
            if(!IsFullInventory)
                return true;
            else
                return false;
        }
        
        else
        {
            if(maximumItems - Items.Count >= amountToReach)
                return true;
            else
                return false;
        }
        
     /*if (itemToHave.Stackable)
     {
         foreach(InventoryItem item in Items)
         {
             if (item.rpgItem.UniqueId != itemToHave.UniqueId)
                  continue;
             if (item.rpgItem.MaximumStack == item.CurrentAmount)
                 continue;
             size = item.rpgItem.MaximumStack - item.CurrentAmount;
         }
         if (size <= 0)
             return true;
         int freeSpaces = maximumItems - Items.Count;
         if (freeSpaces + itemToHave.MaximumStack > size)
             return true;
         else
             return false;
     }
     else
     {
         if (Items.Count <= maximumItems + size)
             return true;
         else
             return false;
     }*/
 	}

    /*public bool AddItem(RPGItem itemToAdd)
	{
		return AddItem(itemToAdd, 1);
	}
 
	public bool AddItem(RPGItem itemToAdd, int amountToReach)
    {
        if (!DoYouHaveSpaceForThisItem(itemToAdd, amountToReach))
            return false;
        
		if (itemToAdd.Stackable)
		{
			foreach(InventoryItem item in Items)
			{
				if (item.rpgItem.UniqueId != itemToAdd.UniqueId)
					continue;
				else
				{
				item.CurrentAmount += amountToReach;
				return true;
				}
			}
		}
	    else
	    {
	        for(int index = 1; index <= amountToReach; index++)
	        {
	            AddInventoryItem(itemToAdd, 1);
	        }
	    }
     //FinalizeInventoryOperation(player);
    	return true;
	}*/
    
 
	/*public bool RemoveItem(int ID, string preffix, int amountToRemove)
	{
		RPGItem item = new RPGItem();

		if (preffix == PreffixType.ARMOR.ToString())
			item = Storage.LoadById<RPGArmor>(ID, new RPGArmor());

		if (preffix == PreffixType.ITEM.ToString())
			item = Storage.LoadById<RPGItem>(ID, new RPGItem());

		return RemoveItem(item, amountToRemove);
	}*/

 
 /// <summary>
 /// Clean inventory from items that have current amount 0 
 /// </summary>
	 private void CleanInventory(int itemsToRemove)
	 {
	     for(int x = 1; x <= itemsToRemove; x++)
	     {
	         foreach(InventoryItem item in Items)
	         {
	             if (item.CurrentAmount == 0)
	             {
	                 Items.Remove(item);
	                 break;
	             }
	         }
	     }
	 }
 
	 public int CountItem(string uniqueId)
	 {
	     int count = 0;
	     foreach(InventoryItem item in Items)
	     {
	         if (item.rpgItem.UniqueId == uniqueId)
	             count += item.CurrentAmount;
	     }
	     return count;
	 }
 
	[XmlIgnore]
	public bool IsFullInventory
	{
	    get
	    {
	        if (maximumItems <= Items.Count)
	            return true;
	        else
	            return false;
	    }
	}
}
