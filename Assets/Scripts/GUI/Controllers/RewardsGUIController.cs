using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardsGUIController : BasicGUIController {

	public string goldIconPath;
	public string goldAtlasName;
	public string gemIconPath;
	public string gemAtlasName;
	public List<InventoryItem> items;
	public Transform root;
	public float offset;
	public GameObject panel;
	public ItemTileButton[] itemTiles;
	
	public  override void Enable()
	{
		panel.SetActive(true);
		PopulateRewards();
	}

	public override void Disable()
	{
		panel.SetActive(false);
	}

	public void PopulateRewards()
	{
		int index = 0;
		Debug.Log(items.Count);
		for (int i = 0; i < itemTiles.Length; i++) 
		{
			if(i < items.Count)
			{
				itemTiles[i].gameObject.SetActive(true);
				itemTiles[i].Load(items[i]);
				itemTiles[i].transform.localPosition = new Vector3(index*offset, 0 ,0);
				index ++;
				Debug.Log(i);
			}
			else
				itemTiles[i].gameObject.SetActive(false);
			//rewardQuantityLabels
		}
		root.localPosition = new Vector3(((index -1)*offset*-0.5f), -50 ,0);
	}

	public void Continue()
	{
		GUIManager.Instance.HideRewards();
		for (int i = 0; i < items.Count; i++) {
			PlayerManager.Instance.Hero.AddItem(items[i]);
		}
		if(PlayerManager.Instance.activityState == PlayerActivityState.arena)
		{
			PlayerManager.Instance.LeaveArena(PlayerManager.Instance.ActiveWorld.DefaultZone);
		}

		else if(PlayerManager.Instance.activityState == PlayerActivityState.minigame)
		{
			PlayerManager.Instance.EndMiniGame();
		}
	}
}
