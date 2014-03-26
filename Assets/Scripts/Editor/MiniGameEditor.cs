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
}
