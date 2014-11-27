//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

public class AttackButtonMessage : MonoBehaviour
{
   
    public delegate void OnClickEvent(int index);
    public static event OnClickEvent onSingleClick;
    
    public delegate void OnPressEvent(int index);
    public static event OnPressEvent onPress;
    
    public delegate void OnReleaseEvent(int index);
    public static event OnReleaseEvent onRelease;
 
    public int id;
    

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
                    onPress(id);
                    //Debug.Log("press");
                }
             
            }
                
            if (!isPressed) 
            {
                if(onRelease != null)
                {
                    onRelease(id);
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
                onSingleClick(id);
                //Debug.Log("click");
            }
        }
    }

 //void OnDoubleClick () { if (enabled) Send(); }

 /*void Send ()
 {
        Debug.Log("send");
     if (string.IsNullOrEmpty(functionName)) return;
     if (target == null) target = gameObject;

     if (includeChildren)
     {
         Transform[] transforms = target.GetComponentsInChildren<Transform>();

         for (int i = 0, imax = transforms.Length; i < imax; ++i)
         {
             Transform t = transforms[i];
             t.gameObject.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
         }
     }
     else
     {
         target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
     }
 }*/
}
