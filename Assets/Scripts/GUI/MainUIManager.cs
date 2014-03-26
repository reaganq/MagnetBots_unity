using UnityEngine;
using System.Collections;

public class MainUIManager : BasicGUIController {

	public UILabel MagnetsCounter = null;
	public GameObject OpenInventoryButton = null;
	public GameObject ActionButtons;
	public GameObject Root;

	public void UpdateMagnetsCount()
	{
		MagnetsCounter.text = PlayerManager.Instance.Hero.Magnets.ToString();
	}

	public override void Enable()
	{
		Root.SetActive(true);

		if(GameManager.Instance.inputType == InputType.TouchInput)
		{
			GameManager.Instance.joystick.enable = true;
		}
		else
			ActionButtons.SetActive(false);
		if(!GUIManager.Instance.CanShowInventory)
			OpenInventoryButton.SetActive(false);
		UpdateMagnetsCount();
	}


	public override void Disable()
	{
		Root.SetActive(false);
	}
}

public class PartyMemberCard
{
	public UILabel name;
}
