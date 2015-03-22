using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationGUIController : BasicGUIController {

	public RPGConversation activeConversation;
	public RPGParagraph activeParagraph;
	public string processedFullParagraphText;
	public UILabel speechText;
	public Camera speakerCamera;
	//public List<string> clampedParagraphTexts;
	public GameObject nextArrow;
	//quest box
	public RPGQuest _activeQuest;
	public RPGQuest activeQuest
	{
		get
		{
			return _activeQuest;
		}
		set
		{
			_activeQuest = value;
			questNameLabel.text = _activeQuest.Name;
		}
	}
	public QuestStep activeQuestStep;
	public GameObject questInfoBox;
	public GameObject questInfoPage;
	public GameObject confirmationPage;
	public GameObject rewardsPage;
	public UILabel questNameLabel;
	public UILabel questDescriptionLabel;
	public GameObject questItemsGrid;
	public UIGrid questItemsPanelGrid;
	public GameObject itemTilePrefab;
	public List<ItemTileButton> itemTiles;
	public List<InventoryItem> questItems = new List<InventoryItem>();
	public GameObject affirmativeButton;
	public GameObject negativeButton;
	public UILabel affirmativeText;
	public UILabel negativeText;
	public GameObject timerObject;
	public UILabel timerLabel;
	public Job displaySpeakerJob;
	public Job displayParagraphTimerJob;

	public bool waitForNextClick;
	public bool isSpeechBubbleDisplayed;
	public bool isSpeakerDisplayed;
	public bool isQuestStatusDisplayed;
	public LineText affirmativeLinetext;
	public LineText negativeLinetext;
	//public GameObject cameraTween;
	public GameObject speechBubble;
	public UIPlayTween speechBubbleIntroTween;
	public UIPlayTween speechBubbleOutroTween;
	public UIPlayTween speakerCameraTween;
	public GameObject speakerObject;
	public Transform speakerRoot;
	public UIPlayTween panelTween;

	public int activeNPCid;

	public void Start()
	{
	}

	public override void Enable()
	{
		base.Enable();
		speakerCamera.gameObject.SetActive(true);
	}

	public void DisplayConversation(RPGConversation newConversation)
	{
		//find the base paragraph
		if(!isDisplayed)
			Enable();

		activeConversation = newConversation;
		for (int i = 0; i < activeConversation.conversationParagraphs.Count; i++) {
			if(activeConversation.conversationParagraphs[i].isBaseParagraph)
			{
				if(activeConversation.conversationParagraphs[i].Validate())
				{
					DisplayParagraph(activeConversation.conversationParagraphs[i]);
					return;
				}
			}
		}
	}

	public void DisplayParagraphByID(int index)
	{
		Debug.Log("display paragraph: " + index);
		DisplayParagraph(activeConversation.conversationParagraphs[index]);
	}

	public void DisplayNextParagraph()
	{
		if(activeParagraph.nextParagraphIDs.Count > 0)
		{
			for (int i = 0; i < activeParagraph.nextParagraphIDs.Count; i++) {
				if(activeConversation.conversationParagraphs[activeParagraph.nextParagraphIDs[i]].Validate())
				{
					DisplayParagraph(activeConversation.conversationParagraphs[activeParagraph.nextParagraphIDs[i]]);
					return;
				}
			}
		}
	}

	public void DisplayParagraph(RPGParagraph newParagraph)
	{
		Debug.Log("display paragraph ");
		PlayerManager.Instance.Hero.questLog.CheckParagraph(activeConversation, activeConversation.conversationParagraphs.IndexOf(newParagraph));
		activeParagraph = newParagraph;
		affirmativeLinetext = null;
		negativeLinetext = null;
		for (int i = 0; i < activeParagraph.LineTexts.Count; i++) {
			if(activeParagraph.LineTexts[i].lineTextType == LineTextType.affirmative)
			{
				affirmativeLinetext = activeParagraph.LineTexts[i];
				Debug.Log(activeParagraph.LineTexts[i].CanYouDisplay());
				affirmativeButton.SetActive(activeParagraph.LineTexts[i].CanYouDisplay());
			}
			else if(activeParagraph.LineTexts[i].lineTextType == LineTextType.negative)
			{
				negativeLinetext = activeParagraph.LineTexts[i];
				negativeButton.SetActive(activeParagraph.LineTexts[i].CanYouDisplay());
			}
		}
		if(affirmativeLinetext == null)
			affirmativeButton.SetActive(false);
		else
		{
			affirmativeText.text = affirmativeLinetext.Text;
		}

		if(negativeLinetext == null)
		{
			negativeText.text = "Done";
		}
		else
		{
			negativeText.text = negativeLinetext.Text;
		}

		//negativeButton.SetActive(true);
		activeParagraph.DoEvents();
		if(!NeedsQuestInfo())
			HideQuestInfo();
		displaySpeakerJob = Job.make(DisplaySpeaker(newParagraph));
		if(activeParagraph.displayTimer > 0)
			displayParagraphTimerJob = Job.make(ParagraphTimeOut(activeParagraph.displayTimer));
		//StartCoroutine(DisplaySpeaker(newParagraph));
	}

	public IEnumerator ParagraphTimeOut(float waitDuration)
	{
		yield return new WaitForSeconds(waitDuration);
		DisplayNextParagraph();
	}

	public bool NeedsQuestInfo()
	{
		if(activeParagraph != null)
		{
			for (int i = 0; i < activeParagraph.Actions.Count; i++) {
				if(activeParagraph.Actions[i].ActionType == ActionEventType.DisplayQuestConfirmation || 
				   activeParagraph.Actions[i].ActionType == ActionEventType.DisplayQuestInfo ||
				   activeParagraph.Actions[i].ActionType == ActionEventType.DisplayQuestDetails ||
				   activeParagraph.Actions[i].ActionType == ActionEventType.DisplayQuestStatus ||
				   activeParagraph.Actions[i].ActionType == ActionEventType.GiveQuestRewards)
					return true;
			}
			return false;
		}
		else
			return false;
	}

	public IEnumerator DisplaySpeaker(RPGParagraph newParagraph)
	{
		if(isSpeakerDisplayed && newParagraph.ownerNPCID != activeNPCid)
		{
			if(isSpeechBubbleDisplayed)
			{
				//speechBubbleTween.tweenGroup = 0;
				speechBubbleOutroTween.Play(true);
				isSpeechBubbleDisplayed = false;
				yield return new WaitForSeconds(0.1f);
			}
			//transition out the current speaker
			speakerCameraTween.Play(false);
			isSpeakerDisplayed = false;
			yield return new WaitForSeconds(0.25f);
			Destroy(speakerObject);

		}
		activeNPCid = newParagraph.ownerNPCID;
		if(string.IsNullOrEmpty( activeParagraph.ParagraphText))
		{
			if(isSpeechBubbleDisplayed)
			{
				//speechBubbleTween.tweenGroup = 0;
				speechBubbleOutroTween.Play(true);
				isSpeechBubbleDisplayed = false;
				yield return new WaitForSeconds(0.1f);
			}
		}
		speechText.text = activeParagraph.ParagraphText;
		if(activeParagraph.nextParagraphCondition == NextParagraphInteraction.NextClick)
		{
			nextArrow.SetActive(true);
			waitForNextClick = true;
		}
		else
		{
			nextArrow.SetActive(false);
			waitForNextClick = false;
		}
		if(!isSpeakerDisplayed && newParagraph.ownerNPCID > 0)
		{
			speakerObject = Instantiate(Resources.Load(GeneralData.GetNPCByID(newParagraph.ownerNPCID).speakerPrefabPath) as GameObject) as GameObject;
			speakerObject.transform.parent = speakerRoot;
			speakerObject.transform.localPosition = Vector3.zero;
			speakerCameraTween.Play(true);
			yield return new WaitForSeconds(0.25f);
			isSpeakerDisplayed = true;
		}
		if(!isSpeechBubbleDisplayed && !string.IsNullOrEmpty(speechText.text))
		{
			//speechBubbleTween.tweenGroup = 1;
			speechBubbleIntroTween.Play(true);
			isSpeechBubbleDisplayed = true;
			yield return new WaitForSeconds(0.2f);
		}
		yield return null;
	}

	public IEnumerator HideSpeaker()
	{
		if(isSpeechBubbleDisplayed)
		{
			//speechBubbleTween.tweenGroup = 0;
			speechBubbleOutroTween.Play(true);
			isSpeechBubbleDisplayed = false;
			yield return new WaitForSeconds(0.25f);
		}
		if(isSpeakerDisplayed)
		{
			speakerCameraTween.Play(false);
			isSpeakerDisplayed = false;
			yield return new WaitForSeconds(0.25f);
			Destroy(speakerObject);
		}
		yield return null;
	}
	
	//display the first quest step of an unstarted quest
	public void DisplayQuestOutline(int questID)
	{
		activeQuest = GeneralData.GetQuestByID(questID);
		activeQuestStep = activeQuest.CurrentStep;
		DisplayQuestInfo(true);
	}

	public void DisplayQuestStatus(int questID)
	{
		activeQuest = PlayerManager.Instance.Hero.questLog.GetCurrentQuestByID(questID);
		if(activeQuest != null)
		{
			activeQuestStep = activeQuest.CurrentStep;
			DisplayQuestInfo(false);
		}
	}

	public void DisplayQuestInfo(bool isNewQuest)
	{
		confirmationPage.SetActive(false);
		questInfoPage.SetActive(true);
		rewardsPage.SetActive(false);
		questDescriptionLabel.text = activeQuestStep.QuestLogNote;
		if(activeQuest.questType == QuestType.collection)
		{
			questItemsGrid.SetActive(true);
			LoadQuestItemTiles(isNewQuest);
		}
		if(activeQuest.timed)
		{
			timerObject.SetActive(true);
			timerLabel.text = activeQuest.timeLimit.ToString() + "s";
		}
		else
			timerObject.SetActive(false);
		IntroQuestInfo();
	}

	public void DisplayQuestconfirmation()
	{
		confirmationPage.SetActive(true);
		questInfoPage.SetActive(false);
		rewardsPage.SetActive(false);
		questItemsGrid.SetActive(false);
		activeQuest = PlayerManager.Instance.Hero.questLog.LastAddedQuest();
		IntroQuestInfo();
	}

	public void DisplayQuestRewards(int questID)
	{

		Debug.Log("display quest rewards");
		confirmationPage.SetActive(false);
		questInfoPage.SetActive(false);
		rewardsPage.SetActive(true);
		IntroQuestInfo();
		activeQuest = PlayerManager.Instance.Hero.questLog.GetCurrentQuestByID(questID);
		DisplayQuestRewardItems(activeQuest.GiveQuestReward());
		//negativeButton.SetActive(true);
		//IntroQuestInfo();
	}

	public void LoadQuestItemTiles(bool isNewQuest)
	{
			questItems.Clear();
			for (int i = 0; i < activeQuestStep.Tasks.Count; i++) {
				if(activeQuestStep.Tasks[i].TaskType == TaskTypeEnum.BringItem)
				{
					InventoryItem item = new InventoryItem();
					if(activeQuestStep.Tasks[i].PreffixTarget == PreffixType.ARMOR)
					{
						item.GenerateNewInventoryItem(Storage.LoadById<RPGArmor>(activeQuestStep.Tasks[i].TaskTarget, new RPGArmor()), activeQuestStep.Tasks[i].Tasklevel, activeQuestStep.Tasks[i].AmountToReach);
					}
					else if(activeQuestStep.Tasks[i].PreffixTarget == PreffixType.ITEM)
					{
						item.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(activeQuestStep.Tasks[i].TaskTarget, new RPGItem()), activeQuestStep.Tasks[i].Tasklevel, activeQuestStep.Tasks[i].AmountToReach);
					}
					questItems.Add(item);
				}
			}
			int num = questItems.Count - itemTiles.Count;
			if(num>0)
			{
				for (int i = 0; i < num; i++) {
					GameObject itemTile = NGUITools.AddChild(questItemsGrid, itemTilePrefab);
					ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
					itemTiles.Add(tileButton);
					tileButton.index = itemTiles.Count-1;
				}
			}
			for (int i = 0; i < itemTiles.Count; i++) {
				if(i>=questItems.Count)
				{
					itemTiles[i].gameObject.SetActive(false);
				}
				else
				{
					itemTiles[i].gameObject.SetActive(true);
					itemTiles[i].LoadQuestDisplayTile(questItems[i], isNewQuest);
					//itemTiles[i].LoadItemTile(questItems[i], this, inventoryType, i);
				}
			}
		questItemsPanelGrid.Reposition();
	}

	public void DisplayQuestRewardItems(LootItemList loots)
	{
		questItemsGrid.SetActive(true);
		questItems.Clear();
		for (int i = 0; i < loots.items.Count; i++) {
			questItems.Add(loots.items[i]);
		}

		int num = questItems.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(questItemsGrid, itemTilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				itemTiles.Add(tileButton);
				tileButton.index = itemTiles.Count-1;
			}
		}
		for (int i = 0; i < itemTiles.Count; i++) {
			if(i>=questItems.Count)
			{

				itemTiles[i].gameObject.SetActive(false);
			}
			else
			{
				itemTiles[i].gameObject.SetActive(true);
				itemTiles[i].LoadItemTile(questItems[i], this, InventoryGUIType.Quest, i);
				//itemTiles[i].LoadItemTile(questItems[i], this, inventoryType, i);
			}
		}
		questItemsPanelGrid.Reposition();
	}

	public void HideQuestInfo()
	{
		if(!isQuestStatusDisplayed)
			return;
		OutroQuestInfo();
		isQuestStatusDisplayed = false;
	}

	public void IntroQuestInfo()
	{
		if(!isQuestStatusDisplayed)
		{
			panelTween.tweenTarget = questInfoBox;
			panelTween.Play(true);
			isQuestStatusDisplayed = true;
			questItemsPanelGrid.Reposition();
		}
    }

	public void OutroQuestInfo()
	{
		questInfoBox.SetActive(false);
	}

	public void OnSpeechBubblePressed()
	{
		if(displayParagraphTimerJob != null && displayParagraphTimerJob.running)
			displayParagraphTimerJob.kill();
		if(waitForNextClick)
			DisplayNextParagraph();
	}

	public void OnAffirmativePressed()
	{
		if(displayParagraphTimerJob != null && displayParagraphTimerJob.running)
			displayParagraphTimerJob.kill();
		if(affirmativeLinetext != null)
			affirmativeLinetext.DoEvents();
	}

	public void OnNegativePressed()
	{
		if(displayParagraphTimerJob != null && displayParagraphTimerJob.running)
			displayParagraphTimerJob.kill();
		if(negativeLinetext != null)
			negativeLinetext.DoEvents();
		else
		{
			if(activeParagraph.nextParagraphCondition == NextParagraphInteraction.WaitForUIAction)
				DisplayNextParagraph();
		}
	}
	
	public void EndConversation()
	{
		StartCoroutine(EndTalk());
	}

	public IEnumerator EndTalk()
	{
		HideQuestInfo();
		yield return StartCoroutine(HideSpeaker());
		Disable();
	}

	public override void Disable ()
	{
		speakerCamera.gameObject.SetActive(false);
		if(displayParagraphTimerJob != null && displayParagraphTimerJob.running)
			displayParagraphTimerJob.kill();
		//if(displaySpeakerJob != null && displaySpeakerJob.running)
		//	displaySpeakerJob.kill();
		base.Disable ();
		GUIManager.Instance.HideNPC();
	}
}
