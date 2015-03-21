using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Construction : MonoBehaviour {

	public RPGConstruction construction;
	public List<InventoryItem> requiredItems = new List<InventoryItem>();
	public int ID;
	public UIPlayTween itemTween;
	public ConstructionTile[] itemHexagons;
	public GameObject trigger;
	public Collider triggerCollider;
	public Collider dragDropCollider;
	public Transform targetCameraPos;
	public Vector3 endScale = Vector3.one;
	public Vector3 startScale = new Vector3(0.5f, 0.5f, 0.5f);
	public bool isInRange;
	public PhotonView myPhotonView;
	public GameObject constructionModel;
	public GameObject finishedModelPrefab;
	public GameObject finishedModel;
	public bool isConstructed;
	// Use this for initialization
	void Start () {
		myPhotonView = GetComponent<PhotonView>();
		construction = Storage.LoadById<RPGConstruction>(ID, new RPGConstruction());
		for (int i = 0; i < construction.requiredItems.Count; i++) {
			if(construction.requiredItems[i].PreffixTarget == PreffixType.ITEM)
			{
				InventoryItem newInventoryItem = new InventoryItem();
				newInventoryItem.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(construction.requiredItems[i].TaskTarget, new RPGItem()), construction.requiredItems[i].Tasklevel, construction.requiredItems[i].AmountToReach);
				requiredItems.Add(newInventoryItem);
            }
        }
		if(trigger)
		{
			triggerCollider = trigger.GetComponent<Collider>();
			triggerCollider.enabled = false;
			trigger.transform.localScale = startScale;
		}
		dragDropCollider.enabled = false;
	}

	public void OnTriggerEnter ( Collider other )
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if(other.gameObject == PlayerManager.Instance.avatarObject)
			{
				ShowTrigger();
				isInRange = true;
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
				ResetTrigger();
				isInRange = false;
			}
			//StartCoroutine("HideNPC");
		}
	}

	public IEnumerator ShowConstruction()
	{
		yield return new WaitForEndOfFrame();
		//PlayerManager.Instance.ActiveNPC = this;
		//Player.Instance.ActiveNPCName = character.Name;
		GUIManager.Instance.DisplayConstruction(this);
	}

	public void ShowTrigger()
	{
		TweenScale.Begin(trigger, 0.2f, endScale);
		triggerCollider.enabled = true;
	}

	public void ResetTrigger()
	{
		TweenScale.Begin(trigger, 0.2f, startScale);
		triggerCollider.enabled = false;
	}

	public void ShowConstructionItems()
	{
		//dragDropCollider.enabled = true;
		ResetTrigger();
		RefreshConstructionItems();
	}

	public void RefreshConstructionItems()
	{
		for (int i = 0; i < itemHexagons.Length; i++) {
			if(i < requiredItems.Count)
			{
				itemHexagons[i].LoadConstructionItem(requiredItems[i]);
				if(!itemHexagons[i].gameObject.activeSelf)
				{
					itemTween.tweenTarget = itemHexagons[i].gameObject;
					itemTween.Play(true);
				}
			}
            else
                itemHexagons[i].gameObject.SetActive(false);
        }
		CheckConstructionItems();
	}

	public void CheckConstructionItems()
	{
		if(requiredItems.Count <= 0)
		{
			isConstructed = true;
			Debug.LogWarning("we finished building this Arcade!");
		}
	}

	public void HideConstructionItems()
	{
		dragDropCollider.enabled = false;
	}

	public void DonateConstructionItem(InventoryItem item)
	{
		for (int i = 0; i < requiredItems.Count; i++) {
			if(requiredItems[i].UniqueItemId == item.UniqueItemId)
			{
				PlayerManager.Instance.Hero.RemoveItem(item, 1);
				requiredItems[i].CurrentAmount --;
				if(requiredItems[i].CurrentAmount <= 0)
					requiredItems.RemoveAt(i);
				RefreshConstructionItems();
				return;
			}
		}
	}

}
