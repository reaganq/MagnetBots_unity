using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CurrencyEditor : BaseEditorWindow {

	// Use this for initialization
	public CurrencyEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Currency";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGCurrency> list = Storage.Load<RPGCurrency>(new RPGCurrency());
		items = new List<IItem>();
		foreach(RPGCurrency category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGCurrency();
	}
	
	public List<RPGCurrency> Currencies
	{
		get
		{
			List<RPGCurrency> list= new List<RPGCurrency>();
			foreach(IItem category in items)
			{
				list.Add((RPGCurrency)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGCurrency>(Currencies, new RPGCurrency());
	}
	
	protected override void EditPart()
	{
		RPGCurrency s = (RPGCurrency)currentItem;
		s.IconPath = EditorUtils.TextField(s.IconPath, "Icon name");
		
		s.AtlasName = EditorUtils.TextField(s.AtlasName, "Atlas name");
		EditorGUILayout.PrefixLabel("Premium");
		s.isPremium = EditorGUILayout.Toggle(s.isPremium ,GUILayout.Width(300));
		//ItemUtils.DisplayItemPart(s, Data);
		
		currentItem = s;
	}
}
