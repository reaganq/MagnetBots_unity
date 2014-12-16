using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuickInventoryGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject inventoryPanelRoot;

	public int inventoryMasterCategory = 1;
	public int selectedInventorySubCategory = 0;
	public GameObject armoryCategoryButtons;
	public GameObject itemCategoryButtons;
	public List<ItemTileButton> itemTiles;
	
    public QuickInventoryUICategory selectedInventoryCategory = QuickInventoryUICategory.None;
	public List<InventoryItem> selectedItemList;
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
    
    public void Enable(QuickInventoryUICategory category)
    {
		Root.SetActive(true);
		if(selectedInventoryCategory == category)
        	OnSubcategoryPressed(currentSelectedSubcategory);
		else
		{
			selectedInventoryCategory = category;
			OnSubcategoryPressed(0);
		}
		if(selectedInventoryCategory == QuickInventoryUICategory.Armors)
		{
			PlayerCamera.Instance.TransitionToInventory();
			PlayerManager.Instance.avatarStatus.Motor.RotateTo(PlayerCamera.Instance.quickArmoryPos);
		}
        //Debug.Log("enable");   
    }

	public override void Disable()
	{
		if(selectedInventoryCategory == QuickInventoryUICategory.Armors)
			PlayerCamera.Instance.TransitionToDefault();
		Root.SetActive(false);
	}
    
	public void OnBackButtonpressed()
    {
        GUIManager.Instance.HideQuickInventory();
    }

	public void OnDragDrop(int index)
	{
		Debug.Log("dropped " + index); 
		if(selectedInventoryCategory == QuickInventoryUICategory.Armors)
		{
			PlayerManager.Instance.Hero.ArmoryInventory.EquipItem(selectedItemList[index]);
		}
		else if(selectedInventoryCategory == QuickInventoryUICategory.Food)
		{
			PlayerManager.Instance.Hero.FeedPlayer(selectedItemList[index]);
		}
		else if(selectedInventoryCategory == QuickInventoryUICategory.Toys)
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

    public void OnSubcategoryPressed(int index)
    {
        if(currentSelectedSubcategory != index)
 			currentSelectedSubcategory = index;

		RefreshItemList();
		RefreshInventoryIcons();
    }

	public void RefreshItemList()
	{
		if(selectedInventoryCategory == QuickInventoryUICategory.Armors)
		{
			if(currentSelectedSubcategory == 0)
			{
				selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.Items;
			}
			if(currentSelectedSubcategory == 1)
			{
				selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.HeadItems;
			}
			if(currentSelectedSubcategory == 2)
			{
				selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.BodyItems;
			}
			if(currentSelectedSubcategory == 3)
			{
				selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.ArmLItems;
			}
			if(currentSelectedSubcategory == 4)
	        {
	            selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.ArmRItems;
	        }
			if(currentSelectedSubcategory == 5)
			{
				selectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.LegsItems;
			}
		}
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
				itemTiles[i].LoadQuickInventoryItem(selectedItemList[i]);
			}
		}



        /*for (int i = 0; i <  ItemTiles.Length; i++) {
            if((i + pageIndex*ItemTiles.Length) < SelectedItemList.Count)
			{
				ItemTiles[i].Show();
				/*ItemTiles[i].Load(SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.AtlasName, 
				                  SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.IconPath, 
				                  SelectedItemList[(i + pageIndex*ItemTiles.Length)].CurrentAmount,
				                  SelectedItemList[(i + pageIndex*ItemTiles.Length)].IsItemUpgradeable,
				                  SelectedItemList[(i + pageIndex*ItemTiles.Length)].Level);*/
				//ItemTiles[i].Load(SelectedItemList[(i + pageIndex*ItemTiles.Length)]);
				//Debug.Log(i + SelectedItemList[(i + pageIndex*ItemTiles.Length)].rpgItem.Name);
			/*}
            else
            {
				ItemTiles[i].Hide();
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
			NextPageButton.SetActive(true);*/
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

public enum QuickInventoryUICategory
{
	None,
	Armors,
	Food,
	Toys
}
