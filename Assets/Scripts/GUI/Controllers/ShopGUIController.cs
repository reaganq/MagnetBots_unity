using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopGUIController : BasicGUIController {

	//public Shop ActiveShop;
	public List<InventoryItem> selectedItemList;
    public int CurrentSelectedItemIndex = -1;
	public List<ShopItemTileButton> itemTiles;

	public GameObject itemTilePrefab;
    //public GameObject InfoPanel = null;
	public GameObject confirmationBox = null;
    public GameObject BuyButton = null;
	//public GameObject PreviousPageButton;
	//public GameObject NextPageButton;
	public GameObject inventoryRoot;
	public UILabel ShopNameLabel = null;
	public UIPlayTween confirmationTween;
	public UILabel itemNameLabel;
	public UILabel costLabel;
	public UISprite currencyIcon;
	public GameObject standardMessageObject;
	public GameObject notEnoughMoney;
    /*public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    public UILabel ItemSkillDescriptionLabel = null;

	public UILabel QuantityLabel = null;
	public UILabel PriceLabel;*/
	public int quantity = 1;
	public InventoryGUIType inventoryType = InventoryGUIType.Shop;
	public Shop activeShop;
	public PhotonPlayer playerShopKeeper;
	public bool isPlayerShop;
	public bool isConfirmationDisplayed;
    
   //public ShopMode CurrentShopMode = ShopMode.buy;
    //public UISprite ItemSkillSprite = null;
    public void EnablePlayerShop(List<ParseInventoryItem> parseItemList, string playerName, PhotonPlayer shopKeeperPlayer)
	{

		ShopNameLabel.text = playerName;
		playerShopKeeper = shopKeeperPlayer;
		UpdatePlayerShopitemList(parseItemList);
		isPlayerShop = true;
		Enable();
	}


    public void Enable(Shop newShop)
    {
		activeShop = newShop;
        //OnInventoryPressed(0);
		//ActiveShop = PlayerManager.Instance.ActiveShop;
		//ChangeShopMode(0);
		selectedItemList = activeShop.ShopItems;
		RefreshInventoryIcons();
		ShopNameLabel.text = activeShop.Name;
		isPlayerShop = false;
		Enable();
    }

	public void UpdatePlayerShopitemList(List<ParseInventoryItem> items)
	{
		List<InventoryItem> newItemList = new List<InventoryItem>();
		for (int i = 0; i < items.Count; i++) 
		{
			InventoryItem newItem = new InventoryItem();
			if(items[i].UniqueItemId.IndexOf("ARMOR") != -1)
			{
				newItem.GenerateNewInventoryItem(Storage.LoadbyUniqueId<RPGArmor>(items[i].UniqueItemId, new RPGArmor()), items[i].ItemLevel, items[i].Amount);
			}
			else if(items[i].UniqueItemId.IndexOf("ITEM") != -1)
			{
				newItem.GenerateNewInventoryItem(Storage.LoadbyUniqueId<RPGItem>(items[i].UniqueItemId, new RPGItem()), items[i].ItemLevel, items[i].Amount);
			}
			newItemList.Add(newItem);
		}
		selectedItemList = newItemList;
		RefreshInventoryIcons();
	}

	public override void Disable()
	{
		if(isConfirmationDisplayed)
			HideConfirmation();
		CurrentSelectedItemIndex = -1;
		base.Disable();
	}
    
    public override void OnItemTilePressed(int index)
    {
		if(!isConfirmationDisplayed)
		{
		CurrentSelectedItemIndex = index;
		DisplayConfirmation();
		}
        /*if(index == _CurrentSelectedItemIndex || index >= selectedItemList.Count)
        {
			UpdateInfoPanel();
        }
        else
        {
            if(_CurrentSelectedItemIndex>-1)
            	ItemTiles[_CurrentSelectedItemIndex].Deselect();
            ItemTiles[index].Select();
            _CurrentSelectedItemIndex = index;
            UpdateInfoPanel();
                //BuyButton.SetActive(true);

            //Player.Instance.currentItem 
        }
		ResetQuantity();*/
    }

	public void DisplayConfirmation()
	{
		isConfirmationDisplayed = true;
		itemNameLabel.text = selectedItemList[CurrentSelectedItemIndex].rpgItem.Name;
		if(selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyCurrency == BuyCurrencyType.Coins)
			currencyIcon.spriteName = "currency_coin";
		else if( selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyCurrency == BuyCurrencyType.Magnets)
			currencyIcon.spriteName = "currency_magnet";
		else if(selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyCurrency == BuyCurrencyType.CitizenPoints)
			currencyIcon.spriteName = "currency_citizen";
		costLabel.text = selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyValue.ToString();
		itemNameLabel.color = GUIManager.Instance.GetRarityColor(selectedItemList[CurrentSelectedItemIndex].rpgItem.Rarity);
		standardMessageObject.SetActive(true);
		notEnoughMoney.SetActive(false);
		confirmationTween.Play(true);

	}

	public void HideConfirmation()
	{
		isConfirmationDisplayed = false;
		confirmationBox.SetActive(false);
	}

	public void CloseShop()
	{
		GUIManager.Instance.NPCGUI.HideShop();
	}

	public void ResetQuantity()
	{
		quantity = 1;
		//QuantityLabel.text = quantity.ToString();
	}
    
    public void OnBuyButtonPressed()
    {
		if(!isPlayerShop)
		{
			BuyTransaction buyTransaction = activeShop.BuyItem(activeShop.ShopItems[CurrentSelectedItemIndex].rpgItem, activeShop.ShopItems[CurrentSelectedItemIndex].Level, quantity);
	        if(buyTransaction == BuyTransaction.NotEnoughGold)
	        {
				notEnoughMoney.SetActive(true);
				standardMessageObject.SetActive(false);
	            return;
	        }
	        
	        if(buyTransaction == BuyTransaction.NotEnoughSpaceInInventory)
	        {
	            //display not enough space message
	            return;
	        }
	        
	        if(buyTransaction == BuyTransaction.OK)
	        {
	            //allgood;
	        }
		}
		else
		{
			if (!PlayerManager.Instance.Hero.CanYouAfford(selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyValue, selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyCurrency))
			{
				notEnoughMoney.SetActive(true);
				standardMessageObject.SetActive(false);
				return;
			}
			//activeShop.SellItem(selectedItemList[CurrentSelectedItemIndex].rpgItem, selectedItemList[CurrentSelectedItemIndex].Level, quantity);
			PlayerManager.Instance.ActiveWorld.BuyItemFromPlayer(selectedItemList[CurrentSelectedItemIndex].rpgItem.UniqueId,
			                                                     selectedItemList[CurrentSelectedItemIndex].Level,
			                                                     1,
			                                                     playerShopKeeper);
			selectedItemList[CurrentSelectedItemIndex].CurrentAmount --;
			if(selectedItemList[CurrentSelectedItemIndex].CurrentAmount <= 0)
				selectedItemList.RemoveAt(CurrentSelectedItemIndex);
		}
		HideConfirmation();
        RefreshInventoryIcons();
    }

	/*public void ChangeShopMode(int index)
	{
		Debug.Log("changing shop mode" + index);

		//CategoryButtons[(int)CurrentShopMode].DeselectCategory();

		if(index == 0)
		{
			CurrentShopMode = ShopMode.buy;
			selectedItemList = activeShop.ShopItems;
		}
		else if(index == 1)
		{
			CurrentShopMode = ShopMode.sellNormal;
			selectedItemList = PlayerManager.Instance.Hero.MainInventory.Items;
		}
		else if(index == 2)
		{
			CurrentShopMode = ShopMode.sellArmor;
			selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.Items;
		}

		RefreshInventoryIcons();
		ResetSelection();
		UpdateInfoPanel();
	}*/
    
    public void RefreshInventoryIcons()
    {
		LoadShopItemTiles(selectedItemList, itemTiles, inventoryRoot, itemTilePrefab, inventoryType);
    }

	public void ResetSelection()
	{
		/*for (int i = 0; i < ItemTiles.Length; i++) {
			ItemTiles[i].Deselect();
				}
		_CurrentSelectedItemIndex = -1;
		/*for (int i = 0; i <  ItemTiles.Length; i++) {
			ItemTiles[i].Unequip();
		}*/
	}
    
    /*public void UpdateInfoPanel()
    {
		if(CurrentSelectedItemIndex >= 0)
		{
			if(CurrentSelectedItemIndex >= selectedItemList.Count)
			{
				ResetSelection();
				UpdateInfoPanel();
				return;
			}

			ItemNameLabel.text = selectedItemList[CurrentSelectedItemIndex].rpgItem.Name;
			ItemDescriptionLabel.text = selectedItemList[CurrentSelectedItemIndex].rpgItem.Description;

		}
		else
		{
	        if(CurrentShopMode == ShopMode.buy)
	        {
	            ItemNameLabel.enabled = true;
	            ItemNameLabel.text = "";
	            ItemDescriptionLabel.enabled = true;
	            ItemDescriptionLabel.text = activeShop.Description;
	            ItemSkillDescriptionLabel.enabled = false;
	        }
	        else
	        {
	            ItemNameLabel.enabled = true;
	            ItemNameLabel.text = "";
	            ItemDescriptionLabel.enabled = true;
				ItemDescriptionLabel.text = activeShop.Description;
	            ItemSkillDescriptionLabel.enabled = false;
	        }
		}
    }*/
    
    /*public void IncreaseCount()
	{
		quantity += 1;
		if(quantity > selectedItemList[CurrentSelectedItemIndex].CurrentAmount)
			quantity = selectedItemList[CurrentSelectedItemIndex].CurrentAmount;
		QuantityLabel.text = quantity.ToString();
		UpdatePriceLabel();
	}

	public void DecreaseCount()
	{
		quantity -= 1;
		if(quantity < 1)
			quantity = 1;
		QuantityLabel.text = quantity.ToString();
		UpdatePriceLabel();
	}

	public void UpdatePriceLabel()
	{
		if(CurrentShopMode == ShopMode.buy)
			PriceLabel.text = (selectedItemList[CurrentSelectedItemIndex].rpgItem.BuyValue * quantity).ToString();
		else
			PriceLabel.text = (selectedItemList[CurrentSelectedItemIndex].rpgItem.SellValue * quantity).ToString();
	}*/
}

public enum ShopMode
{
    buy = 0,
    sellNormal = 1,
	sellArmor = 2
}