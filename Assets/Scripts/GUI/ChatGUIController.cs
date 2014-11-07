using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatGUIController : MonoBehaviour {

	public GameObject chatboxMessageObject;
	public Transform chatPanel; 
	public List<ChatBox> chatBoxes;
	public UITable chatboxesTable;
	public UIInput chatInput;
	// Use this for initialization
	public void OnSubmit()
	{
		string text = NGUIText.StripSymbols(chatInput.value);
		if(!string.IsNullOrEmpty(text))
		{
			WorldChatManager.Instance.SubmitChatMessage(PhotonNetwork.playerName, text, Time.time);
			//tell chat manager to add text;
		}
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
}

[System.Serializable]
public class ChatMessage
{
	public string playerName;
	public string message;
	public float timeStamp;
}