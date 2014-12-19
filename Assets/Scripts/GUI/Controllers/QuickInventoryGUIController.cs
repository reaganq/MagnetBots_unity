using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuickInventoryGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject inventoryPanelRoot;
	
	public List<ItemTileButton> itemTiles;
    public ItemCategories selectedMainInventoryCategory = ItemCategories.None;
	public List<ItemCategoryData> subCategories;
	public List<InventoryItem> selectedItemList;
	public List<CategoryButton> subcategoryButtons;
    public int currentSelectedSubcategory = 0;
    public int currentSelectedItemIndex = -1;
   // public ItemTileButton[] ItemTiles = new ItemTileButton[10];
    //public ItemTileButton[] CategoryButtons;
    //public UISprite[] ItemSprites = new UISprite[10];
    public GameObject infoPanel = null;
    
    public GameObject equipButton = null;
    public GameObject destroyButton = null;

	public GameObject previousPageButton;
	public GameObject nextPageButton;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    //public UILabel ItemSkillDescriptionLabel = null;
    //public UISprite ItemSkillSprite = null;
    
    public void Enable(ItemCategories category)
    {
		Enable();
		selectedMainInventoryCategory = category;
		//setup categories
		if(selectedMainInventoryCategory == ItemCategories.Armors)
		{
			PlayerCamera.Instance.TransitionToQuickArmory();
			PlayerManager.Instance.avatarActionManager.RotateTo(PlayerCamera.Instance.quickArmoryPos);
			subCategories = PlayerManager.Instance.data.GetSubcategories("Armor");
		}
		else if(selectedMainInventoryCategory == ItemCategories.Food)
		{
			PlayerCamera.Instance.TransitionToQuickInventory();
			subCategories = PlayerManager.Instance.data.GetSubcategories("Food");
		}
		else if(selectedMainInventoryCategory == ItemCategories.Toys)
		{
			PlayerCamera.Instance.TransitionToQuickInventory();
			subCategories = PlayerManager.Instance.data.GetSubcategories("Toys");
		}
		SetupSubCategories();
		OnCategoryPressed(0, 1);
    }

	public void SetupSubCategories()
	{
		for (int i = 0; i < subcategoryButtons.Count; i++) {
			if(i>=subCategories.Count)
			{
				subcategoryButtons[i].gameObject.SetActive(false);
			}
			else
			{
				subcategoryButtons[i].gameObject.SetActive(true);
				subcategoryButtons[i].LoadCategoryButton(subCategories[i], i, 1);
			}
		}
	}

	public override void Disable()
	{
		if(selectedMainInventoryCategory == ItemCategories.Armors)
		{
			PlayerManager.Instance.avatarActionManager.RotationReset();
		}
		PlayerCamera.Instance.TransitionToDefault();
		base.Disable();
	}
    
	public void OnBackButtonpressed()
    {
        GUIManager.Instance.HideQuickInventory();
    }

	public override void OnDragDrop(int index)
	{
		Debug.Log("dropped " + index); 
		if(selectedMainInventoryCategory == ItemCategories.Armors)
		{
			PlayerManager.Instance.Hero.ArmoryInventory.EquipItem(selectedItemList[index]);
		}
		else if(selectedMainInventoryCategory == ItemCategories.Food)
		{
			PlayerManager.Instance.Hero.FeedPlayer(selectedItemList[index]);
		}
		else if(selectedMainInventoryCategory == ItemCategories.Toys)
		{
			PlayerManager.Instance.Hero.PlayToy(selectedItemList[index]);
		}
	}
    
    /*public void UpdateInfoPanel()
    {
        
		if(SelectedItemList[CurrentSelectedItemIndex+ pageIndex*ItemTiles.Length].IsItemEquippable)
        {
			if(!SelectedItemList[CurrentSelectedItemIndex+ pageIndex*ItemTiles.Length].IsItemEquipped)
            {
                EquipButton.SetActive(true);
                DestroyButton.SetActive(true);
            }
            else
            {
                EquipButton.SetActive(false);
                DestroyButton.SetActive(false);
            }
			RPGArmor armor = (RPGArmor)SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length].rpgItem;
            if(armor.HasAbility)
            {
                //ItemSkillSprite.enabled = true;
                //ItemSkillSprite.spriteName = armor.AbilityIconPath;
                //ItemSkillDescriptionLabel.enabled = true;
                //ItemSkillDescriptionLabel.text = armor.AbilityString;
            }
        }
        else
            EquipButton.SetActive(false);
        
        
        ItemNameLabel.enabled = true;
		ItemNameLabel.text = SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length].rpgItem.Name;
        ItemDescriptionLabel.enabled = true;
		ItemDescriptionLabel.text = SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length].rpgItem.Description;
    }


    
    public void OnDestroyButtonPressed()
    {
		PlayerManager.Instance.Hero.ArmoryInventory.RemoveItem(SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length]);
		RefreshItemList();
        RefreshInventoryIcons();
        ResetSelection();
		Debug.Log(PlayerManager.Instance.Hero.ArmoryInventory.Items.Count);
		Debug.Log(PlayerManager.Instance.Hero.ArmoryInventory.HeadItems.Count);
    }
    
	public void OnEquipButtonPressed()
	{
		PlayerManager.Instance.Hero.ArmoryInventory.EquipItem(SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length]);
		EquipButton.SetActive(false);
		RefreshSelection();
		UpdateInfoPanel();
	}*/

	public override void OnCategoryPressed(int index, int level)
    {
        if(currentSelectedSubcategory != index)
		{
			subcategoryButtons[currentSelectedSubcategory].DeselectCategory();
 			currentSelectedSubcategory = index;
		}
		subcategoryButtons[index].SelectCategory();
		selectedItemList = InventoryGUIController.RefreshItemListOfSubCategory(selectedMainInventoryCategory, currentSelectedSubcategory);
		RefreshInventoryIcons();
    }
    
    public void RefreshInventoryIcons()
    {
		int num = selectedItemList.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(inventoryPanelRoot, itemTilePrefab);
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
				itemTiles[i].LoadItemTile(selectedItemList[i], this, InventoryGUIType.quickInventory, i);
			}
		}
    }
    
    /*public void RefreshSelection()
    {
        for (int i = 0; i <  ItemTiles.Length; i++) {
			if((i + pageIndex*ItemTiles.Length) < SelectedItemList.Count)
            {
				if(SelectedItemList[(i + pageIndex*ItemTiles.Length)].IsItemEquipped)
                    ItemTiles[i].Equip();
                else
                    ItemTiles[i].Unequip();
            }
            else
                ItemTiles[i].Unequip();
        }
    }
    */
    
    /*public void ResetSelection()
    {
        if(CurrentSelectedItemIndex != -1)
            ItemTiles[CurrentSelectedItemIndex].Deselect();
        CurrentSelectedItemIndex = -1;
        //foreach(
        for (int i = 0; i <  ItemTiles.Length; i++) {
			if((i + pageIndex*ItemTiles.Length) < SelectedItemList.Count)
            {
				if(SelectedItemList[(i + pageIndex*ItemTiles.Length)].IsItemEquipped)
                    ItemTiles[i].Equip();
                else
                    ItemTiles[i].Unequip();
            }
            else
                ItemTiles[i].Unequip();
        }
    }*/

	/*public void NextPage()
	{
		pageIndex += 1;
		ResetSelection();
		RefreshInventoryIcons();
		HideInfoPanel();
	}

	public void PreviousPage()
	{
		pageIndex -= 1;
		if(pageIndex <= 0)
			pageIndex = 0;
		ResetSelection();
		RefreshInventoryIcons();
		HideInfoPanel();
	}*/
    
    /*public void HideInfoPanel()
    {
        EquipButton.SetActive(false);
        DestroyButton.SetActive(false);
        ItemNameLabel.enabled = false;
        ItemDescriptionLabel.enabled = false;
        //ItemSkillSprite.enabled = false;
        //ItemSkillDescriptionLabel.enabled = false;
    }*/
    
}

