using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShopGUIController : BasicGUIController {

	public InventoryGUIType inventoryType = InventoryGUIType.Playershop;
	//public Shop ActiveShop;
	public List<InventoryItem> selectedItemList;
    public int CurrentSelectedItemIndex = -1;
	public List<ShopItemTileButton> itemTiles;

	public GameObject itemTilePrefab;
    //public GameObject InfoPanel = null;
	public GameObject TransactionBox = null;
    public GameObject BuyButton = null;
	public GameObject collectButton;
	//public GameObject PreviousPageButton;
	//public GameObject NextPageButton;
	public GameObject inventoryRoot;
	public UIGrid inventoryGrid;
    
	public void Start()
	{
		for (int i = 0; i < 11; i++) {
			GameObject itemTile = NGUITools.AddChild(inventoryRoot, itemTilePrefab);
			ShopItemTileButton tileButton = itemTile.GetComponent<ShopItemTileButton>();
			itemTiles.Add(tileButton);
		}
	}

	public override void Enable ()
	{
		RefreshInventoryIcons();
		base.Enable ();
	}

	public override void Disable()
	{
		base.Disable();
	}
    
    public override void OnItemTilePressed(int index)
    {
		GUIManager.Instance.InventoryGUI.Enable();
		Debug.Log("opening inventorygui");
    }

	public void CloseShop()
	{
		GUIManager.Instance.HidePlayerShop();
	}
    
	public override void ReceiveDestroyButtonMessage(int index)
	{
		PlayerManager.Instance.Hero.UnstockItem(selectedItemList[index], selectedItemList[index].CurrentAmount);
		RefreshInventoryIcons();
		//unstock item;
	}
	
    public void RefreshInventoryIcons()
    {
		selectedItemList = PlayerManager.Instance.Hero.playerShopInventory.Items;
		LoadShopItemTiles(selectedItemList, itemTiles, inventoryRoot, itemTilePrefab, inventoryType);
		inventoryGrid.Reposition();
    }
    
    public void IncreaseCount()
	{
		/*quantity += 1;
		if(quantity > selectedItemList[CurrentSelectedItemIndex].CurrentAmount)
			quantity = selectedItemList[CurrentSelectedItemIndex].CurrentAmount;
		QuantityLabel.text = quantity.ToString();*/
		//UpdatePriceLabel();
	}

	public void DecreaseCount()
	{
		//quantity -= 1;
		/*if(quantity < 1)
			quantity = 1;
		QuantityLabel.text = quantity.ToString();*/
		//UpdatePriceLabel();
	}

	public void OnCollectPressed()
	{
		if(PlayerManager.Instance.Hero.shopTill > 0)
			PlayerManager.Instance.Hero.CollectFromShopTill();
	}
}