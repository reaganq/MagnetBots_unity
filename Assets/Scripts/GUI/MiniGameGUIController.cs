using UnityEngine;
using System.Collections;

public class MiniGameGUIController : BasicGUIController {

	public GameObject panel;
	public UILabel miniGameName;
	public UILabel description;
	public UISprite portrait;

	public override void Enable ()
	{
		miniGameName.text = PlayerManager.Instance.ActiveMinigame.Name;
		GameObject Atlas = Resources.Load(PlayerManager.Instance.ActiveMinigame.AtlasName) as GameObject;
		portrait.atlas = Atlas.GetComponent<UIAtlas>();
		portrait.spriteName = PlayerManager.Instance.ActiveMinigame.PortraitIcon;
		panel.SetActive(true);
		description.text = PlayerManager.Instance.ActiveMinigame.Description;
		base.Enable ();
	}

	public override void Disable ()
	{
		panel.SetActive(false);
		base.Disable ();
	}

	public void OnPlayButtonPressed()
	{
		PlayerManager.Instance.PlayMiniGame();
	}
}
