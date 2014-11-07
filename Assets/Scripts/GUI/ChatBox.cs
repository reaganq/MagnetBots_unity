using UnityEngine;
using System.Collections;

public class ChatBox : MonoBehaviour {

	public UILabel nameLabel;
	public UILabel textLabel;
	public UILabel timeStamp;

	public void LoadChatBox(ChatMessage cm)
	{
		nameLabel.text = cm.playerName;
		textLabel.text = cm.message;
		timeStamp.text = (Time.time - cm.timeStamp).ToString() + "s ago";
	}
}
