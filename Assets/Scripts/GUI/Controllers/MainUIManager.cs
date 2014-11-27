using UnityEngine;
using System.Collections;

public class MainUIManager : BasicGUIController {

	public UILabel MagnetsCounter = null;
	public UILabel CrystalsCounter = null;
	public GameObject mainButton = null;
	public GameObject actionButtonsRoot;
	public GameObject sideTray;

	public GameObject[] PartyMemberCards;
	public GameObject QuitPartyButton;
	public UILabel[] PartyMemberNames;
	
	public bool isActionButtonsDisplayed = true;
	public bool isSideTrayOpen = false;
	public bool isPartyUIDisplayed = false;

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
		sideTray.SetActive(isSideTrayOpen);
		DisplayActionButtons(isActionButtonsDisplayed);
		UpdateCurrencyCount();
		UpdatePartyMembers();
		PlayerManager.Instance.avatarActionManager.EnableMovement();
	}

	public override void Disable()
	{
		Debug.LogWarning("wtf");
		Root.SetActive(false);
		sideTray.SetActive(false);
		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = false;
		}
		PlayerManager.Instance.avatarActionManager.DisableMovement();
	}

	public override void Hide ()
	{
		Root.SetActive(false);
	}

	public override void Reset()
	{
		sideTray.transform.localPosition = Vector3.zero;
		sideTray.SetActive(false);
		isActionButtonsDisplayed = true;
		isSideTrayOpen = false;
	}

	public void DisplayActionButtons(bool state)
	{
		if(state)
		{
			actionButtonsRoot.SetActive(true);
			isActionButtonsDisplayed = true;
		}
		else
		{
			actionButtonsRoot.SetActive(false);
			isActionButtonsDisplayed = false;
		}
	}

	#region side tray functions

	public void OnSideTrayClick()
	{
		isSideTrayOpen = !isSideTrayOpen;
		tween.Play(isSideTrayOpen);
		DisplayActionButtons(!isSideTrayOpen);
	}

	public void OnInventoryClick()
	{
		GUIManager.Instance.DisplayInventory();
	}

	public void OnSettingsClick()
	{
	}

	public void OnQuestClick()
	{
	}

	public void OnShopWizardClick()
	{
	}

	#endregion

	#region party functions

	public void QuitParty()
	{
		PlayerManager.Instance.ActiveWorld.DisbandParty();
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

	#endregion

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
}
