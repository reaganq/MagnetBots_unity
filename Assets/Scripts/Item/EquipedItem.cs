using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class EquipedItem
{
	public string UniqueItemId;
	public int Level;
	[XmlIgnore]
	public RPGArmor rpgArmor;

	public void LoadItem()
	{
		if (UniqueItemId.IndexOf("ARMOR") != -1)
		{
			//int id = Convert.ToInt32(UniqueItemId.Replace("ARMOR", string.Empty));
			rpgArmor = Storage.LoadbyUniqueId<RPGArmor>(UniqueItemId, new RPGArmor());
		}
	}
}
