using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NPCEditor : BaseEditorWindow 
{
	public NPCEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "NPC";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGNPC> list = Storage.Load<RPGNPC>(new RPGNPC());
		items = new List<IItem>();
		foreach(RPGNPC category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGNPC();
	}
	
	public List<RPGNPC> NPC
	{
		get
		{
			List<RPGNPC> list = new List<RPGNPC>();
			foreach(IItem category in items)
			{
				list.Add((RPGNPC)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGNPC>(NPC, new RPGNPC());
	}
	
	protected override void EditPart()
	{
		RPGNPC s = (RPGNPC)currentItem;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("default conversation id");
		s.defaultConversationID = EditorGUILayout.IntField(s.defaultConversationID, GUILayout.Width(100));
		EditorGUILayout.PrefixLabel("speaker Prefab");
		s.speakerPrefabPath = EditorGUILayout.TextField(s.speakerPrefabPath, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();

		for (int h = 0; h < s.overrideActivities.Count; h++) {
			EditorGUILayout.BeginVertical(skin.box);
			AddActivityData(s.overrideActivities[h], h);
			if (GUILayout.Button("Delete Priority Activity", GUILayout.Width(400)))
			{
				s.overrideActivities.Remove(s.overrideActivities[h]);
				break;
			}
			EditorGUILayout.EndVertical();
		}
		if (GUILayout.Button("Add Priority Activity", GUILayout.Width(400)))
		{
			NPCActivityData p = new NPCActivityData();
			s.overrideActivities.Add(p);
		}
		EditorGUILayout.Separator();

		for (int i = 0; i < s.activities.Count; i++) {
			EditorGUILayout.BeginVertical(skin.box);
			AddActivityData(s.activities[i], i);
			if (GUILayout.Button("Delete Activity", GUILayout.Width(400)))
			{
				s.activities.Remove(s.activities[i]);
				break;
			}
			EditorGUILayout.EndVertical();
		}
		if (GUILayout.Button("Add Activity", GUILayout.Width(400)))
		{
			NPCActivityData p = new NPCActivityData();
			s.activities.Add(p);
		}
		currentItem = s;
	}

	public void AddActivityData(NPCActivityData s, int j)
	{
		EditorUtils.Label("activity No. "+j);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("activity id");
		s.activityID = EditorGUILayout.IntField(s.activityID, GUILayout.Width(100));
		EditorGUILayout.PrefixLabel("activity type");
		s.activityType = (NPCActivityType)EditorGUILayout.EnumPopup(s.activityType, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
	}
}
