using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BurgerMemory : MonoBehaviour {

	public List<GameObject> burgerParts;
	public List<int> mainSequence;
	public List<int> playerSequence;
	public bool isActive;
	public int lives;

	public float maxTime;
	public float curTime;

	// Use this for initialization
	void Start () {
		isActive = false;
		StartCoroutine(GenerateStartingSequence(4));
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
			yield return new WaitForSeconds(1);
		}
		yield return null;
		isActive = true;
	}
	
	void OnFingerUp( FingerUpEvent e )
	{
		Debug.Log("fingerup");
		if(isActive)
		{
			int index = burgerParts.IndexOf(e.Selection);
			Debug.Log(index);
			Debug.Log(e.Selection.name);
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

	void CorrectSelection(int index)
	{
		playerSequence.Add(index);
		curTime = maxTime;
		if(playerSequence.Count == mainSequence.Count)
		{
			playerSequence.Clear();
			mainSequence.Add (Random.Range(0, burgerParts.Count));
		}
	}

	void IncorrectSelection()
	{
		lives --;
		if(lives <= 0)
		{
			StartCoroutine(EndGame());
			return;
		}
		curTime = maxTime;
	}

	public IEnumerator EndGame()
	{
		yield return null;
	}


}
