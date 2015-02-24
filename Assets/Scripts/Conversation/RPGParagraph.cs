using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(Condition))]
[XmlInclude(typeof(LineText))]
[XmlInclude(typeof(ActionEvent))]
public class RPGParagraph 
{
	[XmlElement (ElementName = "PT")]
	public string ParagraphText;
	public List<Condition> Conditions;
	public List<LineText> LineTexts;
	public List<ActionEvent> Actions;
	public float displayTimer;
	public int ownerNPCID;
	public bool isBaseParagraph;
	public NextParagraphInteraction nextParagraphCondition;
	public List<int> nextParagraphIDs;
	
	[XmlIgnore]
	public int QuestID;
	
	[XmlIgnore]
	private List<RPGParagraph> paragraphs;
	
	public RPGParagraph()
	{
		ParagraphText = string.Empty;
		Actions = new List<ActionEvent>();
		LineTexts = new List<LineText>();
		Conditions = new List<Condition>();
		nextParagraphIDs = new List<int>();
		displayTimer = 0;
		//CanEndParagraph = true;
		//preffix = "PARAGRAPH";
	}
	
	//load first paragraph
	/*public RPGParagraph LoadByOwner(RPGNPC owner)
	{
		paragraphs = Storage.Load<RPGParagraph>(new RPGParagraph());
		foreach(RPGParagraph paragraph in paragraphs)
		{
			if (paragraph.OwnerId == owner.ID && paragraph.ParentLineTextId == 0 && paragraph.Validate())
			{
				paragraph.paragraphs = paragraphs;
				return paragraph;
			}
		}
		return null;
	}*/
	
	/*public static RPGParagraph LoadByParentLineText(int lineTextId)
	{
		List<RPGParagraph> paragraphs = Storage.Load<RPGParagraph>(new RPGParagraph());
		
		foreach(RPGParagraph paragraph in paragraphs)
		{
			if (paragraph.ParentLineTextId == lineTextId)
			{
				return paragraph;
			}
		}
		return new RPGParagraph();
	}*/
	
	/*public static List<RPGParagraph> LoadAllByOwner(int ownerId)
	{
		List<RPGParagraph> paragraphs = Storage.Load<RPGParagraph>(new RPGParagraph());
		List<RPGParagraph> result = new List<RPGParagraph>();
		foreach(RPGParagraph paragraph in paragraphs)
		{
			if (paragraph.OwnerId == ownerId)
			{
				result.Add(paragraph);
			}
		}
		return result;
	}*/
	
	public bool Validate()
	{
		foreach (Condition condition in Conditions)
		{
			if (condition.Validate() == false)
			{
				return false;
			}
		}
		return true;
	}
	
	public void DoEvents()
	{
		if (Actions == null || Actions.Count == 0)
			return;
		
		foreach(ActionEvent action in Actions)
		{
			action.DoAction();
		}
	}
	
	/*public void AddGeneralConversation(RPGNPC npc)
	{
		if (npc.GeneralConversationID == null || npc.GeneralConversationID.Count == 0)
			return;
		
		if (ParentLineTextId != 0)
			return;
		
		if (paragraphs == null)
		{
			paragraphs = Storage.Load<RPGParagraph>(new RPGParagraph());
		}
		
		foreach(int ID in npc.GeneralConversationID)
		{
			foreach(RPGParagraph p in paragraphs)
			{
				if (p.ID == ID)
				{
					this.ID = p.ID;
					
					if (string.IsNullOrEmpty(ParagraphText))
						ParagraphText = p.ParagraphText;
					
					foreach(LineText lt in p.LineTexts)
					{
						LineTexts.Add(lt);
					}
				}
			}
		}
	}*/
	
	public void AddLineText()
	{
		if (QuestID == 0)
			return;
		
		//List<RPGParagraph> paragraphs = Storage.Load<RPGParagraph>(this);
		
		LineText lineText;
		Condition c;
		
		int newLineTextId =  0;
		
		lineText = new LineText();
		lineText.ID = newLineTextId;
		lineText.Text = "Quest number " + QuestID + " is not started";
		c = new Condition();
		c.ItemToHave = QuestID.ToString();
		c.ConditionType = ConditionTypeEnum.QuestNotStarted;
		
		lineText.Conditions.Add(c);
		LineTexts.Add(lineText);
		
		newLineTextId++;
		
		lineText = new LineText();
		lineText.ID = newLineTextId;
		lineText.Text = "Quest number " + QuestID + " in progress";
		c = new Condition();
		c.ItemToHave = QuestID.ToString();
		c.ConditionType = ConditionTypeEnum.QuestInProgress;
		
		lineText.Conditions.Add(c);
		LineTexts.Add(lineText);
		
		newLineTextId++;
		
		
		lineText = new LineText();
		lineText.ID = newLineTextId;
		lineText.Text = "Quest number " + QuestID + " in finished";
		c = new Condition();
		c.ItemToHave = QuestID.ToString();
		c.ConditionType = ConditionTypeEnum.QuestFinished;
		
		lineText.Conditions.Add(c);
		LineTexts.Add(lineText);
		
		newLineTextId++;
		
		lineText = new LineText();
		lineText.ID = newLineTextId;
		lineText.Text = "Quest number " + QuestID + " is completed";
		c = new Condition();
		c.ItemToHave = QuestID.ToString();
		c.ConditionType = ConditionTypeEnum.QuestCompleted;
		
		lineText.Conditions.Add(c);
		LineTexts.Add(lineText);
		
		QuestID = 0;
	}
	
	/*int NewLineTextID(List<RPGParagraph> paragraphs)
	{
		int maximum = 0;
		foreach(RPGParagraph p in paragraphs)
		{
			foreach(LineText lt in p.LineTexts)
			{
				if (lt.ID > maximum)
					maximum = lt.ID;
			}
		}
		return maximum + 1;
	}*/
}

public enum NextParagraphInteraction
{
	Nothing = 0,
	WaitForPlayerReply = 1,
	NextClick = 2,
	WaitForUIAction = 3,
}

