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
	public GameObject stockButton;
	public GameObject stockQuantityObject;
	public UILabel stockQuantityLabel;
	public int currentQuantity;
	public GameObject increaseArrow;
	public GameObject decreaseArrow;
	public GameObject useButton;
	public GameObject tickIcon;
	public GameObject armorStatsObject;
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
	public GameObject[] itemDetailsStars;
	public GameObject leftDetails;
	public GameObject rightDetails;
	public GameObject rightUpgradeDetails;

	//Upgrade Item View
	public GameObject upgradeDoor;
	public GameObject upgradeDoorCog;
	public GameObject successPage;
	public GameObject failurePage;
	public GameObject magnetsTick;
	public float successChance;
	public float bonusSuccessChance;
	public int bonusMagnetsCount;
	public InventoryItem upgradedItem;
	public bool usingBonus;
	
	private int newItemCount;

	void Start()
	{
		SetupCategories();
	}

	public override void Enable ()
	{
		currentQuantity = 1;
		UpdateQuantityLabel();
		base.Enable ();
		OnCategoryPressed(0,0);
		loaded = true;
	}

	public override void Disable ()
	{
		selectedCategoryIndex = -1;
		selectedSubCategoryIndex = -1;
		base.Disable ();
	}

	#region normal grid display functions

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
		Debug.Log(fullItemList.Count);
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
					subcategoryButtons[i].LoadSubcategoryButton(PlayerManager.Instance.data.itemCategories[selectedCategoryIndex].subcategories[i], i, 1, 0);
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
			Debug.Log("LOADING UPGRADEMATERIALS");
			fullItems.Add(PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.UpgradeMaterials));
			return fullItems;
		}
		else if(category == ItemCategories.Construction)
		{
			fullItems.Add(PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Construction));
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
		else if(category == ItemCategories.Construction)
		{
			listItem = PlayerManager.Instance.Hero.MainInventory.FilteredItemByCategory(ItemType.Construction);
		}
		return listItem;
	}

	public void RefreshInventoryIcons()
	{
		LoadItemTiles(selectedItemList, itemTiles, gridPanelRoot, itemTilePrefab, inventoryType);
		gridPanel.Reposition();
	}

	public override void OnItemTilePressed(int index)
	{
		inventoryPanel.SetActive(false);
		DisplayItemDetails(index);
	}

	#endregion

	#region item details view

	public void DisplayItemDetails(int index)
	{
		backButton.SetActive(true);
		itemDetailsPanel.SetActive(true);
		HidePageComponents();
		leftDetails.SetActive(true);
		rightDetails.SetActive(true);

		selectedItem = selectedItemList[index];
		nameLabel.text = selectedItem.rpgItem.Name;
		descriptionLabel.text = selectedItem.rpgItem.Description;
		GameObject atlas = Resources.Load(selectedItem.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = selectedItem.rpgItem.IconPath;
		currencyLabel.text = selectedItem.rpgItem.BuyValue.ToString();
		if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.CitizenPoints)
			currencyIcon.spriteName = GeneralData.citizenIconPath;
		else if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.Magnets)
			currencyIcon.spriteName = GeneralData.magnetIconPath;
		else if(selectedItem.rpgItem.BuyCurrency == BuyCurrencyType.Coins)
			currencyIcon.spriteName = GeneralData.coinIconPath;
		rarityLabel.color = GUIManager.Instance.GetRarityColor(selectedItem.rpgItem.Rarity);
		rarityLabel.text = selectedItem.rpgItem.Rarity.ToString();
		quantityLabel.text = selectedItem.CurrentAmount.ToString();
		for (int i = 0; i < itemDetailsStars.Length; i++) {
			itemDetailsStars[i].SetActive(false);
		}
		if(selectedItem.rpgItem.IsUpgradeable)
		{
			for (int i = 0; i < selectedItem.Level; i++) {
				itemDetailsStars[i].SetActive(true);
            }
        }

		UpdateItemDetails();
		if(selectedItem.rpgItem.ItemCategory == ItemType.NakedArmor || selectedItem.rpgItem.ItemCategory == ItemType.Armor)
		{
			RPGArmor armor = (RPGArmor)selectedItem.rpgItem;
			defenseBonus.text = "+"+0;
			attackBonus.text = "+"+0;
			healthBonus.text = "+"+0;
			defenseBonus.color = Color.white;
			attackBonus.color = Color.white;
			healthBonus.color = Color.white;
			armorStatsObject.SetActive(true);
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
		else if(selectedItem.rpgItem.ItemCategory == ItemType.Food)
		{
			skillIcon.enabled = false;
			armorStatsObject.SetActive(true);
		}
	}

	public void UpdateItemDetails()
	{
		for (int i = 0; i < allButtons.Length; i++) {
			allButtons[i].SetActive(false);
		}
		if(GUIManager.Instance.uiState == UIState.inventory)
		{
			stockQuantityObject.SetActive(false);

			if(selectedItem.IsItemUpgradeable)
				upgradeButton.SetActive(true);

			if(selectedItem.IsItemEquippable)
			{
				if(selectedItem.IsItemEquipped)
				{
					unequipButton.SetActive(true);
				}
				else
				{
					EquipButton.SetActive(true);
				}
			}
			else if(selectedItem.IsItemUsable)
			{
				useButton.SetActive(true);
			}

		}
		else if(GUIManager.Instance.uiState == UIState.playerShop)
		{
			for (int i = 0; i < allButtons.Length; i++) {
				allButtons[i].SetActive(false);
			}
			if(MaxStockQuantity() > 0)
				stockButton.SetActive(true);
			else
				stockButton.SetActive(false);
			stockQuantityObject.SetActive(true);
			if(selectedItem.IsItemEquipped && selectedItem.CurrentAmount == 1)
			{
				currentQuantity = 0;

			}
			UpdateQuantityLabel();
		}

		if(selectedItem.IsItemEquippable)
		{
			if(selectedItem.IsItemEquipped)
			{
				tickIcon.SetActive(true);
			}
			else
			{
				tickIcon.SetActive(false);
			}
		}
	}

	#endregion

	#region player shop stocking functions

	public void IncreaseQuantity()
	{
		int maxNumber = selectedItem.CurrentAmount;
		if(selectedItem.IsItemEquipped)
			maxNumber --;
		if(maxNumber > currentQuantity)
			currentQuantity ++;
		UpdateQuantityLabel();
	}

	public void DecreaseQuantity()
	{
		if(currentQuantity > 1)
			currentQuantity --;
		UpdateQuantityLabel();
	}

	public int MinStockQuantity()
	{
		if(selectedItem.IsItemEquipped && selectedItem.CurrentAmount == 1)
			return 0;
		else
			return 1;
	}

	public int MaxStockQuantity()
	{
		if(selectedItem != null)
		{
			if(selectedItem.IsItemEquipped)
				return selectedItem.CurrentAmount -1;
			else
				return selectedItem.CurrentAmount;
		}
		else
			return 0;
	}

	public void UpdateQuantityLabel()
	{
		if(selectedItem == null)
			return;

		if(MaxStockQuantity() <= 0)
		{
			stockQuantityObject.SetActive(false);
		}
		else
		{
			stockQuantityObject.SetActive(true);
			if(MaxStockQuantity() > currentQuantity)
				increaseArrow.SetActive(true);
			else
				increaseArrow.SetActive(false);
            if(currentQuantity > MinStockQuantity())
				decreaseArrow.SetActive(true);
			else
				decreaseArrow.SetActive(false);
			stockQuantityLabel.text = currentQuantity.ToString();
		}
	}

	#endregion

	#region button functions

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
		EnterUpgradeView();
	}

	public void OnUnequipButtonPressed()
	{
	}

	public void OnStockButtonPressed()
	{
		PlayerManager.Instance.Hero.StockItem(selectedItem, currentQuantity);
		Debug.Log(currentQuantity);
		UpdateItemDetails();
		//GUIManager.Instance.PlayerShopGUI.RefreshInventoryIcons();
		OnExitButtonPressed();
	}

	public void OnBackButtonPressed()
	{
		RefreshInventoryIcons();
		itemDetailsPanel.SetActive(false);
		inventoryPanel.SetActive(true);
		backButton.SetActive(false);
	}

	public void OnExitButtonPressed()
	{
		Debug.Log("exit button pressed");
		if(GUIManager.Instance.uiState == UIState.inventory)
			GUIManager.Instance.HideInventory();
		else if(GUIManager.Instance.uiState == UIState.playerShop)
		{
			Disable();
		}
	}

	#endregion

	#region item upgrade

	public void OnMagnetsLuckIncreasePressed()
	{
		usingBonus = !usingBonus;
		magnetsTick.SetActive(usingBonus);
	}

	public void OnConfirmUpgradePressed()
	{
		StartCoroutine(UpgradeItem ());
	}

	public void OnCancelUpgradePressed()
	{
	}

	public void EnterUpgradeView()
	{
		HidePageComponents();
		leftDetails.SetActive(true);
		rightUpgradeDetails.SetActive(true);
		usingBonus = false;
		magnetsTick.SetActive(usingBonus);
	}

	public IEnumerator UpgradeItem()
	{
		yield return null;
		//play door animations
	}

	public void DisplayFailurePage()
	{
		HidePageComponents();
		failurePage.SetActive(true);
	}

	public void DisplaySuccessPage()
	{
		HidePageComponents();
		successPage.SetActive(true);
	}

	public void OnViewNewItemPressed()
	{
	}

	public void OnUpgradeAnotherPressed()
	{
	}

	#endregion

	public void HidePageComponents()
	{
		leftDetails.SetActive(false);
		rightUpgradeDetails.SetActive(false);
		successPage.SetActive(false);
		failurePage.SetActive(false);
		rightDetails.SetActive(false);
	}
}

public enum InventoryGUIType
{
	quickInventory,
	Inventory,
	Shop,
	Other,
	Quest,
	Playershop,
	Reward,
}

public enum ItemCategories
{
	Armors,
	Food,
	Toys,
	Construction,
	UpgradeMaterials,

	None
}

