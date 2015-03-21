//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class ShopItemTileButton: MonoBehaviour 
{
	public RarityColor[] rarityColors;
    public int index;
	public BasicGUIController owner;
	public GameObject[] gradeStars;
    public UISprite icon;
    public UISprite tile;
    public UILabel amountLabel;
	public GameObject quantityTag;
	public UISprite currencyIcon;
	public UILabel priceLabel;
	public InventoryItem referencedItem;
	public InventoryGUIType itemTileType;
	public Collider collider;
	public Color blankColor;
	public GameObject blankCover;
	public GameObject nonBlankObject;
	private Vector2 lastPressDownPos;
	public float movementThreshold = 5;
	public GameObject unStockButton;

	public bool isBlank;
	//bool mStarted = false;
	//bool mHighlighted = false;

	void OnPress (bool isPressed)
	{
		if (isPressed)
		{
			lastPressDownPos = UICamera.lastTouchPosition;
		}
		else 
		{
			if(Vector2.Distance(UICamera.lastTouchPosition, lastPressDownPos) < movementThreshold)
			{
				if(owner != null)
				{
					owner.OnItemTilePressed(index);
				}
				Debug.Log("PRESSED");
			}
		}
	}
    
    public void UnStock()
	{
		if(itemTileType == InventoryGUIType.Playershop)
		{
			owner.ReceiveDestroyButtonMessage(index);
		}
	}

	public void LoadGeneric(InventoryItem item)
	{
		nonBlankObject.SetActive(true);
		if(itemTileType == InventoryGUIType.Playershop)
			collider.enabled = false;
		else
			collider.enabled = true;
		blankCover.SetActive(false);
		isBlank = false;
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		quantityTag.SetActive(true);
		Debug.Log("item has: " + item.CurrentAmount);
		amountLabel.text = item.CurrentAmount.ToString();
		LoadItemRarity(item.rpgItem);
		for (int i = 0; i < gradeStars.Length; i++) {
			gradeStars[i].SetActive(false);
		}
		if(item.rpgItem.IsUpgradeable)
		{
			for (int i = 0; i < item.Level; i++) {
				if(i < 3)
				gradeStars[i].SetActive(true);
			}
		}
		if(itemTileType == InventoryGUIType.Shop)
		{
			if(item.rpgItem.BuyCurrency == BuyCurrencyType.Coins)
				currencyIcon.spriteName = "currency_coin";
			else if( item.rpgItem.BuyCurrency == BuyCurrencyType.Magnets)
				currencyIcon.spriteName = "currency_magnet";
			else if(item.rpgItem.BuyCurrency == BuyCurrencyType.CitizenPoints)
				currencyIcon.spriteName = "currency_citizen";
		}
		else
			currencyIcon.spriteName = "currency_coin";

		priceLabel.text = item.rpgItem.BuyValue.ToString();


	}

	public void LoadShopItemTile(InventoryItem item, BasicGUIController newOwner, InventoryGUIType type, int i)
	{
		index = i;
		owner = newOwner;
		switch (type)
		{
		case InventoryGUIType.Shop:
			unStockButton.SetActive(false);
			break;
		case InventoryGUIType.Playershop:
			unStockButton.SetActive(true);
			break;
		}
		LoadGeneric(item);
		itemTileType = type;
	}

	public void LoadBlank(BasicGUIController newOwner, InventoryGUIType type, int i)
	{
		index = i;
		owner = newOwner;
		itemTileType = type;
		nonBlankObject.SetActive(false);
		blankCover.SetActive(true);
		tile.color = blankColor;
		quantityTag.SetActive(false);
		isBlank = true;
		collider.enabled = true;
	}

	public void LoadItemRarity(RPGItem item)
	{
		for (int i = 0; i < rarityColors.Length; i++) {
			if(rarityColors[i].rarity == item.Rarity)
			{
				tile.color = rarityColors[i].tintColor;
				return;
			}
		}
		tile.color = Color.white;
	}
}