using UnityEngine;
using System.Collections;

public class ConstructionGUIController : BasicGUIController {

	public UIProgressBar progressBar;
	public UILabel percentageLabel;
	public Transform buildingTransform;
	public UILabel panelNameLabel;
	public Construction thisConstruction;
	public GameObject[] materialObjects;
	public UISprite[] materialSprites;
	public UILabel[] materialCounters;
	public UILabel topDonorLabel;

	public void Enable(Construction construct)
	{
		Enable();
		thisConstruction = construct;
		panelNameLabel.text = thisConstruction.construction.Name.ToString();

		UpdateMaterialsCounters();
		UpdateTopDonor();
	}

	public void UpdateTopDonor()
	{
		if(thisConstruction.donors.Count > 0)
		{
			DonorsObject topDonor = thisConstruction.TopDonor();
			topDonorLabel.text = "Top donor: " + topDonor.playerName + " - " + topDonor.donationsQuantity;
		}
		else
			topDonorLabel.text = "Top donor: ";
	}

	public void UpdateMaterialsCounters()
	{
		if(!isDisplayed)
			return;

		int numberLeft = 0;
		for (int i = 0; i < thisConstruction.requiredItems.Count; i++) {
			if(i < materialObjects.Length)
			{
				materialObjects[i].SetActive(true);
				GameObject atlas = Resources.Load(thisConstruction.requiredItems[i].rpgItem.AtlasName) as GameObject;
				materialSprites[i].atlas = atlas.GetComponent<UIAtlas>();
				materialSprites[i].spriteName = thisConstruction.requiredItems[i].rpgItem.IconPath;
				materialCounters[i].text = thisConstruction.requiredItems[i].CurrentAmount.ToString();
			}
			numberLeft += thisConstruction.requiredItems[i].CurrentAmount;
		}
		
		for (int i = thisConstruction.requiredItems.Count; i < materialObjects.Length; i++) {
			materialObjects[i].SetActive(false);
		}
		
		int totalNum = 0;
		for (int i = 0; i < thisConstruction.requiredItemsQuantity.Count; i++) {
			totalNum += thisConstruction.requiredItemsQuantity[i];
		}
		Debug.Log("left: " + numberLeft + " total: " + totalNum);
		//Debug.Log((float)numberLeft/(float)totalNum);
		progressBar.value = 1.0f - ((float)numberLeft/(float)totalNum);
		percentageLabel.text = (progressBar.value*100).ToString("F0") + "%";
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
