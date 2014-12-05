using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ArenaEditor : BaseEditorWindow 
{
	public ArenaEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Arena";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<NPCArena> list = Storage.Load<NPCArena>(new NPCArena());
		items = new List<IItem>();
		foreach(NPCArena category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new NPCArena();
	}
	
	public List<NPCArena> NPC
	{
		get
		{
			List<NPCArena> list = new List<NPCArena>();
			foreach(IItem category in items)
			{
				list.Add((NPCArena)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<NPCArena>(NPC, new NPCArena());
	}
	
	protected override void EditPart()
	{
		NPCArena s = (NPCArena)currentItem;

		EditorGUILayout.Separator();

		for (int i = 0; i < s.EnemyIDs.Count; i++) {

			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			s.EnemyIDs[i] = EditorUtils.IntPopup(s.EnemyIDs[i], Data.enemyEditor.items, "Enemy");
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.EnemyIDs.Remove(s.EnemyIDs[i]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Separator();
		
		if (GUILayout.Button("Add enemy", GUILayout.Width(200)))
		{
			s.EnemyIDs.Add(0);
		}

		currentItem = s;
	}

}
