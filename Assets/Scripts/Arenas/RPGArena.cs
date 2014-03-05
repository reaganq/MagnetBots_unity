using UnityEngine;
using System;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

//[XmlInclude(typeof(ShopCategory))]
[XmlInclude(typeof(ShopItem))]
public class RPGArena : BasicItem
{
	public List<string> Enemies;
	
	public RPGArena()
	{
		Enemies = new List<string>();
		Name = string.Empty;
		Description = string.Empty;
		preffix = "ARENA";
	}
	

}