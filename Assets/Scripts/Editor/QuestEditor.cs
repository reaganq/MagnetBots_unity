using UnityEngine;
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

		
		s.Repeatable = EditorUtils.Toggle(s.Repeatable, "Repeatable");
		
		s.FinalQuestLog = EditorUtils.TextField(s.FinalQuestLog, "Final quest log");
		
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
		
		foreach(QuestStep questStep in s.QuestSteps)
		{
			questStep.QuestLogNote = EditorUtils.TextField(questStep.QuestLogNote, "Quest log note");
			
			AddQuestStep(questStep);
		}
		
		if (GUILayout.Button("Add quest step", GUILayout.Width(400)))
		{
			QuestStep questStep = new QuestStep();
			s.QuestSteps.Add(questStep);
			questStep.StepNumber = s.QuestSteps.Count;
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
		}
		
		EditorUtils.Separator();
	}
}
