using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class NPCMinigame: NPCActivity {
	
	public string PrefabDirectory;
	public List<LootItem> Loots;
	
	public NPCMinigame()
	{
		Name = string.Empty;
		SystemDescription = string.Empty;
		Description = string.Empty;
		PrefabDirectory = "MiniGames/";
		AtlasName = "Atlases/MiniGames/";
		Loots = new List<LootItem>();
		preffix = "MINIGAMES";
		activityType = NPCActivityType.Minigame;
	}
}
