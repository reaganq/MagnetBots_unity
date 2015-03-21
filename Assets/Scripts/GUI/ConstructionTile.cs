using UnityEngine;
using System.Collections;

public class ConstructionTile : MonoBehaviour {

	public UISprite icon;
	public UILabel quantityLabel;
	// Use this for initialization
	public void LoadConstructionItem(InventoryItem item)
	{
		GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = item.rpgItem.IconPath;
		quantityLabel.text = item.CurrentAmount.ToString();
	}
}
