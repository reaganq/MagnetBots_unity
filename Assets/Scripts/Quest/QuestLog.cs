using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestLog 
{
	//repeatable quests
	//finished quests
	public List<RPGQuest> FinishedQuests;
	public List<RPGQuest> CurrentQuests;

	//ignore
	public RPGQuest selectedQuest;
	
	public QuestLog()
	{
		CurrentQuests = new List<RPGQuest>();
		FinishedQuests = new List<RPGQuest>();
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
	

	// Returns true is quest is finished but not get reward from it (wasn't ended)
	public bool IsQuestFinished(int questId)
	{
		foreach(RPGQuest q in CurrentQuests)
		{
			if (q.ID == questId && q.IsFinished)
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
		if (IsQuestStarted(questId))
			return false;

		bool result = false;	
		
		foreach(RPGQuest q in GeneralData.quests)
		{
			if (q.ID == questId)
			{
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
		if (!result)
			return false;
		quest.GiveReward();
		if(!quest.Repeatable)
			FinishedQuests.Add(quest);
		
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
	
	// Check paragraph if it is task of current quest
	public void CheckParagraph(int paragraphId)
	{
		foreach(RPGQuest q in CurrentQuests)
			q.CheckParagraph(paragraphId);
		
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


	public void UpdateQuests()
	{
		
	}
}


