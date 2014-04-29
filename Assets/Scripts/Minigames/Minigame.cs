using UnityEngine;
using System.Collections;

public class Minigame : MonoBehaviour {

	public GameObject minigameUI;
	public int rpgMinigameID;
	public RPGMinigame rpgMinigame;
	public int score;

	public void Awake()
	{
		rpgMinigame = Storage.LoadById<RPGMinigame>( rpgMinigameID, new RPGMinigame());
	}

	public virtual void EndGame()
	{
		PlayerManager.Instance.GiveRewards(score, rpgMinigame.Loots);
	}

}
