using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BadgeEditor : BaseEditorWindow {
	
	// Use this for initialization
	public BadgeEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Badges";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGBadge> list = Storage.Load<RPGBadge>(new RPGBadge());
		items = new List<IItem>();
		foreach(RPGBadge category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGBadge();
	}
	
	public List<RPGBadge> Badges
	{
		get
		{
			List<RPGBadge> list= new List<RPGBadge>();
			foreach(IItem category in items)
			{
				list.Add((RPGBadge)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGBadge>(Badges, new RPGBadge());
	}
	
	protected override void EditPart()
	{
		RPGBadge s = (RPGBadge)currentItem;
		s.IconPath = EditorUtils.TextField(s.IconPath, "Icon name");
		s.AtlasName = EditorUtils.TextField(s.AtlasName, "Atlas name");
		//EditorGUILayout.PrefixLabel("Premium");
		//s.isPremium = EditorGUILayout.Toggle(s.isPremium ,GUILayout.Width(300));
		//ItemUtils.DisplayItemPart(s, Data);
		
		currentItem = s;
	}
}
