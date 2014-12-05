using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Parse;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

public class PlayerInformation  {

    public Inventory MainInventory;
	public Inventory ArmoryInventory;
	public Inventory DepositBox;
    /*public ArmoryInventory HeadInventory;
    public ArmoryInventory BodyInventory;
    public ArmoryInventory ArmLInventory;
    public ArmoryInventory ArmRInventory;
    public ArmoryInventory LegsInventory;*/

    public Equipment Equip;
	public QuestLog Quest;

 	/*public int CurrentLevel;
	public int CurrentXP;
	public int CurrentLevelXP;
    
	public int Trophies;

	public int BaseHp;
	public int TotalHp;
	public int BonusHp;
	public int CurrentHp;*/
	//public float HpRegeneration = 0.0f;
	//public float ManaRegeneration = 0.5f;

	public int Magnets;
	public int Crystals;
	public int BankMagnets;
	public string ParseObjectId;
	ParseObject playerData = new ParseObject("PlayerData");

	public void Awake()
	{
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}
 
    public PlayerInformation()
    {
        MainInventory = new Inventory();
		ArmoryInventory = new Inventory();
		DepositBox = new Inventory();
        Equip = new Equipment();
		Quest = new QuestLog();
	}
	
 	public void UpdatePlayerInformation()
 	{
     	foreach(InventoryItem item in MainInventory.Items)
         	item.LoadItem();
		foreach(InventoryItem item in ArmoryInventory.Items)
			item.LoadItem();
     	Equip.LoadItems();
 	}
    
    public void StartNewGame()
    {
        for (int i = 1; i < 20 ; i++) {
            PreffixSolver.GiveItem(PreffixType.ARMOR, i,1, 1);
            //Debug.Log(i);
        }

		for (int i = 6; i < 11; i++) {
			PreffixSolver.GiveItem(PreffixType.ARMOR, i,3, 1);
				}

		PreffixSolver.GiveItem(PreffixType.ITEM, 2, 1, 20);
        
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 1, 1);
		for (int i = 0; i < 5; i++) {
			ArmoryInventory.EquipItem(ArmoryInventory.Items[i]);
				}
        
        /*BodyInventory.EquipItem(BodyInventory.Items[1]);
        HeadInventory.EquipItem(HeadInventory.Items[1]);
        ArmLInventory.EquipItem(ArmLInventory.Items[0]);
        ArmRInventory.EquipItem(ArmRInventory.Items[3]);
        LegsInventory.EquipItem(LegsInventory.Items[1]);*/
        
        AddCurrency(1000,BuyCurrencyType.Magnets);
        AddCurrency(100,BuyCurrencyType.Crystals);

		if(NetworkManager.Instance.usingParse)
			SaveParseData();

		for (int i = 0; i < 20; i++) {
			SocialManager.Instance.AddFriend("friend" + i);
				}

