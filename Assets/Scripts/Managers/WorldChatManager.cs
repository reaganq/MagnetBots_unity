using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldChatManager : MonoBehaviour {

	private static WorldChatManager instance = null;
	
	private WorldChatManager() {}
	
	public static WorldChatManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(WorldChatManager)) as WorldChatManager;
				if (instance == null)
				{
					WorldChatManager prefab = Resources.Load("Managers/_ChatManager", typeof(WorldChatManager)) as WorldChatManager;
					instance = Instantiate(prefab) as WorldChatManager;
				}
			}
			return instance;
		}
	}

	public List<ChatMessage> worldChatMessages;
	public List<ChatMessage> clanChatMessages;
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

	public void SubmitChatMessage(string playerName, string chatText, float timeStamp)
	{
		//rpc addchatmessage
		myPhotonView.RPC("AddChatMessage", PhotonTargets.All, playerName, chatText, timeStamp); 
	}

	[RPC]
	public void AddChatMessage(string playerName, string chatText, float timeStamp)
	{
		ChatMessage cm = new ChatMessage();
		cm.playerName = playerName;
		cm.message = chatText;
		cm.timeStamp = timeStamp;
		GUIManager.Instance.chatGUI.AddChatBox(cm);
		worldChatMessages.Add(cm);
	}

}
