using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCard : MonoBehaviour {

	public ArenaGUIController controller;
	public int cardIndex;
	public UILabel name;
	public UILabel description;
	public UISprite portrait;

	public void LoadEnemy(string n, string atlas, string sprite, string d, int index)
	{
		Debug.Log("load enemy card");
		name.text = n;
		description.text = d;
		GameObject Atlas = Resources.Load(atlas) as GameObject;
		portrait.atlas = Atlas.GetComponent<UIAtlas>();
		portrait.spriteName = sprite;
		cardIndex = index;
	}

	public void OnChallengeButtonPressed()
	{
		controller.DisplayDetailsBox(cardIndex);
	}

}


