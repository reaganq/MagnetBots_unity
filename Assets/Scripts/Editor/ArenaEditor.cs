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
		List<RPGArena> list = Storage.Load<RPGArena>(new RPGArena());
		items = new List<IItem>();
		foreach(RPGArena category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGArena();
	}
	
	public List<RPGArena> NPC
	{
		get
		{
			List<RPGArena> list = new List<RPGArena>();
			foreach(IItem category in items)
			{
				list.Add((RPGArena)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGArena>(NPC, new RPGArena());
	}
	
	protected override void EditPart()
	{
		RPGArena s = (RPGArena)currentItem;

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
