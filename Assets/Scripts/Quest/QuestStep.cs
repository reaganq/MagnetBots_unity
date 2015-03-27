using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

public class QuestStep {
	
	[XmlAttribute (AttributeName = "QLN")]
	public string QuestLogNote;
	[XmlAttribute (AttributeName = "BN")]
	public string BioNote;
	[XmlAttribute (AttributeName = "SN")]
	public int StepNumber;
	//used by colliection quests
	public bool isMainStep;
	public bool IsLastStep;
	public bool overrideNPC;
	public int overrideNPCID;
	public int overrideNPCConversationID;
	public List<Task> Tasks;
	
	public QuestStep()
	{
		Tasks = new List<Task>();
		QuestLogNote = string.Empty;
		BioNote = string.Empty;
	}

	public bool canFinishQuestStep()
	{
		foreach(Task task in Tasks)
		{
			if(!task.CanTaskBeFinished())
				return false;
		}
		return true;
	}
	
	public bool IsQuestStepFinished()
	{
		foreach(Task task in Tasks)
		{
			if (!task.IsTaskFinished())
			{
				return false;
			}
		}
		return true;
	}
	
	public bool IsEmpty
	{
		get
		{
			bool result = false;
			foreach(Task task in Tasks)
			{
				if (task.TaskTarget == 0)
					return true;
			}
			return result;
		}
	}

	public void TakeItemsFromPlayer()
	{
		foreach(Task task in Tasks)
		{
			if(task.TaskType == TaskTypeEnum.BringItem && task.CanTaskBeFinished())
			{
				InventoryItem item = new InventoryItem();
				if(task.PreffixTarget == PreffixType.ARMOR)
				{
					item.GenerateNewInventoryItem(Storage.LoadById<RPGArmor>(task.TaskTarget, new RPGArmor()), task.Tasklevel, task.AmountToReach - task.CurrentAmount);
					PlayerManager.Instance.Hero.RemoveItem(item);
					task.CurrentAmount += (task.AmountToReach - task.CurrentAmount);
				}
				else if(task.PreffixTarget == PreffixType.ITEM)
				{
					item.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(task.TaskTarget, new RPGItem()), task.Tasklevel, task.AmountToReach - task.CurrentAmount);
					task.CurrentAmount += (task.AmountToReach - task.CurrentAmount);
				}
			}
		}
	}

	public void GenerateRandomTasks(List<Task> possibleTasks, int numberofTasks, int maxAmount)
	{
		Tasks.Clear();
		List<int> taskIDs = new List<int>();
		for (int i = 0; i < numberofTasks; i++) {
			int id = UnityEngine.Random.Range(0, possibleTasks.Count);
			do
			{
				id = UnityEngine.Random.Range(0, possibleTasks.Count);
			}while(taskIDs.Contains(id));
			taskIDs.Add(id);
			Tasks.Add(possibleTasks[id]);
			Tasks[i].AmountToReach = UnityEngine.Random.Range(1, maxAmount);
			Tasks[i].CurrentAmount = 0;
		}
	}
}
