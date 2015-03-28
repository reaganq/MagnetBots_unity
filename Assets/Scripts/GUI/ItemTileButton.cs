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
	public RarityColor[] rarityColors;
    public int index;
	public BasicGUIController owner;
	public GameObject[] gradeStars;
    public UISprite icon;
    public UISprite tile;
	public UISprite Cover;
	public UISprite background;
	public UISprite equippedIndicator;
    public UILabel amountLabel;
	public GameObject quantityTag;

	public UISprite newItemGlow;
	public UISprite tickIcon;
    public bool canDisplayTick = false;
	public bool canDisplayNew = false;
	public bool canDisplayQuantity = true;
    //public Color EquippedColor = Color.cyan;
    //public Color SelectedColor = Color.white;
	public UISprite mainSprite;
	public InventoryItem referencedItem;

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
				if(owner != null)
					owner.OnItemTilePressed(index);
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
        quantityTag.SetActive(true);
    }
    
    public void Hide()
    {
        icon.enabled = false;
		quantityTag.SetActive(false);
		if(Cover != null && Cover.enabled)
			Cover.enabled = false;
    }

	public void LoadGeneric(InventoryItem item)
	{
		if(!icon.enabled)
			icon.enabled = true;
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		amountLabel.text = item.CurrentAmount.ToString();
		quantityTag.SetActive(canDisplayQuantity);
		LoadItemRarity(item.rpgItem);
		if(canDisplayNew)
		{
			newItemGlow.enabled = !item.isItemViewed;
		}
		else
			newItemGlow.enabled = false;
		if(canDisplayTick)
		{
			if(item.IsItemEquipped)
				tickIcon.enabled = true;
			else
				tickIcon.enabled = false;
		}
		else 
			tickIcon.enabled = false;
		for (int i = 0; i < gradeStars.Length; i++) {
			gradeStars[i].SetActive(false);
		}
		if(item.rpgItem.IsUpgradeable)
		{
			for (int i = 0; i < item.Level; i++) {
				gradeStars[i].SetActive(true);
			}
		}


	}

	public void LoadQuestDisplayTile(InventoryItem item, bool isNewQuest)
	{
		LoadGeneric(item);
		newItemGlow.enabled = false;
		if(!isNewQuest)
		{
			amountLabel.text = PlayerManager.Instance.Hero.GetItemAmount(item, false)+"/"+item.CurrentAmount;
			if(PlayerManager.Instance.Hero.GetItemAmount(item, false) >= item.CurrentAmount)
				tickIcon.enabled = true;
			else
				tickIcon.enabled = false;
		}
		else
			tickIcon.enabled = false;
		//check if i have enough
	}

	public void LoadItemTile(InventoryItem item, BasicGUIController newOwner, InventoryGUIType type, int i)
	{
		index = i;
		owner = newOwner;
		switch (type)
		{
		case InventoryGUIType.Inventory:
			draggable = false;
			canDisplayNew = true;
			canDisplayTick = true;
			canDisplayQuantity = true;
			break;
		case InventoryGUIType.quickInventory:
			if(item.rpgItem.ItemCategory == ItemType.Armor || item.rpgItem.ItemCategory == ItemType.NakedArmor)
				canDisplayQuantity = false;
			else
				canDisplayQuantity = true;
			draggable = true;
			canDisplayNew = false;
			canDisplayTick = true;

			break;
		case InventoryGUIType.Shop:
			draggable = false;
			canDisplayNew = false;
			canDisplayTick = false;
			canDisplayQuantity = true;
			break;
		case InventoryGUIType.Reward:
			draggable = false;
			canDisplayNew = false;
			canDisplayTick = false;
			canDisplayQuantity = false;
			break;
		case InventoryGUIType.Other:
			draggable = true;
			canDisplayNew = false;
			canDisplayTick = true;
			break;
		}
		LoadGeneric(item);
		itemTileType = type;
	}

	public void LoadItemRarity(RPGItem item)
	{
		for (int i = 0; i < rarityColors.Length; i++) {
			if(rarityColors[i].rarity == item.Rarity)
			{
				tile.color = rarityColors[i].tintColor;
				return;
			}
		}
		tile.color = Color.white;
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

	public void LoadBasicAmor(string uniqueID, int level)
	{
		RPGArmor item = Storage.LoadbyUniqueId<RPGArmor>(uniqueID, new RPGArmor());
		GameObject atlas = Resources.Load(item.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.IconPath;
		for (int i = 0; i < gradeStars.Length; i++) {
			gradeStars[i].SetActive(false);
		}
		if(item.IsUpgradeable)
		{
			for (int i = 0; i < level; i++) {
				gradeStars[i].SetActive(true);
			}
		}
		tickIcon.enabled = false;
		quantityTag.SetActive(false);
		LoadItemRarity(item);
	}
}