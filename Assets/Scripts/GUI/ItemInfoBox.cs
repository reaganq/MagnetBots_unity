using UnityEngine;
using System.Collections;

public class ItemInfoBox : MonoBehaviour {

	public UISprite mainIcon;
	public UILabel descriptionLabel;
	public UILabel nameLabel;
	public UILabel priceLabel;

	public void LoadItemInfo(InventoryItem item)
	{
		RPGItem rpgItem = item.rpgItem;

		if(mainIcon != null)
		{
			GameObject atlas = Resources.Load(rpgItem.AtlasName) as GameObject;
			mainIcon.atlas = atlas.GetComponent<UIAtlas>();
			mainIcon.spriteName = rpgItem.IconPath;
		}

		if(descriptionLabel != null)
			descriptionLabel.text = rpgItem.Description;

		if(nameLabel != null)
			nameLabel.text = rpgItem.Name;

		if(priceLabel != null)
			priceLabel.text = rpgItem.BuyValue.ToString();

		if(rpgItem.ItemCategory == ItemType.Armor)
		{

		}
		else if(rpgItem.ItemCategory == ItemType.Quest)
		{

		}
	}
}
