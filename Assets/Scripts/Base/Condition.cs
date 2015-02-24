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
			//items must be in inventory
		case ConditionTypeEnum.SomeItemMustBeInInventory :
			bool state = false;
			state = PlayerManager.Instance.Hero.MainInventory.DoYouHaveThisItem(ItemToHave, ItemToHaveLevel, AmountToReach);
			if(!state)
				state = PlayerManager.Instance.Hero.ArmoryInventory.DoYouHaveThisItem(ItemToHave, ItemToHaveLevel, AmountToReach);

			return state;
			//quest not started
		case ConditionTypeEnum.QuestNotStarted: 
			Debug.Log("not started quest");
			Debug.Log(!PlayerManager.Instance.Hero.questLog.IsQuestStarted(Convert.ToInt32(ItemToHave)));
			return !PlayerManager.Instance.Hero.questLog.IsQuestStarted(Convert.ToInt32(ItemToHave));
			//quest startet not finished (some of the tasks are not completed)
		case ConditionTypeEnum.QuestInProgress: 
			return PlayerManager.Instance.Hero.questLog.IsQuestInProgress(Convert.ToInt32(ItemToHave));
			//quest finished = you can end it now
		case ConditionTypeEnum.QuestFinished: 
			return PlayerManager.Instance.Hero.questLog.IsQuestFinished(Convert.ToInt32(ItemToHave));
			//quest completed = in quest log "quest completed"
		case ConditionTypeEnum.QuestCompleted:
			return PlayerManager.Instance.Hero.questLog.IsQuestCompleted(Convert.ToInt32(ItemToHave));	
			//maximum level
		/*case ConditionTypeEnum.LevelMaximum:
			if (PlayerManager.Instance.Hero.CurrentLevel <= AmountToReach)
				return true;
			break;
			
			//minimum level
		case ConditionTypeEnum.LevelMinimum:
			if (PlayerManager.Instance.Hero.CurrentLevel >= AmountToReach)
				return true;
			else
				return false;*/
			
			//killed enemy in history
		/*case ConditionTypeEnum.KillTarget:	
			return PlayerManager.Instance.Hero.Log.IsTargetKilled(Convert.ToInt32(ItemToHave), AmountToReach);
			break;*/
			//attribute point
		/*case ConditionTypeEnum.AttributePoint:
			if (player.Hero.AttributePoint >= AmountToReach)
				return true;
			break;
			
			//skill point
		case ConditionTypeEnum.SkillPoint:
			if (player.Hero.SkillPoint >= AmountToReach)
				return true;
			break;
			
			//base attribute
		case ConditionTypeEnum.BaseAttribute:
			foreach(RPGAttribute atr in player.Hero.Attributes)
			{
				if (atr.ID == Convert.ToInt32(ItemToHave) && atr.Value >= AmountToReach)
					return true;
			}
			break;
			
			//total attribute
		case ConditionTypeEnum.TotalAttribute:
			foreach(RPGAttribute atr in player.Hero.Bonuses.TotalAttributes)
			{
				if (atr.ID == Convert.ToInt32(ItemToHave) && atr.Value >= AmountToReach)
					return true;
			}
			break;
			
			//base skill
		case ConditionTypeEnum.BaseSkill:
			foreach(RPGSkill skill in player.Hero.Skills)
			{
				if (skill.ID == Convert.ToInt32(ItemToHave) && skill.Value >= AmountToReach)
					return true;
			}
			break;*/
			
			//total skill
		/*case ConditionTypeEnum.TotalSkill:
			foreach(RPGSkill skill in player.Hero.Bonuses.TotalSkills)
			{
				if (skill.ID == Convert.ToInt32(ItemToHave) && skill.Value >= AmountToReach)
					return true;
			}
			break;*/
			
			//total number of quests completed
		/*case ConditionTypeEnum.CompletedQuestsCount:
			if (PlayerManager.Instance.Hero.Quest.CompletedQuests.Count >= AmountToReach)
				return true;
			break;*/
			
			//required race
		/*case ConditionTypeEnum.RaceRequired:
			if (player.Hero.RaceID == Convert.ToInt32(ItemToHave))
				return true;
			break;
			
			//race is not allowed
		case ConditionTypeEnum.RaceNotAllowed:
			if (player.Hero.RaceID != Convert.ToInt32(ItemToHave))
				return true;
			break;
			
			//required class
		case ConditionTypeEnum.ClassRequired:
			if (player.Hero.ClassID == Convert.ToInt32(ItemToHave))
				return true;
			break;*/
			
			//race is not allowed
		/*case ConditionTypeEnum.ClassNotAllowed:
			if (player.Hero.ClassID != Convert.ToInt32(ItemToHave))
				return true;
			break;
			
			//guild member
		case ConditionTypeEnum.IsGuildMember:
			if (player.Hero.Guild.IsMemberGuild(Convert.ToInt32(ItemToHave)))
				return true;
			break;
			
			//not guild member				
		case ConditionTypeEnum.IsNotGuildMember:
			if (!player.Hero.Guild.IsMemberGuild(Convert.ToInt32(ItemToHave)))
				return true;
			break;*/
			
			//alternate quest completed
		/*case ConditionTypeEnum.AlternatedQuestCompleted:
			if (player.Hero.Quest.IsAlternateQuestCompleted(Convert.ToInt32(ItemToHave), SecondaryID))
				return true;
			break;*/
			//quest step in progress
		case ConditionTypeEnum.QuestStepInProgress:
			if (PlayerManager.Instance.Hero.questLog.IsQuestStepInProgress(Convert.ToInt32(ItemToHave), SecondaryID))
				return true;
			break;
			
			//adding reputation
		/*case ConditionTypeEnum.ReputationValue:
			if (player.Hero.IsReputationValue(Convert.ToInt32(ItemToHave), AmountToReach))
				return true;
			break;*/
			
		}
		
		return false;
	}
	
	/*private void DisplayCondition(Rect rect, GUISkin skin, string text, Player player)
	{
		GUIStyle style = skin.label;
		Color previousColor = style.normal.textColor;
		if (Validate(player))
		{
			style.normal.textColor = Color.green;
		}
		else
		{
			style.normal.textColor = Color.red;
		}
		
		GUI.Label(rect, text, style);
		style.normal.textColor = previousColor;
	}*/
	

	
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
	SomeItemMustBeInInventory = 0,
	QuestNotStarted = 1,
	QuestInProgress = 2,
	QuestFinished = 3,
	QuestCompleted = 4,
	KillTarget = 5,
	TargetObject = 6,
	LevelMinimum = 7,
	LevelMaximum = 8,
	AttributePoint = 9,
	SkillPoint = 10,
	BaseAttribute = 11,
	BaseSkill = 12,
	TotalAttribute = 13,
	TotalSkill = 14,
	CompletedQuestsCount = 15,
	RaceRequired = 16,
	RaceNotAllowed = 17,
	ClassRequired = 18,
	ClassNotAllowed = 19,
	IsGuildMember = 20,
	IsNotGuildMember = 21,
	QuestStepInProgress = 22,
	AlternatedQuestCompleted = 23,
	QuestFailed = 24,
	ReputationValue = 25
}