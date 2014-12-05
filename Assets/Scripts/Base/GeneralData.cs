using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralData{

	public List<RPGQuest> quests;
	// Use this for initialization
	public GeneralData()
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
}
