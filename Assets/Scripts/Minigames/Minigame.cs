using UnityEngine;
using System.Collections;

public class Minigame : MonoBehaviour {

	public GameObject minigameUI;
	public int npcMinigameID;
	public NPCMinigame npcMinigame;
	public int score;

	public void Awake()
	{
		npcMinigame = Storage.LoadById<NPCMinigame>( npcMinigameID, new NPCMinigame());
	}

	public virtual void EndGame()
	{
		PlayerManager.Instance.GiveRewards(score, npcMinigame.Loots);
	}

}
