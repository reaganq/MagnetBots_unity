using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpeningCinematicGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject inventoryGridRoot;
	public int[] NakedArmorIDs = new int[] {1,2,3,4,5};
	public List<InventoryItem> NakedArmors = new List<InventoryItem>();
	public List<ItemTileButton> itemTiles;
	public InventoryGUIType inventoryType = InventoryGUIType.Other;

	public override void Enable()
	{
		base.Enable();
		LoadNakedArmors();
	}

	public void LoadNakedArmors()
	{
		base.Enable();
		for (int i = 0; i < NakedArmorIDs.Length; i++) {
			//load in naked armors
			RPGItem nakedArmor = Storage.LoadById<RPGItem>(NakedArmorIDs[i], new RPGItem());
			InventoryItem newItem = new InventoryItem();
			newItem.GenerateNewInventoryItem(nakedArmor, 1, 1);
			NakedArmors.Add(newItem);
			//newItem.GenerateNewInventoryItem(
		}
		LoadItemTiles(NakedArmors, itemTiles, inventoryGridRoot, itemTilePrefab, inventoryType);
	}

	public override void OnDragDrop(int index)
	{
		PlayerManager.Instance.Hero.AddItem(NakedArmors[index]);
	}

}
