using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//used for shops, enemy loots, items
//[XmlInclude(typeof(Condition))]
public class BaseLootItem
{
	public int StackAmount = 1;
	public int Level = 1;
	public ItemType itemType;
	public int ID;
	//public List<Condition> Conditions;
	
	public BaseLootItem()
	{
		//Conditions = new List<Condition>();
	}
	
	//if player can loot this item
	public bool CanYouLoot
	{
		/*get
		{
			foreach(Condition condition in Conditions)
			{
                if (condition.Validate(player) == false)
					return false;
			}
			return true;
		}*/
           
        get{return true;}
	}
	
	public void AddOneItem(List<InventoryItem> BaseLootItems)
	{
		//validate conditions
		//if (!CanYouLoot)
			//return;
		RPGItem item = new RPGItem();
		if(itemType == ItemType.Armor)
		{
			RPGArmor armor = Storage.LoadById<RPGArmor>(ID, new RPGArmor());
        	item = (RPGItem)armor;
		}
		else if(itemType != ItemType.Armor && itemType != ItemType.Currency)
		{
			item = Storage.LoadById<RPGItem>(ID, new RPGItem());
		}

        AddItem(item, BaseLootItems);
        //AddItem(
        
		/*if (Preffix == ItemTypeEnum.ITEM)
		{
			foreach(RPGItem item in Player.Data.Items)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}
		
		if (Preffix == ItemTypeEnum.WEAPON)
		{
			foreach(RPGWeapon item in Player.Data.Weapons)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}
		
		if (Preffix == ItemTypeEnum.ARMOR)
		{
			foreach(RPGArmor item in Player.Data.Armors)
			{
				if (item.ID == ID)
				{
					AddItem(item, BaseLootItems);
					return;
				}
			}
		}*/
	}
	
	
	protected void AddItem(RPGItem item, List<InventoryItem> BaseLootItems)
	{
		foreach(InventoryItem i in BaseLootItems)
		{
			if (i.rpgItem.UniqueId == item.UniqueId)
				return;
		}
		
		InventoryItem itemInWorld = new InventoryItem();
		itemInWorld.rpgItem = item;
		itemInWorld.UniqueItemId = item.UniqueId;
		//if (item.Stackable)
		itemInWorld.CurrentAmount = StackAmount;
		itemInWorld.Level = Level;
		//else
			//itemInWorld.CurrentAmount = 1;
		BaseLootItems.Add(itemInWorld);
	}
}
