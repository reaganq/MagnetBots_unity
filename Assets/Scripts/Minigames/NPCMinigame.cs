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
	public int badgeOne;
	public int badgeTwo;
	public int badgeThree;
	public List<LootItem> Loots;
	[XmlIgnore]
	public List<ScoreObject> highScores;
	[XmlIgnore]
	public string parseObjectID;
	[XmlIgnore]
	public int myScore;
	
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
		highScores = new List<ScoreObject>();
		myScore = 0;
	}

	public void AddHighScore(string playerName, int score)
	{
		if(highScores.Count == 0)
		{
			ScoreObject newScore = new ScoreObject(playerName, score);
			highScores.Add(newScore);
		}
		else
		{
			Debug.Log("has more than 0 highscores");
			for (int i = 0; i < highScores.Count; i++) {
				if(score > highScores[i].score)
				{
					ScoreObject newScore = new ScoreObject(playerName, score);
					highScores.Insert(i, newScore);
					Debug.Log("added new high score at rank: " + i);
					break;
				}
			}
		}

		for (int i = highScores.Count - 1; i >= 0; i--) {
			if(i > 2)
			{
				highScores.RemoveAt(i);
			}
				}
	}

	public int HighScoreRank(int score)
	{
		if(score == 0)
			return 0;
		if(highScores.Count == 0)
		{
			return 1;
		}
		for (int i = 0; i < highScores.Count; i++) {
			if(score > highScores[i].score)
				return i+1;
				}
		return 0;
	}

	public bool isPersonalHighscore(int score)
	{
		if(score > myScore)
		{
			Debug.Log("new personal high score!");
			return true;
		}
		else
			return false;
	}
}

[Serializable]
public class ScoreObject
{
	public string playerName;
	public int score;

	public ScoreObject()
	{
	}

	public ScoreObject(string name, int point)
	{
		playerName = name;
		score = point;
	}
}

[Serializable]
public class MinigameHighScores
{
	public int minigameID;
	public List<ScoreObject> highScores;

	public MinigameHighScores()
	{
		highScores = new List<ScoreObject>();
	}
}
