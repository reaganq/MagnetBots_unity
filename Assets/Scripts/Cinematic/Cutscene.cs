using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cutscene : MonoBehaviour {

	public Animation cutsceneAnimation;
	public GameObject[] cutsceneObjects;

	public void Start()
	{
	}

	public void DestroyObject()
	{
		Destroy(this);
	}

	public void EnableGameobject(int index, bool state)
	{
		cutsceneObjects[index].SetActive(state);
	}

	public void ScreenFlash(Color targetColor)
	{
	}
}
