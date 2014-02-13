using UnityEngine;
using System;
using System.Collections;

public class NPC: MonoBehaviour
{
	public RPGNPC character;
    public int ID;
    public bool Active = false;
    public Shop thisShop = null;

    void Start()
    {
		character = Storage.LoadById<RPGNPC>(ID, new RPGNPC());
    }
    
    public void OnTriggerEnter ( Collider other )
    {
        if (other.gameObject.CompareTag("Player"))
        {
			if(other.gameObject == PlayerManager.Instance.avatarObject)
            	StartCoroutine("ShowNPC");
        }
    }
    
    public void OnTriggerExit ( Collider other )
    {
        if(Active && other.gameObject.CompareTag("Player"))
        {
			if(other.gameObject == PlayerManager.Instance.avatarObject)
            StartCoroutine("HideNPC");
            
        }
    }
    
    public IEnumerator ShowNPC()
    {
        yield return new WaitForEndOfFrame();
        Active = true;
        PlayerManager.Instance.ActiveNPC = this;
        //Player.Instance.ActiveNPCName = character.Name;
        if(character.ShopID > 0)
        {
            if(thisShop == null)
            {   
                thisShop = Storage.LoadById<Shop>(character.ShopID, new Shop());
            }
            thisShop.PopulateItems();
            PlayerManager.Instance.ActiveShop = thisShop;
        }
        GUIManager.Instance.DisplayNPC();
    }
    
    public IEnumerator HideNPC()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.Instance.ActiveNPC = null;
        if(PlayerManager.Instance.ActiveShop != null && character.ShopID == PlayerManager.Instance.ActiveShop.ID)
        {
            PlayerManager.Instance.ActiveShop = null;
            
        }
        
        GUIManager.Instance.HideNPC();
        
        Active = false;
    }

}
