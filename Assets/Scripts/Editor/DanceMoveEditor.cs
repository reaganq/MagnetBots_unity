using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DanceMoveEditor : BaseEditorWindow {
	
	// Use this for initialization
	public DanceMoveEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Dance";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<DanceMove> list = Storage.Load<DanceMove>(new DanceMove());
		items = new List<IItem>();
		foreach(DanceMove category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new DanceMove();
	}
	
	public List<DanceMove> DanceMoves
	{
		get
		{
			List<DanceMove> list= new List<DanceMove>();
			foreach(IItem category in items)
			{
				list.Add((DanceMove)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<DanceMove>(DanceMoves, new DanceMove());
	}
	
	protected override void EditPart()
	{
		DanceMove s = (DanceMove)currentItem;
		s.IconPath = EditorUtils.TextField(s.IconPath, "Icon name");
		
		s.AtlasName = EditorUtils.TextField(s.AtlasName, "Atlas name");
		//ItemUtils.DisplayItemPart(s, Data);
		
		currentItem = s;
	}
}