using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//[XmlInclude(typeof(ShopCategory))]
[XmlInclude(typeof(ShopItem))]
public class Shop : NPCActivity
{
	//public int CurrencyID;
	//public float BuyPriceModifier;
	//public float SellPriceModifier;
	//public bool SellSameAsBuy;
	//public ShopRespawnTimer RespawnTimer;
	//public List<ShopCategory> SellCategories;
	//public List<ShopCategory> Categories;
	public List<ShopItem> Items;
    public float RestockTime;
	
	[XmlIgnore]
	public List<InventoryItem> ShopItems = new List<InventoryItem>();
    public float LastRestockTime = 0;
	
	public Shop()
	{
		//SellCategories = new List<ShopCategory>();
		//Categories = new List<ShopCategory>();
		//SellSameAsBuy = true;
		//RespawnTimer = ShopRespawnTimer.Monday;
		//BuyPriceModifier = 1;
		//SellPriceModifier = 1;
		Items = new List<ShopItem>();
		//CurrencyID = GlobalSettings.GoldID;
        Name = string.Empty;
        Description = string.Empty;
        preffix = "SHOP";
		activityType = NPCActivityType.Shop;
	}
	
	public BuyTransaction BuyItem(RPGItem item, int level, int Amount)
	{
		if (item.ID == 0)
			return BuyTransaction.SomeError;
		//calculate price
		int price = (int)(item.SellValue * Amount);
		//currency item
		//RPGItem currencyItem = new RPGItem();
		//currencyItem = Storage.LoadById<RPGItem>(CurrencyID, new RPGItem());
		//skill affecting price put modifier here
		//check if you have enough gold
        if (!PlayerManager.Instance.Hero.CanYouAfford(price, item.BuyCurrency))
			return BuyTransaction.NotEnoughGold;
		
		//check space in inventory
        if (!PlayerManager.Instance.Hero.DoYouHaveSpaceForThisItem(item, level, Amount))
		{
			return BuyTransaction.NotEnoughSpaceInInventory;
		}
		else
		{
			//add item to inventory
            if(item.ItemCategory == ItemType.Armor)
            {
                PreffixSolver.GiveItem(PreffixType.ARMOR, item.ID, level, Amount);
            }
            else
				PreffixSolver.GiveItem(PreffixType.ARMOR, item.ID, level, Amount);
            
			//remove currency amount from inventory
            PlayerManager.Instance.Hero.RemoveCurrency(price, item.BuyCurrency);
			
			//remove item from current shop collection
			foreach(InventoryItem shopItem in ShopItems)
			{
				if (shopItem.rpgItem.UniqueId == item.UniqueId)
				{
					shopItem.CurrentAmount = shopItem.CurrentAmount - Amount;
					if (shopItem.CurrentAmount <= 0)
					{
						ShopItems.Remove(shopItem);
					}
					break;
				}
			}
			
			//add bio log info
			//player.Hero.Log.BuyItem(Player.activeNPC, Amount, item.UniqueId);
			return BuyTransaction.OK;	
		}
	}
	
	public BuyTransaction BuyItem(RPGItem rpgItem)
	{
		return BuyItem(rpgItem, 1, 1);
	}

	private void AddItem(RPGItem item, int Amount)
	{
		foreach(InventoryItem i in ShopItems)
		{
			if (i.rpgItem.UniqueId == item.UniqueId)
            {
                i.CurrentAmount += Amount;
				return;
            }
		}
		
		InventoryItem itemInWorld = new InventoryItem();
		itemInWorld.UniqueItemId = item.UniqueId;
		itemInWorld.rpgItem = item;
		itemInWorld.CurrentAmount = Amount;
		itemInWorld.IsItemEquipped = false;
		ShopItems.Add(itemInWorld);
	}

	public bool SellItem(RPGItem item, int level)
	{
		return SellItem(item, level, 1);
	}

	public bool SellItem(RPGItem item, int level, int Amount)
	{
		if (item.ID == 0)
			return false;
		//calculate price
		int price = (int)(item.SellValue * Amount);
		//remove item
		PlayerManager.Instance.Hero.RemoveItem(item, Amount);
		//add gold
		PlayerManager.Instance.Hero.AddCurrency(price, BuyCurrencyType.Magnets);
		//add item to temp shop collection
        //AddItem(item, Amount);
        return true;
    }
    
    /*private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = dt.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
                diff += 7;
        }

        return dt.AddDays(-1 * diff).Date;
    }*/
	
	public void PopulateItems()
	{
		//load categories
		//foreach(ShopCategory category in Categories)
            //category.GetItems(ShopItems, player);
		//load items
        Debug.Log(Items.Count);
        
        if(LastRestockTime == 0 || Time.realtimeSinceStartup - LastRestockTime > RestockTime)
        {
            ShopItems.Clear();
    		foreach(ShopItem shopItem in Items)
            {
    			shopItem.AddOneItem(ShopItems);
            }
            LastRestockTime = Time.realtimeSinceStartup;
            Debug.Log("shop items" + ShopItems.Count);
        }

        //BuyedItems(ShopItems, NPCId, player);
        //removes the items that were previously bought, it loads the full list of items every time you access shop. Shop doesnt update shop items list
		
		//return ShopItems;
	}
	
	/*private void BuyedItems(List<ItemInWorld> items, int NPCId, Player player)
	{
		//check if some item was sold in this period
		DateTime date = Weather.CurrentDateTime;
		
		DateTime startPeriodDate = date;
		//determine datum
		switch(RespawnTimer)
		{
			case ShopRespawnTimer.Never : return;
			case ShopRespawnTimer.Monday: 
				int diff = date.DayOfWeek - DayOfWeek.Monday;
				if (diff < 0)
					diff += 7;
				startPeriodDate = date.AddDays(-1 * diff).Date;
				break;
			case ShopRespawnTimer.NewMonth:
				startPeriodDate = date.AddDays(-1 * date.Day).Date; 
				break;
		}
		//search through log and remove buyed items in current period for current NPC
		foreach(LogEvent logEvent in player.Hero.Log.Events)
		{
			//log must be same as NPC id and buying item
			if (logEvent.EventType != LogEventType.BuyItem || NPCId != logEvent.NPCId)
				continue;
			if (startPeriodDate > logEvent.Date)
				continue;
			foreach(ItemInWorld item in items)
			{
				if (item.rpgItem.UniqueId == logEvent.UniqueId)
				{
					item.CurrentAmount -= logEvent.Amount;
					//if amount is ZERO remove from collection
					if (item.CurrentAmount == 0)
					{
						items.Remove(item);
						break;
					}
				}
			}
		}
	}*/
}

public enum ShopRespawnTimer
{
	Never = 0,
	Monday = 1,
	NewMonth = 2
}
	
public enum BuyTransaction
{
	SomeError = 0,
	OK = 1,
	NotEnoughSpaceInInventory = 2,
	NotEnoughGold = 3
}
	