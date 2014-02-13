using UnityEngine;
using System.Collections;

public class ShopGUIController : BasicGUIController {

    public UsableItem SelectedItem = null;
    //public Inventory SelectedInventory = null;
    
    //public int CurrentSelectedInventory = -1;
    public int CurrentSelectedItemIndex = -1;
    public ItemTileButton[] ItemTiles = new ItemTileButton[10];
    //public GameObject[] CategoryButtons;
    //public UISprite[] ItemSprites = new UISprite[10];
    public GameObject InfoPanel = null;
    
    //public GameObject UseButton = null;
    
    
    public GameObject BuyButton = null;
    public GameObject SellButton = null;
    
    public UILabel ItemNameLabel = null;
    public UILabel ItemDescriptionLabel = null;
    public UILabel ItemSkillDescriptionLabel = null;
    
    public ShopMode CurrentShopMode = ShopMode.buy;
    //public UISprite ItemSkillSprite = null;
    
    public void Enable()
    {
        //OnInventoryPressed(0);
        ResetSelection();
        RefreshInventoryIcons();
        DefaultInfoPanel();
        Debug.Log("enable");   
    }
    
	public void OnBackButtonpressed()
    {
        GUIManager.Instance.HideInventory();
    }
    
    public void OnItemTilePressed(int index)
    {
        if(index == CurrentSelectedItemIndex || index >= PlayerManager.Instance.ActiveShop.ShopItems.Count)
        {
            Debug.Log("return");
            return;
        }
        else
        {
            if(CurrentSelectedItemIndex>-1)
                ItemTiles[CurrentSelectedItemIndex].Deselect();
            ItemTiles[index].Select();
            CurrentSelectedItemIndex = index;
            UpdateInfoPanel();
            if(CurrentShopMode == ShopMode.buy)
                BuyButton.SetActive(true);
            else
                SellButton.SetActive(true);
            //Player.Instance.currentItem 
        }
    }
    
