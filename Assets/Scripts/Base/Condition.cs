using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

public class Condition  
{
	[XmlAttribute (AttributeName = "ITH")]
	public string ItemToHave;
	[XmlAttribute (AttributeName = "ITHLvl")]
	public int ItemToHaveLevel;
	[XmlAttribute (AttributeName = "ATR")]
	public int AmountToReach;
	[XmlAttribute (AttributeName = "CT")]
	public ConditionTypeEnum ConditionType;
	[XmlAttribute (AttributeName = "SI")]
	public int SecondaryID;
	
	//only for GUI operations, not used in the game
	[XmlIgnore]
	public string SearchString;
	[XmlIgnore]
	public string SearchId;
	[XmlIgnore]
	public List<IItem> displayedItems;
	[XmlIgnore]
	public bool FirstLoad;
	
	[XmlIgnore]
	public int ID
	{
		get
		{
			return Convert.ToInt32(ItemToHave);
		}
	}
	
	public Condition()
	{
		displayedItems = new List<IItem>();
		//Clear();
		FirstLoad = true;
	}
	
	public bool Validate()
	{
		if (string.IsNullOrEmpty(ItemToHave))
			return true;
		switch(ConditionType)
		{
		case ConditionTypeEnum.CanBattle:
			//Debug.Log("canbattle: " + PlayerManager.Instance.avatarActionManager.CanBattle());
			return PlayerManager.Instance.avatarActionManager.CanBattle();
			//items must be in inventory
		case ConditionTypeEnum.SomeItemMustBeInInventory :
			bool state = false;
			state = PlayerManager.Instance.Hero.MainInventory.DoYouHaveThisItem(ItemToHave, ItemToHaveLevel, AmountToReach);
			if(!state)
				state = PlayerManager.Instance.Hero.ArmoryInventory.DoYouHaveThisItem(ItemToHave, ItemToHaveLevel, AmountToReach);

			return state;
			//quest not started
		case ConditionTypeEnum.QuestNotStarted: 
			return !PlayerManager.Instance.Hero.questLog.IsQuestStarted(Convert.ToInt32(ItemToHave));
			//quest startet not finished (some of the tasks are not completed)
		case ConditionTypeEnum.QuestInProgress: 
			return PlayerManager.Instance.Hero.questLog.IsQuestInProgress(Convert.ToInt32(ItemToHave));
			//quest finished = you can end it now
		case ConditionTypeEnum.QuestCanFinish:
			return PlayerManager.Instance.Hero.questLog.CanFinishQuest(Convert.ToInt32(ItemToHave));
			break;
		case ConditionTypeEnum.QuestFinished: 
			return PlayerManager.Instance.Hero.questLog.IsQuestFinished(Convert.ToInt32(ItemToHave));
			//quest completed = in quest log "quest completed"
		case ConditionTypeEnum.QuestCompleted:
			return PlayerManager.Instance.Hero.questLog.IsQuestCompleted(Convert.ToInt32(ItemToHave));	
		case ConditionTypeEnum.QuestStepInProgress:
			//itemtohave = quest id;
			//amount to reach = quest step
			if (PlayerManager.Instance.Hero.questLog.IsQuestStepInProgress(Convert.ToInt32(ItemToHave), AmountToReach))
				return true;
			break;
		}
		
		return false;
	}
	
	public void Search(List<IItem> items)
	{
		displayedItems = new List<IItem>();
		foreach(IItem item in items)
		{
			if (item.Name.ToLower().IndexOf(SearchString.ToLower()) != -1)
			{
				displayedItems.Add(item);
			}
		}
	}

	public void Clear()
	{
		SearchString = string.Empty;
		SearchId = string.Empty;
		ItemToHave = string.Empty;
	}
	
	public void PrepareSearch(string temp, List<IItem> list)
	{
		if (temp != SearchString && string.IsNullOrEmpty(temp))
		{
			displayedItems = list;
			Clear();
		}
		
		if (temp != SearchString)
		{
			SearchString = temp;
			Search(list);
		}
	}
}

public enum ConditionTypeEnum
{
	SomeItemMustBeInInventory,
	QuestNotStarted,
	QuestInProgress,
	QuestCanFinish,
	QuestFinished,
	QuestCompleted,
	KillTarget,
	CompletedQuestsCount,
	QuestStepInProgress,
	QuestFailed,
	ReachMinigameHighScoreRank,
	CanBattle,
}