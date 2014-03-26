using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardsGUIController : BasicGUIController {

	public string goldIconPath;
	public string goldAtlasName;
	public string gemIconPath;
	public string gemAtlasName;

	public GameObject parentObject;
	public ItemTileButton[] itemTiles;

	public  override void Enable()
	{
		parentObject.SetActive(true);
	}

	public override void Disable()
	{
		parentObject.SetActive(false);
	}

	public void PopulateRewards(List<LootItem> rewardItems)
	{
		for (int i = 0; i < rewardItems.Count; i++) 
		{
			//rewardQuantityLabels
		}
	}

	public void Continue()
	{
		Disable();
	}
}
