using UnityEngine;
using System.Collections;

public class Minigame : MonoBehaviour {

	public GameObject minigameUI;
	public int npcMinigameID;
	//public NPCMinigame npcMinigame;
	public int score;
	public MinigameState minigameState;

	public void Awake()
	{
		minigameState = MinigameState.initialised;
		//npcMinigame = Storage.LoadById<NPCMinigame>( npcMinigameID, new NPCMinigame());
	}

	public virtual void EndGame()
	{
		//PlayerManager.Instance.GiveRewards(score, npcMinigame.Loots);
		minigameState = MinigameState.ended;
		StartCoroutine(EndGameSequence());
	}

	public virtual IEnumerator EndGameSequence()
	{
		yield return new WaitForSeconds(0.2f);
		Debug.Log(score);
		GUIManager.Instance.DisplayMinigameRewards(PlayerManager.Instance.GiveRewards(PlayerManager.Instance.ActiveMinigame.Loots, 0), score);
	}

}

public enum MinigameState{
	initialised,
	active,
	ended,
}
