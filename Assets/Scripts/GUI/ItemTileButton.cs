//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class ItemTileButton: UIDragDropItem
{
    public int index;
    
    public UISprite icon = null;
    public UISprite selectBorder = null;
	public UISprite Cover = null;
	public UISprite background = null;
	public UISprite equippedIndicator = null;
    public UILabel amountLabel = null;
	public UILabel LevelLabel = null;
    public bool IsEquipped = false;
    
    public Color EquippedColor = Color.cyan;
    public Color SelectedColor = Color.white;
	public UISprite mainSprite;

	public bool draggable;
	public ItemTileType itemTileType;

	private Vector2 lastPressDownPos;
	public float movementThreshold = 5;
	//bool mStarted = false;
	//bool mHighlighted = false;

	void OnPress (bool isPressed)
	{
		if (isPressed)
		{
			lastPressDownPos = UICamera.lastTouchPosition;
		}
		else 
		{
			if(Vector2.Distance(UICamera.lastTouchPosition, lastPressDownPos) < movementThreshold)
			{
				if(itemTileType == ItemTileType.Shop)
				{
					//GUIManager.Instance.ShopGUI
				}
			}
		}
	}
	
	protected override void StartDragging ()
	{
		if(draggable)
			base.StartDragging ();
	}
    
	protected override void OnDragDropEnd ()
	{
		if(draggable)
			base.OnDragDropEnd ();
	}

	protected override void OnDragDropRelease (GameObject surface)
	{
		if(draggable)
		{
			if(surface != null)
			{
				ExampleDragDropSurface dds = surface.GetComponent<ExampleDragDropSurface>();
				
				if (dds != null)
				{
					if(itemTileType == ItemTileType.quickInventory)
						GUIManager.Instance.QuickInventoryGUI.OnDragDrop(index);
					// Destroy this icon as it's no longer needed
					NGUITools.Destroy(gameObject);
					return;
				}
			}
			base.OnDragDropRelease (surface);
		}
	}

    public void Select()
    {
        //if(!selectBorder.enabled)
            //selectBorder.enabled = true;
        
    }
    
    public void Deselect()
    {
        //if(!IsEquipped && selectBorder.enabled)
            //selectBorder.enabled = false;
    }
    
    public void Show()
    {
        icon.enabled = true;
        amountLabel.enabled = true;
    }
    
    public void Hide()
    {
        icon.enabled = false;
        amountLabel.enabled = false;
        if(selectBorder.enabled)
            selectBorder.enabled = false;
		if(Cover != null && Cover.enabled)
			Cover.enabled = false;
		LevelLabel.enabled = false;
    }

	public void LoadQuickInventoryItem(InventoryItem item)
	{
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		if(!item.rpgItem.IsUpgradeable)
			LevelLabel.enabled = false;
		else
		{
			LevelLabel.text = item.Level.ToString();
			LevelLabel.enabled = true;
		}
		selectBorder.enabled = false;
		/*if(item.IsItemEquipped)
		{
			equippedIndicator.enabled = true;
		}*/
		amountLabel.text = item.CurrentAmount.ToString();
		amountLabel.enabled = true;
		draggable = true;
		itemTileType = ItemTileType.quickInventory;
	}

	public void LoadShopItem(InventoryItem item)
	{
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		if(!item.rpgItem.IsUpgradeable)
			LevelLabel.enabled = false;
		else
		{
			LevelLabel.text = item.Level.ToString();
			LevelLabel.enabled = true;
		}
		selectBorder.enabled = false;
		amountLabel.text = item.CurrentAmount.ToString();
		amountLabel.enabled = true;
		selectBorder.enabled = false;
		itemTileType = ItemTileType.Shop;
	}

	public void LoadInventoryItem(InventoryItem item)
	{
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		if(!item.rpgItem.IsUpgradeable)
			LevelLabel.enabled = false;
		else
		{
			LevelLabel.text = item.Level.ToString();
			LevelLabel.enabled = true;
		}
		selectBorder.enabled = false;
		if(item.IsItemEquipped)
		{
			equippedIndicator.enabled = true;
		}
		amountLabel.text = item.CurrentAmount.ToString();
		amountLabel.enabled = true;
		draggable = false;
		itemTileType = ItemTileType.Inventory;
	}
    
    public void Equip()
    {
		equippedIndicator.enabled = false;
    }
    
    public void Unequip()
    {
		equippedIndicator.enabled = false;
    }
}

public enum ItemTileType
{
	quickInventory,
	Inventory,
	Shop,
	Other,
}