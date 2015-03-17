using UnityEngine;
using System.Collections;

public class NotificationGUIController : BasicGUIController {

	public NotificationUIState state;
	public GameObject notificationBox;
	public UILabel notificationMessage;
	public PhotonPlayer prospectivePartyLeader;

	public GameObject messageBoxConfirmButton;
	public GameObject partyChallengeAcceptButton;
	public UILabel partyChallengeAcceptButtonLabel;
	public GameObject partyChallengeRejectButton;

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

	public void DisplayMessageBox(string message)
	{
		messageBoxLabel.text = message;
		notificationTween.tweenTarget = messageBox;
		state = NotificationUIState.message;
		notificationTween.Play(true);
	}
	
	public void HideMessageBox()
	{
		notificationTween.tweenTarget = messageBox;
		notificationTween.Play(false);
		state = NotificationUIState.none;
	}

	public void OnMessageConfirm()
	{
		HideMessageBox();
	}

	public void OnAccept()
	{
		if(state == NotificationUIState.teamInvite)
		{
			if(prospectivePartyLeader == null)
				return;
			Debug.Log("accept party invite");
			PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("AcceptPartyInvite", prospectivePartyLeader, PlayerManager.Instance.avatarPhotonView.viewID);
			prospectivePartyLeader = null;
			HideNotificationBox();
		}
		else if(state == NotificationUIState.teamChallenge)
		{
			PlayerManager.Instance.ActiveWorld.PartyChallengeReply(challenger, true);
			DisplayPartyWaiting();
		}
		else if(state == NotificationUIState.cancelTeamChallenge)
			HidePartyChallenge();
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
		else if(state == NotificationUIState.teamChallenge || state == NotificationUIState.teamChallengeWait)
		{
			PlayerManager.Instance.ActiveWorld.PartyChallengeReply(challenger, false);
			HidePartyChallenge();
		}
		else if(state == NotificationUIState.cancelTeamChallenge)
		{
			HidePartyChallenge();
		}
		else if(state == NotificationUIState.challengerWait)
		{
			PlayerManager.Instance.ActiveWorld.CancelPartyChallenge();
		}
		else if(state == NotificationUIState.teamChallengeLoading)
			HidePartyChallenge();
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

	public void DisplayPartyChallenge(int enemyID, PhotonPlayer c)
	{
		challenger = c;
		notificationTween.tweenTarget = partyChallenge;
		notificationTween.Play(true);
		state = NotificationUIState.teamChallenge;
		RPGEnemy enemy = Storage.LoadById<RPGEnemy>(enemyID, new RPGEnemy());
		partyChallengeMessage.text = challenger.name + " wants your help to defeat: " + enemy.Name;
		partyChallengeAcceptButton.SetActive(true);
		partyChallengeRejectButton.SetActive(true);
		partyChallengeAcceptButtonLabel.text = "Accept";

	}

	public void HidePartyChallenge()
	{
		notificationTween.tweenTarget = partyChallenge;
		notificationTween.Play(false);
		state = NotificationUIState.none;
	}

	public void DisplayPartyWaiting()
	{
		//notificationTween.tweenTarget = partyChallengeWait;
		//notificationTween.Play(true);
		partyChallenge.SetActive(true);
		partyChallengeMessage.text = "Waiting for your team to respond.";
		state = NotificationUIState.teamChallengeWait;
		partyChallengeAcceptButton.SetActive(false);
		partyChallengeRejectButton.SetActive(true);
	}

	public void DisplayChallengerPartyWaiting()
	{
		notificationTween.tweenTarget = partyChallenge;
		partyChallengeMessage.text = "Waiting for your team to respond.";
		notificationTween.Play(true);
		state = NotificationUIState.challengerWait;
		partyChallengeAcceptButton.SetActive(false);
		partyChallengeRejectButton.SetActive(true);
	}

	public void StartTeamChallenge()
	{
		partyChallengeMessage.text = "Entering the battle arena!";
		state = NotificationUIState.teamChallengeLoading;
	}

	public void CancelPartyChallenge()
	{
		if(state == NotificationUIState.teamChallengeWait || state == NotificationUIState.teamChallenge || state == NotificationUIState.challengerWait)
		{
			state = NotificationUIState.cancelTeamChallenge;
			partyChallenge.SetActive(true);
			partyChallengeMessage.text = "Battle arena challenge has been cancelled";
			partyChallengeAcceptButtonLabel.text = "Confirm";
			partyChallengeAcceptButton.SetActive(true);
			partyChallengeRejectButton.SetActive(false);
			//rejectbutton.setactive(false);
		}
	}

	public void LeaveZone()
	{
		partyChallenge.SetActive(false);
		OnReject();
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
	cancelTeamChallenge,
	challengerWait,
	teamChallengeLoading,
	message
}
