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

	public void LoadArena()
	{
		Enemies.Clear();
		for (int i = 0; i < EnemyIDs.Count; i++) {
			RPGEnemy newEnemy = Storage.LoadById<RPGEnemy>(EnemyIDs[i], new RPGEnemy());
			Enemies.Add(newEnemy);
		}
	}
}