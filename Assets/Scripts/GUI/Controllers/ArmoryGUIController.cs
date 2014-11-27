using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmoryGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject inventoryPanelRoot;

	public int inventoryMasterCategory = 1;
	public int selectedInventorySubCategory = 0;
	public GameObject armoryCategoryButtons;
	public GameObject itemCategoryButtons;
	public List<ItemTileButton> itemTiles;

    public InventoryUICategory SelectedInventoryCategory = InventoryUICategory.None;
	public List<InventoryItem> SelectedItemList;
    public int CurrentSelectedInventory = 0;
    public int CurrentSelectedItemIndex = -1;
    public ItemTileButton[] ItemTiles = new ItemTileButton[10];
    public ItemTileButton[] CategoryButtons;
    //public UISprite[] ItemSprites = new UISprite[10];
    public GameObject InfoPanel = null;
    
    public GameObject EquipButton = null;
    public GameObject DestroyButton = null;

	public GameObject Panel = null;

	public GameObject PreviousPageButton;
	public GameObject NextPageButton;

	public int pageIndex = 0;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    //public UILabel ItemSkillDescriptionLabel = null;
    //public UISprite ItemSkillSprite = null;
    
	void Update()
	{
		if(Input.GetKey(KeyCode.E))
		{
			OnEquipButtonPressed();
		}
	}

    public override void Enable()
    {
		Panel.SetActive(true);
        OnSubcategoryPressed(CurrentSelectedInventory);
        //Debug.Log("enable");   
    }

	public override void Disable()
	{
		Panel.SetActive(false);
	}
    
	public void OnBackButtonpressed()
    {
        GUIManager.Instance.HideInventory();
    }
    
    public void OnItemTilePressed(int index)
    {
        if(index == CurrentSelectedItemIndex || index >= SelectedItemList.Count)
        {
            //Debug.Log("return");
            return;
        }
        else
        {
            if(CurrentSelectedItemIndex>-1)
                ItemTiles[CurrentSelectedItemIndex].Deselect();
            ItemTiles[index].Select();
            CurrentSelectedItemIndex = index;
            UpdateInfoPanel();
            //Player.Instance.currentItem 
        }
    }
    
    public void UpdateInfoPanel()
    {
        
        /*if(SelectedItemList[CurrentSelectedItemIndex].IsItemUsable)
        {
            UseButton.SetActive(true);
            DestroyButton.SetActive(true);
        }
        else
            UseButton.SetActive(false);*/
        
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

    public void OnEquipButtonPressed()
    {
		PlayerManager.Instance.Hero.ArmoryInventory.EquipItem(SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length]);
        EquipButton.SetActive(false);
        RefreshSelection();
        UpdateInfoPanel();
    }
    
    public void OnDestroyButtonPressed()
    {
		PlayerManager.Instance.Hero.ArmoryInventory.RemoveItem(SelectedItemList[CurrentSelectedItemIndex + pageIndex*ItemTiles.Length]);
		RefreshItemList();
        HideInfoPanel();
        RefreshInventoryIcons();
        ResetSelection();
		HideInfoPanel();
		Debug.Log(PlayerManager.Instance.Hero.ArmoryInventory.Items.Count);
		Debug.Log(PlayerManager.Instance.Hero.ArmoryInventory.HeadItems.Count);
    }
    
    public void OnSubcategoryPressed(int index)
    {
        if(CurrentSelectedInventory != index)
        {
            if(CurrentSelectedInventory >= 0)
				CategoryButtons[CurrentSelectedInventory].DeselectCategory();
			CurrentSelectedInventory = index;
			//pageIndex = 0;
			RefreshItemList();
            ResetSelection();
            RefreshInventoryIcons();
            HideInfoPanel();
            CategoryButtons[CurrentSelectedInventory].SelectCategory();
        }
		else
		{
			RefreshItemList();
			ResetSelection();
			RefreshInventoryIcons();
			HideInfoPanel();
			CategoryButtons[CurrentSelectedInventory].SelectCategory();
        }
        
    }

	public void RefreshItemList()
	{
		if(CurrentSelectedInventory == 0)
		{
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.Items;
		}
		if(CurrentSelectedInventory == 1)
		{
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.HeadItems;
		}
		if(CurrentSelectedInventory == 2)
		{
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.BodyItems;
		}
		if(CurrentSelectedInventory == 3)
		{
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.ArmLItems;
		}
		if(CurrentSelectedInventory == 4)
        {
            SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.ArmRItems;
        }
		if(CurrentSelectedInventory == 5)
		{
			SelectedItemList = PlayerManager.Instance.Hero.ArmoryInventory.LegsItems;
		}
    }
    
    public void RefreshInventoryIcons()
    {
		int num = SelectedItemList.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(inventoryPanelRoot, itemTilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				itemTiles.Add(tileButton);
			}
		}
		for (int i = 0; i < itemTiles.Count; i++) {
			if(i>=SelectedItemList.Count)
			{
				itemTiles[i].gameObject.SetActive(false);
			}
			else
			{
				itemTiles[i].gameObject.SetActive(true);
				itemTiles[i].Load(SelectedItemList[i]);
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
    
    public void RefreshSelection()
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
    
    public void ResetSelection()
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
    }

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
    
    public void HideInfoPanel()
    {
        EquipButton.SetActive(false);
        DestroyButton.SetActive(false);
        ItemNameLabel.enabled = false;
        ItemDescriptionLabel.enabled = false;
        //ItemSkillSprite.enabled = false;
        //ItemSkillDescriptionLabel.enabled = false;
    }
    
}

public enum InventoryUICategory
{
	None,
	All,
	Normal,
	Upgradeable,
	Useable,
	AllArmors,
	Head,
	Body,
	ArmL,
	ArmR,
	Legs,
	Food,
	Books,
	Quest
}
