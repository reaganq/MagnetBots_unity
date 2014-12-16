using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoverPopupGUIController : BasicGUIController {

	//player buttons
	/*
	 * Profile
	 * Talk
	 * Dance
	 * Items
	*/

	//other player buttons
	/*
	 * Profile
	 * add friend (nf)
	 * invite to party
	 * shop
	*/

	public float offsetAngle;
	public float offsetDistance;
	public Transform popupParent;
	public Transform unusedButtonsParent;

	public GameObject talkButton;
	public GameObject emoteButton;
	public GameObject armoryButton;
	public GameObject foodButton;
	public GameObject toyButton;
	public GameObject profileButton;
	public GameObject invitePartyButton;
	public GameObject shopButton;
	public GameObject addFriendButton;

	//my buttons
	public TweenPosition talkButtonTween;
	public TweenPosition emoteButtonTween;
	public TweenPosition armoryButtonTween;
	public TweenPosition foodButtonTween;
	public TweenPosition toyButtonTween;
	//others buttons
	public TweenPosition profilebuttonTween;
	public TweenPosition invitePartyButtonTween;
	public TweenPosition shopButtonTween;
	public TweenPosition addFriendButtonTween;
	public List<GameObject> buttons;
	public CharacterStatus selectedCharacter;
	public bool isPlayerPopupDisplayed = false;
	public bool isOtherPopupDisplayed = false;
	public float duration;
	public float timer;

	public void SelectPlayer(CharacterStatus target)
	{

		if(selectedCharacter == target)
		{
			Debug.Log("same guy");
			Disable();
			return;
		}
		selectedCharacter = target;
		if(target.gameObject.CompareTag("Player"))
			DisplayPlayerPopup();
		else if(target.gameObject.CompareTag("OtherPlayer"))
			DisplayFriendPopup();
		timer = 0;
		Enable();
	}

	public override void Disable()
	{
		for (int i = 0; i < buttons.Count; i++) {
			if(buttons[i].activeSelf)
			{
				tween.tweenTarget = buttons[i];
				tween.Play(false);
			}
		}
		if(isPlayerPopupDisplayed)
			isPlayerPopupDisplayed = false;
		if(isOtherPopupDisplayed)
			isOtherPopupDisplayed = false;
		selectedCharacter = null;
		base.Disable();
	}

	public void HidePopups()
	{
		for (int i = 0; i < buttons.Count; i++) {
			if(buttons[i].activeSelf)
				buttons[i].SetActive(false);
				}
	}
	
	public void DisplayPlayerPopup()
	{
		isPlayerPopupDisplayed = true;
		int t = 5;
		int i = 0;

		float startingAngle = (90 - offsetAngle*0.5f*(t-1))*Mathf.Deg2Rad;
		float radOffsetAngle = offsetAngle*Mathf.Deg2Rad;
		//move it into position
		talkButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		tween.tweenTarget = talkButton;
		tween.Play(true);
		i++;
		emoteButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		tween.tweenTarget = emoteButton;
		tween.Play(true);
		i++;
		armoryButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		tween.tweenTarget = armoryButton;
		tween.Play(true);
		i++;
		foodButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		tween.tweenTarget = foodButton;
		tween.Play(true);
		i++;
		toyButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		tween.tweenTarget = toyButton;
		tween.Play(true);
		i++;

	}

	public void DisplayFriendPopup()
	{
		//move it into position
	}

	public void DisplayStrangerPopup()
	{
		//move into position
	}

	public void ViewProfile()
	{
	}

	public void InviteToParty()
	{
	}

	public void AddFriend()
	{
	}

	public void OpenShop()
	{
	}

	public void Talk()
	{
	}

	public void Dance()
	{
	}

	public void OpenArmory()
	{
	}

	public void OpenFood()
	{
	}

	public void OpenToys()
	{
	}

	public void Update()
	{
		if(isPlayerPopupDisplayed || isOtherPopupDisplayed)
			timer += Time.deltaTime;

		if(timer > duration)
		{
			Disable();
			timer = 0;
		}
	}

}
