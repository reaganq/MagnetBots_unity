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
	public bool isMainStep;
	public bool IsLastStep;
	public List<Task> Tasks;
	
	public QuestStep()
	{
		Tasks = new List<Task>();
		QuestLogNote = string.Empty;
		BioNote = string.Empty;
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
			if(id < possibleTasks.Count)
			{
				Tasks.Add(possibleTasks[id]);
				Tasks[i].AmountToReach = UnityEngine.Random.Range(1, maxAmount);
			}
		}
	}
}
