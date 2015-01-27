using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

//[XmlInclude(typeof(Effect))]
//[XmlInclude(typeof(RPGItemCategory))]
[Serializable]
public class RPGItem : UsableItem
{
 	public RPGItem()
 	{
    	 //Categories = new List<RPGItemCategory>();
     //Destroyable = true;
     //Droppable = false;
     	Name = string.Empty;
     	preffix = "ITEM";
		Stackable = true;
		IsUpgradeable = false;
		AtlasName = "Atlases/Item";
		EquipmentSlotIndex = EquipmentSlots.None;
     //IconPath = "Icon/";
 	}

	public BuyCurrencyType BuyCurrency;
	public bool Stackable;
	public int BuyValue;
	public int SellValue;
	public RarityType Rarity;

	public virtual void Use()
	{
		Debug.Log("using item: " + Name);
	}
}

public enum RarityType
{
 Worthless = 0,
 Common = 1,
 Magic = 2,
 Rare = 3,
 Epic = 4,
 Legendary = 5,
 None = 6
}

public enum BuyCurrencyType
{
    Coins = 0,
    Sparks = 1,
	Magnets = 2,
	None = 3,
}
