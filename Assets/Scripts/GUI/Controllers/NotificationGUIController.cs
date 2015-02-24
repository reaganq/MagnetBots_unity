using UnityEngine;
using System.Collections;

public class NotificationGUIController : BasicGUIController {

	public NotificationUIState state;
	public GameObject notificationBox;
	public UILabel notificationMessage;
	public PhotonPlayer prospectivePartyLeader;

	public GameObject partyChallenge;
	public UILabel partyChallengeMessage;
	public UILabel enemyLabel;
	public PhotonPlayer challenger;

	public GameObject partyChallengeWait;

	public GameObject messageBox;
	public UILabel messageBoxLabel;

	/*public GameObject selectedCharacter;
	public GameObject hoverBox;
	public GameObject addToFriendButton;
	public GameObject inviteButton;*/
	public UIPlayTween notificationTween;

	// Use this for initialization
	public void UpdateMessage (string name) 
	{
		notificationMessage.text = name + "would like to form a party with you!";
	}
	
	public void OnConfirm()
	{
		if(state == NotificationUIState.teamInvite)
		{
			if(prospectivePartyLeader == null)
				return;
			PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("AcceptPartyInvite", prospectivePartyLeader);
			prospectivePartyLeader = null;
			HideNotificationBox();
		}
		else if(state == NotificationUIState.teamChallenge)
		{

		}
	}

	public void OnReject()
	{
		if(state == NotificationUIState.teamInvite)
		{
			if(prospectivePartyLeader == null)
				return;
			PlayerManager.Instance.ActiveWorld.RejectPartyInvite(prospectivePartyLeader);
			prospectivePartyLeader = null;
			HideNotificationBox();
		}
	}

	public void DisplayPartyWaiting()
	{
		notificationTween.tweenTarget = partyChallengeWait;
		notificationTween.Play(true);
		state = NotificationUIState.teamChallengeWait;
	}

	public void HidePartyWaiting()
	{
		partyChallengeWait.SetActive(false);
		state = NotificationUIState.none;
	}

	public void DisplayPartyChallenge(int enemyID, PhotonPlayer c)
	{
		challenger = c;
		notificationTween.tweenTarget = partyChallenge;
		notificationTween.Play(true);
		state = NotificationUIState.teamChallenge;
		RPGEnemy enemy = Storage.LoadById<RPGEnemy>(enemyID, new RPGEnemy());
		partyChallengeMessage.text = challenger.name;
		enemyLabel.text = enemy.Name;
	}

	public void CancelPartyChallenge(PhotonPlayer c)
	{
		if(challenger == c && state == NotificationUIState.teamChallenge)
		{
			HidePartyChallenge();
		}
	}

	public void HidePartyChallenge()
	{
		partyChallenge.SetActive(false);
		state = NotificationUIState.none;
	}

	public void DisplayNotificationBox(PhotonPlayer player, int partyLeaderID)
	{
		notificationTween.tweenTarget = notificationBox;
		state = NotificationUIState.teamInvite;
		UpdateMessage(player.ToString());
		notificationTween.Play(true);
		prospectivePartyLeader = PhotonPlayer.Find(partyLeaderID);
	}

	public void HideNotificationBox()
	{
		notificationBox.SetActive(false);
		state = NotificationUIState.none;
	}

	public void DisplayMessageBox(string message)
	{
		messageBoxLabel.text = message;
		notificationTween.tweenTarget = messageBox;
		state = NotificationUIState.message;
		notificationTween.Play(true);
	}

	public void HideMessageBox()
	{
		messageBox.SetActive(false);
		state = NotificationUIState.none;
	}

	/*public void OnAddToFriends()
	{
		GUIManager.Instance.HideCharacterPopUp();
	}

	public void OnInviteToParty()
	{
		//Debug.Log(PlayerManager.Instance.partyMembers.Count);
		//TODO allow non party leader to recommend party invites.
		//PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("SendPartyInvite", selectedCharacter.GetComponent<PhotonView>().owner);
		//GUIManager.Instance.HideCharacterPopUp();
	}

	public void DisplayHoverBox(GameObject character)
	{
		if(selectedCharacter == character)
		{
			GUIManager.Instance.HideCharacterPopUp();
		}
		else
		{
			selectedCharacter = character;
			hoverBox.SetActive(true);
		}
	}

	public void HideHoverBox()
	{
		hoverBox.SetActive(false);
		selectedCharacter = null;
	}*/
}

public enum NotificationUIState
{
	none,
	teamInvite,
	teamChallenge,
	teamChallengeWait,
	message
}
