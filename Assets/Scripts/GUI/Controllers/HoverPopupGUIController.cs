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
	public UIPlayTween buttonTween;

	public void Start()
	{
		int t = 5;
		int i = 0;
		
		float startingAngle = (90 - offsetAngle*0.5f*(t-1))*Mathf.Deg2Rad;
		float radOffsetAngle = offsetAngle*Mathf.Deg2Rad;
		//move it into position
		talkButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		emoteButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		armoryButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		foodButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		toyButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;

		t = 4;
		i = 0;

		startingAngle = (90 - offsetAngle*0.5f*(t-1))*Mathf.Deg2Rad;
		radOffsetAngle = offsetAngle*Mathf.Deg2Rad;
		//move it into position
		profilebuttonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		invitePartyButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		addFriendButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
		shopButtonTween.to = new Vector3(offsetDistance*Mathf.Cos(startingAngle + radOffsetAngle*i),offsetDistance*Mathf.Sin(startingAngle + radOffsetAngle*i), 0);
		i++;
	}
	
	public void SelectPlayer(CharacterStatus target)
	{
		if(selectedCharacter == target)
		{
			Debug.Log("same guy");
			Disable();
			return;
		}

		if(isPlayerPopupDisplayed || isOtherPopupDisplayed)
		{
			HidePopups();
		}
		selectedCharacter = target;
		if(target.gameObject.CompareTag("Player"))
			DisplayPlayerPopup();
		else if(target.gameObject.CompareTag("OtherPlayer"))
		{
			//TODO check if friend
			DisplayFriendPopup();
		}
		timer = 0;
		Enable();
	}
	
	public override void Disable()
	{
		if(isPlayerPopupDisplayed)
		{
			isPlayerPopupDisplayed = false;
			buttonTween.tweenTarget = talkButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = emoteButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = armoryButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = foodButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = toyButton;
			buttonTween.Play(false);
		}
		if(isOtherPopupDisplayed)
		{
			isOtherPopupDisplayed = false;
			buttonTween.tweenTarget = profileButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = invitePartyButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = addFriendButton;
			buttonTween.Play(false);
			buttonTween.tweenTarget = shopButton;
			buttonTween.Play(false);
		}
		selectedCharacter = null;
		base.Disable();
	}
	
	public void HidePopups()
	{
		if(isPlayerPopupDisplayed)
		{
			isPlayerPopupDisplayed = false;
			talkButton.SetActive(false);
			emoteButton.SetActive(false);
			armoryButton.SetActive(false);
			foodButton.SetActive(false);
			toyButton.SetActive(false);
			talkButton.transform.localPosition = Vector3.zero;
			emoteButton.transform.localPosition = Vector3.zero;
			armoryButton.transform.localPosition = Vector3.zero;
			foodButton.transform.localPosition = Vector3.zero;
			toyButton.transform.localPosition = Vector3.zero;
		}
		if(isOtherPopupDisplayed)
		{
			isOtherPopupDisplayed = false;
			profileButton.SetActive(false);
			invitePartyButton.SetActive(false);
			addFriendButton.SetActive(false);
			shopButton.SetActive(false);
			profileButton.transform.localPosition = Vector3.zero;
			invitePartyButton.transform.localPosition = Vector3.zero;
			addFriendButton.transform.localPosition = Vector3.zero;
			shopButton.transform.localPosition = Vector3.zero;
		}
	}
	
	public void DisplayPlayerPopup()
	{
		isPlayerPopupDisplayed = true;

		buttonTween.tweenTarget = talkButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = emoteButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = armoryButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = foodButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = toyButton;
		buttonTween.Play(true);

		
	}
	
	public void DisplayFriendPopup()
	{
		isOtherPopupDisplayed = true;
		buttonTween.tweenTarget = profileButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = invitePartyButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = addFriendButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = shopButton;
		buttonTween.Play(true);
	}
	
	public void DisplayStrangerPopup()
	{
		isOtherPopupDisplayed = true;
		buttonTween.tweenTarget = profileButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = invitePartyButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = addFriendButton;
		buttonTween.Play(true);
		buttonTween.tweenTarget = shopButton;
		buttonTween.Play(true);
	}
	
	public void ViewProfile()
	{
		GUIManager.Instance.DisplayProfile(selectedCharacter.myPhotonView.owner);
	}
	
	public void InviteToParty()
	{
		if(PlayerManager.Instance.partyMembers.Count >= GeneralData.maxPartySize)
			return;

		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].viewID == selectedCharacter.myPhotonView.viewID)
			{
				Debug.Log("already in party");
				return;
			}
		}

		Debug.Log("invite to party");
		if(PlayerManager.Instance.partyMembers.Count > 0)
			PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("SendPartyInvite", selectedCharacter.GetComponent<PhotonView>().owner, PlayerManager.Instance.partyMembers[0]);
		else if(PlayerManager.Instance.partyMembers.Count == 0)
			PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("SendPartyInvite", selectedCharacter.GetComponent<PhotonView>().owner, PhotonNetwork.player.ID);
		Disable();
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
		GUIManager.Instance.DisplayQuickInventory(ItemCategories.Armors);
	}
	
	public void OpenFood()
	{
		GUIManager.Instance.DisplayQuickInventory(ItemCategories.Food);
	}
	
	public void OpenToys()
	{
		GUIManager.Instance.DisplayQuickInventory(ItemCategories.Toys);
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
