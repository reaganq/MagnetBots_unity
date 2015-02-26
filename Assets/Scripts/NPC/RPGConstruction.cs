using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System;

[XmlInclude(typeof(Task))]
public class RPGConstruction : NPCActivity {

	public List<Task> requiredItems;

	[XmlIgnore]
	public List<InventoryItem> constructionItems;
	
	public RPGConstruction()
	{
		preffix = "CONSTRUCTION";
		requiredItems = new List<Task>();
		constructionItems = new List<InventoryItem>();
	}

	public void LoadRequiredItems()
	{
		for (int i = 0; i < requiredItems.Count; i++) {
			if(requiredItems[i].PreffixTarget == PreffixType.ITEM)
			{
				InventoryItem newInventoryItem = new InventoryItem();
				newInventoryItem.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(requiredItems[i].TaskTarget, new RPGItem()), requiredItems[i].Tasklevel, requiredItems[i].AmountToReach);
			}
		}
	}
}
