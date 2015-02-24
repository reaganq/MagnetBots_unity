using UnityEngine;
using System.Collections;

public class MiniGameGUIController : BasicGUIController {

	public GameObject panel;
	public UILabel miniGameName;
	public UILabel description;
	public UISprite portrait;
	public NPCMinigame minigame;

	public void Enable (NPCMinigame mg)
	{
		minigame = mg;
		miniGameName.text = minigame.Name;
		GameObject Atlas = Resources.Load(minigame.AtlasName) as GameObject;
		portrait.atlas = Atlas.GetComponent<UIAtlas>();
		portrait.spriteName = minigame.IconPath;
		panel.SetActive(true);
		description.text = minigame.Description;
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
}
