using UnityEngine;
using System.Collections;

public class MiniGameGUIController : BasicGUIController {

	public GameObject panel;
	public UILabel miniGameName;
	public UILabel description;
	public UISprite portrait;
	public NPCMinigame minigame;
	public UILabel myScoreLabel;
	public GameObject detailsBox;
	public GameObject instructions;
	public GameObject highScores;
	public bool isDetailsDisplayed;
	public Color inactiveColor;
	public Color activeColor;
	public UISprite highscoreBG;
	public UISprite instructionsBG;

	public void Enable (NPCMinigame mg)
	{
		minigame = mg;
		miniGameName.text = minigame.Name;
		GameObject Atlas = Resources.Load(minigame.AtlasName) as GameObject;
		portrait.atlas = Atlas.GetComponent<UIAtlas>();
		portrait.spriteName = minigame.IconPath;
		panel.SetActive(true);
		description.text = minigame.Description;
		OnInstructionsPressed();
		Enable();
	}

	public override void Disable (bool resetState)
	{
		panel.SetActive(false);
		base.Disable(resetState);
	}

	public void OnPlayButtonPressed()
	{
		PlayerManager.Instance.PlayMiniGame(minigame);
	}

	public void OnHighScoresPressed()
	{
		instructions.SetActive(false);
		highScores.SetActive(true);
		instructionsBG.color = inactiveColor;
		highscoreBG.color = activeColor;
	}

	public void OnInstructionsPressed()
	{
		instructions.SetActive(true);
		highScores.SetActive(false);
		instructionsBG.color = activeColor;
		highscoreBG.color = inactiveColor;
	}

	public void OnExitButtonPressed()
	{
		GUIManager.Instance.NPCGUI.HideShop();
	}
}