        //PreffixSolver.GiveItem(PreffixType.ARMOR, 1, 1);
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 2, 1);
    }

    public void AddCurrency(int amount, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Magnets)
        {
            Magnets += amount;
        }
        else if(currency == BuyCurrencyType.Crystals)
        {
            Crystals += amount;
        }
        GUIManager.Instance.MainGUI.UpdateCurrencyCount();

		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
    }
    
    public void RemoveCurrency(int amount, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Magnets)
        {
            Magnets -= amount;
        }
        else if(currency == BuyCurrencyType.Crystals)
        {
            Crystals -= amount;
        }
        GUIManager.Instance.MainGUI.UpdateCurrencyCount();
		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
    }
 
    public bool CanYouAfford(int price, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Magnets)
        {
            if(Magnets >= price)
                return true;
            else
                return false;
        }
        else if(currency == BuyCurrencyType.Crystals)
        {
            if(Crystals >= price)
                return true;
            else
                return false;
        }
        
        else
            return false;
    }
	
	#region inventory

	public bool DoYouHaveSpaceForThisItem(RPGItem item, int level, int amount)
	{
		if(item.ItemCategory == ItemType.Armor)
		{
			if(ArmoryInventory.DoYouHaveSpaceForThisItem(item, level, amount))
				return true;
		}
		else
		{
			if(MainInventory.DoYouHaveSpaceForThisItem(item, level, amount))
				return true;
		}

		return false;
	}

	public bool DoYouHaveSpaceForThisItem(InventoryItem item)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			if(ArmoryInventory.DoYouHaveSpaceForThisItem(item))
				return true;
		}
		else
		{
			if(MainInventory.DoYouHaveSpaceForThisItem(item))
				return true;
		}
		
		return false;
	}

	public void AddItem(InventoryItem item)
	{
		Debug.Log(item.rpgItem.UniqueId + item.Level + item.rpgItem.Stackable);
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.AddItem(item);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else if(item.rpgItem.ItemCategory == ItemType.Currency)
		{
			if(item.rpgItem.ID == 1)
			{
				AddCurrency(item.CurrentAmount, BuyCurrencyType.Magnets);
			}
			if(item.rpgItem.ID == 2)
			{
				AddCurrency(item.CurrentAmount, BuyCurrencyType.Crystals);
			}
			if(NetworkManager.Instance.usingParse)
				UpdateWalletParseData();
		}
		else
		{
			MainInventory.AddItem(item);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
	}

	public void RemoveItem(InventoryItem item)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.RemoveItem(item);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else if(item.rpgItem.ItemCategory == ItemType.Currency)
		{
			if(item.rpgItem.ID == 1)
			{
				RemoveCurrency(item.CurrentAmount, BuyCurrencyType.Magnets);
			}
			if(item.rpgItem.ID == 2)
			{
				RemoveCurrency(item.CurrentAmount, BuyCurrencyType.Crystals);
			}
			if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
		}
		else
		{
			MainInventory.RemoveItem(item);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
	}

	public void AddItem(RPGItem item, int level)
	{
		AddItem(item, level, 1);
	}
	
	public void AddItem(RPGItem item, int level, int amount)
	{
		if(item.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.AddItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else
		{
			MainInventory.AddItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
	}
	
	public void RemoveItem(RPGItem item, int level)
	{
		RemoveItem(item, level, 1);
	}
	
	public void RemoveItem(RPGItem item, int level, int amount)
	{
		if(item.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.RemoveItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else
		{
			MainInventory.RemoveItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
    }
    
    #endregion

	#region Parse Player Data

	public void SaveParseData()
	{
		Debug.Log("saving parse data");
		byte[] mainInventoryParseList = ParseInventoryList(MainInventory);
		byte[] armoryInventoryParseList = ParseInventoryList(ArmoryInventory);
		byte[] depositBoxParseList = ParseInventoryList(DepositBox);
		//IList<object> mainInventoryParseList = ParseInventoryList(MainInventory);
		//Debug.Log(mainInventoryParseList[0]);
		//IList<Object> armoryInventoryParseList = ParseInventoryList(ArmoryInventory);
		//Debug.Log(armoryInventoryParseList.Count);
	
		playerData["username"] = ParseUser.CurrentUser.Username;
		playerData["InventoryList"] = mainInventoryParseList;
		playerData["ArmoryList"] = armoryInventoryParseList;
		playerData["DepositBox"] = depositBoxParseList;
		playerData["magnets"] = Magnets;
		playerData["crystals"] = Crystals;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				ParseObjectId = playerData.ObjectId;
				Debug.Log(ParseObjectId);
			}
			else{
				Debug.LogError(t.Exception.Message);
			}
		}
		);
	}

	public void UpdateWalletParseData()
	{
		if(string.IsNullOrEmpty(ParseObjectId) || !NetworkManager.Instance.usingParse)
		{
			Debug.LogWarning("no parse id");
			return;
		}

		playerData["magnets"] = Magnets;
		playerData["crystals"] = Crystals;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				Debug.Log("truly updated wallet");
			}
		}
		);
		Debug.Log("updating wallet");
	}

	public void UpdateInventoryParseData(string field, byte[] inventoryList)
	{
		if(string.IsNullOrEmpty(ParseObjectId) || !NetworkManager.Instance.usingParse)
		{
			Debug.LogWarning("no parse id");
			return;
		}

		playerData[field] = inventoryList;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				Debug.Log("truly updated " + field);
			}
		}
		);
		Debug.Log("updating " + field);
	}

	public IEnumerator RetrieveParseData()
	{
		var playerDataQuery = new ParseQuery<ParseObject>("PlayerData").WhereEqualTo("username", ParseUser.CurrentUser.Username);
		var queryTask = playerDataQuery.FindAsync();
		while (!queryTask.IsCompleted)
			yield return null;

		IEnumerable<ParseObject> obj = queryTask.Result;
		foreach(var player in obj)
		{
			Debug.LogWarning("retrieved 1 player data profile");
			//Debug.Log(player.Get<int>("magnets").ToString());
			InterpretParseInventoryList(MainInventory, player.Get<byte[]>("InventoryList"));
			InterpretParseInventoryList(ArmoryInventory, player.Get<byte[]>("ArmoryList"));
			InterpretParseInventoryList(DepositBox, player.Get<byte[]>("DepositBox"));
			Magnets = player.Get<int>("magnets");
			Crystals = player.Get<int>("crystals");
		}
		
		GUIManager.Instance.IntroGUI.StartGame();
	}

	public byte[] ParseInventoryList(Inventory invent)
	{

		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		List<ParseInventoryItem> items = new List<ParseInventoryItem>();

		for (int i = 0; i < invent.Items.Count; i++) {
			ParseInventoryItem item = new ParseInventoryItem();
			item.IsItemEquipped = invent.Items[i].IsItemEquipped;
			item.UniqueItemId = invent.Items[i].UniqueItemId;
			item.Amount = invent.Items[i].CurrentAmount;
			item.ItemLevel = invent.Items[i].Level;
			items.Add(item);
		}
		//return items
		b.Serialize(m, items);
		return m.GetBuffer();
	}

	public void InterpretParseInventoryList(Inventory inventory, byte[] data)
	{
		List<ParseInventoryItem> items = new List<ParseInventoryItem>();
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(data);
		items = (List<ParseInventoryItem>)bb.Deserialize(mm);
		for (int i = 0; i < items.Count; i++) 
		{
			if(items[i].UniqueItemId.IndexOf("ARMOR") != -1)
			{
				inventory.AddItem(Storage.LoadbyUniqueId<RPGArmor>(items[i].UniqueItemId, new RPGArmor()), items[i].ItemLevel, items[i].Amount);
				if(items[i].IsItemEquipped)
					inventory.EquipItem(items[i].UniqueItemId, items[i].ItemLevel);
			}
			else if(items[i].UniqueItemId.IndexOf("ITEM") != -1)
			{
				inventory.AddItem(Storage.LoadbyUniqueId<RPGItem>(items[i].UniqueItemId, new RPGItem()), items[i].ItemLevel, items[i].Amount);
			}
		}
	}

	#endregion

}
