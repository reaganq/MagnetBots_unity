using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralData: MonoBehaviour{

	public List<RPGQuest> quests;
	public List<ItemCategoryData> itemCategories;
	// Use this for initialization
	public void Awake()
	{
		quests = Storage.Load<RPGQuest>(new RPGQuest());
		Debug.Log(quests.Count + "quests");
	}

	public RPGQuest GetQuestByID(int ID)
	{
		for (int i = 0; i < quests.Count; i++) {
			if(quests[i].ID == ID)
				return quests[i];
		}
		return null;
	}

	public List<ItemCategoryData> GetSubcategories(string category)
	{
		for (int i = 0; i < itemCategories.Count; i++) {
			if(itemCategories[i].name == category)
				return itemCategories[i].subcategories;
		}
		return null;
	}
}

[System.Serializable]
public class ItemCategoryData
{
	public string name;
	public string iconName;
	public List<ItemCategoryData> subcategories;
}
