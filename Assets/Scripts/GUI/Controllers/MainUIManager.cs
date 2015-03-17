using UnityEngine;
using System.Collections;

public class MainUIManager : BasicGUIController {

	public UILabel coinsCounter = null;
	public UILabel magnetsCounter = null;
	public UILabel citizenpointCounter = null;
	public GameObject currenciesRoot;
	public GameObject mainButton = null;
	public GameObject actionButtonsRoot;
	public GameObject sideTray;

	public GameObject[] actionButtons;
	public SkillButton[] actionButtonScripts;

	public TeamMemberUI[] PartyMemberCards;
	public GameObject QuitPartyButton;
	public Transform singlePartymemberLeaveButtonPos;
	public Transform fullPartyMemberLeaveButtonPos;
	//public UILabel[] PartyMemberNames;
	
	public bool isActionButtonsDisplayed = true;
	public bool isSideTrayOpen = false;
	public bool isPartyUIDisplayed = false;
	public UISprite soundButton;

	public void UpdateCurrencyCount()
	{
		coinsCounter.text = PlayerManager.Instance.Hero.Coins.ToString();
		magnetsCounter.text = PlayerManager.Instance.Hero.Magnets.ToString();
		citizenpointCounter.text = PlayerManager.Instance.Hero.CitizenPoints.ToString();
		//magnetsCounter.text = PlayerManager.Instance.Hero.Magnets.ToString();
	}

	public override void Enable()
	{
		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = true;
		}
		sideTray.SetActive(isSideTrayOpen);
		DisplayActionButtons(isActionButtonsDisplayed);
		UpdateCurrencyCount();
		UpdatePartyMembers();
		PlayerManager.Instance.avatarActionManager.EnableMovement();
		base.Enable();
	}

	public override void Disable()
	{
		sideTray.SetActive(false);
		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = false;
		}
		PlayerManager.Instance.avatarActionManager.DisableMovement();

		base.Disable();
		for (int i = 0; i < actionButtonScripts.Length; i++) {
			if(actionButtonScripts[i].isbuttonPressed)
				actionButtonScripts[i].OnPress(false);
		}
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

	public void EnterBattleMode(bool state)
	{
		mainButton.SetActive(!state);
		currenciesRoot.SetActive(!state);
	}

	public void DisplayCurrencies(bool state)
	{
		currenciesRoot.SetActive(state);
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

	public void EnableActionButton(int index)
	{
		actionButtons[index].SetActive(true);
		actionButtonScripts[index].SetupSkillButton(index);
	}

	public void DisableActionButton(int index)
	{
		actionButtons[index].SetActive(false);
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

	public void OnProfileClick()
	{
		GUIManager.Instance.DisplayProfile(PhotonNetwork.player);
	}

	#endregion

	#region party functions

	public void QuitParty()
	{
		PlayerManager.Instance.ActiveWorld.DisbandParty();
	}

	public void UpdatePartyMembers()
	{
		int index = 0;

		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) {
			if(PlayerManager.Instance.partyMembers[i].playerID != PhotonNetwork.player.ID && index < PartyMemberCards.Length)
			{
				PartyMemberCards[index].Initialise(PlayerManager.Instance.partyMembers[i].viewID);
				index ++;
			}
		}

		if(index < PartyMemberCards.Length)
		{
			for (int j = index; j < PartyMemberCards.Length; j++) 
			{
				PartyMemberCards[j].Deactivate();
			}
		}

		if(index > 0)
			QuitPartyButton.SetActive(true);
		else
			QuitPartyButton.SetActive(false);

		if(index == 1)
			QuitPartyButton.transform.position = singlePartymemberLeaveButtonPos.position;
		else if(index == 2)
			QuitPartyButton.transform.position = fullPartyMemberLeaveButtonPos.position;
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
