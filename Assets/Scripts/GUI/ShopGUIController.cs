using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopGUIController : BasicGUIController {

	//public Shop ActiveShop;
	public List<InventoryItem> SelectedItemList;
    public int _CurrentSelectedItemIndex = -1;
	public int pageIndex = 0;
	public ItemTileButton[] ItemTiles = new ItemTileButton[10];
	public ItemTileButton[] CategoryButtons;
	public int CurrentSelectedItemIndex {
		get {
			return (_CurrentSelectedItemIndex + pageIndex* ItemTiles.Length);
		}
	}

	public GameObject Panel = null;
    public GameObject InfoPanel = null;
	public GameObject TransactionBox = null;
    public GameObject BuyButton = null;
	public GameObject PreviousPageButton;
	public GameObject NextPageButton;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    public UILabel ItemSkillDescriptionLabel = null;
	public UILabel ShopNameLabel = null;
	public UILabel QuantityLabel = null;
	public UILabel PriceLabel;
	public int quantity = 1;

    
    public ShopMode CurrentShopMode = ShopMode.buy;
    //public UISprite ItemSkillSprite = null;
    
    public override void Enable()
    {
        //OnInventoryPressed(0);
		//ActiveShop = PlayerManager.Instance.ActiveShop;
		Panel.SetActive(true);
		ChangeShopMode(0);
		ShopNameLabel.text = PlayerManager.Instance.ActiveShop.Name;

    }

	public override void Disable()
	{
		Panel.SetActive(false);
	}
    
    public void OnItemTilePressed(int index)
    {
        if(index == _CurrentSelectedItemIndex || index >= SelectedItemList.Count)
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
		ResetQuantity();
    }

	public void ResetQuantity()
	{
		quantity = 1;
		QuantityLabel.text = quantity.ToString();
	}
    
    public void OnBuyButtonPressed()
    {
		if(CurrentShopMode == ShopMode.buy)
		{
			BuyTransaction buyTransaction = PlayerManager.Instance.ActiveShop.BuyItem(PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem, PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].Level, quantity);
	        if(buyTransaction == BuyTransaction.NotEnoughGold)
	        {
	            return;
	        }
	        
	        if(buyTransaction == BuyTransaction.NotEnoughSpaceInInventory)
	        {
	            //display not enough space message
	            return;
	        }
	        
	        if(buyTransaction == BuyTransaction.OK)
	        {
	            UpdateInfoPanel();
	            //allgood;
	        }
		}
		else
		{

		}
  
        RefreshInventoryIcons();
    }

	public void ChangeShopMode(int index)
	{
		Debug.Log("changing shop mode" + index);

		CategoryButtons[(int)CurrentShopMode].DeselectCategory();

		if(index == 0)
		{
			CurrentShopMode = ShopMode.buy;
			SelectedItemList = PlayerManager.Instance.ActiveShop.ShopItems;
		}
		else if(index == 1)
		{
			CurrentShopMode = ShopMode.sellNormal;
			SelectedItemList = PlayerManager.Instance.Hero.MainInventory.Items;
		}
		else if(index == 2)
		{
			CurrentShopMode = ShopMode.sellArmor;
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.Items;
		}

		CategoryButtons[index].SelectCategory();
		pageIndex = 0;
		RefreshInventoryIcons();
		ResetSelection();
		UpdateInfoPanel();
	}
    
    public void RefreshInventoryIcons()
    {

		for (int i = 0; i <  ItemTiles.Length; i++) {
			if((i + pageIndex*ItemTiles.Length) >= SelectedItemList.Count)
				ItemTiles[i].Hide();
			else
			{
				ItemTiles[i].Show();
				/*ItemTiles[i].LoadWithCover(SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.AtlasName, 
				                           SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.IconPath, 
				                           SelectedItemList[(i + pageIndex*ItemTiles.Length)].CurrentAmount, 
				                           SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.IsUpgradeable,
				                           SelectedItemList[(i + pageIndex*ItemTiles.Length)].Level,
				                           SelectedItemList[(i + pageIndex*ItemTiles.Length)].IsItemTradeable);*/
				ItemTiles[i].LoadWithCover(SelectedItemList[(i + pageIndex*ItemTiles.Length)], SelectedItemList[(i + pageIndex*ItemTiles.Length)].IsItemTradeable);
			}
		}

		if(pageIndex == 0)
			PreviousPageButton.SetActive(false);
		else
			PreviousPageButton.SetActive(true);

		if(((pageIndex+1)*ItemTiles.Length) >= SelectedItemList.Count)
		{
			NextPageButton.SetActive(false);
		}
		else
			NextPageButton.SetActive(true);

		ResetQuantity();
		/*if(CurrentShopMode == ShopMode.buy)
		{
	        for (int i = 0; i <  ItemTiles.Length; i++) {
	            if((i + PageIndex*ItemTiles.Length) >= PlayerManager.Instance.ActiveShop.ShopItems.Count)
	                ItemTiles[i].Hide();
	            else
	            {
	                ItemTiles[i].Show();
					ItemTiles[i].Load(PlayerManager.Instance.ActiveShop.ShopItems[(i + PageIndex*ItemTiles.Length)].rpgItem.AtlasName, PlayerManager.Instance.ActiveShop.ShopItems[(i + PageIndex*ItemTiles.Length)].rpgItem.IconPath, PlayerManager.Instance.ActiveShop.ShopItems[(i + PageIndex*ItemTiles.Length)].CurrentAmount);
				}
	        }
		}

		if(CurrentShopMode == ShopMode.sellNormal)
		{
			for (int i = 0; i <  ItemTiles.Length; i++) {
				if((i + PageIndex*ItemTiles.Length) >= PlayerManager.Instance.Hero.MainInventory.Items.Count)
					ItemTiles[i].Hide();
				else
				{
					ItemTiles[i].Show();
					ItemTiles[i].Load(PlayerManager.Instance.Hero.MainInventory.Items[(i + PageIndex*ItemTiles.Length)].rpgItem.AtlasName, PlayerManager.Instance.Hero.MainInventory.Items[(i + PageIndex*ItemTiles.Length)].rpgItem.IconPath, PlayerManager.Instance.Hero.MainInventory.Items[(i + PageIndex*ItemTiles.Length)].CurrentAmount);

				}
			}
		}
		if(CurrentShopMode == ShopMode.sellArmor)
		{
			for (int i = 0; i <  ItemTiles.Length; i++) {
				if((i + PageIndex*ItemTiles.Length) >= PlayerManager.Instance.Hero.ArmoryInventory.Items.Count)
					ItemTiles[i].Hide();
				else
				{
					ItemTiles[i].Show();
					ItemTiles[i].Load(PlayerManager.Instance.Hero.ArmoryInventory.Items[(i + PageIndex*ItemTiles.Length)].rpgItem.AtlasName, PlayerManager.Instance.Hero.ArmoryInventory.Items[(i + PageIndex*ItemTiles.Length)].rpgItem.IconPath, PlayerManager.Instance.Hero.ArmoryInventory.Items[(i + PageIndex*ItemTiles.Length)].CurrentAmount);
				}
			}
		}*/
    }

	public void ResetSelection()
	{
		for (int i = 0; i < ItemTiles.Length; i++) {
			ItemTiles[i].Deselect();
				}
		_CurrentSelectedItemIndex = -1;
		/*for (int i = 0; i <  ItemTiles.Length; i++) {
			ItemTiles[i].Unequip();
		}*/
	}

	public void DisplayTransactionBox()
	{
		TransactionBox.SetActive(true);
		ResetQuantity();
		UpdatePriceLabel();
	}

	public void HideTransactionBox()
	{
		TransactionBox.SetActive(false);
	}
    
    public void UpdateInfoPanel()
    {
		if(CurrentSelectedItemIndex >= 0)
		{
			/*if(CurrentShopMode == ShopMode.buy)
			{
				ItemNameLabel.text = PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem.Name;
				ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem.Description;
			}
			else if (CurrentShopMode == ShopMode.sellNormal)
			{
				ItemNameLabel.text = PlayerManager.Instance.Hero.MainInventory.Items[CurrentSelectedItemIndex].rpgItem.Name;
				ItemDescriptionLabel.text = PlayerManager.Hero.MainInventory.Items[CurrentSelectedItemIndex].rpgItem.Description;
			}
			else if (CurrentShopMode == ShopMode.sellArmor)
			{
				ItemNameLabel.text = PlayerManager.Instance.Hero.ArmoryInventory.Items[CurrentSelectedItemIndex].rpgItem.Name;
				ItemDescriptionLabel.text = PlayerManager.Hero.ArmoryInventory.Items[CurrentSelectedItemIndex].rpgItem.Description;
			}*/

			ItemNameLabel.text = SelectedItemList[CurrentSelectedItemIndex].rpgItem.Name;
			ItemDescriptionLabel.text = SelectedItemList[CurrentSelectedItemIndex].rpgItem.Description;

			if(SelectedItemList[CurrentSelectedItemIndex].IsItemEquipped)
			{
				HideTransactionBox();
			}
			else
				DisplayTransactionBox();
		}
		else
		{
	        if(CurrentShopMode == ShopMode.buy)
	        {
	            ItemNameLabel.enabled = true;
	            ItemNameLabel.text = "";
	            ItemDescriptionLabel.enabled = true;
	            ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.Description;
	            ItemSkillDescriptionLabel.enabled = false;
	        }
	        else
	        {
	            ItemNameLabel.enabled = true;
	            ItemNameLabel.text = "";
	            ItemDescriptionLabel.enabled = true;
	            ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.Description;
	            ItemSkillDescriptionLabel.enabled = false;
	        }
			HideTransactionBox();
		}
    }

	public void NextPage()
	{
		pageIndex += 1;
		RefreshInventoryIcons();
		OnItemTilePressed(0);
	}

	public void PreviousPage()
	{
		pageIndex -= 1;
		if(pageIndex < 0)
			pageIndex = 0;
		RefreshInventoryIcons();
		OnItemTilePressed(0);
	}
    
    public void IncreaseCount()
	{
		quantity += 1;
		if(quantity > SelectedItemList[CurrentSelectedItemIndex].CurrentAmount)
			quantity = SelectedItemList[CurrentSelectedItemIndex].CurrentAmount;
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
			PriceLabel.text = (SelectedItemList[CurrentSelectedItemIndex].rpgItem.BuyValue * quantity).ToString();
		else
			PriceLabel.text = (SelectedItemList[CurrentSelectedItemIndex].rpgItem.SellValue * quantity).ToString();
	}
}

public enum ShopMode
{
    buy = 0,
    sellNormal = 1,
	sellArmor = 2
}