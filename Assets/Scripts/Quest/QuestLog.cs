using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		Debug.Log("checking for npc conversation " + CurrentQuests.Count + " npcid: "+npcID);
		for (int i = 0; i < CurrentQuests.Count; i++) {
			Debug.Log(CurrentQuests[i].CurrentStep.overrideNPCConversationID);
			if(CurrentQuests[i].CurrentStep.overrideNPC && CurrentQuests[i].CurrentStep.overrideNPCID == npcID)
			{
				Debug.Log("found an override quest!");
				return CurrentQuests[i];
			}
				}

		 return null;
	}
	
	// Check paragraph if it is task of current quest
	public void CheckParagraph(RPGConversation conversation, int paragraphId)
	{
		Debug.LogWarning("checking paragrap");
		foreach(RPGQuest q in CurrentQuests)
			q.CheckParagraph(conversation.ID, paragraphId);
		
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
	}


	public void UpdateQuests()
	{
		
	}
}

public class ParseQuestLogData
{
	public List<int> finishedQuestIDs;
	public List<int> currentQuestIDs;
}


