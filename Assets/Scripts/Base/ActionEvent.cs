﻿using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class ActionEvent  
{
	
	[XmlAttribute (AttributeName = "PI")]
	public PreffixType PreffixItem;
	
	[XmlAttribute (AttributeName = "ITEM")]
	public int Item;

	[XmlAttribute (AttributeName = "ITEMLvl")]
	public int ItemLevel;
	
	[XmlAttribute (AttributeName = "AT")]
	public ActionEventType ActionType;
	
	[XmlAttribute (AttributeName = "AM")]
	public int Amount;
	
	[XmlAttribute (AttributeName = "TX")]
	public string Text;
	
	public ActionEvent()
	{
		Text = string.Empty;
	}
	
	//do action based on parameters
	public void DoAction()
	{
		switch(ActionType)
		{
			//quest start
		case ActionEventType.QuestStart:
			PlayerManager.Instance.Hero.Quest.StartQuest(Item);
			break;
			//quest end
		case ActionEventType.QuestEnd:
			PlayerManager.Instance.Hero.Quest.EndQuest(Item);
			break;
			//give item
		case ActionEventType.GiveItem:
			PreffixSolver.GiveItem(PreffixItem, Item, ItemLevel, Amount);
			break;
			//take item from inventory
		case ActionEventType.TakeItem:
			RPGItem rpgItem = new RPGItem();
			if (Amount == 0)
				Amount = 1;
			if (PreffixItem == PreffixType.ITEM)
			{
				rpgItem = Storage.LoadById<RPGItem>(Item, new RPGItem());
				PlayerManager.Instance.Hero.MainInventory.RemoveItem(rpgItem, ItemLevel, Amount);
			}
			else if (PreffixItem == PreffixType.ARMOR)
			{
				rpgItem = Storage.LoadById<RPGArmor>(Item, new RPGArmor());
				PlayerManager.Instance.Hero.ArmoryInventory.RemoveItem(rpgItem,ItemLevel, Amount);
			}
		

			break;
			//end conversation
		case ActionEventType.EndConversation:
			//BasicGUI.isConversationDisplayed = false;
			break;
			//remove worldobject
		}
	}
}

public enum ActionEventType
{
	QuestStart = 0,
	QuestEnd = 1,
	GiveItem = 2,
	TakeItem = 3,
	EndConversation = 4,
	QuestFailed = 5,
	RemoveWorldObject = 6,
	QuestAlternateEnd = 7,
	NoteDisplay = 8,
	SpawnCreature = 9,
	UseTeleport = 10,
	DisplayQuestDetails = 11,
	GiveQuestRewards = 12,
	Continueconversation = 13,
	OpenActivityGUI = 14,
}