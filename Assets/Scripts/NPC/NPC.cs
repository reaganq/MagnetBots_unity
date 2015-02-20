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
	//public NPCArena arena = null;
	//public NPCActivity activity = null;
	//public NPCMinigame miniGame = null;
	public List<NPCActivity> activities;
	public List<NPCActivity> _availableActivities;
	public List<NPCActivity> availableActivities
	{
		get{
			_availableActivities.Clear();
			for (int i = 0; i < activities.Count; i++) {
				if(activities[i].Validate())
				{

					_availableActivities.Add(activities[i]);
				}
					
			}
			return _availableActivities;
		}
	}
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
		character = GeneralData.GetNPCByID(ID);
		foreach(NPCActivityData ad in character.activities)
		{
			if(ad.activityType == NPCActivityType.Minigame)
			{
				NPCMinigame minigame = Storage.LoadById<NPCMinigame>(ad.activityID, new NPCMinigame());
				Debug.Log(minigame.activityType.ToString());
				activities.Add(minigame);
			}
			else if(ad.activityType == NPCActivityType.Quest)
			{
				NPCQuest quest = Storage.LoadById<NPCQuest>(ad.activityID, new NPCQuest());
				activities.Add(quest);
			}
			else if(ad.activityType == NPCActivityType.Shop)
			{
				Shop shop = Storage.LoadById<Shop>(ad.activityID, new Shop());
				shop.PopulateItems();
				activities.Add(shop);
			}
			else if(ad.activityType == NPCActivityType.Service)
			{
				NPCService service = Storage.LoadById<NPCService>(ad.activityID, new NPCService());
				activities.Add(service);
			}
			else if(ad.activityType == NPCActivityType.Arena)
			{
				NPCArena arena = Storage.LoadById<NPCArena>(ad.activityID, new NPCArena());
				arena.LoadArena();
				arena.activityType = NPCActivityType.Arena;
				activities.Add(arena);
				Debug.Log(arena.activityType + " " + arena.Enemies.Count);
				Debug.LogWarning("adding arena" + arena.Name);
			}
			else if(ad.activityType == NPCActivityType.Teleporter)
			{
				NPCActivity teleporter = new NPCActivity();
				teleporter.activityType = NPCActivityType.Teleporter;
				teleporter.Name = "Teleporter";
				activities.Add(teleporter);
			}
		}

		/*if(character.ArenaID > 0)
		{
			arena = Storage.LoadById<NPCArena>(character.ArenaID, new NPCArena());

				for (int j = 0; j < arena.EnemyIDs.Count; j++) 
				{
					arena.Enemies.Add(Storage.LoadById<RPGEnemy>(arena.EnemyIDs[j], new RPGEnemy()));
				}
		}
		if(character.MinigameID > 0)
		{
			miniGame = Storage.LoadById<NPCMinigame>(character.MinigameID, new NPCMinigame());

		}

		if(character.ActivityID > 0)
		{
			activity = Storage.LoadById<NPCActivity>(character.ActivityID, new NPCActivity());
		}*/
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
        //PlayerManager.Instance.ActiveNPC = this;
        //Player.Instance.ActiveNPCName = character.Name;
		GUIManager.Instance.DisplayNPC(this);
    }
    
    public IEnumerator HideNPC()
    {
        yield return new WaitForEndOfFrame();
        //PlayerManager.Instance.ActiveNPC = null;
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
