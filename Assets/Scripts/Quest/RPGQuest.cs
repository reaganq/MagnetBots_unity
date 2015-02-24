using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

//[XmlInclude(typeof(Reward))]
[XmlInclude(typeof(QuestStep))]
[XmlInclude(typeof(Condition))]
[XmlInclude(typeof(LootItem))]
[XmlInclude(typeof(Task))]
public class RPGQuest : BasicItem 
{
	//public List<Reward> Rewards;
	public List<QuestStep> questSteps;
	//public List<AlternateEnd> AlternateEnds;
	public bool repeatable;
	public bool timed;
	public float timeLimit;
	public int maxNumberOfLoots;
	public string initialQuestLog;
	public string finalQuestLog;
	public List<LootItem> allLoots;
	public QuestType questType;
	public List<Task> allTasks;
	public List<Condition> Conditions;
	//recorded in questlog
	[XmlIgnore]
	public bool Rewarded;
	[XmlIgnore]
	public float startTime;
	[XmlIgnore]
	public float endTime;
	//public List<LootItem> finalLoots;
	//public List<LootItem> determinedLoots;
	
	public RPGQuest()
	{
		//Rewards = new List<Reward>();
		questSteps = new List<QuestStep>();
		finalQuestLog = string.Empty;
		preffix = "QUEST";
		allLoots = new List<LootItem>();
		maxNumberOfLoots = 0;
		//determinedLoots = new List<LootItem>();
		allTasks = new List<Task>();
		Conditions = new List<Condition>();
		//finalLoots = new List<LootItem>();
		//AlternateEnds = new List<AlternateEnd>();
	}

	public bool Validate()
	{
		for (int i = 0; i < Conditions.Count; i++) {
			if(Conditions[i].Validate() == false)
				return false;
		}
		return true;
	}
	
	public QuestStep CurrentStep
	{
		get
		{
			foreach(QuestStep questStep in questSteps)
			{
				if (questStep.IsQuestStepFinished() == false)
					return questStep;
			}
			return null;
		}
	}

	public LootItemList GiveQuestReward()
	{
		LootItemList lootItemList = new LootItemList();
		int num = 0;
		for (int i = 0; i < allLoots.Count; i++) { 
			if(!allLoots[i].Validate())
				continue;
			float chance = UnityEngine.Random.Range(0.0f, 1.0f);
			if(chance <= allLoots[i].dropRate)
			{
				int index = UnityEngine.Random.Range(0, allLoots[i].itemID.Count);
				if(allLoots[i].itemType == ItemType.Currency)
				{
					RPGCurrency currency = Storage.LoadById<RPGCurrency>(allLoots[i].itemID[index], new RPGCurrency());
					lootItemList.currencies.Add(currency);
				}
				else if(allLoots[i].itemType == ItemType.Badge)
				{
					RPGBadge badge = Storage.LoadById<RPGBadge>(allLoots[i].itemID[index], new RPGBadge());
					lootItemList.badges.Add(badge);
				}
				else 
				{
					InventoryItem newItem = new InventoryItem();
					if(allLoots[i].itemType == ItemType.Armor)
					{
						RPGArmor armor = Storage.LoadById<RPGArmor>(allLoots[i].itemID[index], new RPGArmor());
						newItem.rpgItem = armor;
					}
					else
					{
						RPGItem item = Storage.LoadById<RPGItem>(allLoots[i].itemID[index], new RPGItem());
						newItem.rpgItem = item;
					}
					newItem.CurrentAmount = UnityEngine.Random.Range(allLoots[i].minQuantity, allLoots[i].maxQuantity);
					newItem.UniqueItemId = newItem.rpgItem.UniqueId;
					newItem.Level = UnityEngine.Random.Range(1, allLoots[i].itemLevel);
					lootItemList.items.Add(newItem);
				}
				if(!allLoots[i].definiteDrop)
					num ++;
			}
			if(num >= maxNumberOfLoots)
				break;
		}

		PlayerManager.Instance.GiveReward(lootItemList);
		return lootItemList;
	}

