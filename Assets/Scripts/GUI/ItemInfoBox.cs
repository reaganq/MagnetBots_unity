using UnityEngine;
using System.Collections;

public class ItemInfoBox : MonoBehaviour {

	public UISprite icon;
	//public UILabel descriptionLabel;
	public UILabel nameLabel;
	public UILabel priceLabel;
	public UILabel rarityLabel;
	public UISprite currencyIcon;
	public GameObject quantityTag;
	public UILabel quantityLabel;
	public GameObject tickIcon;
	public GameObject[] stars;

	public void LoadItemInfo(InventoryItem item)
	{
		if(icon != null)
		{
			GameObject atlas = Resources.Load(item.rpgItem.AtlasName) as GameObject;
			icon.atlas = atlas.GetComponent<UIAtlas>();
			icon.spriteName = item.rpgItem.IconPath;
		}
		if(nameLabel != null)
			nameLabel.text = item.rpgItem.Name;
		for (int i = 0; i < stars.Length; i++) {
			stars[i].SetActive(false);
				}
		if(item.rpgItem.IsUpgradeable)
		{
			for (int i = 0; i < item.Level; i++) {
				stars[i].SetActive(true);
			}
		}


		if(item.IsItemEquippable && tickIcon != null)
		{
			if(item.IsItemEquipped)
			{
				tickIcon.SetActive(true);
			}
			else
			{
				tickIcon.SetActive(false);
			}
		}
		else
		{
			if(tickIcon != null)
			tickIcon.SetActive(false);
		}

		if(rarityLabel != null)
		{
			rarityLabel.color = GUIManager.Instance.GetRarityColor(item.rpgItem.Rarity);
			rarityLabel.text = item.rpgItem.Rarity.ToString();
		}

		if(quantityLabel != null)
			quantityLabel.text = item.CurrentAmount.ToString();

		if(currencyIcon != null)
		{
		if(item.rpgItem.BuyCurrency == BuyCurrencyType.CitizenPoints)
				currencyIcon.spriteName = GeneralData.citizenIconPath;
			else if(item.rpgItem.BuyCurrency == BuyCurrencyType.Magnets)
				currencyIcon.spriteName = GeneralData.magnetIconPath;
			else if(item.rpgItem.BuyCurrency == BuyCurrencyType.Coins)
				currencyIcon.spriteName = GeneralData.coinIconPath;
		}
		if(priceLabel != null)
			priceLabel.text = item.rpgItem.BuyValue.ToString();

	}
}
