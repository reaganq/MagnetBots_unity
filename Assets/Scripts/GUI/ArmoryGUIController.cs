using UnityEngine;
using System.Collections;

public class ArmoryGUIController : BasicGUIController {

    public UsableItem SelectedItem = null;
    public Inventory SelectedInventory = null;
    public int CurrentSelectedInventory = -1;
    public int CurrentSelectedItemIndex = -1;
    public ItemTileButton[] ItemTiles = new ItemTileButton[10];
    public GameObject[] CategoryButtons;
    //public UISprite[] ItemSprites = new UISprite[10];
    public GameObject InfoPanel = null;
    
    public GameObject UseButton = null;
    
    
    public GameObject EquipButton = null;
    public GameObject DestroyButton = null;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    public UILabel ItemSkillDescriptionLabel = null;
    //public UISprite ItemSkillSprite = null;
    
    public void Enable()
    {
        OnInventoryPressed(0);
        //Debug.Log("enable");   
    }
    
	public void OnBackButtonpressed()
    {
        GUIManager.Instance.HideInventory();
    }
    
    public void OnItemTilePressed(int index)
    {
        if(index == CurrentSelectedItemIndex || index >= SelectedInventory.Items.Count)
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
        
        if(SelectedInventory.Items[CurrentSelectedItemIndex].IsItemUsable)
        {
            UseButton.SetActive(true);
            DestroyButton.SetActive(true);
        }
        else
            UseButton.SetActive(false);
        
        if(SelectedInventory.Items[CurrentSelectedItemIndex].IsItemEquippable)
        {
            if(!SelectedInventory.Items[CurrentSelectedItemIndex].IsItemEquipped)
            {
                EquipButton.SetActive(true);
                DestroyButton.SetActive(true);
            }
            else
            {
                EquipButton.SetActive(false);
                DestroyButton.SetActive(false);
            }
            RPGArmor armor = (RPGArmor)SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem;
            if(armor.HasAbility)
            {
                //ItemSkillSprite.enabled = true;
                //ItemSkillSprite.spriteName = armor.AbilityIconPath;
                ItemSkillDescriptionLabel.enabled = true;
                ItemSkillDescriptionLabel.text = armor.AbilityString;
            }
        }
        else
            EquipButton.SetActive(false);
        
        
        ItemNameLabel.enabled = true;
        ItemNameLabel.text = SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem.Name;
        ItemDescriptionLabel.enabled = true;
        ItemDescriptionLabel.text = SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem.Description;
    }
    
    public void OnUseButtonPressed()
    {
        
    }
    
    public void OnEquipButtonPressed()
    {
        SelectedInventory.EquipItem(SelectedInventory.Items[CurrentSelectedItemIndex]);
        EquipButton.SetActive(false);
        RefreshSelection();
        UpdateInfoPanel();
    }
    
    public void OnDestroyButtonPressed()
    {
        SelectedInventory.RemoveItem(SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem);
        HideInfoPanel();
        RefreshInventoryIcons();
        ResetSelection();
    }
    
    public void OnInventoryPressed(int index)
    {
        
        if(CurrentSelectedInventory != index)
        {
            if(CurrentSelectedInventory >= 0)
                CategoryButtons[CurrentSelectedInventory].SetActive(true);
            if(index == 0)
            {
                SelectedInventory = PlayerManager.Instance.Hero.HeadInventory;
            }
            if(index == 1)
            {
                SelectedInventory = PlayerManager.Instance.Hero.BodyInventory;
            }
            if(index == 2)
            {
                SelectedInventory = PlayerManager.Instance.Hero.ArmLInventory;
            }
            if(index == 3)
            {
                SelectedInventory = PlayerManager.Instance.Hero.ArmRInventory;
            }
            if(index == 4)
            {
                SelectedInventory = PlayerManager.Instance.Hero.LegsInventory;
            }
            if(index == 5)
            {
                SelectedInventory = PlayerManager.Instance.Hero.Inventory;
            }
            ResetSelection();
            RefreshInventoryIcons();
            HideInfoPanel();
            CurrentSelectedInventory = index;
            CategoryButtons[CurrentSelectedInventory].SetActive(false);
        }
        
    }
    
    public void RefreshInventoryIcons()
    {
        for (int i = 0; i <  ItemTiles.Length; i++) {
            if(i >= SelectedInventory.Items.Count)
                ItemTiles[i].Hide();
            else
            {
                ItemTiles[i].Show();
                ItemTiles[i].Load(SelectedInventory.Items[i].rpgItem.AtlasName, SelectedInventory.Items[i].rpgItem.IconPath, SelectedInventory.Items[i].CurrentAmount);
            }
        }
    }
    
    public void RefreshSelection()
    {
        for (int i = 0; i <  ItemTiles.Length; i++) {
            if(i < SelectedInventory.Items.Count)
            {
                if(SelectedInventory.Items[i].IsItemEquipped)
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
            if(i < SelectedInventory.Items.Count)
            {
                if(SelectedInventory.Items[i].IsItemEquipped)
                    ItemTiles[i].Equip();
                else
                    ItemTiles[i].Unequip();
            }
            else
                ItemTiles[i].Unequip();
        }
    }
    
    public void HideInfoPanel()
    {
        UseButton.SetActive(false);
        EquipButton.SetActive(false);
        DestroyButton.SetActive(false);
        ItemNameLabel.enabled = false;
        ItemDescriptionLabel.enabled = false;
        //ItemSkillSprite.enabled = false;
        ItemSkillDescriptionLabel.enabled = false;
    }
    
}
