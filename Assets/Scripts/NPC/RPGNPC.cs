using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

public class RPGNPC : IItem
{
	#region implementing interface IItem
	private int id;
	public int ID
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}
	
	private string _name;
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}
	
	private string description;
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}
	
	private string systemDescription;
	public string SystemDescription
	{
		get
		{
			return systemDescription;
		}
		set
		{
			systemDescription = value;
		}
	}
	
	protected string preffix = "NPC";
	public string Preffix
	{
		get
		{
			return preffix;
		}
	}
	
	public string UniqueId
	{
		get
		{
			return preffix + ID.ToString();
		}
	}
	#endregion
	
	//public int ShopID;
	//public int ArenaID;
	//public int MinigameID;
	//public int ActivityID;
	public List<NPCActivityData> activities;
	public List<NPCActivityData> overrideActivities;
	public int defaultConversationID;
	
	public RPGNPC()
	{
		Name = string.Empty;
		SystemDescription = string.Empty;
		Description = string.Empty;
		activities = new List<NPCActivityData>();
		overrideActivities = new List<NPCActivityData>();
	}
}

[Serializable]
public class NPCActivityData
{
	public int activityID;
	public NPCActivityType activityType;
}

