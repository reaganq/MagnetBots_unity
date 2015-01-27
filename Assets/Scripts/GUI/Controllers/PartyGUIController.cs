using UnityEngine;
using System.Collections;

public class PartyGUIController : BasicGUIController {

	public GameObject notificationBox;
	public UILabel notificationMessage;
	public PhotonPlayer prospectivePartyLeader;

	public GameObject selectedCharacter;
	public GameObject hoverBox;
	public GameObject addToFriendButton;
	public GameObject inviteButton;
	public UIPlayTween notificationTween;

	// Use this for initialization
	public void UpdateMessage (string name) 
	{
		notificationMessage.text = name + "would like to form a party with you!";
	}
	
	public void OnConfirm()
	{
		if(prospectivePartyLeader == null)
			return;
		PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("AcceptPartyInvite", prospectivePartyLeader);
		prospectivePartyLeader = null;
		notificationTween.Play(false);
	}

	public void OnReject()
	{
		notificationTween.Play(false);
	}

	public void DisplayNotificationBox(PhotonPlayer player, int partyLeaderID)
	{
		UpdateMessage(player.ToString());
		notificationTween.Play(true);
		prospectivePartyLeader = PhotonPlayer.Find(partyLeaderID);
	}

	public void HideNotificationBox()
	{
		notificationBox.SetActive(false);
	}

	public void OnAddToFriends()
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
	}
}
