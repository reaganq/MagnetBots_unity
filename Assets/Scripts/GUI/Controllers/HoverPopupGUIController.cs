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
	public GameObject danceButton;
	public GameObject inventoryButton;
	public GameObject profileButton;
	public GameObject invitePartyButton;
	public GameObject shopButton;
	public GameObject addFriendButton;

	public TweenPosition talkButtonTween;
	public TweenPosition danceButtonTween;
	public TweenPosition inventoryButtonTween;
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
			return;

		Disable();
		selectedCharacter = target;

		if(target.gameObject.CompareTag("Player"))
		{
			DisplayPlayerPopup();
		}
		else if(target.gameObject.CompareTag("OtherPlayer"))
		{
			DisplayFriendPopup();
			//DisplayStrangerPopup();
		}
		timer = 0;
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
	}
	
	public void DisplayPlayerPopup()
	{
		isPlayerPopupDisplayed = true;
		int t = 3;
		int i = 0;
		//move it into position
		talkButtonTween.to = new Vector3(offsetDistance*Mathf.Sin(((t-1)*offsetAngle/2)-i*offsetAngle),offsetDistance*Mathf.Cos(((t-1)*offsetAngle/2)-i*offsetAngle),0);
		tween.tweenTarget = talkButton;
		tween.Play(true);
		i++;
		danceButtonTween.to = new Vector3(offsetDistance*Mathf.Sin(((t-1)*offsetAngle/2)-i*offsetAngle),offsetDistance*Mathf.Cos(((t-1)*offsetAngle/2)-i*offsetAngle),0);
		tween.tweenTarget = danceButton;
		tween.Play(true);
		i++;
		inventoryButtonTween.to = new Vector3(offsetDistance*Mathf.Sin(((t-1)*offsetAngle/2)-i*offsetAngle),offsetDistance*Mathf.Cos(((t-1)*offsetAngle/2)-i*offsetAngle),0);
		tween.tweenTarget = inventoryButton;
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

	public void OpenInventory()
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
