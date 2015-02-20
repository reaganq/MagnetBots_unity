using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(RPGParagraph))]
[XmlInclude(typeof(Condition))]
public class RPGConversation : BasicItem {

	public List<RPGParagraph> conversationParagraphs;
	public List<Condition> conditions;

	[XmlIgnore]
	public int activeParagraphID;
	[XmlIgnore]
	public int activeLineTextID;

	public RPGConversation()
	{
		conversationParagraphs = new List<RPGParagraph>();
		conditions = new List<Condition>();
		preffix = "CONVERSATION";
	}
}
