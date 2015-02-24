using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class RPGCurrency : BasicItem {
	
	public bool isPremium;

	[XmlIgnore]
	public int amount;

	public RPGCurrency()
	{
		preffix = "CURRENCY";
	}
}
