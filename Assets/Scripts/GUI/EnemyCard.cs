using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCard : MonoBehaviour {

	public ArenaGUIController controller;
	public TeleporterGUIController teleporter;
	public int cardIndex;
	public UISprite portrait;
	public Collider collider;
	public GameObject lockIcon;
	public TweenScale scaleDown;
	public TweenScale scaleUp;
	public bool isLocked;

	public void LoadEnemy(string atlas, string sprite, int index, bool isAvailable, bool isLocked)
	{
		GameObject Atlas = Resources.Load(atlas) as GameObject;
		portrait.atlas = Atlas.GetComponent<UIAtlas>();
		portrait.spriteName = sprite;
		cardIndex = index;
		if(isAvailable)
		{
			if(isLocked)
			{
				collider.enabled = false;
				lockIcon.SetActive(true);
			}
			else
			{
				collider.enabled = true;
				lockIcon.SetActive(false);
			}
		}
		else
		{
			collider.enabled = false;
			lockIcon.SetActive(true);
		}
	}

	public void OnChallengeButtonPressed()
	{
		if(controller != null)
			controller.DisplayDetailsBox(cardIndex);
		if(teleporter != null)
			teleporter.DisplayDetailsBox(cardIndex);
	}

	public void ScaleDown()
	{
			//if(scaleDown != null)
			//	scaleDown.Play(true);
	}

	public void ScaleUP()
	{
		//	if(scaleUp != null)
		//		scaleUp.Play(true);
		
	}

}


