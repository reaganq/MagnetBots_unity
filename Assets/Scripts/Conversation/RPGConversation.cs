using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(RPGParagraph))]
public class RPGConversation : BasicItem {

	public List<RPGParagraph> ConversationParagraphs;

	[XmlIgnore]
	public int activeParagraphID;
	[XmlIgnore]
	public int activeLineTextID;

	public RPGConversation()
	{
		ConversationParagraphs = new List<RPGParagraph>();
		preffix = "CONVERSATION";
	}
}