	/*public void GenerateLoot()
	{
		finalLoots.Clear();
		for (int i = 0; i < allLoots.Count; i++) 
		{
			float chance = Random.Range(0.0f, 1.0f);
			if(chance <= allLoots[i].dropRate)
			{
				Debug.Log(loots[i].itemType.ToString() + i);
				InventoryItem newItem = new InventoryItem();
				if(loots[i].itemType == ItemType.Currency)
				{
					RPGCurrency currency = Storage.LoadById<RPGCurrency>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGCurrency());
					newItem.rpgItem = currency;
				}
				else if(loots[i].itemType == ItemType.Armor)
				{
					RPGArmor armor = Storage.LoadById<RPGArmor>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGArmor());
					newItem.rpgItem = armor;
				}
				else if(loots[i].itemType == ItemType.Normal || loots[i].itemType == ItemType.UpgradeMaterials)
				{
					RPGItem item = Storage.LoadById<RPGItem>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGItem());
					newItem.rpgItem = item;
				}
				newItem.CurrentAmount = Random.Range(loots[i].minQuantity, loots[i].maxQuantity);
				newItem.UniqueItemId = newItem.rpgItem.UniqueId;
				newItem.Level = Random.Range(1, loots[i].itemLevel);
				lootItems.Add(newItem);
			}
		}
	}*/
	
	public bool IsCurrentStep(int stepId)
	{
		QuestStep qs = CurrentStep;
		
		if (qs == null)
			return false;
		
		if (qs.StepNumber == stepId)
			return true;
		
		return false;
	}
	
	public bool IsFinished
	{
		get
		{
			bool result = true;
			foreach(QuestStep questStep in questSteps)
			{
				if (!questStep.IsQuestStepFinished())
					result = false;
			}
			return result;
		}
	}

	public bool HasFailed()
	{
		if(timed && ((Time.realtimeSinceStartup - startTime) > timeLimit))
			return true;
		return false;
	}
	//paragraph task
	public void CheckParagraph(int paragraphID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.PARAGRAPH && task.TaskType == TaskTypeEnum.ReachPartOfConversation 
			    && task.TaskTarget == paragraphID)
				task.CurrentAmount = 1;
		}
	}
	
	//line text task
	public void CheckLineText(int lineTextID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.LINETEXT && task.TaskType == TaskTypeEnum.ReachPartOfConversation 
			    && task.TaskTarget == lineTextID)
				task.CurrentAmount = 1;
		}
	}
	
	//enemytask
	public void CheckKilledEnemy(int enemyID)
	{
		QuestStep questStep = CurrentStep;
		
		if(questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.PreffixTarget == PreffixType.ENEMY && task.TaskType == TaskTypeEnum.KillEnemy
			    && task.TaskTarget == enemyID)
				task.CurrentAmount++;
		}
	}
	
	//bring item task
	public void CheckInventory()
	{
		QuestStep questStep = CurrentStep;
		
		if (questStep == null)
			return;
		foreach(Task task in questStep.Tasks)
		{
			if (task.TaskType != TaskTypeEnum.BringItem)
				continue;
			if (task.PreffixTarget == PreffixType.ITEM)
			{
				task.CurrentAmount = PlayerManager.Instance.Hero.MainInventory.CountItem(task.PreffixTarget.ToString() + task.TaskTarget.ToString()); 
			}
			else if (task.PreffixTarget == PreffixType.ARMOR)
			{
				task.CurrentAmount = PlayerManager.Instance.Hero.ArmoryInventory.CountItem(task.PreffixTarget.ToString() + task.TaskTarget.ToString()); 
			}
		}
	}
	
	//visit area
	public void VisitArea(int WorldObjectID)
	{
		QuestStep questStep = CurrentStep;
		
		if (questStep == null)
			return;
		
		foreach(Task task in questStep.Tasks)
		{
			if (task.TaskType == TaskTypeEnum.VisitArea && task.TaskTarget == WorldObjectID)
			{
				task.CurrentAmount = 1; 
			}
		}
	}
	
	public void GiveReward()
	{
		/*foreach(Reward r in Rewards)
		{
			PreffixSolver.GiveItem(r.Preffix, r.ItemId, r.Amount);
		}*/
	}
}

public enum QuestType
{
	story,
	worldEvent,
	collection,
}

