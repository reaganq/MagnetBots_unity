using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocialManager : MonoBehaviour {

	private static SocialManager instance;
	
	private SocialManager() {}
	
	public static SocialManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(SocialManager)) as SocialManager;
				if (instance == null)
				{
					SocialManager prefab = Resources.Load("Managers/_SocialManager", typeof(SocialManager)) as SocialManager;
					instance = Instantiate(prefab) as SocialManager;
				}
			}
			return instance;
		}
	}

	public List<ChatMessage> worldChatMessages;
	public List<ChatMessage> teamChatMessages;
	public List<Friend> friends;
	public PhotonView myPhotonView;

	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			GameObject.DestroyImmediate(this.gameObject);
		}
		DontDestroyOnLoad(this);
		myPhotonView = GetComponent<PhotonView>();
	}

	public void SubmitChatMessage(string playerName, string chatText, float timeStamp, int messageType)
	{
		//rpc addchatmessage
		myPhotonView.RPC("AddChatMessage", PhotonTargets.All, playerName, chatText, timeStamp, messageType); 
	}

	//[RPC]
	public void AddChatMessage(string playerName, string chatText, float timeStamp, int messageType)
	{
		ChatMessage cm = new ChatMessage();
		cm.playerName = playerName;
		cm.message = chatText;
		cm.timeStamp = timeStamp;
		GUIManager.Instance.chatGUI.AddChatBox(cm);
		worldChatMessages.Add(cm);
	}

	public void AddFriend(string name)
	{
		Friend friend = new Friend();
		friend.Name = name;
		friends.Add(friend);

	}

	public void RemoveFriend(string name)
	{
		int index = -1;
		for (int i = 0; i < friends.Count; i++) {
			if(friends[i].Name == name)
			{
				index = i;
				continue;
			}
		}
		if(index > -1)
			friends.RemoveAt(index);
	}
}
