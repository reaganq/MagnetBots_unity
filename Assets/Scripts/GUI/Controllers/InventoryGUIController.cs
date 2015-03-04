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
	public GameObject inventoryPanel;
	public InventoryGUIType inventoryType = InventoryGUIType.Inventory;
	public List<List<InventoryItem>> fullItemList;
	public List<InventoryItem> selectedItemList;
	public List<ItemTileButton> itemTiles;
	public List<CategoryButton> categoryButtons;
	public List<CategoryButton> subcategoryButtons;
	public int selectedCategoryIndex = -1;
	public int selectedSubCategoryIndex = -1;
	public ItemCategories selectedMainInventoryCategory;

	//item details view
	public GameObject backButton;
	public InventoryItem selectedItem;
	public GameObject itemDetailsPanel;
	public float buttonsGap;
	public GameObject[] allButtons;
	public Transform buttonsStartPos;
	public GameObject EquipButton = null;
	public GameObject DestroyButton = null;
	public GameObject unequipButton;
	public GameObject upgradeButton;
	public GameObject useButton;
	public GameObject tickIcon;
	public UISprite currencyIcon;
	public UILabel currencyLabel;
	public UILabel quantityLabel;
	public UISprite skillIcon;
	public UILabel healthBonus;
	public UILabel defenseBonus;
	public UILabel attackBonus;
	public UILabel nameLabel;
	public UILabel descriptionLabel;
	public UISprite descriptionBG;
	public UISprite icon;
	public UILabel rarityLabel;

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
		itemDetailsPanel.SetActive(false);
		inventoryPanel.SetActive(true);
		backButton.SetActive(false);
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
		LoadItemTiles(selectedItemList, itemTiles, gridPanelRoot, itemTilePrefab, inventoryType);
	}

	public override void OnItemTilePressed(int index)
	{
		inventoryPanel.SetActive(false);
		DisplayItemDetails(index);
	}

	public void DisplayItemDetails(int index)
	{
		backButton.SetActive(true);
		itemDetailsPanel.SetActive(true);
		selectedItem = selectedItemList[index];
		nameLabel.text = selectedItem.rpgItem.Name;
		descriptionLabel.text = selectedItem.rpgItem.Description;
		GameObject atlas = Resources.Load(selectedItem.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = selectedItem.rpgItem.IconPath;
		currencyLabel.text = selectedItem.rpgItem.SellValue.ToString();
		if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.CitizenPoints)
			currencyIcon.spriteName = GeneralData.citizenIconPath;
		else if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.Magnets)
			currencyIcon.spriteName = GeneralData.magnetIconPath;
		else if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.Coins)
			currencyIcon.spriteName = GeneralData.coinIconPath;
		rarityLabel.color = GUIManager.Instance.GetRarityColor(selectedItem.rpgItem.Rarity);
		rarityLabel.text = selectedItem.rpgItem.Rarity.ToString();
		UpdateItemDetails();
		if(selectedItem.IsItemEquippable)
		{
			RPGArmor armor = (RPGArmor)selectedItem.rpgItem;
			defenseBonus.text = "+"+0;
			attackBonus.text = "+"+0;
			healthBonus.text = "+"+0;
			defenseBonus.color = Color.white;
			attackBonus.color = Color.white;
			healthBonus.color = Color.white;
			if(armor.armorStatsSets.Count > 0)
			{
				int statsLevel = (Mathf.Min(armor.armorStatsSets.Count, selectedItem.Level) -1);
				for (int i = 0; i < armor.armorStatsSets[statsLevel].armorStats.Count; i++) {
					if(armor.armorStatsSets[statsLevel].armorStats[i].armorStatsType == ArmorStatsType.defence)
					{
						defenseBonus.text = "+" + armor.armorStatsSets[statsLevel].armorStats[i].armorStatsValue;
						defenseBonus.color = Color.green;
					}
					else if(armor.armorStatsSets[statsLevel].armorStats[i].armorStatsType == ArmorStatsType.strength)
					{
						attackBonus.text = "+" + armor.armorStatsSets[statsLevel].armorStats[i].armorStatsValue;
						attackBonus.color = Color.green;
					}
					else if(armor.armorStatsSets[statsLevel].armorStats[i].armorStatsType == ArmorStatsType.vitality)
					{
						healthBonus.text = "+" + armor.armorStatsSets[statsLevel].armorStats[i].armorStatsValue;
						healthBonus.color = Color.green;
					}
				}
			}
			if(armor.HasAbility)
			{
				skillIcon.spriteName = armor.AbilityIconPath;
				skillIcon.enabled = true;
			}
			else
				skillIcon.enabled=false;
			//if armor has skillicon

		}

	}

	public void UpdateItemDetails()
	{
		for (int i = 0; i < allButtons.Length; i++) {
			allButtons[i].SetActive(false);
		}
		if(selectedItem.IsItemUpgradeable)
			upgradeButton.SetActive(true);

		if(selectedItem.IsItemEquippable)
		{
			if(selectedItem.IsItemEquipped)
			{
				//turn on unequip button
				tickIcon.SetActive(true);
				unequipButton.SetActive(true);
			}
			else
			{

				tickIcon.SetActive(false);
				EquipButton.SetActive(true);
			}
		}
		else if(selectedItem.IsItemUsable)
		{
			useButton.SetActive(true);
		}
		quantityLabel.text = selectedItem.CurrentAmount.ToString();
	}

	public void OnEquipButtonPressed()
	{
		PlayerManager.Instance.Hero.EquipItem(selectedItem);
		UpdateItemDetails();
		Debug.Log("equipped item");
	}

	public void OnUseButtonPressed()
	{
	}

	public void OnUpgradeButtonPressed()
	{
	}

	public void OnUnequipButtonPressed()
	{
	}

	public void OnBackButtonPressed()
	{
		RefreshInventoryIcons();
		itemDetailsPanel.SetActive(false);
		inventoryPanel.SetActive(true);
		backButton.SetActive(false);
	}
}

public enum InventoryGUIType
{
	quickInventory,
	Inventory,
	Shop,
	Other,
	Playershop,
}

public enum ItemCategories
{
	Armors,
	Food,
	Toys,
	UpgradeMaterials,
	None
}

