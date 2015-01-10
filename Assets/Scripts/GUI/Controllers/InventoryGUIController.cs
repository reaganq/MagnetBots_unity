using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryGUIController : BasicGUIController {

	public bool loaded = false;
	public GameObject itemTilePrefab;
	public UIScrollBar mainScrollBar;
	public float cachedMainScrollVal;
	public UIGrid gridPanel;
	public GameObject gridPanelRoot;
	public InventoryGUIType inventoryType = InventoryGUIType.Inventory;
	public List<List<InventoryItem>> fullItemList;
	public List<InventoryItem> selectedItemList;
	public List<ItemTileButton> itemTiles;
	public List<CategoryButton> categoryButtons;
	public List<CategoryButton> subcategoryButtons;
	public int selectedCategoryIndex = -1;
	public int selectedSubCategoryIndex = -1;
	public ItemCategories selectedMainInventoryCategory;

	private int newItemCount;

	void Start()
	{
		SetupCategories();
	}

	public override void Enable ()
	{
		base.Enable ();
		OnCategoryPressed(0,0);
		loaded = true;
	}

	public override void Disable ()
	{
		base.Disable ();
	}

	public override void OnCategoryPressed (int index, int level)
	{
		if(level == 0)
		{
			if(selectedCategoryIndex != index )
			{
				if(selectedCategoryIndex >= 0)
					categoryButtons[selectedCategoryIndex].DeselectCategory();
				selectedCategoryIndex = index;
				selectedMainInventoryCategory = (ItemCategories)selectedCategoryIndex;
				fullItemList = loadAllItemsFromCategory(selectedMainInventoryCategory);
				RefreshSubcategories();
				DisplayNewCategory();
			}
			categoryButtons[selectedCategoryIndex].SelectCategory();
		}
		else if(level ==1)
		{
			if(selectedSubCategoryIndex != index)
			{
				if(selectedSubCategoryIndex >= 0)
					subcategoryButtons[selectedSubCategoryIndex].DeselectCategory();
				selectedSubCategoryIndex = index;
				subcategoryButtons[selectedSubCategoryIndex].SelectCategory();
				selectedItemList = fullItemList[selectedSubCategoryIndex];
				RefreshInventoryIcons();
				mainScrollBar.value = 0;
			}
			subcategoryButtons[selectedSubCategoryIndex].SelectCategory();
		}
	}

	public void DisplayNewCategory()
	{
		if(selectedSubCategoryIndex > 0)
			subcategoryButtons[selectedSubCategoryIndex].DeselectCategory();
		selectedSubCategoryIndex = 0;
		subcategoryButtons[selectedSubCategoryIndex].SelectCategory();
		selectedItemList = fullItemList[selectedSubCategoryIndex];
		RefreshInventoryIcons();
		mainScrollBar.value = 0;
	}

	public void SetupCategories()
	{
		for (int i = 0; i < categoryButtons.Count; i++) {
			if(i>=PlayerManager.Instance.data.itemCategories.Count)
			{
				categoryButtons[i].gameObject.SetActive(false);
			}
			else
			{
				categoryButtons[i].gameObject.SetActive(true);
				categoryButtons[i].LoadCategoryButton(PlayerManager.Instance.data.itemCategories[i], i, 0);
			}
		}
	}

	public void RefreshSubcategories()
	{
		for (int i = 0; i < subcategoryButtons.Count; i++) {
			if(i >= PlayerManager.Instance.data.itemCategories[selectedCategoryIndex].subcategories.Count)
			{
				subcategoryButtons[i].gameObject.SetActive(false);
			}
			else
			{
				subcategoryButtons[i].gameObject.SetActive(true);
				if(i > 0 || subcategoryButtons.Count == 1 )
				{
					List<InventoryItem> newitemcount = fullItemList[i].FindAll(UnviewedItem);
					subcategoryButtons[i].LoadSubcategoryButton(PlayerManager.Instance.data.itemCategories[selectedCategoryIndex].subcategories[i], i, 1, newitemcount.Count);
				}
				else
					subcategoryButtons[i].LoadCategoryButton(PlayerManager.Instance.data.itemCategories[selectedCategoryIndex].subcategories[i], i, 1);
			}
		}
	}

	public static bool UnviewedItem(InventoryItem item)
	{
		if(!item.isItemViewed)
			return true;
		else
			return false;
	}

	public static List<List<InventoryItem>> loadAllItemsFromCategory(ItemCategories category)
	{
		List<List<InventoryItem>> fullItems = new List<List<InventoryItem>>();
		if(category == ItemCategories.Armors)
		{
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.Items);
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Head));
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Body));
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.ArmL));
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.ArmR));
			fullItems.Add(PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Legs));
			return fullItems;
		}
		else if(category == ItemCategories.Food)
		{
			fullItems.Add(PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Food));
			return fullItems;
		}
		else if(category == ItemCategories.Toys)
		{
			fullItems.Add(PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Toys));
			return fullItems;
		}
		else if(category == ItemCategories.UpgradeMaterials)
		{
			fullItems.Add(PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.UpgradeMaterials));
			return fullItems;
		}
		return fullItems;
	}

	public static List<InventoryItem> RefreshItemListOfSubCategory(ItemCategories category, int subcategory)
	{
		List<InventoryItem> listItem = new List<InventoryItem>();

		if(category == ItemCategories.Armors)
		{
			if(subcategory == 0)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.Items;
			}
			if(subcategory == 1)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Head);
			}
			if(subcategory == 2)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Body);
			}
			if(subcategory == 3)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.ArmL);
			}
			if(subcategory == 4)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.ArmR);
			}
			if(subcategory == 5)
			{
				listItem = PlayerManager.Instance.Hero.ArmoryInventory.FilteredArmorBySlot(EquipmentSlots.Legs);
			}
		}
		else if(category == ItemCategories.Food)
		{
			listItem = PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Food);
		}
		else if(category == ItemCategories.Toys)
		{
			listItem = PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Toys);
		}
		else if(category == ItemCategories.UpgradeMaterials)
		{
			listItem = PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.UpgradeMaterials);
		}
		return listItem;
	}

	public void RefreshInventoryIcons()
	{
		int num = selectedItemList.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(gridPanelRoot, itemTilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				itemTiles.Add(tileButton);
				tileButton.index = itemTiles.Count-1;
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
				itemTiles[i].LoadItemTile(selectedItemList[i], this, inventoryType, i);
			}
		}
	}
}

public enum InventoryGUIType
{
	quickInventory,
	Inventory,
	Shop,
	Other,
	Playershop,
	Deposit,
	Withdraw
}

public enum ItemCategories
{
	Armors,
	Food,
	Toys,
	UpgradeMaterials,
	None
}

