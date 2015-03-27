using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Construction : MonoBehaviour {

	public RPGConstruction construction;
	public RPGNPC constructedNPC;
	public List<InventoryItem> requiredItems = new List<InventoryItem>();
	public List<int> requiredItemsQuantity = new List<int>();
	public int ID;
	public int NPCID;
	public UIPlayTween itemTween;
	public ConstructionTile[] itemHexagons;
	public GameObject trigger;
	public GameObject constructedTrigger;
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
	public GameObject decal;
	public bool isConstructed;
	public List<DonorsObject> donors;
	// Use this for initialization
	void Start () {
		myPhotonView = GetComponent<PhotonView>();
		construction = Storage.LoadById<RPGConstruction>(ID, new RPGConstruction());
		constructedNPC = Storage.LoadById<RPGNPC>(NPCID, new RPGNPC());
		if(construction != null)
		{
			GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().AddConstruction(construction);
		}
		for (int i = 0; i < construction.requiredItems.Count; i++) {
			if(construction.requiredItems[i].PreffixTarget == PreffixType.ITEM)
			{
				InventoryItem newInventoryItem = new InventoryItem();
				newInventoryItem.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(construction.requiredItems[i].TaskTarget, new RPGItem()), construction.requiredItems[i].Tasklevel, construction.requiredItems[i].AmountToReach);
				requiredItems.Add(newInventoryItem);
				requiredItemsQuantity.Add(construction.requiredItems[i].AmountToReach);
            }
        }
		if(trigger)
		{
			triggerCollider = trigger.GetComponent<Collider>();
			trigger.transform.localScale = startScale;
		}
		dragDropCollider.enabled = false;
		if(!PhotonNetwork.isMasterClient)
			RequestConstructionUpdate();
	}
	
	public void DonateConstructionItem(InventoryItem item, string playerName)
	{
		if(!PhotonNetwork.isMasterClient)
		{
			for (int i = 0; i < requiredItems.Count; i++) {
				if(requiredItems[i].UniqueItemId == item.UniqueItemId)
					requiredItems[i].CurrentAmount --;
					}
			RefreshConstructionItems(true);
		}
		myPhotonView.RPC("NetworkAddDonation", PhotonTargets.MasterClient, item.rpgItem.ID, playerName);
	}

	[RPC]
	public void NetworkAddDonation(int itemID, string playerName)
	{
		for (int i = 0; i < requiredItems.Count; i++) {
			if(requiredItems[i].rpgItem.ID == itemID && requiredItems[i].CurrentAmount > 0)
			{
				PlayerManager.Instance.Hero.RemoveItem(requiredItems[i], 1);
				requiredItems[i].CurrentAmount --;
				break;
			}
		}
		bool isNewDonor = true;
		for (int i = 0; i < donors.Count; i++) {
			if(donors[i].playerName == playerName)
				donors[i].donationsQuantity ++;
		}
		if(isNewDonor)
			donors.Add(new DonorsObject(playerName));
		UpdateConstructionStatus();
	}
	
	public void UpdateConstructionStatus()
	{
		ConstructionDonationInfo data = new ConstructionDonationInfo();
		for (int i = 0; i < requiredItems.Count; i++) {
			data.itemQuantities.Add(requiredItems[i].CurrentAmount);
		}
		data.donors = donors;
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, data);
		myPhotonView.RPC("NetworkUpdateConstructionStatus", PhotonTargets.Others, m.GetBuffer());
		RefreshConstructionItems(false);
	}

	[RPC]
	public void NetworkUpdateConstructionStatus(byte[] data)
	{
		ConstructionDonationInfo newConstructionInfo = new ConstructionDonationInfo();
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream(data);
		newConstructionInfo = (ConstructionDonationInfo)b.Deserialize(m);
		for (int i = 0; i < newConstructionInfo.itemQuantities.Count; i++) {
			requiredItems[i].CurrentAmount = newConstructionInfo.itemQuantities[i];
		}
		donors.Clear();
		for (int i = 0; i < newConstructionInfo.donors.Count; i++) {
			donors.Add(new DonorsObject(newConstructionInfo.donors[i].playerName, newConstructionInfo.donors[i].hasAlerted, newConstructionInfo.donors[i].donationsQuantity));
		}
		RefreshConstructionItems(false);
	}
	
	public void RequestConstructionUpdate()
	{
		myPhotonView.RPC("NetworkRequestConstructionUpdate", PhotonTargets.MasterClient);
	}
	
	[RPC]
	public void NetworkRequestConstructionUpdate(PhotonMessageInfo info)
	{
		ConstructionDonationInfo data = new ConstructionDonationInfo();
		for (int i = 0; i < requiredItems.Count; i++) {
			data.itemQuantities.Add(requiredItems[i].CurrentAmount);
		}
		data.donors = donors;
		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, data);
		myPhotonView.RPC("NetworkUpdateConstructionStatus", info.sender, m.GetBuffer());
	}

	public void OnTriggerEnter ( Collider other )
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if(other.gameObject == PlayerManager.Instance.avatarObject)
			{
				if(!isInRange)
					ShowTrigger();
				isInRange = true;
			}
		}
	}
	
	public void OnTriggerExit ( Collider other )
	{
		if(other.gameObject.CompareTag("Player"))
		{
			if(other.gameObject == PlayerManager.Instance.avatarObject)
			{
				if(isInRange)
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
		if(!isConstructed)
		{
		TweenScale.Begin(trigger, 0.2f, endScale);
		//triggerCollider.enabled = true;
		}
		else
		{
			TweenScale.Begin(constructedTrigger, 0.2f, endScale);
			//triggerCollider.enabled = true;
		}
	}

	public void ResetTrigger()
	{
		if(!isConstructed)
		{
			TweenScale.Begin(trigger, 0.2f, startScale);
			//triggerCollider.enabled = false;
		}
		else
			TweenScale.Begin(constructedTrigger, 0.2f, startScale);
	}

	public void ShowConstructionItems()
	{
		//dragDropCollider.enabled = true;
		ResetTrigger();
		RefreshConstructionItems(true);
	}

	public void RefreshConstructionItems(bool uiUpdateOnly)
	{
		for (int i = 0; i < itemHexagons.Length; i++) {
			if(i < requiredItems.Count)
			{
				if(requiredItems[i].CurrentAmount > 0)
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
            else
                itemHexagons[i].gameObject.SetActive(false);
        }
		GUIManager.Instance.constructionGUI.UpdateMaterialsCounters();
		if(!uiUpdateOnly)
			CheckConstructionItems();
	}

	public void CheckConstructionItems()
	{
		bool _isConstructed = true;
		for (int i = 0; i < requiredItems.Count; i++) {
			if(requiredItems[i].CurrentAmount > 0)
				_isConstructed = false;
		}
		if(_isConstructed)
		{
			isConstructed = true;
			Debug.LogWarning("we finished building this Arcade!");
			StartCoroutine(ConstructSequence());
		}

	}

	public IEnumerator ConstructSequence()
	{
		finishedModel = GameObject.Instantiate(finishedModelPrefab, constructionModel.transform.position, constructionModel.transform.rotation) as GameObject;
		Destroy(constructionModel);
		yield return new WaitForSeconds(0.4f);
		GameObject particle = GameObject.Instantiate(decal, finishedModel.transform.position, decal.transform.rotation) as GameObject;
		yield return new WaitForSeconds(1f);
		DisplayCongratulationsMessage();
	}

	public void DisplayCongratulationsMessage()
	{
		GUIManager.Instance.NotificationGUI.DisplayMessageBox("Congratulations! You just helped to construct the Arcade!");
	}

	public void HideConstructionItems()
	{
		dragDropCollider.enabled = false;
		for (int i = 0; i < itemHexagons.Length; i++) {
			itemHexagons[i].gameObject.SetActive(false);
		}
		if(isInRange)
			ShowTrigger();
	}



}

[Serializable]
public class DonorsObject
{
	public string playerName;
	public bool hasAlerted = false;
	//all donations are treated equal, in the future donations will be weighted by value of pieces
	public int donationsQuantity;

	public DonorsObject(string name)
	{
		playerName = name;
		hasAlerted = false;
		donationsQuantity = 1;
	}

	public DonorsObject(string name, bool alerted, int quantity)
	{
		playerName = name;
		hasAlerted = true;
		donationsQuantity = quantity;
	}
}

[Serializable]
public class ConstructionDonationInfo
{
	public List<int> itemQuantities;
	public List<DonorsObject> donors;

	public ConstructionDonationInfo()
	{
		itemQuantities = new List<int>();
		donors = new List<DonorsObject>();
	}
}
