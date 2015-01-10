using UnityEngine;
using System.Collections;

public class ItemInfoBoxGUIController : BasicGUIController {

	public float buttonsGap;
	public GameObject[] allButtons;
	public Transform buttonsStartPos;
	public GameObject EquipButton = null;
	public GameObject DestroyButton = null;
	public UILabel nameLabel;
	public UILabel descriptionLabel;
	public UISprite icon;
	public BasicGUIController activeGUIController;

	public void DisplayItemDetails(RPGItem item, InventoryGUIType type, BasicGUIController gui)
	{
		Enable();
		activeGUIController = gui;
		nameLabel.text = item.Name;
		descriptionLabel.text = item.Description;
		GameObject atlas = Resources.Load(item.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.IconPath;
		for (int i = 0; i < allButtons.Length; i++) {
			allButtons[i].SetActive(false);
				}
		if(type == InventoryGUIType.quickInventory)
		{
			if(item.IsEquippable)
				EquipButton.SetActive(true);
		}
		else if(type == InventoryGUIType.Inventory)
		{
			if(item.IsEquippable)
				EquipButton.SetActive(true);
			DestroyButton.SetActive(true);
		}
		int count = 0;
		for (int i = 0; i < allButtons.Length; i++) {
			if(allButtons[i].activeSelf)
			{
				allButtons[i].transform.localPosition = new Vector3(buttonsStartPos.localPosition.x + count*buttonsGap, buttonsStartPos.localPosition.y, buttonsStartPos.localPosition.z);
				count++;
			}
		}
	}

	public void DisplayItemDetails(InventoryItem item, InventoryGUIType type, BasicGUIController gui)
	{
		DisplayItemDetails(item.rpgItem, type, gui);

	}

	public void HideItemInfoBox()
	{
		Disable();
	}

	public void OnEquipButtonPressed()
	{
		activeGUIController.ReceiveEquipButtonMessage();
	}

	public void OnUseButtonPressed()
	{
		activeGUIController.ReceiveUseButtonMessage();
	}

	public void OnDestroyButtonPressed()
	{
		activeGUIController.ReceiveDestroyButtonMessage();
	}

	public void OnDepositButtonPressed()
	{
		activeGUIController.ReceiveDepositButtonMessage();
	}

	public void OnWithdrawButtonPressed()
	{
		activeGUIController.ReceiveWithdrawButtonMessage();
	}

	public void OnBuyButtonPressed()
	{
		activeGUIController.ReceiveBuyButtonMessage();
	}

	public override void Enable()
	{
		base.Enable();
		//TweenScale.Begin(Root, 0.1f, Vector3.one);
	}

	public override void Disable()
	{
		base.Disable();
		//TweenScale.Begin(Root, 0.1f, Vector3.zero);
	}
}