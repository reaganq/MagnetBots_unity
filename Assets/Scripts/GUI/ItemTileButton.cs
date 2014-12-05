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
	public GameObject target;
    
    public UISprite icon = null;
    public UISprite selectBorder = null;
	public UISprite Cover = null;
	public UISprite background = null;
    public UILabel amountLabel = null;
	public UILabel LevelLabel = null;
    public bool IsEquipped = false;
    
    public Color EquippedColor = Color.cyan;
    public Color SelectedColor = Color.white;
	public UISprite mainSprite;


	//bool mStarted = false;
	//bool mHighlighted = false;

	void Start () { //mStarted = true; 
    }

	//void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnPress (bool isPressed)
	{
		if(isPressed)
		{
			Debug.Log("pressed down on item tile");
		}
		else
			Debug.Log("released item tile");
	}

	/*void OnClick () { if (enabled && trigger == Trigger.OnClick) Send(); }

	void OnDoubleClick () { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }

	void Send ()
	{
        //Debug.Log("send");
		if (string.IsNullOrEmpty(functionName)) return;
		//if (target == null) target = gameObject;
  
		    target.SendMessage(functionName, index, SendMessageOptions.DontRequireReceiver);

	}*/
    
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

	public void Load(InventoryItem item)
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
		amountLabel.text = item.CurrentAmount.ToString();
		amountLabel.enabled = true;
	}

	public void LoadWithCover(InventoryItem item, bool condition)
	{
		Load(item);

		if(condition)
			Cover.enabled = false;
		else
			Cover.enabled = true;
	}
    
    public void Load(string atlaspath, string iconpath, int amount, bool displayLevel, int level )
    {
        if(!icon.enabled)
            icon.enabled = true;
        GameObject atlas = Resources.Load(atlaspath) as GameObject;
        icon.atlas = atlas.GetComponent<UIAtlas>();
        icon.spriteName = iconpath;
		if(!displayLevel)
			LevelLabel.enabled = false;
		else
		{
			LevelLabel.text = level.ToString();
			LevelLabel.enabled = true;
		}
        amountLabel.text = amount.ToString();
		amountLabel.enabled = true;
    }

	public void LoadWithCover(string atlaspath, string iconpath, int amount, bool displayLevel, int level, bool coverState )
	{
		Load(atlaspath, iconpath, amount, displayLevel, level);
		if(coverState)
			Cover.enabled = false;
		else
			Cover.enabled = true;
	}
    
    public void Equip()
    {
        //selectBorder.color = EquippedColor;
        Select();
        IsEquipped = true;
    }
    
    public void Unequip()
    {
        //selectBorder.color = SelectedColor;
        IsEquipped = false;
        Deselect();
    }

	public void SelectCategory()
	{
		mainSprite.color = SelectedColor;
	}

	public void DeselectCategory()
	{
		mainSprite.color = Color.grey;
	}
    
    
}