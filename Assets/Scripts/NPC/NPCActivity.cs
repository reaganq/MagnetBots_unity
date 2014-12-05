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
	public RPGConversation conversation;
	public string npcIconAtlas;
	public string npcIconSprite;
	public string npcButtonText;
}

public enum NPCActivityType
{
	Minigame = 0,
	Shop = 1,
	Quest = 2,
	Service = 3,
	Arena = 4,
	Default = 5
}