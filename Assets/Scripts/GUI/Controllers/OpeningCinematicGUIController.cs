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
	public UIGrid nakedArmorPanelGrid;

	public UIPlayTween subtitleTween;
	public UILabel subtitleLabel;

	public IntroCinematicSequence introCutscene;
	public GameObject setNameUIRoot;
	public UIPlayTween setNameUITween;
	public UIInput nameInput;

	public float tipWaitTime;
	public bool shouldDisplayTip = true;
	public GameObject tipObject;

	public float quickSingleInventoryCameraRectOffset;

	void Start()
	{
		quickSingleInventoryCameraRectOffset = Mathf.Ceil(((GameManager.Instance.defaultAspectRatio/GameManager.Instance.nativeAspectRatio)*-390/2048)*100) / 100;
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
		StartCoroutine(MoveCamera(introCutscene.interiorCamera, 0.3f, quickSingleInventoryCameraRectOffset));
		Debug.Log("load naked armor times!");
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
		nakedArmorPanelGrid.Reposition();
		Invoke("DisplayTip", tipWaitTime);
	}

	public IEnumerator MoveCamera(Camera targetCamera, float duration, float offset)
	{
		float startTime = Time.time;
		float timer = 0;
		Camera childCamera = targetCamera;
		float origX = childCamera.rect.x;
		while(Time.time < startTime + duration)
		{
			childCamera.rect = new Rect(Mathf.Lerp(origX, offset, timer), childCamera.rect.y, childCamera.rect.width, childCamera.rect.height);
			timer += Time.deltaTime/duration;
			yield return null;
		}
		childCamera.rect = new Rect(offset, childCamera.rect.y, childCamera.rect.width, childCamera.rect.height);
		yield return null;
	}

	public void HideNakedArmors()
	{
		StartCoroutine(MoveCamera(introCutscene.interiorCamera, 0.3f, 0));
		nakedArmorPanelTween.Play(false);
	}

	public override void OnDragDrop(int index)
	{
		PlayerManager.Instance.Hero.AddItem(NakedArmors[index]);
		PlayerManager.Instance.Hero.EquipItem(NakedArmors[index]);
		introCutscene.EquipNakedArmor(NakedArmors[index]);
		LoadItemTiles(NakedArmors, itemTiles, inventoryGridRoot, itemTilePrefab, inventoryType);
		HideTip();
	}

	public void DisplayTip()
	{
		if(shouldDisplayTip)
		{
			setNameUITween.tweenTarget = tipObject;
			setNameUITween.Play(true);
		}
	}

	public void HideTip()
	{
		if(shouldDisplayTip || tipObject.activeSelf)
		{
			shouldDisplayTip = false;
			setNameUITween.tweenTarget = tipObject;
			setNameUITween.Play(false);
		}
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
		setNameUITween.tweenTarget = setNameUIRoot;
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
