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
	public BasicGUIController owner;
    public UISprite icon;
    public UISprite selectBorder;
	public UISprite Cover;
	public UISprite background;
	public UISprite equippedIndicator;
    public UILabel amountLabel;
	public UILabel levelLabel;
	public UISprite newItemGlow;
    public bool IsEquipped = false;
    
    public Color EquippedColor = Color.cyan;
    public Color SelectedColor = Color.white;
	public UISprite mainSprite;

	public bool draggable;
	public InventoryGUIType itemTileType;

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
				owner.OnItemTilePressed(index);
				Debug.Log("PRESSED");
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
					owner.OnDragDrop(index);
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
		levelLabel.enabled = false;
    }

	public void LoadGeneric(InventoryItem item)
	{
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		if(!item.rpgItem.IsUpgradeable)
			levelLabel.enabled = false;
		else
		{
			levelLabel.text = item.Level.ToString();
			levelLabel.enabled = true;
		}
		amountLabel.text = item.CurrentAmount.ToString();
		amountLabel.enabled = true;
		selectBorder.enabled = false;
	}

	public void LoadItemTile(InventoryItem item, BasicGUIController newOwner, InventoryGUIType type, int i)
	{
		index = i;
		owner = newOwner;
		LoadGeneric(item);
		itemTileType = type;
		switch (type)
		{
		case InventoryGUIType.Inventory:
			draggable = false;
			newItemGlow.enabled = !item.isItemViewed;
			break;
		case InventoryGUIType.quickInventory:
			draggable = true;
			newItemGlow.enabled = false;
			break;
		case InventoryGUIType.Shop:
			draggable = false;
			newItemGlow.enabled = false;
			break;
		}
	}
	
	public void setDraggable(bool state)
	{
		draggable = state;
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