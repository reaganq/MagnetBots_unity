﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class QuestEditor : BaseEditorWindow 
{
	public QuestEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Quests";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGQuest> list = Storage.Load<RPGQuest>(new RPGQuest());
		items = new List<IItem>();
		foreach(RPGQuest category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGQuest();
	}
	
	public List<RPGQuest> Quests
	{
		get
		{
			List<RPGQuest> list = new List<RPGQuest>();
			foreach(IItem category in items)
			{
				list.Add((RPGQuest)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		List<RPGQuest> quests = Quests;
		Storage.Save<RPGQuest>(quests, new RPGQuest());
	}
	
	protected override void EditPart()
	{
		RPGQuest s = (RPGQuest)currentItem;

		
		s.repeatable = EditorUtils.Toggle(s.repeatable, "Repeatable");

		s.initialQuestLog = EditorUtils.TextField(s.initialQuestLog, "Initial quest log");
		
		s.finalQuestLog = EditorUtils.TextField(s.finalQuestLog, "Final quest log");

		s.questType = (QuestType)EditorGUILayout.EnumPopup(s.questType, GUILayout.Width(300));
		
		/*if (GUILayout.Button("Add reward", GUILayout.Width(400)))
		{
			Reward reward = new Reward();
			s.Rewards.Add(reward);
		}
		if (s.Rewards != null && s.Rewards.Count >0)
		{
			foreach(Reward reward in s.Rewards)
			{
				AddReward(reward);	
			}
		}*/
		EditorUtils.Separator();
		
		foreach(QuestStep questStep in s.questSteps)
		{
			EditorGUILayout.BeginVertical(skin.box);
			EditorGUILayout.LabelField("quest step:" + questStep.StepNumber.ToString());

			questStep.isMainStep = EditorUtils.Toggle(questStep.isMainStep, "is main step");

			questStep.QuestLogNote = EditorUtils.TextField(questStep.QuestLogNote, "Quest log note");
			
			AddQuestStep(questStep);
			EditorGUILayout.EndVertical();
			if (GUILayout.Button("Remove Quest Step", GUILayout.Width(200)))
			{
				s.questSteps.Remove(questStep);
                break;
            }
		}
		
		if (GUILayout.Button("Add quest step", GUILayout.Width(400)))
		{
			QuestStep questStep = new QuestStep();
			s.questSteps.Add(questStep);
			questStep.StepNumber = s.questSteps.Count;
		}
		EditorUtils.Separator();

		foreach(LootItem item in s.allLoots)
		{
			DisplayLootItem( item );
			if (GUILayout.Button("Delete Loot", GUILayout.Width(200)))
			{
				s.allLoots.Remove(item);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Separator();
		
		if (GUILayout.Button("Add Loot", GUILayout.Width(200)))
		{
			s.allLoots.Add(new LootItem());
		}
		
		currentItem = s;
	}
	
	void AddQuestStep(QuestStep questStep)
	{
		if (GUILayout.Button("Add task", GUILayout.Width(400))) 
		{
			questStep.Tasks.Add(new Task());
		}
		
		foreach(Task task in questStep.Tasks)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical(skin.box);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Task type");
			
			task.TaskType = (TaskTypeEnum)EditorGUILayout.EnumPopup(task.TaskType, GUILayout.Width(300));
			
			switch(task.TaskType)
			{
			case TaskTypeEnum.BringItem:
				task.PreffixTarget = (PreffixType)EditorGUILayout.EnumPopup(task.PreffixTarget, GUILayout.Width(200));
				
				switch(task.PreffixTarget)
				{
				case PreffixType.ITEM:
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.itemEditor.items, "Item", 100, FieldTypeEnum.Middle);
					break;
				case PreffixType.ARMOR:
					task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.armorEditor.items, "Armor", 100, FieldTypeEnum.Middle);
					break;
				}
				task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
				break;
				
			case TaskTypeEnum.KillEnemy:
				task.PreffixTarget = PreffixType.ENEMY;
				task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
				task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.enemyEditor.items, "Enemy", 100, FieldTypeEnum.Middle);
				
				break;
				
			/*case TaskTypeEnum.ReachPartOfConversation:
				task.PreffixTarget = PreffixType.PARAGRAPH;
				task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.conversationEditor.items, "Paragraph", 100, FieldTypeEnum.Middle);
				break;*/
				

			/*case TaskTypeEnum.VisitArea:
				task.PreffixTarget = PreffixType.WORLDOBJECT;
				task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.worldObjectEditor.items, "WO", 100, FieldTypeEnum.Middle);
				break;*/
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.PrefixLabel("Task quest log");
			task.QuestLogDescription = EditorGUILayout.TextField(task.QuestLogDescription, GUILayout.Width(500));
			EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Remove Task", GUILayout.Width(200)))
			{
				questStep.Tasks.Remove(task);
				break;
            }
            EditorGUILayout.EndVertical();
		}
		
		EditorUtils.Separator();
	}

	public void DisplayLootItem(LootItem item)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		item.itemType = (ItemType)EditorGUILayout.EnumPopup(item.itemType , GUILayout.Width(200));
		EditorGUILayout.PrefixLabel(" ID: ");
		for (int i = 0; i < item.itemID.Count; i++) 
		{
			item.itemID[i] = EditorGUILayout.IntField(item.itemID[i], GUILayout.Width(90));
		}
		if (GUILayout.Button("Add new item index", GUILayout.Width(200)))
		{
			item.itemID.Add(1);
		}
		
		EditorGUILayout.PrefixLabel(" itemlevel: ");
		item.itemLevel = EditorGUILayout.IntField(item.itemLevel, GUILayout.Width(90));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(" min amount: ");
		item.minQuantity = EditorGUILayout.IntField(item.minQuantity, GUILayout.Width(90));
		EditorGUILayout.PrefixLabel(" max amount: ");
		item.maxQuantity = EditorGUILayout.IntField(item.maxQuantity, GUILayout.Width(90));
		EditorGUILayout.PrefixLabel(" droprate: ");
		item.dropRate = EditorGUILayout.FloatField(item.dropRate, GUILayout.Width(90));
	}
}
