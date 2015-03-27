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

	public string PlayerName;
	public Inventory NakedArmorInventory;
    public Inventory MainInventory;
	public Inventory ArmoryInventory;
	//public Inventory DepositBox;
    public Equipment Equip;
	public QuestLog questLog;
	public Jukebox jukeBox;
		
	public bool hasDoneTutorial;

	public Inventory playerShopInventory;

	public int shopTill;

	public PlayerProfile profile;
	
	public int Coins;
	public int Magnets;
	public int CitizenPoints;
	public int BankCoins;
	public string parseObjectID;
	ParseObject playerData;

	public bool hasItemChange;
	public bool hasWalletChange;
	public bool hasArmorChange;
	public bool hasNakedArmorChange;
	public bool hasPlayerShopChange;
	public bool hasProfileChange;
	public bool hasQuestChange;

    public PlayerInformation()
    {
		NakedArmorInventory = new Inventory();
        MainInventory = new Inventory();
		ArmoryInventory = new Inventory();
		jukeBox = new Jukebox();
		playerShopInventory = new Inventory();
        Equip = new Equipment();
		questLog = new QuestLog();
		profile = new PlayerProfile();
		playerData = new ParseObject("PlayerData");
		parseObjectID = " ";
		PlayerName = " ";
	}
    
    public void StartTestingGame()
    {
		NetworkManager.Instance.usingParse = false;
		SetPlayerName(GeneralData.GenerateRandomString(6));

        for (int i = 1; i < 25 ; i++) {
            PreffixSolver.GiveItem(PreffixType.ARMOR, i,1, 10);
        }
		for (int i = 6; i < 11; i++) {
			PreffixSolver.GiveItem(PreffixType.ARMOR, i,3, 10);
		}

		PreffixSolver.GiveItem(PreffixType.ITEM, 4, 1, 20);
		PreffixSolver.GiveItem(PreffixType.ITEM, 1, 1 , 5);
		//PreffixSolver.GiveItem(PreffixType.ITEM, 4, 1 , 1);
		PreffixSolver.GiveItem(PreffixType.ITEM, 6, 1 , 10);
		PreffixSolver.GiveItem(PreffixType.ITEM, 7, 1 , 3);
        AddCurrency(957,BuyCurrencyType.Coins);
        AddCurrency(100,BuyCurrencyType.Magnets);
		AddCurrency(152, BuyCurrencyType.CitizenPoints);
		StockItem (ArmoryInventory.Items[2], 1);
		StockItem(ArmoryInventory.Items[4], 1);

		EquipBaseNakedArmor();
		for (int i = 0; i < 20; i++) {
			SocialManager.Instance.AddFriend("friend" + i);
		}
		//EquipItem(ArmoryInventory.Items[16]);
		//EquipItem(ArmoryInventory.Items[15]);
		//EquipItem(ArmoryInventory.Items[17]);
		SaveParseData();

    }

	//start this after you create your character
	public void StartGameAfterTutorial()
	{
		hasDoneTutorial = true;
		//name already set
		SaveParseData();
		questLog.StartQuest(3);
		GameObject tesla = GameObject.FindGameObjectWithTag("Tesla");
		if(tesla != null)
		{
			NPC npc = tesla.GetComponent<NPC>();
			if(npc != null)
				GUIManager.Instance.DisplayNPC(npc);
		}
		GUIManager.Instance.UpdateCurrency();
	}

	public void SetPlayerName(string newName)
	{
		PlayerName = newName;
		profile.name = PlayerName;
		PhotonNetwork.playerName = PlayerName;
	}

	public void EquipBaseNakedArmor()
	{
		for (int i = 0; i < NakedArmorInventory.Items.Count; i++) {
			EquipItem(NakedArmorInventory.Items[i]);
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
		hasWalletChange = true;
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
		hasWalletChange = true;
    }

	public void Withdraw(int amount)
	{
		BankCoins -= amount;
		Coins += amount;
		hasWalletChange = true;
	}
 
	public void Deposit(int amount)
	{
		BankCoins += amount;
		Coins -= amount;
		hasWalletChange = true;
	}

	public void CollectInterest(int amount)
	{
		BankCoins += amount;
		hasWalletChange = true;
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
		else if(currency == BuyCurrencyType.CitizenPoints)
		{
			if(CitizenPoints >= price)
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

	public bool EquipItem(InventoryItem item)
	{
		if (!item.IsItemEquippable || item.IsItemEquipped)
			return false;
		
		if(PlayerManager.Instance.Hero.Equip.EquipItem((RPGArmor)item.rpgItem, item.Level))
		{
			item.IsItemEquipped = true;
			item.isItemViewed = true;
			if(item.rpgItem.ItemCategory == ItemType.NakedArmor)
			{
				hasNakedArmorChange = true;
			}
			else if(item.rpgItem.ItemCategory == ItemType.Armor)
			{
				hasArmorChange = true;

			}
			return true;
		}
		else
			return false;

	}

	public bool UnequipItem(InventoryItem item)
	{
		if (!item.IsItemEquipped)
			return false;

		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			if(item.rpgItem.EquipmentSlotIndex == EquipmentSlots.Head)
			{
				Equip.UnequipHead();
				return true;
			}
			else
			{
				for (int i = 0; i < NakedArmorInventory.Items.Count; i++) {
					if(NakedArmorInventory.Items[i].rpgItem.EquipmentSlotIndex == item.rpgItem.EquipmentSlotIndex)
					{
						EquipItem(NakedArmorInventory.Items[i]);
						return true;
					}
				}
				return false;
			}
		}
		else
			return false;
	}

	public bool UnequipItem(string itemID, int level)
	{
		for (int i = 0; i < NakedArmorInventory.Items.Count; i++) 
		{
			if(NakedArmorInventory.Items[i].UniqueItemId == itemID && NakedArmorInventory.Items[i].Level == level)
			{
				NakedArmorInventory.Items[i].IsItemEquipped = false;
				hasNakedArmorChange = true;
				return true;
			}
		}
		for (int i = 0; i < ArmoryInventory.Items.Count; i++) 
		{
			if(ArmoryInventory.Items[i].UniqueItemId == itemID && ArmoryInventory.Items[i].Level == level)
			{
				ArmoryInventory.Items[i].IsItemEquipped = false;
				hasArmorChange = true;
				return true;
			}
		}
		return false;
	}

	#region player shop

	public void CollectFromShopTill()
	{
		Coins += shopTill;
		shopTill = 0;
		hasWalletChange = true;
    }

	public void UpdatePlayerShop()
	{
		if(PlayerManager.Instance.avatarStatus != null)
			PlayerManager.Instance.avatarStatus.UpdateShopItems();
		hasPlayerShopChange = true;

		if(GUIManager.Instance.PlayerShopGUI.isDisplayed)
			GUIManager.Instance.PlayerShopGUI.RefreshInventoryIcons();
	}

	public void UnstockItem(InventoryItem item, int amount)
	{
		playerShopInventory.RemoveItem(item, amount);
		AddItem(item, amount);
		UpdatePlayerShop();
	}

	public void StockItem(InventoryItem item, int amount)
	{
		RemoveItem(item, amount);
		playerShopInventory.AddItem(item, amount);
		UpdatePlayerShop();
	}

	public void SoldItem(string uniqueItemId, int level, int amount)
	{
		shopTill = shopTill + playerShopInventory.RemoveItemByUniqueID(uniqueItemId, level, amount);
		hasWalletChange = true;
		UpdatePlayerShop();
	}

	#endregion

	#region inventory

	public bool DoYouHaveThisItem(InventoryItem item, bool ignoreLevel)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			return ArmoryInventory.DoYouHaveThisItem(item, ignoreLevel);
		}
		else
		{
			return MainInventory.DoYouHaveThisItem(item, ignoreLevel);
		}
	}

	public int GetItemAmount(InventoryItem item, bool ignoreLevel)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			return ArmoryInventory.GetItemAmount(item, ignoreLevel);
		}
		else
		{
			return MainInventory.GetItemAmount(item, ignoreLevel);
		}
	}

	public InventoryItem FindItem(RPGItem item, int level)
	{
		if(item.ItemCategory == ItemType.Armor || item.ItemCategory == ItemType.NakedArmor)
			return ArmoryInventory.FindItem(item, level);
		else
			return MainInventory.FindItem(item, level);
	}

	public int GetItemAmount(int id, int level, PreffixType type)
	{
		if(type == PreffixType.ARMOR)
		{
			return ArmoryInventory.GetItemAmount(id, level);
		}
		else if(type == PreffixType.NAKEDARMOR)
		{
			return NakedArmorInventory.GetItemAmount(id, level);
		}
		else
		{
			return MainInventory.GetItemAmount(id, level);
		}
	}

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

	public void AddRPGCurrency(RPGCurrency currency)
	{
		switch(currency.ID)
		{
		case 1:
			AddCurrency(currency.amount, BuyCurrencyType.Coins);
			break;
		case 2:
			AddCurrency(currency.amount, BuyCurrencyType.Magnets);
			break;
		case 3:
			AddCurrency(currency.amount, BuyCurrencyType.CitizenPoints);
			break;
		case 4:
			questLog.AddQuestPoints(currency.amount);
			break;
		case 5:
			break;
		}
	}

	public void AddBadge(RPGBadge badge)
	{
		if(profile.AddBadge(badge))
			hasProfileChange = true;
	}

	public void AddItem(InventoryItem item)
	{
		AddItem(item, item.CurrentAmount);
	}

	public void AddItem(InventoryItem item, int amount)
	{
		if(item.rpgItem.ItemCategory == ItemType.Armor)
		{
			ArmoryInventory.AddItem(item, amount);
			hasArmorChange = true;
		}
		else if(item.rpgItem.ItemCategory == ItemType.NakedArmor)
		{
			bool alreadyHasItemAtSameSlot = false;
			for (int i = 0; i < NakedArmorInventory.Items.Count; i++) {
				if(NakedArmorInventory.Items[i].rpgItem.EquipmentSlotIndex == item.rpgItem.EquipmentSlotIndex)
				{
					//TODO unequip already equipped cosmetics
					//if(NakedArmorInventory.Items[i].IsItemEquipped);
					NakedArmorInventory.Items[i] = item;
					alreadyHasItemAtSameSlot = true;
					break;
				}
			}
			if(!alreadyHasItemAtSameSlot)
				NakedArmorInventory.AddItem(item, amount);
			hasNakedArmorChange = true;
		}
		else
		{
			MainInventory.AddItem(item, amount);
			hasItemChange = true;
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
			hasArmorChange = true;
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
		}
		else if(item.rpgItem.ItemCategory == ItemType.NakedArmor)
		{
			NakedArmorInventory.RemoveItem(item, amount);
			hasNakedArmorChange = true;
		}
		else
		{
			MainInventory.RemoveItem(item, amount);
			hasItemChange = true;
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
			hasArmorChange = true;
		}
		else if(item.ItemCategory == ItemType.NakedArmor)
		{
			NakedArmorInventory.ReplaceNakedItem(item, level, 1);
			hasNakedArmorChange = true;
		}
		else
		{
			MainInventory.AddItem(item, level, amount);
			hasItemChange = true;
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
			hasArmorChange = true;
		}
		else if(item.ItemCategory == ItemType.NakedArmor)
		{
			NakedArmorInventory.RemoveItem(item, level, amount);
			hasNakedArmorChange = true;
		}
		else
		{
			MainInventory.RemoveItem(item, level, amount);
			hasItemChange = true;
		}
    }
    
    #endregion

	#region Parse Player Data

	public void SaveParseData()
	{
		Debug.Log("enter saving parse data");
		if (!NetworkManager.Instance.usingParse) {
			Debug.Log("return from saving parse data");
			return;
				}
		Debug.Log("saving parse data");
		byte[] nakedArmoryParseList = ParseInventoryList(NakedArmorInventory);
		byte[] mainInventoryParseList = ParseInventoryList(MainInventory);
		byte[] armoryInventoryParseList = ParseInventoryList(ArmoryInventory);
		byte[] playerShopParseList = ParseInventoryList(playerShopInventory);
		byte[] jukeBoxList = ParseJukeBoxList();
		byte[] profile = ParsePlayerProfile();
		byte[] questLogData = ParseQuestLog();

		playerData["username"] = ParseUser.CurrentUser.Username;
		playerData["playername"] = PlayerName;
		playerData["profile"] = profile;
		playerData["NakedArmoryList"] = nakedArmoryParseList;
		playerData["InventoryList"] = mainInventoryParseList;
		playerData["ArmoryList"] = armoryInventoryParseList;
		//playerData["DepositBox"] = depositBoxParseList;
		playerData["PlayerShopList"] = playerShopParseList;
		//playerData["JukeBoxList"] = jukeBoxList;
		playerData["coins"] = Coins;
		playerData["magnets"] = Magnets;
		playerData["citizenpoints"] = CitizenPoints;
		playerData["shopTill"] = shopTill;
		playerData["bankcoins"] = BankCoins;
		playerData["hasDoneTutorial"] = hasDoneTutorial;
		playerData["parseObjectID"] = "";
		playerData["questLog"] = questLogData;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				parseObjectID = playerData.ObjectId;
				Debug.Log(parseObjectID);
			}
			else{
				Debug.LogError(t.Exception.Message);
			}
		}
		);
		UpdateParseObjectID();
	}

	public void UpdateParseObjectID()
	{
		playerData["parseObjectID"] = parseObjectID;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
			if(t.IsCompleted)
			{
				Debug.Log("truly updated parse object id");
			}
		}
		);
		Debug.Log("updating parseobject id");
	}

	//string.IsNullOrEmpty(ParseObjectId) || 
	public void UpdateTutorialState()
	{
		if(!NetworkManager.Instance.usingParse || !GameManager.Instance.GameHasStarted)
		{
			Debug.LogWarning("no parse id");
			return;
		}
		playerData["hasDoneTutorial"] = hasDoneTutorial;
		playerData.SaveAsync().ContinueWith( t =>
		                                    {
            if(t.IsCompleted)
            {
				Debug.Log("truly updated has done tutorial state");
			}
		}
		);
		Debug.Log("updating tutorial state");
	}

	public void UpdateWalletParseData()
	{

		playerData["coins"] = Coins;
		playerData["magnets"] = Magnets;
		playerData["citizenpoints"] = CitizenPoints;
		playerData["bankcoins"] = BankCoins;
		playerData["shopTill"] = shopTill;
	}

	public void UpdateInventoryParseData(string field, byte[] inventoryList)
	{
		playerData[field] = inventoryList;

	}

	public void UpdateQuestLog()
	{
		playerData["questLog"] = ParseQuestLog();
	}

	public void UpdateProfile()
	{
		playerData["profile"] = ParsePlayerProfile();
	}

	public void UpdateJukeBox()
	{
		if(!NetworkManager.Instance.usingParse)
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
			InterpretParseInventoryList(NakedArmorInventory, player.Get<byte[]>("NakedArmoryList"));
			InterpretParseInventoryList(MainInventory, player.Get<byte[]>("InventoryList"));
			InterpretParseInventoryList(ArmoryInventory, player.Get<byte[]>("ArmoryList"));
			//InterpretParseInventoryList(DepositBox, player.Get<byte[]>("DepositBox"));
			InterpretParseInventoryList(playerShopInventory, player.Get<byte[]>("PlayerShopList"));
			InterpretParseQuestLog(player.Get<byte[]>("questLog"));
			//InterpretParseJukeBox(player.Get<byte[]>("JukeBoxList"));
			PlayerName = player.Get<string>("playername");
			Coins = player.Get<int>("coins");
			Magnets = player.Get<int>("magnets");
			CitizenPoints = player.Get<int>("citizenpoints");
			shopTill = player.Get<int>("shopTill");
			BankCoins = player.Get<int>("bankcoins");
			hasDoneTutorial = player.Get<bool>("hasDoneTutorial");
			parseObjectID = player.Get<string>("parseObjectID");

		}
		if(hasDoneTutorial)
			GameManager.Instance.newGame = false;
		else
			GameManager.Instance.newGame = true;

		ParseQuery<ParseObject> query = ParseObject.GetQuery("PlayerData");
		query.GetAsync(parseObjectID).ContinueWith(t =>
		{
			playerData = t.Result;
		});

		GUIManager.Instance.IntroGUI.StartGame();
	}

	public byte[] ParsePlayerProfile()
	{
		//populate ParsePlayerProfileData
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		ParsePlayerProfileData p = new ParsePlayerProfileData(profile);
		b.Serialize(m, p);
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
		b.Serialize(m, questLog.ParseThisQuestLog());
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
		ParsePlayerProfileData p = new ParsePlayerProfileData();
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(data);
		p = (ParsePlayerProfileData)bb.Deserialize(mm);
		profile.UpdateProfileFromParse(p);
	}

	public void InterpretParseQuestLog(byte[] data)
	{
		ParseQuestLogData newQuestLog = new ParseQuestLogData();
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(data);
		newQuestLog = (ParseQuestLogData)bb.Deserialize(mm);
		questLog.InterpretParseQuestLog(newQuestLog);
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

	public void TryToUpdateParse()
	{
		if(hasWalletChange)
		{
			GUIManager.Instance.UpdateCurrency();
		}

		if(!NetworkManager.Instance.usingParse || !GameManager.Instance.GameHasStarted)
					return;
	
		bool shouldSave = false;
		if(hasItemChange)
		{
			UpdateInventoryParseData("InventoryList", ParseInventoryList(MainInventory));
			hasItemChange = false;
			shouldSave = true;
		}
		if(hasNakedArmorChange)
		{
			UpdateInventoryParseData("NakedArmoryList", ParseInventoryList(NakedArmorInventory));
			hasNakedArmorChange = false;
			shouldSave = true;
		}
		if(hasArmorChange)
		{
			UpdateInventoryParseData("ArmoryList", ParseInventoryList(ArmoryInventory));
			hasArmorChange = false;
			shouldSave = true;
		}
		if(hasWalletChange)
		{
			UpdateWalletParseData();
			hasWalletChange = false;
			shouldSave = true;
		}
		if(hasPlayerShopChange)
		{
			UpdateInventoryParseData("PlayerShopList", ParseInventoryList(playerShopInventory));
			hasPlayerShopChange = false;
			shouldSave = true;
		}
		if(hasProfileChange)
		{
			UpdateProfile();
			hasProfileChange = false;
			shouldSave = true;
		}
		if(hasQuestChange)
		{
			UpdateQuestLog();
			hasQuestChange = false;
			shouldSave = true;
		}
		if(shouldSave)
		{
			playerData.SaveAsync().ContinueWith( t =>
			                                    {
				if(t.IsCompleted)
				{
					Debug.Log("truly saved player data");
				}
			}
			);
			Debug.LogWarning("saving player data");
		}

	}


}

