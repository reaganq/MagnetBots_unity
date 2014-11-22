using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Friend{

	public string Name;
	public List<ChatMessage> ChatMessages = new List<ChatMessage>();
}
