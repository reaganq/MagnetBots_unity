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
     //IconPath = "Icon/";
 }
 
    public BuyCurrencyType BuyCurrency;
    public bool Stackable;
 //public bool Destroyable;
 //public int LevelItem;
 public int BuyValue;
    public int SellValue;
 public RarityType Rarity;
 //public string PrefabName;
 //public float CurrentDurability;
 //public List<RPGItemCategory> Categories;
 
 //item generator variables (not used in game, used only in editors)
 //public bool IsCopy;
 //public int SourceItem;
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
    Magnets = 0,
    Crystals = 1
}
