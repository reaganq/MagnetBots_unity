using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestGUIController : BasicGUIController {

	public GameObject questBookPrefab;
	public GameObject questBookPanel;
	public UIGrid questBookGrid;
	public GameObject questPageObject;
	public GameObject leftArrow;
	public GameObject rightArrow;
	public List<QuestBook> questBooks;
	public List<RPGQuest> selectedQuests;
	public RPGQuest selectedQuest;
	public int questStepId;
	public QuestPage questPage;
	public UISprite currentQuestButtonSprite;
	public UISprite finishedQuestButtonSprite;
	public string unselectedCategoryButtonSprite;
	public string selectedCategoryButtonSprite;
	public bool isPageMode;
	public GameObject backButton;

	public override void Enable ()
	{
		SelectCurrentQuests();
		base.Enable ();
	}

	public void SelectCurrentQuests()
	{
		selectedQuests = PlayerManager.Instance.Hero.questLog.CurrentQuests;
		currentQuestButtonSprite.spriteName = selectedCategoryButtonSprite;
		finishedQuestButtonSprite.spriteName = unselectedCategoryButtonSprite;
		RefreshQuests();
	}

	public void SelectFinishedQuests()
	{
		selectedQuests = PlayerManager.Instance.Hero.questLog.FinishedQuests;
		currentQuestButtonSprite.spriteName = unselectedCategoryButtonSprite;
		finishedQuestButtonSprite.spriteName = selectedCategoryButtonSprite;
		RefreshQuests();
	}

	public void RefreshQuests()
	{
		isPageMode = false;
		backButton.SetActive(false);
		questPageObject.SetActive(false);
		questBookPanel.SetActive(true);
		int num = selectedQuests.Count - questBooks.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject questTile = NGUITools.AddChild(questBookGrid.gameObject, questBookPrefab);
				QuestBook questBook = questTile.GetComponent<QuestBook>();
				questBook.questGUI = this;
				questBooks.Add(questBook);
			}
		}
		for (int i = 0; i < questBooks.Count; i++) {
			if(i >= selectedQuests.Count)
			{
				questBooks[i].gameObject.SetActive(false);
			}
			else
			{
				questBooks[i].gameObject.SetActive(true);
				questBooks[i].LoadQuest(selectedQuests[i], i);
			}
		}
		questBookGrid.Reposition();
	}

	public void QuestBookPressed(int index)
	{
		selectedQuest = selectedQuests[index];
		questStepId = 0;

		LoadQuestPage();
	}

	public void LoadQuestPage()
	{
		backButton.SetActive(true);
		isPageMode = true;
		questPageObject.SetActive(true);
		questBookPanel.SetActive(false);
		if(questStepId > 0)
			leftArrow.SetActive(true);
		else
			leftArrow.SetActive(false);
		
		if(selectedQuest.questSteps.Count > questStepId + 1)
			rightArrow.SetActive(true);
		else
			rightArrow.SetActive(false);
		questPage.LoadQuestStep(selectedQuest.questSteps[questStepId], selectedQuest.questSteps.Count, questStepId + 1);
	}

	public void IncreaseQuestStep()
	{
		if(questStepId + 1 < selectedQuest.questSteps.Count)
			questStepId ++;
		LoadQuestPage();
	}

	public void DecreaseQuestSTep()
	{
		if(questStepId > 0)
			questStepId --;
		LoadQuestPage();
	}

	public void OnBackButtonPressed()
	{
		RefreshQuests();
	}

	public void OnExitButtonPressed()
	{
		GUIManager.Instance.HideQuest();
	}
}
