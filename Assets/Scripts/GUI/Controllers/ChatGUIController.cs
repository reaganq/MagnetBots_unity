using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatGUIController : BasicGUIController {

	//chat box/friend box prefabs
	public GameObject chatboxMessageObject;
	public GameObject friendObject;

	//list of chat boxes
	public List<ChatBox> chatBoxes;
	//list of friend boxes
	public List<FriendBox> friendBoxes;
	public List<ChatBox> friendChatBoxes;

	//add/remove friend buttons
	public GameObject friendListControls;

	//uitables
	public UITable chatboxesTable;
	public UITable friendsTable;
	public UITable friendChatTable;

	public UIInput chatInput;
	public int maxNumMessages = 50;

	//0 = world
	//1 = team
	//2 = Friends
	public int messagePanelState = 0;
	public int activeFriendChatIndex;
	//cached panels
	public UIPanel mainPanel;
	public UIPanel friendChatPanel;
	public UIPanel friendPanel;

	//state of friends list ui
	public bool removeFriends = false;
	public bool isFriendChatDisplayed = false;
	public Vector3 cachedFriendListPos;
	public Transform friendTableTopPosRef;

	public GameObject mainChatScrollBar;
	public GameObject friendsListScrollBar;
	public GameObject mainScrollPanelCollider;
	public Animation animation;
	// Use this for initialization
	public void Start()
	{
		messagePanelState = 0;
		/*for (int i = 0; i < maxNumMessages; i++) 
		{
			GameObject chatBox = Instantiate(chatboxMessageObject) as GameObject;
			chatBox.transform.parent = chatboxesTable.transform;
			chatBox.transform.localPosition = Vector3.zero;
			chatBox.transform.localScale = Vector3.one;
			ChatBox cb = chatBox.GetComponent<ChatBox>();
			UnusedChatBoxes.Add(cb);
			chatBox.SetActive(false);
		}*/
		UpdateChatPanel();
	}

	public override void Enable()
	{
		animation.Play("OpenChatBox");
	}

	public override void Disable()
	{
		animation.Play("CloseChatBox");
	}

	public void OnSubmit()
	{
		string text = NGUIText.StripSymbols(chatInput.value);
		if(!string.IsNullOrEmpty(text))
		{
			SocialManager.Instance.SubmitChatMessage(PhotonNetwork.playerName, text, Time.time, messagePanelState);
			//tell chat manager to add text;
		}
	}

	public void OnAddFriend()
	{
	}

	public void OnRemoveFriend()
	{
		removeFriends = !removeFriends;
		if(removeFriends)
		{
			for (int i = 0; i < friendBoxes.Count; i++) {
				if(friendBoxes[i].enabled)
					friendBoxes[i].ShowRemove();
			}
		}
		else
		{
			for (int i = 0; i < friendBoxes.Count; i++) {
				if(friendBoxes[i].enabled)
					friendBoxes[i].ShowNormal();
			}
		}

	}

	public void SwitchChatPanel(int panelIndex)
	{
		messagePanelState = panelIndex;
		UpdateChatPanel();
	}

	public void UpdateChatPanel()
	{
		if(messagePanelState == 2)
			ShowFriendsList();
		else
			ShowNormalChat();
	}

	public void ShowNormalChat()
	{
		chatboxesTable.gameObject.SetActive(true);
		friendsTable.gameObject.SetActive(false);
		friendListControls.SetActive(false);
		mainChatScrollBar.SetActive(true);
		friendsListScrollBar.SetActive(false);
	}

	public void UpdateNormalChat()
	{
	}

	public void ShowFriendsList()
	{
		//reposition and disable the chatboxes
		chatboxesTable.gameObject.SetActive(false);
		friendsTable.gameObject.SetActive(true);
		friendListControls.SetActive(true);
		friendsListScrollBar.SetActive(true);
		mainChatScrollBar.SetActive(false);
		//load up correct number of friendboxes
		UpdateFriendsList();
	}

	public void UpdateFriendsList()
	{
		int num = SocialManager.Instance.friends.Count - friendBoxes.Count;
		if(num > 0)
		{
			for (int i = 0; i < num; i++) {
				/*GameObject friendBox = Instantiate(friendObject) as GameObject;
				friendBox.transform.parent = friendsTable.transform;
				friendBox.transform.localPosition = Vector3.zero;
				friendBox.transform.localScale = Vector3.one;
				*/
				GameObject friendBox = NGUITools.AddChild(friendsTable.gameObject, friendObject);

				FriendBox fb = friendBox.GetComponent<FriendBox>();
				friendBoxes.Add(fb);
			}
		}
		for (int i = 0; i < friendBoxes.Count; i++) {
			if((i + 1) > SocialManager.Instance.friends.Count)
				friendBoxes[i].gameObject.SetActive(false);
			else
			{
				friendBoxes[i].gameObject.SetActive(true);
				friendBoxes[i].LoadFriendBox(SocialManager.Instance.friends[i].Name, removeFriends);
				friendBoxes[i].name = friendBoxes[i].Name.text;
			}
		}
		
		friendsTable.repositionNow = true;
	}

	public void ShowFriendChat(FriendBox friendBox)
	{
		//move friends over to top
		int index = friendBoxes.IndexOf(friendBox);
		Vector3 offset = Vector3.zero;

		//friend chat is not open
		if(!isFriendChatDisplayed)
		{
			isFriendChatDisplayed = true;
			activeFriendChatIndex = index;
			offset = -friendPanel.cachedTransform.InverseTransformPoint(friendBox.transform.position - friendTableTopPosRef.position);
			//offset = friendBox.transform.position - friendTableTopPosRef.position;
		}
		else
		{
			//friend is already open. close chat
			if(index == activeFriendChatIndex)
			{
				isFriendChatDisplayed = false;
				offset = cachedFriendListPos;
				activeFriendChatIndex = -1;
				offset = -friendPanel.cachedTransform.InverseTransformPoint(friendBox.transform.position - cachedFriendListPos);
			}
			//open different friend chat
			else
			{
				isFriendChatDisplayed = true;
				activeFriendChatIndex = index;
				//cachedFriendListPos = friendBox.transform.position;
				offset = -friendPanel.cachedTransform.InverseTransformPoint(friendBox.transform.position- friendTableTopPosRef.position);
				//offset = friendBox.transform.position - friendTableTopPosRef.position;
			}
		}

		offset.x = friendPanel.cachedTransform.localPosition.x;
		Debug.Log(offset);
		cachedFriendListPos = friendBox.transform.position;
		SpringPanel.Begin(friendPanel.cachedGameObject, offset, 12f);
		cachedFriendListPos.x = friendPanel.cachedTransform.localPosition.x;
		Debug.Log("cache pos: "+cachedFriendListPos);
		//scale dummy object
		//display friend chat
	}

	public void AddChatBox(ChatMessage cm)
	{
		GameObject chatBox = Instantiate(chatboxMessageObject) as GameObject;
		ChatBox cb = chatBox.GetComponent<ChatBox>();
		chatBox.transform.parent = chatboxesTable.transform;
		chatBox.transform.localPosition = Vector3.zero;
		chatBox.transform.localScale = Vector3.one;
		cb.LoadChatBox(cm);
		chatBoxes.Add(cb);
		chatboxesTable.repositionNow = true;
	}

	public void RemoveFriend(FriendBox fb)
	{
	}
}

[System.Serializable]
public class ChatMessage
{
	public string playerName;
	public string message;
	public float timeStamp;
}