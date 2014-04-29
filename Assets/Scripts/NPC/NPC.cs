using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NPC: MonoBehaviour
{
	public RPGNPC character;
    public int ID;
    public bool Active = false;
    public Shop thisShop = null;
	public RPGArena arena = null;
	public RPGActivity activity = null;
	public RPGMinigame miniGame = null;
	public GameObject trigger;
	public Collider triggerCollider;

	public Transform targetCameraPos;
	public float targetCameraFOV = 60;

	public Vector3 endScale = Vector3.one;
	public Vector3 startScale = new Vector3(0.5f, 0.5f, 0.5f);

    void Start()
    {
		if(trigger)
		{
			triggerCollider = trigger.GetComponent<Collider>();
			triggerCollider.enabled = false;
			trigger.transform.localScale = startScale;
		}
		character = Storage.LoadById<RPGNPC>(ID, new RPGNPC());

		if(character.ArenaID > 0)
		{
			arena = Storage.LoadById<RPGArena>(character.ArenaID, new RPGArena());

				for (int j = 0; j < arena.EnemyIDs.Count; j++) 
				{
					arena.Enemies.Add(Storage.LoadById<RPGEnemy>(arena.EnemyIDs[j], new RPGEnemy()));
				}
		}
		if(character.MinigameID > 0)
		{
			miniGame = Storage.LoadById<RPGMinigame>(character.MinigameID, new RPGMinigame());

		}

		if(character.ActivityID > 0)
		{
			activity = Storage.LoadById<RPGActivity>(character.ActivityID, new RPGActivity());
		}
    }
    
    public void OnTriggerEnter ( Collider other )
    {
        if (other.gameObject.CompareTag("Player"))
        {
			if(other.gameObject == PlayerManager.Instance.avatarObject)
			{
				TweenScale.Begin(trigger, 0.2f, endScale);
				triggerCollider.enabled = true;
            	//StartCoroutine("ShowNPC");
			}
        }
    }
    
    public void OnTriggerExit ( Collider other )
    {
        if(other.gameObject.CompareTag("Player"))
        {
			if(other.gameObject == PlayerManager.Instance.avatarObject)
			{
				Reset();
			}
            //StartCoroutine("HideNPC");
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
            //PlayerManager.Instance.ActiveShop = thisShop;
        }

        GUIManager.Instance.DisplayNPC();
    }
    
    public IEnumerator HideNPC()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.Instance.ActiveNPC = null;
        /*if(PlayerManager.Instance.ActiveShop != null && character.ShopID == PlayerManager.Instance.ActiveShop.ID)
        {
            PlayerManager.Instance.ActiveShop = null;
            
        }*/
        
        GUIManager.Instance.HideNPC();
        
        Active = false;
    }

	public void Reset()
	{
		TweenScale.Begin(trigger, 0.2f, startScale);
		triggerCollider.enabled = false;
	}

}
