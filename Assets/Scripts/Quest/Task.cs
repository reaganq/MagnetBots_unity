using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class Task {
	
	[XmlAttribute (AttributeName = "TT")]
	public TaskTypeEnum TaskType;
	
	[XmlAttribute (AttributeName = "ATR")]
	public int AmountToReach;
	
	[XmlAttribute (AttributeName = "PT")]
	public PreffixType PreffixTarget;
	
	[XmlAttribute (AttributeName = "TASKTARGET")]
	public int TaskTarget;

	[XmlAttribute (AttributeName = "TL")]
	public int Tasklevel;
	
	[XmlAttribute (AttributeName = "CA")]
	public int CurrentAmount;
	
	[XmlAttribute (AttributeName = "QLD")]
	public string QuestLogDescription;
	
	public Task()
	{
		TaskType = TaskTypeEnum.KillEnemy;
		QuestLogDescription = string.Empty;
		CurrentAmount = 0;
	}
	
	public bool IsTaskFinished()
	{
		switch(TaskType)
		{
		case TaskTypeEnum.KillEnemy:
			if(CurrentAmount >= AmountToReach)	
				return true;
			break;
		case TaskTypeEnum.ReachPartOfConversation:
			if (CurrentAmount == 1)
				return true;
			break;
		case TaskTypeEnum.VisitArea:
			if (CurrentAmount == 1)
				return true;
			break;
		case TaskTypeEnum.BringItem:
			if(CurrentAmount >= AmountToReach)
				return true;
			break;
		}
		return false;
	}

	public bool CanTaskBeFinished()
	{
		switch(TaskType)
		{
		case TaskTypeEnum.BringItem:
			Debug.Log("got to here: " + PlayerManager.Instance.Hero.GetItemAmount(TaskTarget, Tasklevel, PreffixTarget) + " : " + AmountToReach);
			if(PlayerManager.Instance.Hero.GetItemAmount(TaskTarget, Tasklevel, PreffixTarget) < AmountToReach)
			{
				Debug.Log(PlayerManager.Instance.Hero.GetItemAmount(TaskTarget, Tasklevel, PreffixTarget) + " : " + AmountToReach);
				return false;
			}
			break;
		}
		return true;
	}
}

public enum TaskTypeEnum
{
	BringItem = 0,
	KillEnemy = 1,
ReachPartOfConversation = 2,
	VisitArea = 3,
	WaitForAccept = 4
}