using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class PlayerInformation  {
 
 //public List<RPGContainer> SharedContainers;
 //public List<RPGAttribute> Attributes;
 //public List<RPGSkill> Skills;
 //public List<ActionEvent> ActionsToDo;
 //public List<HeroReputation> Reputations;
 
 //public ActiveBuffs Buffs;
 //public BonusCollection Bonuses;
 //public QuestLog Quest;
 //public AllLog Log;
    public Inventory Inventory;
    public Inventory HeadInventory;
    public Inventory BodyInventory;
    public Inventory ArmLInventory;
    public Inventory ArmRInventory;
    public Inventory LegsInventory;
    //public GlobalSettings Settings;
 //public InputControls Controls;
    //public ObjectPosition HeroPosition;
    //public Vector3 SpawnPosition;
    public Equipment Equip;
    //public GameTime gameTime;
    //public HotBar Bar;
 //public RPGCharacterRace characterRace;
 //public PlayerSpellbook Spellbook;
 //public PlayerGuild Guild;
 
 
 //current scene level - non multi terrain
 //public int CurrentScene;
 
 //current scene level - multi terrain
 //public string CurrentSceneName;
 
 //xp level
 public int CurrentLevel;
    
    public int Trophies;
 
 public int BaseHp;
 public int TotalHp;
 public int BonusHp;
 public int CurrentHp;
 //public float HpRegeneration = 0.0f;
 
 public float BaseMana;
 public float TotalMana;
 public float BonusMana;
 public float CurrentMana;
 //public float ManaRegeneration = 0.5f;
 
    public int Magnets;
    public int Crystals;
    
 public int CurrentXP;
 public int CurrentLevelXP;
 
 //public int AttributePoint;
 //public int SkillPoint;
 
 //public float AttackDelay;
 //public float SpellDelay;
 //public float UseItemDelay;
 
 //public int RaceID;
 //public int ClassID;
 
    [XmlIgnore]
    public GameObject playerGameObject;
 
    public PlayerInformation()
    {
     //Buffs = new ActiveBuffs();
     //Bonuses = new BonusCollection();
     //ActionsToDo = new List<ActionEvent>();
     //Attributes = new List<RPGAttribute>();
     //Skills = new List<RPGSkill>();
     //Quest = new QuestLog();
     //Log = new AllLog();
        Inventory = new Inventory();
        HeadInventory = new Inventory();
        BodyInventory = new Inventory();
        LegsInventory = new Inventory();
        ArmLInventory = new Inventory();
        ArmRInventory = new Inventory();
     //Settings = new GlobalSettings();
     //Controls = new InputControls();
     //HeroPosition = new ObjectPosition();
        Equip = new Equipment();
     //Spellbook = new PlayerSpellbook();
     //gameTime = new GameTime();
     //Bar = new HotBar();
        //characterRace = new RPGCharacterRace();
     //SharedContainers = new List<RPGContainer>();
     //Guild = new PlayerGuild();
     //Reputations = new List<HeroReputation>();
     
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
     //load items in inventory
     foreach(InventoryItem item in Inventory.Items)
         item.LoadItem();
        
        /*foreach(InventoryItem item in BodyInventory.Items)
         item.LoadItem();
 
        foreach(InventoryItem item in ArmLInventory.Items)
         item.LoadItem();
        
        foreach(InventoryItem item in HeadInventory.Items)
         item.LoadItem();
        
        foreach(InventoryItem item in ArmRInventory.Items)
         item.LoadItem();
        
        foreach(InventoryItem item in LegsInventory.Items)
         item.LoadItem();*/
     //load equipment
     Equip.LoadItems();
     
     //game time and day cycle
     //gameTime.FillGameObjects();
     //RenderSettings.fogDensity = gameTime.FogIntensity;
 }
    
    public void StartNewGame()
    {
        for (int i = 1; i < 15 ; i++) {
            PreffixSolver.GiveItem(PreffixType.ARMOR, i, 1);
            //Debug.Log(i);
        }
        
        //PreffixSolver.GiveItem(PreffixType.ARMOR, 1, 1);
        
        BodyInventory.EquipItem(BodyInventory.Items[1]);
        HeadInventory.EquipItem(HeadInventory.Items[1]);
        ArmLInventory.EquipItem(ArmLInventory.Items[0]);
        ArmRInventory.EquipItem(ArmRInventory.Items[3]);
        LegsInventory.EquipItem(LegsInventory.Items[1]);
        
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
        GUIManager.Instance.UpdateMagnetsCount();
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
        GUIManager.Instance.UpdateMagnetsCount();
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
    
 /*public void CalculateDamage(Player player)
 {
     PlayerAttackStats pa = new PlayerAttackStats(1, null, player);
     PlayerAttackStats.MinimumDamage = pa.SimulateMinimum();
     PlayerAttackStats.MaximumDamage = pa.SimulateMaximum();
     PlayerAttackStats.ChanceToHit = pa.CalculateChanceToHit();
 }*/

 private bool IsNewLevel
 {
     get
     {
         return CurrentXP >= CurrentLevelXP;
     }
 }
 
}
