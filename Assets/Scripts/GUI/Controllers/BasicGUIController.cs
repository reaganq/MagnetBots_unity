using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicGUIController : MonoBehaviour {

	public UIPlayTween tween;
	public UIPlayTween endTween;
	public GameObject Root;
	public bool isDisplayed;
	public bool autoPlay;

	public virtual void Enable()
	{
		isDisplayed = true;
		if(tween != null && autoPlay)
		{
			tween.Play(isDisplayed);
		}
		else
		{
			if(Root != null)
				Root.SetActive(true);
		}
	}

	public virtual void Hide()
	{
		isDisplayed = false;
		if(endTween != null)
		{
			endTween.Play(true);
		}
		else
		{
			if(tween != null && autoPlay)
			{
				tween.Play(isDisplayed);
			}
			else
			{
				if(Root != null)
					Root.SetActive(false);
			}
		}
	}

	public virtual void Disable()
	{
		Disable(false);
	}

	public virtual void Disable(bool resetState)
	{
		if(resetState)
			Reset();
		Hide();
	}

	public virtual void Reset()
	{
	}

	public virtual void LoadItemTiles(List<InventoryItem> itemList, List<ItemTileButton> tiles, GameObject gridPanelRoot, GameObject tilePrefab, InventoryGUIType inventType)
	{
		int num = itemList.Count - tiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(gridPanelRoot, tilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				tiles.Add(tileButton);
			}
		}
		for (int i = 0; i < tiles.Count; i++) {
			if(i>=itemList.Count)
			{
				tiles[i].gameObject.SetActive(false);
			}
			else
			{
				tiles[i].gameObject.SetActive(true);
				tiles[i].LoadItemTile(itemList[i], this, inventType, i);
			}
		}
		Debug.Log("refresh item tiles");
	}

	public void LoadCurrencyTiles(List<RPGCurrency> itemList, List<CurrencyTilebutton> tiles, GameObject gridPanelRoot, GameObject tilePrefab)
	{
		int num = itemList.Count - tiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(gridPanelRoot, tilePrefab);
				CurrencyTilebutton tileButton = itemTile.GetComponent<CurrencyTilebutton>();
				tiles.Add(tileButton);
			}
		}
		for (int i = 0; i < tiles.Count; i++) {
			if(i>=itemList.Count)
			{
				tiles[i].gameObject.SetActive(false);
			}
			else
			{
				tiles[i].gameObject.SetActive(true);
				tiles[i].Load(itemList[i]);
			}
		}
		Debug.Log("refresh currency tiles");
	}

	public virtual void OnCategoryPressed(int index, int level)
	{
	}
	
	public virtual void OnItemTilePressed(int index)
	{
	}

	public virtual void OnDragDrop(int index)
	{
	}

	public virtual void ReceiveEquipButtonMessage()
	{
	}

	public virtual void ReceiveUseButtonMessage()
	{
	}

	public virtual void ReceiveDestroyButtonMessage()
	{
	}

	public virtual void ReceiveWithdrawButtonMessage()
	{
	}

	public virtual void ReceiveDepositButtonMessage()
	{
	}

	public virtual void ReceiveBuyButtonMessage()
	{
	}
}
