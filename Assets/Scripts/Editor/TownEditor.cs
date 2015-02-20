using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TownEditor : BaseEditorWindow {

	public TownEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Town";
		
		Init(guiSkin, data);
		
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGTown> list = Storage.Load<RPGTown>(new RPGTown());
		items = new List<IItem>();
		foreach(RPGTown category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGTown();
	}
	
	public List<RPGTown> Towns
	{
		get
		{
			List<RPGTown> list= new List<RPGTown>();
			foreach(IItem category in items)
			{
				list.Add((RPGTown)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGTown>(Towns, new RPGTown());
	}
	
	protected override void EditPart()
	{
		RPGTown s = (RPGTown)currentItem;

	}
}
