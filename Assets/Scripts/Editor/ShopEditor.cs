using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ShopEditor : BaseEditorWindow 
{
	public ShopEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Shop";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<Shop> list = Storage.Load<Shop>(new Shop());
		items = new List<IItem>();
		foreach(Shop category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new Shop();
	}
	
	public List<Shop> Shops
	{
		get
		{
			List<Shop> list= new List<Shop>();
			foreach(IItem category in items)
			{
				list.Add((Shop)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<Shop>(Shops, new Shop());
	}
	
	protected override void EditPart()
	{
		Shop s = (Shop)currentItem;

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.PrefixLabel("Respawn");
		
		//s.RespawnTimer = (ShopRespawnTimer)EditorGUILayout.EnumPopup(s.RespawnTimer,  GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		//s.CurrencyID = EditorUtils.IntPopup(s.CurrencyID, Data.itemEditor.items, "Currency");
		
		//s.BuyPriceModifier = EditorUtils.FloatField(s.BuyPriceModifier, "Buy modifier");
		
		//s.SellPriceModifier = EditorUtils.FloatField(s.SellPriceModifier, "Sell modifier");
		
		//s.SellSameAsBuy = EditorUtils.Toggle(s.SellSameAsBuy, "Accept all goods");
		
		/*if (s.SellSameAsBuy == false)
		{
			//shop categories to sell
			foreach(ShopCategory category in s.SellCategories)
			{
				DisplayShopCategory(category);
				
				if (GUILayout.Button("Delete", GUILayout.Width(200)))
				{
					s.SellCategories.Remove(category);
					break;
				}
				EditorGUILayout.EndHorizontal();
			}
			if (GUILayout.Button("Add category", GUILayout.Width(300)))
			{
				s.SellCategories.Add(new ShopCategory());
			}
		}*/
		
		EditorUtils.Separator();
		
		//shop categories
		/*foreach(ShopCategory category in s.Categories)
		{
			DisplayShopCategory(category);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Categories.Remove(category);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		if (GUILayout.Button("Add category", GUILayout.Width(300)))
		{
			s.Categories.Add(new ShopCategory());
		} 
		EditorUtils.Separator();*/
		//shop items
		foreach(ShopItem item in s.Items)
		{
            
			DisplayShopItem(item);
			
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				s.Items.Remove(item);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorUtils.Separator();
		if (GUILayout.Button("Add item", GUILayout.Width(200)))
		{
			s.Items.Add(new ShopItem());
		}
        
        EditorUtils.Separator();
        EditorGUILayout.PrefixLabel("Restock Time");
        
        s.RestockTime = EditorGUILayout.FloatField(s.RestockTime, GUILayout.Width(100));
			
		currentItem = s;
	}
	
	void DisplayShopItem(ShopItem item)
	{
		//ConditionsUtils.Conditions(item.Conditions, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.PrefixLabel("Item");
		
		//item.Preffix = (ItemTypeEnum)EditorGUILayout.EnumPopup(item.Preffix, GUILayout.Width(200));
		EditorGUILayout.PrefixLabel(" ID: ");
		item.ID = EditorGUILayout.IntField(item.ID, GUILayout.Width(50));
		EditorGUILayout.PrefixLabel(" amount: ");
		item.StackAmount = EditorGUILayout.IntField(item.StackAmount, GUILayout.Width(50));
		EditorGUILayout.PrefixLabel(" level: ");
		item.Level = EditorGUILayout.IntField(item.Level, GUILayout.Width(50));
		EditorGUILayout.PrefixLabel("Item Type");
		item.itemType = (ItemType)EditorGUILayout.EnumPopup(item.itemType , GUILayout.Width(100));
		
	}
	
	/*void DisplayShopCategory(ShopCategory category)
	{
		EditorUtils.Separator();
		
		DisplayBaseCategory(category, Data);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Stack amount");
		
		category.StackAmount = EditorGUILayout.IntField(category.StackAmount ,GUILayout.Width(100));
	}*/
	
	/*public void DisplayBaseCategory(BaseLootCategory category, MainWindowEditor Data)
	{
		ConditionsUtils.Conditions(category.Conditions, Data);
		

		category.Category.ID = EditorUtils.IntPopup(category.Category.ID, Data.itemCategory.items, "Category", 100, FieldTypeEnum.WholeLine);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Levels");
		category.LevelAdjustment = (LevelAdjustmentType)EditorGUILayout.EnumPopup(category.LevelAdjustment ,GUILayout.Width(200));
		
		EditorGUILayout.PrefixLabel("Min");
		category.MinValue = EditorGUILayout.IntField(category.MinValue ,GUILayout.Width(100));
		
		EditorGUILayout.PrefixLabel("Max");
		category.MaxValue = EditorGUILayout.IntField(category.MaxValue ,GUILayout.Width(100));
		
		EditorGUILayout.EndHorizontal();
	}*/
}
