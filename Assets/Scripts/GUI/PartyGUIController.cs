using UnityEngine;
using System.Collections;

public class PartyGUIController : MonoBehaviour {

	public GameObject notificationBox;
	public UILabel notificationMessage;
	public PhotonPlayer prospectivePartyLeader;

	public GameObject selectedCharacter;
	public GameObject hoverBox;
	public GameObject addToFriendButton;
	public GameObject inviteButton;

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
		GUIManager.Instance.HidePartyNotification();
	}

	public void OnReject()
	{
		GUIManager.Instance.HidePartyNotification();
	}

	public void DisplayNotificationBox(PhotonPlayer player)
	{
		notificationBox.SetActive(true);
		UpdateMessage(player.ToString());
		prospectivePartyLeader = player;
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
		if(PlayerManager.Instance.isPartyLeader || PlayerManager.Instance.partyMembers.Count == 0)
			PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("SendPartyInvite", selectedCharacter.GetComponent<PhotonView>().owner);
		GUIManager.Instance.HideCharacterPopUp();
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
