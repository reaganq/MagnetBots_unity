using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ConstructionEditor : BaseEditorWindow {
	
	// Use this for initialization
	public ConstructionEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Constructions";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGConstruction> list = Storage.Load<RPGConstruction>(new RPGConstruction());
		items = new List<IItem>();
		foreach(RPGConstruction category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGConstruction();
	}
	
	public List<RPGConstruction> Constructions
	{
		get
		{
			List<RPGConstruction> list= new List<RPGConstruction>();
			foreach(IItem category in items)
			{
				list.Add((RPGConstruction)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGConstruction>(Constructions, new RPGConstruction());
	}
	
	protected override void EditPart()
	{
		RPGConstruction s = (RPGConstruction)currentItem;

		if (GUILayout.Button("Add task", GUILayout.Width(400))) 
		{
			s.requiredItems.Add(new Task());
		}
		
		foreach(Task task in s.requiredItems)
		{
			DisplayTask(task);
			if (GUILayout.Button("Remove Task", GUILayout.Width(200)))
			{
				s.requiredItems.Remove(task);
				break;
			}
		}

		currentItem = s;
	}

	public void DisplayTask(Task task)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginVertical(skin.box);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Task type");
		
		task.TaskType = (TaskTypeEnum)EditorGUILayout.EnumPopup(task.TaskType, GUILayout.Width(100));
		
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
			EditorGUILayout.PrefixLabel("amount: ");
			task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
			EditorGUILayout.PrefixLabel("level: ");
			task.Tasklevel = EditorGUILayout.IntField(task.Tasklevel, GUILayout.Width(100));
			break;
			
		case TaskTypeEnum.KillEnemy:
			task.PreffixTarget = PreffixType.ENEMY;
			task.AmountToReach = EditorGUILayout.IntField(task.AmountToReach, GUILayout.Width(100));
			task.TaskTarget = EditorUtils.IntPopup(task.TaskTarget, Data.enemyEditor.items, "Enemy", 100, FieldTypeEnum.Middle);
			
			break;
		}
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Task quest log");
		task.QuestLogDescription = EditorGUILayout.TextField(task.QuestLogDescription, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.EndVertical();
	}
}
