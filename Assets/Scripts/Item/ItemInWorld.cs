using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

public class ItemInWorld 
{
 	public string UniqueItemId;
 	public int CurrentAmount;
 	public int Level;

	[XmlIgnore]
	public RPGItem rpgItem;

	public bool IsItemEquippable
	{
		get
		{
			if (rpgItem.IsEquippable)
				return true;
			
			return false;
		}
	}
	
	public bool IsItemUsable
	{
		get
		{
			if(rpgItem.IsUsable)
				return true;
			return false;
		}
	}



	public virtual void LoadItem()
	{
		if (UniqueItemId.IndexOf("ITEM") != -1)
		{
			int id = Convert.ToInt32(UniqueItemId.Replace("ITEM", string.Empty));
			rpgItem = Storage.LoadById<RPGItem>(id, new RPGItem());
		}
		if (UniqueItemId.IndexOf("ARMOR") != -1)
		{
			int id = Convert.ToInt32(UniqueItemId.Replace("ARMOR", string.Empty));
			rpgItem = Storage.LoadById<RPGArmor>(id, new RPGArmor());
		}
	}
}
