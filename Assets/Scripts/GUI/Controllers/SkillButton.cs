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
    public delegate void OnClickEvent(int index);
    public static event OnClickEvent onSingleClick;
    
    public delegate void OnPressEvent(int index);
    public static event OnPressEvent onPress;
    
    public delegate void OnReleaseEvent(int index);
    public static event OnReleaseEvent onRelease;
 
	public int skillIndex;
	public UISprite skillIcon;
	
	public void SetupSkillButton(int index)
	{
		skillIndex = index;
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
					onPress(skillIndex);
                    //Debug.Log("press");
                }
             
            }
                
            if (!isPressed) 
            {
                if(onRelease != null)
                {
					onRelease(skillIndex);
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
				onSingleClick(skillIndex);
                //Debug.Log("click");
            }
        }
    }
}
