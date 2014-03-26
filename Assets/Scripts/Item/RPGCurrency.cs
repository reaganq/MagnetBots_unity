using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class RPGCurrency : RPGItem {

	public bool isPremium;

	public RPGCurrency()
	{
		Name = string.Empty;
		preffix = "CURRENCY";
		BuyCurrency = BuyCurrencyType.None;
		BuyValue = 0;
		Stackable = true;
		SellValue = 0;
		Rarity = RarityType.None;
	}
}
