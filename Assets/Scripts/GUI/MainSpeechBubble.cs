using UnityEngine;
using System.Collections;

public class MainSpeechBubble : BasicGUIController {

	public UILabel speechBubbleText;

	public void DisplaySpeechBubble(string text)
	{
		speechBubbleText.text = text;
		Enable();
	}

	public void HideSpeechBubble()
	{
		Disable();
	}
}
