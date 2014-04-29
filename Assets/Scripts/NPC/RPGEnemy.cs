using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(LootItem))]
public class RPGEnemy : IItem
{
	#region implementing interface IItem
	private int id;
	public int ID
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}
	
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
	
	private string description;
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}
	
	private string systemDescription;
	public string SystemDescription
	{
		get
		{
			return systemDescription;
		}
		set
		{
			systemDescription = value;
		}
	}
	
	protected string preffix = "ENEMY";
	public string Preffix
	{
		get
		{
			return preffix;
		}
	}
	
	public string UniqueId
	{
		get
		{
			return preffix + ID.ToString();
		}
	}
	#endregion
	
	public string PortraitIcon;
	public string PortraitAtlas;
	public List<LootItem> Loots;
	public List<string> PrefabPaths;
	
	public RPGEnemy()
	{
		PortraitIcon = string.Empty;
		Loots = new List<LootItem>();
		Name = string.Empty;

		SystemDescription = string.Empty;
		Description = string.Empty;
		PortraitAtlas = "Atlases/Enemy/EnemiesAtlas";
	}
}

public class LootItem
{
	public ItemType itemType;
	public List<int> itemID;
	public int itemLevel;
	public int minQuantity;
	public int maxQuantity;
	public float dropRate;

	public LootItem()
	{
		itemType = ItemType.Currency;
		itemID = new List<int>();
		itemLevel = 1;
		minQuantity = 1;
		maxQuantity = 1;
		dropRate = 0.5f;
	}
}


