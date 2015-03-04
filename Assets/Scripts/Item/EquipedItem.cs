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
	public EquipmentSlots equipSlot;
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

	public EquipedItem(string id, int lvl, EquipmentSlots slot)
	{
		UniqueItemId = id;
		Level = lvl;
		equipSlot = slot;
		LoadItem();
	}

	public EquipedItem(string id, int lvl, RPGArmor armor)
	{
		UniqueItemId = id;
		Level = lvl;
		equipSlot = armor.EquipmentSlotIndex;
		rpgArmor = armor;
	}

	public EquipedItem(ParseEquippedItem parseItem)
	{
		UniqueItemId = parseItem.uniqueItemId;
		Level = parseItem.level;
		equipSlot = (EquipmentSlots)parseItem.slotIndex;
		LoadItem();
	}
}

[Serializable]
public class ParseEquippedItem
{
	public string uniqueItemId;
	public int level;
	public int slotIndex;

	public ParseEquippedItem(EquipedItem item)
	{
		Debug.Log(item.UniqueItemId);
		uniqueItemId = item.UniqueItemId;
		level = item.Level;
		slotIndex = (int)item.equipSlot;
	}
}
