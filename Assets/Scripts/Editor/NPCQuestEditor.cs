using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NPCQuestEditor : BaseEditorWindow 
{
	public NPCQuestEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "NPCQuest";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<NPCQuest> list = Storage.Load<NPCQuest>(new NPCQuest());
		items = new List<IItem>();
		foreach(NPCQuest category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new NPCQuest();
	}
	
	public List<NPCQuest> Activity
	{
		get
		{
			List<NPCQuest> list = new List<NPCQuest>();
			foreach(IItem category in items)
			{
				list.Add((NPCQuest)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<NPCQuest>(Activity, new NPCQuest());
	}
	
	protected override void EditPart()
	{
		NPCQuest s = (NPCQuest)currentItem;
		ActivityUtils.DisplayActivityComponents(s, Data);
		EditorGUILayout.BeginHorizontal();
		//EditorUtils.Label("Quest ID: ");
		s.questID = EditorUtils.IntPopup(s.questID, Data.questEditor.items, "Quest ID: ", 200, FieldTypeEnum.Middle);
		EditorGUILayout.EndHorizontal();
        currentItem = s;
    }
}
