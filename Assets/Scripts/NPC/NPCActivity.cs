using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class NPCActivity: BasicItem {

	public NPCActivityType activityType;
	public int conversationID;
	public string npcIconAtlas;
	public string npcIconSprite;
	public string npcButtonText;
	public List<Condition> Conditions;

	[XmlIgnore]
	public RPGConversation conversation;

	public bool Validate()
	{
		foreach (Condition condition in Conditions)
		{
			if (condition.Validate() == false)
				return false;
		}
		return true;
	}

	public void LoadConversation()
	{
		if(conversationID > 0)
			conversation = Storage.LoadById<RPGConversation>(conversationID, new RPGConversation());
	}
}

public enum NPCActivityType
{
	Minigame = 0,
	Shop = 1,
	Quest = 2,
	Service = 3,
	Arena = 4,
	Default = 5,
	Teleporter = 6,
	Construction = 7
}