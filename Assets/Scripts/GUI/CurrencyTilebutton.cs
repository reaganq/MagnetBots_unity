using UnityEngine;
using System.Collections;

public class CurrencyTilebutton : MonoBehaviour {

	public UILabel amountLabel;
	public UISprite iconSprite;

	public void Load(RPGCurrency currency)
	{
		iconSprite.spriteName = currency.IconPath;
		amountLabel.text = currency.amount.ToString();
	}
}
