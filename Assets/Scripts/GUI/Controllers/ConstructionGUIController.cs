using UnityEngine;
using System.Collections;

public class ConstructionGUIController : BasicGUIController {

	public UISlider progressBar;
	public Transform buildingTransform;
	public UILabel panelNameLabel;
	public Construction thisConstruction;

	public void Enable(Construction construct)
	{
		Enable();
		thisConstruction = construct;
	}

	public void OnDonateButtonPressed()
	{
		EnterDonateMode();
		Disable();
	}

	public void EnterDonateMode()
	{
		//move camera focus to building;
		//yield return new WaitForSeconds(1);
		Debug.Log("entering donate mode");
		PlayerCamera.Instance.TransitionTo(thisConstruction.targetCameraPos, PlayerCamera.Instance.defaultFOV, 0.3f, PlayerCamera.Instance.quickInventoryCameraRectOffset);
		GUIManager.Instance.DisplayQuickInventory(ItemCategories.Construction);
		thisConstruction.ShowConstructionItems();
	}

	public void OnExitButtonPressed()
	{
		Disable();
		GUIManager.Instance.EnterGUIState(UIState.main);
	}
}
