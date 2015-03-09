//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

public class SkillButton : MonoBehaviour
{
    public delegate void OnClickEvent(int slot);
    public static event OnClickEvent onSingleClick;
    
    public delegate void OnPressEvent( int slot);
    public static event OnPressEvent onPress;
    
    public delegate void OnReleaseEvent( int slot);
    public static event OnReleaseEvent onRelease;
 
	public int skillIndex;
	public int slotIndex;
	public UISprite skillIcon;
	
	public void SetupSkillButton(int slot)
	{
		slotIndex = slot;
		if(slotIndex == 0)
		{
			if(!string.IsNullOrEmpty(PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.AbilityIconPath))
				skillIcon.spriteName = PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.AbilityIconPath;
		}
		else if(slotIndex == 1)
		{
			if(!string.IsNullOrEmpty(PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.AbilityIconPath))
				skillIcon.spriteName = PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.AbilityIconPath;
		}
	}

 //bool mStarted = false;
 //bool mHighlighted = false;

 void Start () 
    {
    }
        //{ mStarted = true; }

 //void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

 void OnPress (bool isPressed)
 {
     if (enabled)
     {
            if (isPressed)
            {
                if(onPress != null)
                {
					onPress(slotIndex);
                    //Debug.Log("press");
                }
             
            }
                
            if (!isPressed) 
            {
                if(onRelease != null)
                {
					onRelease(slotIndex);
                    //Debug.Log("release");
                }
            }
     }
 }

 void OnClick () 
    {
        if (enabled) 
        {
            if(onSingleClick != null)
            {
				onSingleClick(slotIndex);
                //Debug.Log("click");
            }
        }
    }
}
