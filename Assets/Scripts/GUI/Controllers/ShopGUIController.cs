using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopGUIController : BasicGUIController {

	//public Shop ActiveShop;
	public List<InventoryItem> selectedItemList;
    public int CurrentSelectedItemIndex = -1;
	public List<ItemTileButton> itemTiles;

	public GameObject itemTilePrefab;
    public GameObject InfoPanel = null;
	public GameObject TransactionBox = null;
    public GameObject BuyButton = null;
	public GameObject PreviousPageButton;
	public GameObject NextPageButton;
	public GameObject inventoryRoot;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    public UILabel ItemSkillDescriptionLabel = null;
	public UILabel ShopNameLabel = null;
	public UILabel QuantityLabel = null;
	public UILabel PriceLabel;
	public int quantity = 1;
	public Shop activeShop;

    
    public ShopMode CurrentShopMode = ShopMode.buy;
    //public UISprite ItemSkillSprite = null;
    
    public void Enable(Shop newShop)
    {
		activeShop = newShop;
        //OnInventoryPressed(0);
		//ActiveShop = PlayerManager.Instance.ActiveShop;
		ChangeShopMode(0);
		ShopNameLabel.text = activeShop.Name;
		Enable();
    }

	public override void Disable()
	{
		base.Disable();
	}
    
    public void OnItemTilePressed(int index)
    {
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

	public void CloseShop()
	{
		GUIManager.Instance.NPCGUI.HideShop();
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
			BuyTransaction buyTransaction = activeShop.BuyItem(activeShop.ShopItems[CurrentSelectedItemIndex].rpgItem, activeShop.ShopItems[CurrentSelectedItemIndex].Level, quantity);
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
	            //allgood;
	        }
		}
		else
		{
			activeShop.SellItem(selectedItemList[CurrentSelectedItemIndex].rpgItem, selectedItemList[CurrentSelectedItemIndex].Level, quantity);

		}
  
        RefreshInventoryIcons();
		UpdateInfoPanel();
    }

	public void ChangeShopMode(int index)
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
	}
    
    public void RefreshInventoryIcons()
    {
		int num = selectedItemList.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(inventoryRoot, itemTilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				itemTiles.Add(tileButton);
			}
		}
		for (int i = 0; i < itemTiles.Count; i++) {
			if(i>=selectedItemList.Count)
			{
				itemTiles[i].gameObject.SetActive(false);
			}
			else
			{
				itemTiles[i].gameObject.SetActive(true);
				itemTiles[i].LoadItemTile(selectedItemList[i], this, InventoryGUIType.Shop, i);
			}
		}
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
    }
    
    public void IncreaseCount()
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
	}
}

public enum ShopMode
{
    buy = 0,
    sellNormal = 1,
	sellArmor = 2
}