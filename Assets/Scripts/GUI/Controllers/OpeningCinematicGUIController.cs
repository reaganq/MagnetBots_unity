using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpeningCinematicGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject inventoryGridRoot;
	public int[] NakedArmorIDs = new int[] {1,2,3,4,5};
	public List<InventoryItem> NakedArmors = new List<InventoryItem>();
	public List<ItemTileButton> itemTiles;
	public InventoryGUIType inventoryType = InventoryGUIType.Other;
	//HACK
	public UIPlayTween nakedArmorPanelTween;
	public GameObject dummyConfirmationButton;

	public UIPlayTween subtitleTween;
	public UILabel subtitleLabel;

	public IntroCinematicSequence introCutscene;

	public GameObject setNameUIRoot;
	public UIPlayTween setNameUITween;
	public UIInput nameInput;

	public override void Enable()
	{
		base.Enable();

	}

	public void DisplaySubtitle(string text)
	{
		subtitleTween.Play(true);
		subtitleLabel.text = text;
	}

	public void UpdateSubtitle(string text)
	{
		subtitleLabel.text = text;
	}

	public void HideSubtitle()
	{
		subtitleTween.Play(false);
	}

	public void LoadNakedArmors()
	{
		nakedArmorPanelTween.Play(true);
		for (int i = 0; i < NakedArmorIDs.Length; i++) {
			//load in naked armors
			RPGArmor nakedArmor = Storage.LoadById<RPGArmor>(NakedArmorIDs[i], new RPGArmor());
			InventoryItem newItem = new InventoryItem();
			newItem.GenerateNewInventoryItem(nakedArmor, 1, 1);
			NakedArmors.Add(newItem);
			//newItem.GenerateNewInventoryItem(
		}
		LoadItemTiles(NakedArmors, itemTiles, inventoryGridRoot, itemTilePrefab, inventoryType);
	}

	public void HideNakedArmors()
	{
		nakedArmorPanelTween.Play(false);
	}

	public override void OnDragDrop(int index)
	{
		PlayerManager.Instance.Hero.AddItem(NakedArmors[index]);
		introCutscene.EquipNakedArmor(NakedArmors[index]);
	}

	public void DisplayAvatarConfirmation()
	{
		dummyConfirmationButton.SetActive(true);
	}
	
	public void OnDummyConfirmPressed()
	{
		introCutscene.ProceedNextStep();
		dummyConfirmationButton.SetActive(false);
	}
	
	public void EnableSetNameUI()
	{
		setNameUITween.Play(true);
	}

	public void OnConfirmNamePressed()
	{
		PlayerManager.Instance.Hero.SetPlayerName(nameInput.value);
		introCutscene.ProceedNextStep();
		setNameUIRoot.SetActive(false);
		//playermanager set name
	}

}
