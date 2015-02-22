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

	public Inventory NakedArmoryInventory;
    public Inventory MainInventory;
	public Inventory ArmoryInventory;
	public Inventory PlayerShop;
	//public Inventory DepositBox;
    public Equipment Equip;
	public QuestLog questLog;
	public Jukebox jukeBox;
		
	public Inventory playerShopInventory;
	public int shopTill;

	public PlayerProfile profile;

	public int Coins;
	public int Magnets;
	public int CitizenPoints;
	public int BankCoins;
	public string ParseObjectId;
	ParseObject playerData = new ParseObject("PlayerData");

	public void Awake()
	{
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
	}
 
    public PlayerInformation()
    {
		NakedArmoryInventory = new Inventory();
        MainInventory = new Inventory();
		ArmoryInventory = new Inventory();
		jukeBox = new Jukebox();
		playerShopInventory = new Inventory();
        Equip = new Equipment();
		questLog = new QuestLog();
		profile = new PlayerProfile();
	}
    
    public void StartNewGame()
    {
		profile.name = PlayerManager.Instance.data.GenerateRandomString(6);

        for (int i = 1; i < 20 ; i++) {
            PreffixSolver.GiveItem(PreffixType.ARMOR, i,1, 1);
            //Debug.Log(i);
        }

		for (int i = 6; i < 11; i++) {
			PreffixSolver.GiveItem(PreffixType.ARMOR, i,3, 1);
				}

		PreffixSolver.GiveItem(PreffixType.ITEM, 2, 1, 20);
        
		for (int i = 0; i < 5; i++) {
			ArmoryInventory.EquipItem(NakedArmoryInventory.Items[i]);
				}
        
        AddCurrency(1000,BuyCurrencyType.Coins);
        AddCurrency(100,BuyCurrencyType.Magnets);
		AddCurrency(100, BuyCurrencyType.Magnets);

		if(NetworkManager.Instance.usingParse)
			SaveParseData();

		for (int i = 0; i < 20; i++) {
			SocialManager.Instance.AddFriend("friend" + i);
				}
    }

    public void AddCurrency(int amount, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Coins)
        {
            Coins += amount;
        }
        else if(currency == BuyCurrencyType.Magnets)
        {
            Magnets += amount;
        }
		else if(currency == BuyCurrencyType.CitizenPoints)
		{
			CitizenPoints += amount;
		}
        GUIManager.Instance.MainGUI.UpdateCurrencyCount();

		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
    }
    
    public void RemoveCurrency(int amount, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Coins)
        {
            Coins -= amount;
        }
        else if(currency == BuyCurrencyType.Magnets)
        {
            Magnets -= amount;
        }
		else if(currency == BuyCurrencyType.CitizenPoints)
		{
			CitizenPoints -= amount;
		}
        GUIManager.Instance.MainGUI.UpdateCurrencyCount();
		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
    }

	public void Withdraw(int amount)
	{
		BankCoins -= amount;
		Coins += amount;
		GUIManager.Instance.MainGUI.UpdateCurrencyCount();
		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
	}
 
	public void Deposit(int amount)
	{
		BankCoins += amount;
		Coins -= amount;
		GUIManager.Instance.MainGUI.UpdateCurrencyCount();
		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
	}

	public void CollectInterest(int amount)
	{
		BankCoins += amount;
		if(NetworkManager.Instance.usingParse)
			UpdateWalletParseData();
	}

    public bool CanYouAfford(int price, BuyCurrencyType currency)
    {
        if(currency == BuyCurrencyType.Coins)
        {
            if(Coins >= price)
                return true;
            else
                return false;
        }
        else if(currency == BuyCurrencyType.Magnets)
        {
            if(Magnets >= price)
                return true;
            else
                return false;
        }
        
        else
            return false;
    }
	#region jukebox
		
	public void AddDanceMove(int id)
	{
		jukeBox.AddDanceMove(id);
		Debug.Log(jukeBox.danceMoves.Count);
	}
	#endregion
	
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
		AddItem(item, item.CurrentAmount);
	}

	public void AddItem(InventoryItem item, int amount)
	{
		Debug.Log(item.rpgItem.UniqueId + item.Level + item.rpgItem.Stackable);
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.AddItem(item, amount);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else if(item.rpgItem.ItemCategory == ItemType.Currency)
		{
			if(item.rpgItem.ID == 1)
			{
				AddCurrency(amount, BuyCurrencyType.Coins);
			}
			if(item.rpgItem.ID == 2)
			{
				AddCurrency(amount, BuyCurrencyType.Magnets);
			}
			if(NetworkManager.Instance.usingParse)
				UpdateWalletParseData();
		}
		else
		{
			MainInventory.AddItem(item, amount);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
	}

	public void RemoveItem(InventoryItem item)
	{
		RemoveItem(item, item.CurrentAmount);
	}

	public void RemoveItem(InventoryItem item, int amount)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.RemoveItem(item, amount);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
		}
		else if(item.rpgItem.ItemCategory == ItemType.Currency)
		{
			if(item.rpgItem.ID == 1)
			{
				RemoveCurrency(amount, BuyCurrencyType.Coins);
			}
			if(item.rpgItem.ID == 2)
			{
				RemoveCurrency(amount, BuyCurrencyType.Magnets);
			}
			if(NetworkManager.Instance.usingParse)
				UpdateWalletParseData();
		}
		else
		{
			MainInventory.RemoveItem(item, amount);
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
		else if(item.ItemCategory == ItemType.NakedArmor)
		{
			NakedArmoryInventory.ReplaceNakedItem(item, level, 1);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("NakedArmoryList", ParseInventoryList(NakedArmoryInventory));
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
		else if(item.ItemCategory == ItemType.NakedArmor)
		{
			NakedArmoryInventory.RemoveItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
				UpdateInventoryParseData("NakedArmoryList", ParseInventoryList(NakedArmoryInventory));
		}
		else
		{
			MainInventory.RemoveItem(item, level, amount);
			if(NetworkManager.Instance.usingParse)
			UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
		}
    }

	public void FeedPlayer(InventoryItem item)
	{
		Debug.Log("feed " + item.rpgItem.Name);
	}

	public void PlayToy(InventoryItem item)
	{
		Debug.Log("playing with toy " + item.rpgItem.Name);
	}
    
    #endregion

	#region Parse Player Data

	public void SaveParseData()
	{
		Debug.Log("saving parse data");
		byte[] nakedArmoryParseList = ParseInventoryList(NakedArmoryInventory);
		byte[] mainInventoryParseList = ParseInventoryList(MainInventory);
		byte[] armoryInventoryParseList = ParseInventoryList(ArmoryInventory);
		//byte[] depositBoxParseList = ParseInventoryList(DepositBox);
		byte[] playerShopParseList = ParseInventoryList(playerShopInventory);
		byte[] jukeBoxList = ParseJukeBoxList();
		byte[] profile = ParsePlayerProfile();
		//IList<object> mainInventoryParseList = ParseInventoryList(MainInventory);
		//Debug.Log(mainInventoryParseList[0]);
		//IList<Object> armoryInventoryParseList = ParseInventoryList(ArmoryInventory);
		//Debug.Log(armoryInventoryParseList.Count);
	
		playerData["username"] = ParseUser.CurrentUser.Username;
		playerData["profile"] = profile;
		playerData["NakedArmoryList"] = nakedArmoryParseList;
		playerData["InventoryList"] = mainInventoryParseList;
		playerData["ArmoryList"] = armoryInventoryParseList;
		//playerData["DepositBox"] = depositBoxParseList;
		playerData["PlayerShopList"] = playerShopParseList;
		playerData["JukeBoxList"] = jukeBoxList;
		playerData["coins"] = Coins;
		playerData["magnets"] = Magnets;
		playerData["citizenpoints"] = CitizenPoints;
		playerData["shopTill"] = shopTill;
		playerData["bankcoins"] = BankCoins;
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

		playerData["coins"] = Coins;
		playerData["magnets"] = Magnets;
		playerData["citizenpoints"] = CitizenPoints;
		playerData["bankcoins"] = BankCoins;
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

	public void UpdateQuestLog()
	{
	}

	public void UpdateJukeBox()
	{
		if(string.IsNullOrEmpty(ParseObjectId) || !NetworkManager.Instance.usingParse)
		{
			Debug.LogWarning("no parse id");
			return;
		}
		playerData["JukeBoxList"] = ParseJukeBoxList();
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				Debug.Log("truly updated jukebox");
			}
		}
		);
		Debug.Log("updating jukebox");
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
			InterpretParseProfile(player.Get<byte[]>("profile"));
			InterpretParseInventoryList(NakedArmoryInventory, player.Get<byte[]>("NakedArmoryList"));
			InterpretParseInventoryList(MainInventory, player.Get<byte[]>("InventoryList"));
			InterpretParseInventoryList(ArmoryInventory, player.Get<byte[]>("ArmoryList"));
			//InterpretParseInventoryList(DepositBox, player.Get<byte[]>("DepositBox"));
			InterpretParseInventoryList(playerShopInventory, player.Get<byte[]>("PlayerShopList"));
			InterpretParseJukeBox(player.Get<byte[]>("JukeBoxList"));
			Coins = player.Get<int>("coins");
			Magnets = player.Get<int>("magnets");
			CitizenPoints = player.Get<int>("citizenpoints");
			shopTill = player.Get<int>("shopTill");
			BankCoins = player.Get<int>("bankcoins");
		}
		
		GUIManager.Instance.IntroGUI.StartGame();
	}

	public byte[] ParsePlayerProfile()
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, profile);
		return m.GetBuffer();
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
			item.isItemViewed = invent.Items[i].isItemViewed;
			items.Add(item);
		}
		//return items
		b.Serialize(m, items);
		return m.GetBuffer();
	}

	public byte[] ParseQuestLog()
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, questLog);
		return m.GetBuffer();
	}

	public byte[] ParseJukeBoxList()
	{
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		List<int> newList = new List<int>();
		for (int i = 0; i < jukeBox.danceMoves.Count; i++) {
			newList.Add(jukeBox.danceMoves[i].ID);
				}
		b.Serialize(m, newList);
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
				inventory.AddItem(Storage.LoadbyUniqueId<RPGArmor>(items[i].UniqueItemId, new RPGArmor()), items[i].ItemLevel, items[i].Amount, items[i].isItemViewed);
				if(items[i].IsItemEquipped)
				{
					inventory.EquipItem(items[i].UniqueItemId, items[i].ItemLevel);
				}
			}
			else if(items[i].UniqueItemId.IndexOf("ITEM") != -1)
			{
				inventory.AddItem(Storage.LoadbyUniqueId<RPGItem>(items[i].UniqueItemId, new RPGItem()), items[i].ItemLevel, items[i].Amount, items[i].isItemViewed);
			}
		}
	}

	public void InterpretParseProfile(byte[] data)
	{
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(data);
		profile = (PlayerProfile)bb.Deserialize(mm);
	}

	public void InterpretParseQuestLog()
	{
	}

	public void InterpretParseJukeBox(byte[] data)
	{
		List<int> items = new List<int>();
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(data);
		items = (List<int>)bb.Deserialize(mm);

		for (int i = 0; i < items.Count; i++) {
			jukeBox.AddDanceMove(items[i]);
				}
	}

	#endregion

}

