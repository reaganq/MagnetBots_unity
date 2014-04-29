using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MiniGameEditor : BaseEditorWindow 
{
	public MiniGameEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "MiniGame";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGMinigame> list = Storage.Load<RPGMinigame>(new RPGMinigame());
		items = new List<IItem>();
		foreach(RPGMinigame category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGMinigame();
	}
	
	public List<RPGMinigame> MiniGame
	{
		get
		{
			List<RPGMinigame> list = new List<RPGMinigame>();
			foreach(IItem category in items)
			{
				list.Add((RPGMinigame)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGMinigame>(MiniGame, new RPGMinigame());
	}
	
	protected override void EditPart()
	{
		RPGMinigame s = (RPGMinigame)currentItem;
		
		s.PrefabDirectory = EditorUtils.TextField(s.PrefabDirectory, "Prefab Location");
		s.AtlasName = EditorUtils.TextField(s.AtlasName, "Atlas");
		s.PortraitIcon = EditorUtils.TextField(s.PortraitIcon, "portrait");
		EditorGUILayout.Separator();
		
		
		foreach(LootItem item in s.Loots)
		{
			
			//DisplayShopItem(item);
			DisplayLootItem( item );
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Loots.Remove(item);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Separator();
		
		if (GUILayout.Button("Add Loot", GUILayout.Width(200)))
		{
			s.Loots.Add(new LootItem());
		}

		EditorGUILayout.Separator();
		currentItem = s;
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
		if (GUILayout.Button("Add new item index", GUILayout.Width(100)))
		{
			item.itemID.Add(1);
		}
		if (GUILayout.Button("remove item index", GUILayout.Width(100)))
		{
			item.itemID.RemoveAt(item.itemID.Count -1);
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
