using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurgerMemory : Minigame {

	public List<GameObject> burgerParts;
	public List<int> mainSequence;
	public List<int> playerSequence;
	public bool isActive;
	public int lives;

	public float maxTime;
	public float curTime;
	public UILabel scoreText;
	public UILabel livesText;

	public int level;

	// Use this for initialization
	void Start () {
		isActive = false;
		StartCoroutine(GenerateStartingSequence(4));
		level = 1;
		scoreText.text = "Score: " + score.ToString();
		livesText.text = "Lives: " + lives.ToString();
	}

	void Update()
	{
		if(isActive)
		{
			curTime -= Time.deltaTime;
			if(curTime <= 0)
				IncorrectSelection();
		}
	}

	public IEnumerator GenerateStartingSequence(int index)
	{
		for (int i = 0; i <index; i++)
		{
			mainSequence.Add(Random.Range(0, burgerParts.Count));
		}
		//yield return null;

		yield return StartCoroutine(ShowSequence());
	}

	public IEnumerator ShowSequence()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < mainSequence.Count; i++) {
			StartCoroutine(Enlarge(burgerParts[mainSequence[i]]));
			yield return new WaitForSeconds(1);
		}
		isActive = true;
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		if(e.Selection != null)
		{
			Debug.Log("fingerup");
			if(isActive)
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
			isActive = false;
			playerSequence.Clear();
			mainSequence.Add (Random.Range(0, burgerParts.Count));
			level ++;
			score += 100;
			scoreText.text = "Score: " + score.ToString();
			//score += 100;
			//scoreText.text = "Score: " + score.ToString();
			StartCoroutine(ShowSequence());
		}
	}

	void IncorrectSelection()
	{
		lives --;
		livesText.text = "Lives: " + lives.ToString();
		playerSequence.Clear();
		if(lives <= 0)
		{
			isActive = false;
			EndGame();
			return;
		}
		curTime = maxTime;
	}

	public IEnumerator Enlarge(GameObject go)
	{
		TweenScale.Begin(go, 0.1f, new Vector3(1.5f, 1.5f, 1.5f));
		yield return new WaitForSeconds(0.1f);
		TweenScale.Begin(go, 0.1f, new Vector3(1,1,1));
	}
}
