using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnvilGUIController : BasicGUIController {

	public List<InventoryItem> ItemList;
	public List<InventoryItem> MaterialsList;
	public int state = 0;
	public int inventoryPageIndex = 0;
	public int upgradeMaterialsPageIndex = 0;
	public ItemTileButton[] ItemTiles = new ItemTileButton[10];
	public GameObject Inventory;
	public GameObject Panel;
	public GameObject PreviousPageButton;
	public GameObject NextPageButton;
	public GameObject UpgradeButton;
	public GameObject CollectButton;

	public bool hasItem = false;
	public bool isInventoryDisplayed = false;
	public bool hasMaterial = false;
	public bool CanUpgrade = true;

	public ItemTileButton UpgradeItemTile;
	public ItemTileButton MaterialItemTile;
	public ItemTileButton FinalItemTile;
	public InventoryItem UpgradeItem = new InventoryItem();
	public InventoryItem MaterialItem = new InventoryItem();
	public InventoryItem FinalItem = new InventoryItem();
	public float timer;

	void Update()
	{
		if(timer >= 0)
			timer -= Time.deltaTime;

		if(timer <= 0 && isInventoryDisplayed)
			HideInventory();
	}
	public override void Enable()
	{
		hasItem = false;
		isInventoryDisplayed = false;
		CanUpgrade = true;
		hasMaterial = false;
		state = 0;
		inventoryPageIndex = 0;
		upgradeMaterialsPageIndex = 0;
		UpgradeItem = null;
		MaterialItem = null;
		FinalItem = null;
		Panel.SetActive(true);
		RefreshItemList();
		Inventory.SetActive(false);
		UpgradeButton.SetActive(false);
		CollectButton.SetActive(false);
	}

	public void RefreshItemList()
	{
		ItemList = PlayerManager.Instance.Hero.ArmoryInventory.UpgradeableItems;
		ItemList.AddRange(PlayerManager.Instance.Hero.MainInventory.UpgradeableItems);
		MaterialsList = PlayerManager.Instance.Hero.MainInventory.UpgradeMaterials;
	}

	public override void Disable ()
	{
		Panel.SetActive(false);

	}

	public void OnUpgradeItemTilePressed()
	{
		state = 1;
		RefreshInventoryIcons();
		ShowInventory();
	}

	public void OnUpgradeMaterialTilePressed()
	{
		state = 2;
		RefreshInventoryIcons();
		ShowInventory();
	}
	
	public void OnItemTilePressed(int index)
	{
		if(state == 1)
		{
			if(index >= ItemList.Count)
				return;
			else
			{
				UpgradeItem = ItemList[index+inventoryPageIndex*ItemTiles.Length];
				UpgradeItemTile.Load(UpgradeItem);
				hasItem = true;
			}
		}
		if(state == 2)
		{
			if(index >= MaterialsList.Count)
				return;
			else
			{
				MaterialItem = MaterialsList[index+upgradeMaterialsPageIndex*ItemTiles.Length];
				MaterialItemTile.Load(MaterialItem);
				hasMaterial = true;
			}
		}

		CheckIfCanUpgrade();
	}

	public void CheckIfCanUpgrade()
	{
		if(hasMaterial && hasItem)
			UpgradeButton.SetActive(true);
		else
			UpgradeButton.SetActive(false);
	}

	public void NextPage()
	{
		if(state == 1)
		{
			inventoryPageIndex += 1;
		}
		if(state == 2)
		{
			upgradeMaterialsPageIndex += 1;
		}
	}

	public void PreviousPage()
	{
		if(state == 1)
		{
			inventoryPageIndex -= 1;
			if(inventoryPageIndex < 0)
				inventoryPageIndex = 0;
		}
		if(state == 2)
		{
			upgradeMaterialsPageIndex -= 1;
			if(upgradeMaterialsPageIndex < 0)
				upgradeMaterialsPageIndex = 0;
		}
	}

	public void RefreshInventoryIcons()
	{
		RefreshItemList();
		if(state == 1)
		{
			for (int i = 0; i <  ItemTiles.Length; i++) {
				if((i + inventoryPageIndex*ItemTiles.Length) < ItemList.Count)
				{
					ItemTiles[i].Show();
					ItemTiles[i].LoadWithCover(ItemList[(i + inventoryPageIndex*ItemTiles.Length)], ItemList[(i + inventoryPageIndex*ItemTiles.Length)].IsItemUpgradeable);
				}
				else
				{
					ItemTiles[i].Hide();
				}
			}

			if(inventoryPageIndex == 0)
				PreviousPageButton.SetActive(false);
			else
				PreviousPageButton.SetActive(true);
			
			if(((inventoryPageIndex+1)*ItemTiles.Length) >= ItemList.Count)
			{
				NextPageButton.SetActive(false);
			}
			else
				NextPageButton.SetActive(true);
		}

		if(state == 2)
		{
			for (int i = 0; i <  ItemTiles.Length; i++) {
				if((i + upgradeMaterialsPageIndex*ItemTiles.Length) < ItemList.Count)
				{
					ItemTiles[i].Show();
					ItemTiles[i].Load(MaterialsList[(i + inventoryPageIndex*ItemTiles.Length)]);
				}
				else
				{
					ItemTiles[i].Hide();
				}
			}

			if(upgradeMaterialsPageIndex == 0)
				PreviousPageButton.SetActive(false);
			else
				PreviousPageButton.SetActive(true);
			
			if(((upgradeMaterialsPageIndex+1)*ItemTiles.Length) >= MaterialsList.Count)
			{
				NextPageButton.SetActive(false);
			}
			else
				NextPageButton.SetActive(true);
		}
	}

	public void ShowInventory()
	{
		if(!isInventoryDisplayed)
			Inventory.SetActive(true);
		isInventoryDisplayed = true;
		timer += 5.0f;
	}

	public void HideInventory()
	{
		Inventory.SetActive(false);
		state = 0;
		isInventoryDisplayed = false;
	}

	public void OnUpgradePressed()
	{
		HideInventory();
		MaterialItem = null;
		UpgradeItem = null;
		hasItem = false;
		hasMaterial = false;

		MaterialItemTile.Hide();
		UpgradeItemTile.Hide();
		float luck = Random.Range(0.0f, 1.0f);
		if(luck < 0.5f)
		{
			//success
			FinalItem = UpgradeItem;
			FinalItem.Level += 1;
			FinalItemTile.Load(FinalItem);
			FinalItem.CurrentAmount = 1;
			PlayerManager.Instance.Hero.AddItem(FinalItem);
			CollectButton.SetActive(true);
			CanUpgrade = false;
		}
		else
		{
			OnCollectPressed();
		}
		CheckIfCanUpgrade();

	}

	public void OnCollectPressed()
	{
		FinalItemTile.Hide();
		FinalItem = null;
		CanUpgrade = true;
	}
	
}
