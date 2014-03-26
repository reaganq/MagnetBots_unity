using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

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

 	public int CurrentLevel;
	public int CurrentXP;
	public int CurrentLevelXP;
    
	public int Trophies;

	public int BaseHp;
	public int TotalHp;
	public int BonusHp;
	public int CurrentHp;
	//public float HpRegeneration = 0.0f;
	//public float ManaRegeneration = 0.5f;

	public int Magnets;
	public int Crystals;
	public int BankMagnets;
 
    public PlayerInformation()
    {
        MainInventory = new Inventory();
		ArmoryInventory = new Inventory();
		DepositBox = new Inventory();
        /*HeadInventory = new ArmoryInventory();
        BodyInventory = new ArmoryInventory();
        LegsInventory = new ArmoryInventory();
        ArmLInventory = new ArmoryInventory();
        ArmRInventory = new ArmoryInventory();*/
     //Settings = new GlobalSettings();
     //Controls = new InputControls();
     //HeroPosition = new ObjectPosition();
        Equip = new Equipment();

     
     //TotalHp = BaseHp = CurrentHp = Settings.StartHitPoint;
     //TotalMana = BaseMana = Settings.StartMana;
     //CurrentHp = CurrentMana = 50;
     //CurrentScene = -1;
 	}
 
 /*public void StorePlayerPosition(Transform transform)
 {   
     
     HeroPosition = ObjectPosition.FromTransform(transform);
     
     //game time and day cycle
     gameTime.Store(RenderSettings.fogDensity);
 }*/
 
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
        
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 1, 1);
		for (int i = 0; i < 6; i++) {
			ArmoryInventory.EquipItem(ArmoryInventory.Items[i]);
				}
        
		Debug.Log("items count" + ArmoryInventory.Items.Count);
        /*BodyInventory.EquipItem(BodyInventory.Items[1]);
        HeadInventory.EquipItem(HeadInventory.Items[1]);
        ArmLInventory.EquipItem(ArmLInventory.Items[0]);
        ArmRInventory.EquipItem(ArmRInventory.Items[3]);
        LegsInventory.EquipItem(LegsInventory.Items[1]);*/
        
        AddCurrency(1000,BuyCurrencyType.Magnets);
        AddCurrency(100,BuyCurrencyType.Crystals);
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 1, 1);
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 2, 1);
        
        
    }
 /*public void LevelUp()
 {
     //you cannot have higher level than in settings
     if (CurrentLevel >= Settings.MaximumLevel)
         return;
     
     CurrentLevel++;
     CurrentXP = CurrentXP - CurrentLevelXP;
     //determine new amount for XP
     if (Settings.Leveling == LevelingSystem.XP && !Settings.AllLevelsSameXp)
     {
         int totalXp = (CurrentLevelXP * (100 + Settings.NextLevelXp));
         CurrentLevelXP = (int)(totalXp / 100);
     }
     
     //add level up bonuses
     BaseHp += Settings.HitPointPerLevel;
        if (Settings.IsNewLevelHealPlayer)
         CurrentHp = BaseHp;
     AttributePoint += Settings.AttributePointPerLevel;
     SkillPoint += Settings.SkillPointPerLevel;
 }*/
 
	public void AddXp(int amount)
	{
	    CurrentXP += amount;
	     
	     //if (IsNewLevel)
	         //LevelUp();
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
        GUIManager.Instance.MainGUI.UpdateMagnetsCount();
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
        GUIManager.Instance.MainGUI.UpdateMagnetsCount();
    }
 
 	public void AddXpPercent(int percentOfLevel)
 	{
     	int amountToGain = (int)((CurrentLevelXP /100) * (100 + percentOfLevel));
     	CurrentXP += amountToGain;
     //if (IsNewLevel)
         //LevelUp();
 	}
 
 	public void ChangeHitPoint(int hitPointChange)
 	{
     	if (CurrentHp + hitPointChange > TotalHp)
         	CurrentHp = TotalHp;
     	else if (CurrentHp + hitPointChange < 0)
         	CurrentHp = 0;
     	else
         	CurrentHp += hitPointChange;
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

 	private bool IsNewLevel
 	{
     	get
     	{
         	return CurrentXP >= CurrentLevelXP;
     	}
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
	
	public void RemoveItem(RPGItem item, int level)
	{
		RemoveItem(item, level, 1);
	}
	
	public void RemoveItem(RPGItem item, int level, int amount)
	{
		if(item.ItemCategory == ItemType.Armor)
			ArmoryInventory.RemoveItem(item, level, amount);
		else
			MainInventory.RemoveItem(item, level, amount);
    }
    
    #endregion
 
}