    public void UpdateInfoPanel()
    {
        if(CurrentShopMode == ShopMode.buy)
        {
            if(PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex] != null)
            {
                BuyButton.SetActive(true);
                SellButton.SetActive(false);
                /*ItemNameLabel.enabled = true;
                ItemNameLabel.text = PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem.Name;
                ItemDescriptionLabel.enabled = true;
                ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem.Description;
                */
            }
        }
        else
        {
            if(PlayerManager.Instance.Hero.Inventory.Items[CurrentSelectedItemIndex] != null)
            {
                SellButton.SetActive(true);
                BuyButton.SetActive(false);
				/*
                ItemNameLabel.enabled = true;
                ItemNameLabel.text = PlayerManager.Instance.Hero.Inventory.Items[CurrentSelectedItemIndex].rpgItem.Name;
                ItemDescriptionLabel.enabled = true;
                ItemDescriptionLabel.text = PlayerManager.Instance.Hero.Inventory.Items[CurrentSelectedItemIndex].rpgItem.Description;
                */
            }
        }
        /*if(SelectedInventory.Items[CurrentSelectedItemIndex].IsItemUsable )
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
        ItemDescriptionLabel.text = SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem.Description;*/
    }
    
    public void OnBuyButtonPressed()
    {
        BuyTransaction buyTransaction = PlayerManager.Instance.ActiveShop.BuyItem(PlayerManager.Instance.ActiveShop.ShopItems[CurrentSelectedItemIndex].rpgItem);
        if(buyTransaction == BuyTransaction.NotEnoughGold)
        {
            //display not enough gold message
            return;
        }
        
        if(buyTransaction == BuyTransaction.NotEnoughSpaceInInventory)
        {
            //display not enough space message
            return;
        }
        
        if(buyTransaction == BuyTransaction.OK)
        {
            DefaultInfoPanel();
            //allgood;
        }
  
        RefreshInventoryIcons();
    }
    
    public void OnSellButtonPressed()
    {
        PlayerManager.Instance.ActiveShop.SellItem(PlayerManager.Instance.Hero.Inventory.Items[CurrentSelectedItemIndex].rpgItem);
        /*SelectedInventory.RemoveItem(SelectedInventory.Items[CurrentSelectedItemIndex].rpgItem);
        HideInfoPanel();
        RefreshInventoryIcons();
        ResetSelection();*/
    }
    
    public void OnInventoryPressed(int index)
    {
        
        /*if(CurrentSelectedInventory != index)
        {
            if(CurrentSelectedInventory >= 0)
                CategoryButtons[CurrentSelectedInventory].SetActive(true);
            if(index == 0)
            {
                SelectedInventory = Player.Instance.Hero.HeadInventory;
            }
            if(index == 1)
            {
                SelectedInventory = Player.Instance.Hero.BodyInventory;
            }
            if(index == 2)
            {
                SelectedInventory = Player.Instance.Hero.ArmLInventory;
            }
            if(index == 3)
            {
                SelectedInventory = Player.Instance.Hero.ArmRInventory;
            }
            if(index == 4)
            {
                SelectedInventory = Player.Instance.Hero.LegsInventory;
            }
            if(index == 5)
            {
                SelectedInventory = Player.Instance.Hero.Inventory;
            }
            ResetSelection();
            RefreshInventoryIcons();
            HideInfoPanel();
            CurrentSelectedInventory = index;
            CategoryButtons[CurrentSelectedInventory].SetActive(false);
        }
        */
    }
    
    public void RefreshInventoryIcons()
    {
        Debug.Log("refresh inventory");
        Debug.Log(PlayerManager.Instance.ActiveShop.ShopItems.Count);
        for (int i = 0; i <  ItemTiles.Length; i++) {
            if(i >= PlayerManager.Instance.ActiveShop.ShopItems.Count)
                ItemTiles[i].Hide();
            else
            {
                ItemTiles[i].Show();
                if(CurrentShopMode == ShopMode.buy)
                    ItemTiles[i].Load(PlayerManager.Instance.ActiveShop.ShopItems[i].rpgItem.AtlasName, PlayerManager.Instance.ActiveShop.ShopItems[i].rpgItem.IconPath, PlayerManager.Instance.ActiveShop.ShopItems[i].CurrentAmount);
                else
                    ItemTiles[i].Load(PlayerManager.Instance.Hero.Inventory.Items[i].rpgItem.AtlasName, PlayerManager.Instance.Hero.Inventory.Items[i].rpgItem.IconPath, PlayerManager.Instance.Hero.Inventory.Items[i].CurrentAmount);
            }
        }
    }
    
    public void RefreshSelection()
    {
        /*for (int i = 0; i <  ItemTiles.Length; i++) {
            if(i < SelectedInventory.Items.Count)
            {
                if(SelectedInventory.Items[i].IsItemEquipped)
                    ItemTiles[i].Equip();
                else
                    ItemTiles[i].Unequip();
            }
            else
                ItemTiles[i].Unequip();
        }*/
    }
    
    public void ResetSelection()
    {
        if(CurrentSelectedItemIndex != -1)
            ItemTiles[CurrentSelectedItemIndex].Deselect();
        CurrentSelectedItemIndex = -1;
        for (int i = 0; i <  ItemTiles.Length; i++) {
            ItemTiles[i].Unequip();
        }
        BuyButton.SetActive(false);
        SellButton.SetActive(false);
    }
    
    public void HideInfoPanel()
    {
        //UseButton.SetActive(false);
        BuyButton.SetActive(false);
        SellButton.SetActive(false);
        ItemNameLabel.enabled = false;
        ItemDescriptionLabel.enabled = false;
        //ItemSkillSprite.enabled = false;
        ItemSkillDescriptionLabel.enabled = false;
    }
    
    public void DefaultInfoPanel()
    {
        if(CurrentShopMode == ShopMode.buy)
        {
            ItemNameLabel.enabled = true;
            ItemNameLabel.text = PlayerManager.Instance.ActiveShop.Name;
            ItemDescriptionLabel.enabled = true;
            ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.Description;
            ItemSkillDescriptionLabel.enabled = false;
        }
        else
        {
            ItemNameLabel.enabled = true;
            ItemNameLabel.text = PlayerManager.Instance.ActiveShop.Name;
            ItemDescriptionLabel.enabled = true;
            ItemDescriptionLabel.text = PlayerManager.Instance.ActiveShop.Description;
            ItemSkillDescriptionLabel.enabled = false;
        }
    }
    
    
}

public enum ShopMode
{
    buy = 0,
    sell = 1,
}