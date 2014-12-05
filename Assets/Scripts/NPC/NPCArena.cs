using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//[XmlInclude(typeof(ShopCategory))]
[XmlInclude(typeof(RPGEnemy))]
public class NPCArena : NPCActivity
{
	public List<int> EnemyIDs;

	[XmlIgnore]
	public List<RPGEnemy> Enemies;

	public NPCArena()
	{
		EnemyIDs = new List<int>();
		Enemies = new List<RPGEnemy>();
		Name = string.Empty;
		Description = string.Empty;
		preffix = "ARENA";
		activityType = NPCActivityType.Arena;
	}
}