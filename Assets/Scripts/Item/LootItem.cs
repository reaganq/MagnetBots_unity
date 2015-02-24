using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(Condition))]
public class LootItem
{
	public ItemType itemType;
	public List<int> itemID;
	public int itemLevel;
	public int minQuantity;
	public int maxQuantity;
	public float dropRate;
	public bool definiteDrop;
	public List<Condition> Conditions;
	
	public LootItem()
	{
		itemType = ItemType.Currency;
		itemID = new List<int>();
		itemLevel = 1;
		minQuantity = 1;
		maxQuantity = 1;
		dropRate = 0.5f;
		Conditions = new List<Condition>();
	}

	public bool Validate()
	{
		for (int i = 0; i < Conditions.Count; i++) {
			if(Conditions[i].Validate() == false)
				return false;
		}
		return true;
	}
}

public class LootItemList
{
	public List<InventoryItem> items;
	public List<RPGCurrency> currencies;
	public List<RPGBadge> badges;

	public LootItemList()
	{
		items = new List<InventoryItem>();
		currencies = new List<RPGCurrency>();
		badges = new List<RPGBadge>();
	}
}
