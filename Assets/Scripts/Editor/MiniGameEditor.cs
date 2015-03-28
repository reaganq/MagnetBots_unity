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
		List<NPCMinigame> list = Storage.Load<NPCMinigame>(new NPCMinigame());
		items = new List<IItem>();
		foreach(NPCMinigame category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new NPCMinigame();
	}
	
	public List<NPCMinigame> MiniGame
	{
		get
		{
			List<NPCMinigame> list = new List<NPCMinigame>();
			foreach(IItem category in items)
			{
				list.Add((NPCMinigame)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<NPCMinigame>(MiniGame, new NPCMinigame());
	}
	
	protected override void EditPart()
	{
		NPCMinigame s = (NPCMinigame)currentItem;
		
		s.PrefabDirectory = EditorUtils.TextField(s.PrefabDirectory, "Prefab Location");
		s.badgeOne = EditorUtils.IntField(s.badgeOne, "Rank 1 badge");
		s.badgeTwo = EditorUtils.IntField(s.badgeTwo, "Rank 2 badge");
		s.badgeThree = EditorUtils.IntField(s.badgeThree, "Rank 3 badge");
		s.AtlasName = EditorUtils.TextField(s.AtlasName, "Atlas");
		s.IconPath = EditorUtils.TextField(s.IconPath, "portrait");
		ActivityUtils.DisplayActivityComponents(s, Data);
		EditorGUILayout.Separator();

		/*for (int i = 0; i < s.conversationIDs.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			s.conversationIDs[i] = EditorUtils.IntPopup(s.conversationIDs[i], Data.conversationEditor.items, "conversationID: " + i, 90, FieldTypeEnum.Middle);
			if(GUILayout.Button("Remove conversation id", GUILayout.Width(400)))
			{
				s.conversationIDs.Remove(s.conversationIDs[i]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		if(GUILayout.Button("Add Conversation ID", GUILayout.Width(400)))
		{
			s.conversationIDs.Add(1);
		}*/
		foreach(LootItem item in s.Loots)
		{
			EditorGUILayout.BeginVertical(skin.box);
			//DisplayShopItem(item);
			DisplayLootItem( item );
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Loots.Remove(item);
				break;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
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
