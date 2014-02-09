//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class ItemTileButton: MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
	}
 
    public int index;
	public GameObject target;
	public string functionName;
	public Trigger trigger = Trigger.OnClick;
    
    public UISprite Icon = null;
    public UISprite Border = null;
    public UILabel AmountLabel = null;
    public bool IsEquipped = false;
    
    public Color EquippedColor = Color.cyan;
    public Color SelectedColor = Color.white;

	//bool mStarted = false;
	//bool mHighlighted = false;

	void Start () { //mStarted = true; 
    }

	//void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (((isPressed && trigger == Trigger.OnPress) ||
				(!isPressed && trigger == Trigger.OnRelease))) Send();
		}
	}

	void OnClick () { if (enabled && trigger == Trigger.OnClick) Send(); }

	void OnDoubleClick () { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }

	void Send ()
	{
        //Debug.Log("send");
		if (string.IsNullOrEmpty(functionName)) return;
		//if (target == null) target = gameObject;
  
		    target.SendMessage(functionName, index, SendMessageOptions.DontRequireReceiver);

	}
    
    public void Select()
    {
        if(!Border.enabled)
            Border.enabled = true;
        
    }
    
    public void Deselect()
    {
        if(!IsEquipped && Border.enabled)
            Border.enabled = false;
    }
    
    public void Show()
    {
        Icon.enabled = true;
        AmountLabel.enabled = true;
    }
    
    public void Hide()
    {
        Icon.enabled = false;
        AmountLabel.enabled = false;
        if(Border.enabled)
            Border.enabled = false;
    }
    
    public void Load(string atlaspath, string iconpath, int amount )
    {
        if(!Icon.enabled)
            Icon.enabled = true;
        GameObject atlas = Resources.Load(atlaspath) as GameObject;
        Icon.atlas = atlas.GetComponent<UIAtlas>();
        Icon.spriteName = iconpath;
        AmountLabel.text = amount.ToString();
    }
    
    public void Equip()
    {
        Border.color = EquippedColor;
        Select();
        IsEquipped = true;
    }
    
    public void Unequip()
    {
        Border.color = SelectedColor;
        IsEquipped = false;
        Deselect();
    }
    
    
}