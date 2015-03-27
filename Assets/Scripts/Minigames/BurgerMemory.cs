using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurgerMemory : Minigame {

	public List<GameObject> burgerParts;
	public List<int> mainSequence;
	public List<int> playerSequence;
	public bool isPlaying;
	public int lives = 1;
	public int startingDifficulty = 4;
	public float maxTime;
	public float curTime;
	public UILabel scoreText;
	public UILabel levelText;
	public UILabel instructionsLabel;
	public UISprite progressBar;

	public int level;

	// Use this for initialization
	void Start () {
		isPlaying = false;
		instructionsLabel.text = "";
		level = 1;
		scoreText.text = "Score: " + score.ToString();
		levelText.text = "Level: " + level.ToString();
		StartCoroutine(StartGame());
	}

	void Update()
	{
		if(minigameState == MinigameState.active)
		{
			if(isPlaying)
			{
				curTime -= Time.deltaTime;
				if(curTime <= 0)
					IncorrectSelection();
				progressBar.fillAmount = Mathf.Clamp(curTime/maxTime, 0.0f, 1.0f);
			}
			else
				progressBar.fillAmount = 1;
		}
	}

	public IEnumerator StartGame()
	{
		yield return new WaitForSeconds(1);
		GUIManager.Instance.rewardsGUI.StartCountdown();
		yield return new WaitForSeconds(4.5f);
		minigameState = MinigameState.active;
		StartCoroutine(GenerateStartingSequence(startingDifficulty));
	}

	public IEnumerator GenerateStartingSequence(int index)
	{
		isPlaying = false;
		for (int i = 0; i <index; i++)
		{
			mainSequence.Add(Random.Range(0, burgerParts.Count));
		}
		//yield return null;

		yield return StartCoroutine(ShowSequence());
	}

	public IEnumerator ShowSequence()
	{
		instructionsLabel.text = "Watch!";
		yield return new WaitForSeconds(1);
		for (int i = 0; i < mainSequence.Count; i++) {
			StartCoroutine(Enlarge(burgerParts[mainSequence[i]]));
			yield return new WaitForSeconds(0.5f);
		}
		if(minigameState == MinigameState.active)
		{
			isPlaying = true;
			instructionsLabel.text = "Play!";
			curTime = maxTime;
		}
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		if(minigameState != MinigameState.active && isPlaying)
			return;

		if(e.Selection != null)
		{
			Debug.Log("fingerup");
			if(isPlaying)
			{
				int index = burgerParts.IndexOf(e.Selection);
				Debug.Log(index);
				Debug.Log(e.Selection.name);
				StartCoroutine(Enlarge(e.Selection));
				if(index == mainSequence[playerSequence.Count])
				{
					Debug.Log("correct");
					CorrectSelection(index);
				}
				else
				{
					IncorrectSelection();
					Debug.Log("incorrect");
				}
			}
		}
	}

	void CorrectSelection(int index)
	{
		playerSequence.Add(index);
		curTime = maxTime;
		if(playerSequence.Count == mainSequence.Count)
		{
			//levle up essentially
			isPlaying = false;
			playerSequence.Clear();
			mainSequence.Add (Random.Range(0, burgerParts.Count));
			level ++;
			levelText.text = "Level: " + level.ToString();
			score += 100;
			scoreText.text = "Score: " + score.ToString();
			maxTime = maxTime * 0.9f;
			//score += 100;
			//scoreText.text = "Score: " + score.ToString();
			StartCoroutine(ShowSequence());
		}
	}

	void IncorrectSelection()
	{
		isPlaying = false;
		EndGame();
	}

	public IEnumerator Enlarge(GameObject go)
	{
		TweenScale.Begin(go, 0.1f, new Vector3(1.5f, 1.5f, 1.5f));
		yield return new WaitForSeconds(0.1f);
		TweenScale.Begin(go, 0.1f, new Vector3(1,1,1));
	}

	public override void EndGame ()
	{
		if(minigameState != MinigameState.active)
			return;
		Debug.Log("1");
		base.EndGame ();
		isPlaying = false;
		instructionsLabel.text = "";
		Debug.Log("0");
	}
}


