using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EnemyEditor : BaseEditorWindow 
{
	public EnemyEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Enemy";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGEnemy> list = Storage.Load<RPGEnemy>(new RPGEnemy());
		items = new List<IItem>();
		foreach(RPGEnemy category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGEnemy();
	}
	
	public List<RPGEnemy> NPC
	{
		get
		{
			List<RPGEnemy> list = new List<RPGEnemy>();
			foreach(IItem category in items)
			{
				list.Add((RPGEnemy)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGEnemy>(NPC, new RPGEnemy());
	}
	
	protected override void EditPart()
	{
		RPGEnemy s = (RPGEnemy)currentItem;

		//s.ShopID = EditorUtils.IntPopup(s.ShopID, Data.arenaEditor.items, "Arena");
		
		EditorGUILayout.Separator();

		s.PortraitIcon = EditorUtils.TextField(s.PortraitIcon, "Portrait Icon name");
		s.PortraitAtlas = EditorUtils.TextField(s.PortraitAtlas, "Portrait Atlas name");
		EditorGUILayout.Separator();
		for (int i = 0; i < s.PrefabPaths.Count; i++) 
		{
			
			//DisplayShopItem(item);
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Separator();
			s.PrefabPaths[i] = EditorUtils.TextField(s.PrefabPaths[i], "enemy prefab path");
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.PrefabPaths.Remove(s.PrefabPaths[i]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		if (GUILayout.Button("Add Enemy Prefab", GUILayout.Width(200)))
		{
			s.PrefabPaths.Add("Enemies/");
		}
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

		//if (GUILayout.Button("No shop", GUILayout.Width(150)))
		//{
		//s.ShopID = 0;
		//}
		EditorGUILayout.Separator();

		if (GUILayout.Button("Add Loot", GUILayout.Width(200)))
		{
			s.Loots.Add(new LootItem());
		}


		
		//if (GUILayout.Button("No shop", GUILayout.Width(150)))
		//{
		//s.ShopID = 0;
		//}
		EditorGUILayout.Separator();
		

		
		//s.LevelName = EditorUtils.TextField(s.LevelName, "level name");
		
		
		/*ConditionsUtils.Conditions(s.ShopConditions, Data);
		
		s.SpellShopID = EditorUtils.IntPopup(s.SpellShopID, Data.spellShop.items, "Spell shop");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("No spell shop", GUILayout.Width(150)))
		{
			s.SpellShopID = 0;
		}
		EditorGUILayout.EndHorizontal();
		
		ConditionsUtils.Conditions(s.SpellShopConditions, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Repair");
		s.Repairing = EditorGUILayout.Toggle(s.Repairing, GUILayout.Width(100));
		if (s.Repairing)
		{
			EditorGUILayout.PrefixLabel("Price modifier");
			s.RepairPriceModifier = EditorGUILayout.FloatField(s.RepairPriceModifier ,GUILayout.Width(200));
			
			EditorGUILayout.PrefixLabel("Currency");
			s.RepairCurrencyID = EditorGUILayout.IntField(s.RepairCurrencyID ,GUILayout.Width(200));
		}
		EditorGUILayout.EndHorizontal();
		
		ConditionsUtils.Conditions(s.ReparingConditions, Data);
		
		EditorUtils.Label("Guild");
		
		s.IsGuildMember = EditorUtils.Toggle(s.IsGuildMember, "Guild");
		
		if (s.IsGuildMember)
		{
			s.GuildID = EditorUtils.IntPopup(s.GuildID, Data.guildEditor.items, "Guild ID");
			
			s.IsRecruit = EditorUtils.Toggle(s.IsRecruit, "Can recruit");
			
			s.AdvanceRankLevel = EditorUtils.IntField(s.AdvanceRankLevel, "Advance to rank");
		}
		
		EditorUtils.Label("General conversation");
		
		EditorGUILayout.BeginHorizontal();
		
		if (GUILayout.Button("Add new conversation ID", GUILayout.Width(300)))
		{
			s.GeneralConversationID.Add(0);
		}
		
		EditorGUILayout.EndHorizontal();
		
		for (int index = 0; index <= s.GeneralConversationID.Count -1; index++)
		{
			
			s.GeneralConversationID[index] = EditorUtils.IntPopup(s.GeneralConversationID[index], Data.conversationEditor.items, "Conversation ID", 200, FieldTypeEnum.BeginningOnly);
			
			if (GUILayout.Button("Delete", GUILayout.Width(150)))
			{
				 s.GeneralConversationID.Remove(s.GeneralConversationID[index]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}*/
		
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
