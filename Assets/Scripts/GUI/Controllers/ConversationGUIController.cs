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
	public GameObject questInfoBox;
	public UILabel questNameLabel;
	public Transform questItemsGrid;
	public GameObject affirmativeButton;
	public GameObject negativeButton;
	public UILabel affirmativeText;
	public UILabel negativeText;

	public bool waitForNextClick;
	public bool isSpeechBubbleDisplayed;
	public bool isSpeakerDisplayed;
	public bool isQuestStatusDisplayed;
	public LineText affirmativeLinetext;
	public LineText negativeLinetext;
	public GameObject cameraTween;
	public GameObject speechBubble;
	public UIPlayTween speechBubbleTween;
	public UIPlayTween speakerTween;
	public GameObject speakerObject;
	public Transform speakerRoot;

	public int activeNPCid;

	public void Start()
	{
		//reposition camera and speechbubble;
	}

	public override void Enable()
	{
		base.Enable();
		speakerCamera.gameObject.SetActive(true);
	}

	public void DisplayConversation(RPGConversation newConversation)
	{
		//find the base paragraph
		activeConversation = newConversation;
		for (int i = 0; i < activeConversation.conversationParagraphs.Count; i++) {
			if(activeConversation.conversationParagraphs[i].isBaseParagraph)
			{
				if(activeConversation.conversationParagraphs[i].Validate())
				{
					DisplayParagraph(activeConversation.conversationParagraphs[i]);
				}
			}
		}
	}

	public void DisplayParagraphByID(int index)
	{
		StartCoroutine(DisplayParagraph(activeConversation.conversationParagraphs[index]));
	}

	public IEnumerator DisplayParagraph(RPGParagraph newParagraph)
	{
		if(newParagraph.ownerNPCID > 0)
		{
			yield return StartCoroutine(DisplaySpeaker(newParagraph));
		}
		activeParagraph = newParagraph;
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
		activeParagraph.DoEvents();
		yield return null;
	}

	public IEnumerator DisplaySpeaker(RPGParagraph newParagraph)
	{
		if(isSpeakerDisplayed && newParagraph.ownerNPCID != activeNPCid)
		{
			if(isSpeechBubbleDisplayed)
			{
				speechBubbleTween.Play(false);
				isSpeechBubbleDisplayed = false;
				yield return new WaitForSeconds(0.1f);
			}
			//transition out the current speaker
			speakerTween.Play(false);
			isSpeakerDisplayed = false;
			yield return new WaitForSeconds(0.1f);
			Destroy(speakerObject);

		}
		if(!isSpeakerDisplayed)
		{
			speakerObject = Instantiate(Resources.Load(GeneralData.NPCs[newParagraph.ownerNPCID].speakerPrefabPath) as GameObject) as GameObject;
			speakerObject.transform.parent = speakerRoot;
			speakerObject.transform.localPosition = Vector3.zero;
			speakerObject.transform.localRotation = Quaternion.identity;
			speakerTween.Play(true);
			yield return new WaitForSeconds(0.1f);
			isSpeakerDisplayed = true;
		}
		if(!isSpeechBubbleDisplayed)
		{
			speechBubbleTween.Play(true);
			yield return new WaitForSeconds(0.1f);
			isSpeechBubbleDisplayed = true;
		}
		yield return null;
	}

	public void HideSpeaker()
	{
		isSpeakerDisplayed = false;
	}

	public void OnSpeechBubblePressed()
	{
		if(waitForNextClick)
			DisplayNextParagraph();
	}

	public void DisplayNextParagraph()
	{
		if(activeParagraph.nextParagraphIDs.Count > 0)
		{
			for (int i = 0; i < activeParagraph.nextParagraphIDs.Count; i++) {
				if(activeConversation.conversationParagraphs[activeParagraph.nextParagraphIDs[i]].Validate())
					StartCoroutine( DisplayParagraph(activeConversation.conversationParagraphs[activeParagraph.nextParagraphIDs[i]]));
				return;
			}
		}
	}

	public void DisplayQuestOutline()
	{
		for (int i = 0; i < activeParagraph.LineTexts.Count; i++) {
			if(activeParagraph.LineTexts[i].lineTextType == LineTextType.affirmative)
				affirmativeLinetext = activeParagraph.LineTexts[i];
			else if(activeParagraph.LineTexts[i].lineTextType == LineTextType.negative)
				negativeLinetext = activeParagraph.LineTexts[i];
		}
	}

	public void DisplayQuestStatus()
	{
	}

	public void EndConversation()
	{
		StartCoroutine(EndTalk());
	}

	public IEnumerator EndTalk()
	{
		if(isSpeechBubbleDisplayed)
		{
			speechBubbleTween.Play(false);
			isSpeechBubbleDisplayed = false;
			yield return new WaitForSeconds(0.1f);
		}
		if(isSpeakerDisplayed)
		{
			speakerTween.Play(false);
			isSpeakerDisplayed = false;
			yield return new WaitForSeconds(0.1f);
			Destroy(speakerObject);
		}
		Disable();
	}

	public void OnAffirmativePressed()
	{
	}

	public void OnNegativePressed()
	{
	}

	public override void Disable ()
	{
		base.Disable ();
	}
}
