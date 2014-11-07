using UnityEngine;
using System.Collections;

public class MainUIManager : BasicGUIController {

	public UILabel MagnetsCounter = null;
	public UILabel CrystalsCounter = null;
	public GameObject OpenInventoryButton = null;
	public GameObject ActionButtons;
	public GameObject Root;

	public GameObject[] PartyMemberCards;
	public GameObject QuitPartyButton;
	public UILabel[] PartyMemberNames;

	public Animation bottomPanelAnimation;
	public bool isBottomTrayOpen = false;

	public UISprite soundButton;

	public void UpdateCurrencyCount()
	{
		MagnetsCounter.text = PlayerManager.Instance.Hero.Magnets.ToString();
		CrystalsCounter.text = PlayerManager.Instance.Hero.Crystals.ToString();
	}

	public override void Enable()
	{
		Root.SetActive(true);

		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = true;
		}
		else
			DisplayActionButtons(false);
		if(!GUIManager.Instance.CanShowInventory)
			OpenInventoryButton.SetActive(false);
		UpdateCurrencyCount();
		UpdatePartyMembers();
		PlayerManager.Instance.avatarActionManager.EnableMovement();
	}

	public void QuitParty()
	{
		PlayerManager.Instance.ActiveWorld.DisbandParty();
	}

	public void DisplayActionButtons(bool state)
	{
		if(state)
			ActionButtons.SetActive(true);
		else
			ActionButtons.SetActive(false);
	}

	public override void Disable()
	{
		Debug.LogWarning("wtf");
		Root.SetActive(false);
		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = false;
		}
		PlayerManager.Instance.avatarActionManager.DisableMovement();
	}

	public void UpdatePartyMembers()
	{

		for (int i = 0; i < PartyMemberCards.Length; i++) 
		{
			if(i < PlayerManager.Instance.partyMembers.Count)
			{
				PartyMemberCards[i].SetActive(true);
				PartyMemberNames[i].text = PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]).ToString();
			}
			else
				PartyMemberCards[i].SetActive(false);
		}

		if(PlayerManager.Instance.partyMembers.Count > 1)
			QuitPartyButton.SetActive(true);
		else
			QuitPartyButton.SetActive(false);
	}

	public void MuteSound()
	{
		SfxManager.Instance.MuteBackgroundMusic();
		if(SfxManager.Instance.isBackgroundMuted)
		{
			soundButton.spriteName = "mutebutton";
		}
		else
		{
			soundButton.spriteName = "soundbutton";
		}
	}

	public void OpenChatBox(bool state)
	{
		if(state)
		{
			bottomPanelAnimation.Play("OpenChatBox");
			isBottomTrayOpen = true;
			DisplayActionButtons(false);
		}
		else
		{
			bottomPanelAnimation.Play("CloseChatBox");
			isBottomTrayOpen = false;
			DisplayActionButtons(true);
		}
	}

}
