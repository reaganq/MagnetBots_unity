using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Parse;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

public class QuestLog 
{
	//repeatable quests
	//finished quests
	public List<RPGQuest> FinishedQuests;
	public List<RPGQuest> CurrentQuests;
	public int questPoints;

	//ignore
	public RPGQuest selectedQuest;
	
	public QuestLog()
	{
		CurrentQuests = new List<RPGQuest>();
		FinishedQuests = new List<RPGQuest>();
	}

	public void AddQuestPoints(int amount)
	{
		questPoints += amount;
	}

	public RPGQuest LastAddedQuest()
	{
		if(CurrentQuests.Count < 1)
			return null;
		Debug.Log("found quest");
		return CurrentQuests[CurrentQuests.Count -1];
	}

	public RPGQuest GetCurrentQuestByID(int questID)
	{
		for (int i = 0; i < CurrentQuests.Count; i++) {
			if(CurrentQuests[i].ID == questID)
			{
				return CurrentQuests[i];
			}
				}
		return null;
	}

	//quest step in progress
	public bool IsQuestStepInProgress(int questId, int questStepNumber)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsCurrentStep(questStepNumber))
				return true;
		}
		return false;
	}

	public bool CanFinishQuest(int questID)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if(q.ID == questID && q.CanFinish)
				return true;
		}
		return false;
	}

	// Returns true is quest is finished but not get reward from it (wasn't ended)
	public bool IsQuestFinished(int questId)
	{
		foreach(RPGQuest q in FinishedQuests)
		{
			if (q.ID == questId)
				return true;
		}
		return false;
	}
	
	// Is quest is completed (rewarded)
	public bool IsQuestCompleted(int questId)
	{
		for (int i = 0; i < FinishedQuests.Count; i++) {
			if(FinishedQuests[i].ID == questId)
				return true;
				}
		return false;
	}
	
	public bool IsQuestInProgress(int questId)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsFinished == false)
				return true;
		}
		return false;
	}
	
	// Is quest in quest log (started or completed)
	public bool IsQuestStarted(int questId)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
				return true;
		}
		return false;
	}
	
	// Start new quest
	public bool StartQuest(int questId)
	{
		Debug.Log("start quest" + questId);
		if (IsQuestStarted(questId))
			return false;

		bool result = false;	
		
		foreach(RPGQuest q in GeneralData.quests)
		{
			if (q.ID == questId)
			{
				Debug.Log("start time: "+q.startTime);
				q.startTime = Time.realtimeSinceStartup;
				CurrentQuests.Add(q);
				result = true;
			}
		}
		UpdateQuests();
		return result;
	}
	
	// End quest in quest log
	public bool EndQuest(int questId)
	{
		RPGQuest quest = new RPGQuest();
		
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				quest = q;
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		Debug.Log("finished quest: " + result);
		if (!result)
			return false;
		//quest.GiveReward();
		if(!quest.repeatable)
			FinishedQuests.Add(quest);
		else
			GeneralData.ReRollQuest(questId);
		UpdateQuests();
		/*if (!quest.Repeatable)
		{
			CompletedQuest cq = new CompletedQuest();
			cq.Name = quest.Name;
			cq.ID = quest.ID;
			cq.GuildID = quest.GuildID;
			cq.GuildRankID = quest.GuildRank;
			cq.Description = quest.FinalQuestLog;
			CompletedQuests.Add(cq);
		}*/
		return result;
	}
	
	public bool FailQuest(int questId)
	{
		RPGQuest quest = new RPGQuest();
		
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				quest = q;
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		if (!result)
			return false;
		
		/*if (!quest.Repeatable)
		{
			CompletedQuest cq = new CompletedQuest();
			cq.Name = quest.Name;
			cq.ID = quest.ID;
			cq.Failed = true;
			cq.GuildID = quest.GuildID;
			cq.GuildRankID = quest.GuildRank;
			cq.Description = quest.FinalQuestLog;
			CompletedQuests.Add(cq);
		}*/
		return result;
	}
	
	// Abandond quest
	public bool AbandonQuest(int questId)
	{
		bool result = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId)
			{
				CurrentQuests.Remove(q);
				result = true;
				break;
			}
		}
		return result;
	}

	public RPGQuest CheckNPCConversation(int npcID)
	{
		//Debug.Log("checking for npc conversation " + CurrentQuests.Count + " npcid: "+npcID);
		for (int i = 0; i < CurrentQuests.Count; i++) {
			//Debug.Log(CurrentQuests[i].CurrentStep.overrideNPCConversationID);
			if(CurrentQuests[i].CurrentStep.overrideNPC && CurrentQuests[i].CurrentStep.overrideNPCID == npcID)
			{
				//Debug.Log("found an override quest!");
				return CurrentQuests[i];
			}
				}

		 return null;
	}
	
	// Check paragraph if it is task of current quest
	public void CheckParagraph(RPGConversation conversation, int paragraphId)
	{
		bool validChange = false;
		foreach(RPGQuest q in CurrentQuests)
		{
			if(q.CheckParagraph(conversation.ID, paragraphId))
				validChange = true;
		}

		if(validChange)
			UpdateQuests();
	}
	
	// Check line text if it is task of current quest
	public void CheckLineText(int lineTextId)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckLineText(lineTextId);
		
		UpdateQuests();
	}
	
	// Check if killed enemy is task of current quest
	public void KillEnemy(int enemyID)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckKilledEnemy(enemyID);
		
		UpdateQuests();
	}

	public void VisitArea(int worldObjectID)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.VisitArea(worldObjectID);
		
		UpdateQuests();
	}
	
	public void CheckInventoryItem()
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckInventory();
		
		UpdateQuests();
	}

	public void TakeItemsFromPlayer(int questID)
	{
		for (int i = 0; i < CurrentQuests.Count; i++) {
			if(CurrentQuests[i].ID == questID)
			{
				CurrentQuests[i].CurrentStep.TakeItemsFromPlayer();
			}
		}
		UpdateQuests();
	}


	public void UpdateQuests()
	{
		PlayerManager.Instance.Hero.hasQuestChange = true;
	}

	public ParseQuestLogData ParseThisQuestLog()
	{
		Debug.LogWarning("saving questlog to parse");
		ParseQuestLogData questData = new ParseQuestLogData();
		for (int i = 0; i < CurrentQuests.Count; i++) {
			//dont save repeatable quests
			if(CurrentQuests[i].repeatable)
			{
				Debug.Log("ignoring a repeatable quest");
				continue;
			}
			ParseQuestData newQuest = new ParseQuestData();
			newQuest.questID = CurrentQuests[i].ID;
			Debug.Log("adding a new quest: " + CurrentQuests[i].ID);
			for (int j = 0; j < CurrentQuests[i].questSteps.Count; j++) {
				ParseQuestStepData newStep = new ParseQuestStepData();
				Debug.Log("adding a new queststep :" + j + CurrentQuests[i].questSteps[j].QuestLogNote);
				for (int k = 0; k < CurrentQuests[i].questSteps[j].Tasks.Count; k++) {
					newStep.taskCurrentAmounts.Add(CurrentQuests[i].questSteps[j].Tasks[k].CurrentAmount);
					Debug.Log(CurrentQuests[i].questSteps[j].Tasks[i].TaskType.ToString() + " current amount: " + newStep.taskCurrentAmounts[k]);
				}
				newQuest.questSteps.Add(newStep);
			}
			questData.currentQuests.Add(newQuest);
		}
		for (int i = 0; i < FinishedQuests.Count; i++) {
			ParseQuestData newQuest = new ParseQuestData();
			newQuest.questID = FinishedQuests[i].ID;
			Debug.Log("adding a finished quest: " +FinishedQuests[i].ID);
			for (int j = 0; j < FinishedQuests[i].questSteps.Count; j++) {
				ParseQuestStepData newStep = new ParseQuestStepData();
				Debug.Log("adding a new queststep :" + j + FinishedQuests[i].questSteps[j].QuestLogNote);
				for (int k = 0; k < FinishedQuests[i].questSteps[j].Tasks.Count; k++) {
					newStep.taskCurrentAmounts.Add(FinishedQuests[i].questSteps[j].Tasks[k].CurrentAmount);
					Debug.Log(FinishedQuests[i].questSteps[j].Tasks[k].TaskType.ToString() + " current amount: " + newStep.taskCurrentAmounts[k]);
				}
				newQuest.questSteps.Add(newStep);
			}
			questData.finishedQuests.Add(newQuest);
		}
		Debug.LogWarning("finished converting questlog to parse");
		return questData;
	}

	public void InterpretParseQuestLog(ParseQuestLogData questLogData)
	{
		Debug.LogWarning("unpacking parse quest log data");
		for (int i = 0; i < questLogData.currentQuests.Count; i++) {
			RPGQuest newQuest = Storage.LoadById<RPGQuest>(questLogData.currentQuests[i].questID, new RPGQuest());
			Debug.Log("added new current quest: " + newQuest.ID);
			for (int j = 0; j < newQuest.questSteps.Count; j++) {
				for (int k = 0; k < newQuest.questSteps[j].Tasks.Count; k++) {
					newQuest.questSteps[j].Tasks[k].CurrentAmount = questLogData.currentQuests[i].questSteps[j].taskCurrentAmounts[k];
					Debug.Log("updating quest step: " + j + " task: " + k + "current amount: " + questLogData.currentQuests[i].questSteps[j].taskCurrentAmounts[k]);
				}
			}
			CurrentQuests.Add(newQuest);
		}
		for (int i = 0; i < questLogData.finishedQuests.Count; i++) {
			RPGQuest newQuest = Storage.LoadById<RPGQuest>(questLogData.finishedQuests[i].questID, new RPGQuest());
			Debug.Log("added new finished quest: " + newQuest.ID);
			for (int j = 0; j < newQuest.questSteps.Count; j++) {
				for (int k = 0; k < newQuest.questSteps[j].Tasks.Count; k++) {
					newQuest.questSteps[j].Tasks[k].CurrentAmount = questLogData.finishedQuests[i].questSteps[j].taskCurrentAmounts[k];
					Debug.Log("updating quest step: " + j + " task: " + k + "current amount: " + questLogData.finishedQuests[i].questSteps[j].taskCurrentAmounts[k]);
				}
			}
			Debug.LogWarning("ufinished npacking parse quest log data");
			FinishedQuests.Add(newQuest);
		}
	}
}

[Serializable]
public class ParseQuestLogData
{
	public List<ParseQuestData> finishedQuests;
	public List<ParseQuestData> currentQuests;

	public ParseQuestLogData()
	{
		finishedQuests = new List<ParseQuestData>();
		currentQuests = new List<ParseQuestData>();
	}
}

[Serializable]
public class ParseQuestData
{
	public int questID;
	public List<ParseQuestStepData> questSteps;

	public ParseQuestData()
	{
		questSteps = new List<ParseQuestStepData>();
	}
}

[Serializable]
public class ParseQuestStepData
{
	public List<int> taskCurrentAmounts;
	public ParseQuestStepData()
	{
		taskCurrentAmounts = new List<int>();
	}
}


