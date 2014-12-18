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

	public Anvil anvil;

	void Update()
	{
		if(timer >= 0)
			timer -= Time.deltaTime;

		if(timer <= 0 && isInventoryDisplayed)
			HideInventory();
	}
	public override void Enable()
	{
		for (int i = 0; i < ItemTiles.Length; i++) {
			ItemTiles[i].Deselect();
		}
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
		FinalItemTile.Hide();
		UpgradeItemTile.Hide();
		MaterialItemTile.Hide();
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
		if(CanUpgrade)
		{
			state = 1;
			RefreshInventoryIcons();
			ShowInventory();
		}
	}

	public void OnUpgradeMaterialTilePressed()
	{
		if(CanUpgrade)
		{
			state = 2;
			RefreshInventoryIcons();
			ShowInventory();
		}
	}
	
	public void OnItemTilePressed(int index)
	{
		if(ItemTiles[index].Cover.enabled)
			return;

		if(state == 1)
		{
			if(index >= ItemList.Count)
				return;
			else
			{
				/*InventoryItem tempItem = ItemList[index+inventoryPageIndex*ItemTiles.Length];
				UpgradeItem = new InventoryItem();
				UpgradeItem.rpgItem = tempItem.rpgItem;
				UpgradeItem.UniqueItemId = tempItem.UniqueItemId;
				UpgradeItem.Level = tempItem.Level;
				UpgradeItem.CurrentAmount = 1;
				UpgradeItemTile.LoadItemTile(UpgradeItem);
				hasItem = true;*/
			}
		}
		if(state == 2)
		{
			if(index >= MaterialsList.Count)
				return;
			else
			{
				/*InventoryItem tempItem = MaterialsList[index+upgradeMaterialsPageIndex*ItemTiles.Length];
				MaterialItem = new InventoryItem();
				MaterialItem.rpgItem = tempItem.rpgItem;
				MaterialItem.UniqueItemId = tempItem.UniqueItemId;
				MaterialItem.Level = tempItem.Level;
				MaterialItem.CurrentAmount = 1;
				MaterialItemTile.LoadItemTile(MaterialItem);
				hasMaterial = true;*/
			}
		}

		CheckIfCanUpgrade();
		HideInventory();

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
		RefreshInventoryIcons();
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
		RefreshInventoryIcons();
	}

	public void RefreshInventoryIcons()
	{
		RefreshItemList();
		if(state == 1)
		{
			for (int i = 0; i <  ItemTiles.Length; i++) {
				if((i + inventoryPageIndex*ItemTiles.Length) < ItemList.Count)
				{
					//ItemTiles[i].Show();
					//ItemTiles[i].LoadWithCover(ItemList[(i + inventoryPageIndex*ItemTiles.Length)], ItemList[(i + inventoryPageIndex*ItemTiles.Length)].IsItemUpgradeable);
				}
				else
				{
					//ItemTiles[i].Hide();
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
				if((i + upgradeMaterialsPageIndex*ItemTiles.Length) < MaterialsList.Count)
				{
					//ItemTiles[i].Show();
					//ItemTiles[i].LoadWithCover(MaterialsList[(i + upgradeMaterialsPageIndex*ItemTiles.Length)], true);
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
		timer = 5.0f;
	}

	public void ShowInventory()
	{
		if(!isInventoryDisplayed)
			Inventory.SetActive(true);
		isInventoryDisplayed = true;
		timer = 5.0f;
	}

	public void HideInventory()
	{
		Inventory.SetActive(false);
		state = 0;
		isInventoryDisplayed = false;
	}

	public void OnUpgradePressed()
	{
		Debug.Log("ugprading");
		HideInventory();
		MaterialItemTile.Hide();
		UpgradeItemTile.Hide();
		float luck = Random.Range(0.0f, 1.0f);
		Debug.Log("luck " + luck);
		CanUpgrade = false;

		if(luck < 0.5f)
		{
			FinalItem = new InventoryItem();
			FinalItem.rpgItem = UpgradeItem.rpgItem;
			FinalItem.CurrentAmount = 1;
			FinalItem.Level = UpgradeItem.Level + 1;
			FinalItem.UniqueItemId = UpgradeItem.UniqueItemId;
			PlayerManager.Instance.Hero.AddItem(FinalItem);
			PlayerManager.Instance.Hero.RemoveItem(UpgradeItem);
			PlayerManager.Instance.Hero.RemoveItem(MaterialItem);
			StartCoroutine(Success());
		}
		else
		{
			FinalItem = null;
			PlayerManager.Instance.Hero.RemoveItem(UpgradeItem);
			PlayerManager.Instance.Hero.RemoveItem(MaterialItem);
			StartCoroutine(Fail());
		}
		MaterialItem = null;
		UpgradeItem = null;
		hasItem = false;
		hasMaterial = false;
		CheckIfCanUpgrade();
	}

	public IEnumerator Success()
	{
		anvil.Sucess();
		yield return new WaitForSeconds(2);
		Debug.Log("SUCCESS");
		//FinalItemTile.LoadItemTile(FinalItem);
		CollectButton.SetActive(true);
	}

	public IEnumerator Fail()
	{
		Debug.Log("FAIL");
		anvil.Fail();
		yield return new WaitForSeconds(2);
		OnCollectPressed();
	}

	public void OnCollectPressed()
	{
		FinalItemTile.Hide();
		FinalItem = null;
		CollectButton.SetActive(false);
		CanUpgrade = true;
	}
	
}
