using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlInclude(typeof(LootItem))]
[XmlInclude(typeof(ActionEvent))]
[XmlInclude(typeof(Condition))]
public class RPGEnemy : IItem
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
	
	protected string preffix = "ENEMY";
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
	
	public string PortraitIcon;
	public string PortraitAtlas;
	public bool isAvailable;
	public List<int> prebattleConversations;
	public List<int> postbattleConversations;
	public List<LootItem> Loots;
	public List<string> PrefabPaths;
	public List<ActionEvent> preFightActions;
	public List<ActionEvent> postFightActions;
	public List<Condition> conditions;
	//HACK 
	public List<Condition> postActionConditions;

	public RPGEnemy()
	{
		PortraitIcon = string.Empty;
		Loots = new List<LootItem>();
		Name = string.Empty;

		SystemDescription = string.Empty;
		Description = string.Empty;
		PortraitAtlas = "Atlases/Enemy/EnemiesAtlas";
		preFightActions = new List<ActionEvent>();
		postFightActions = new List<ActionEvent>();
		conditions = new List<Condition>();
		postActionConditions = new List<Condition>();
		prebattleConversations = new List<int>();
		postbattleConversations = new List<int>();
	}

	public bool Validate()
	{
		for (int i = 0; i < conditions.Count; i++) {
			if(!conditions[i].Validate())
			{
				Debug.Log(conditions[i].ConditionType.ToString());
				return false;
			}
		}
		return true;
	}

	public bool ValidatePostAction()
	{
		for (int i = 0; i < postActionConditions.Count; i++) {
			if(!postActionConditions[i].Validate())
			{
				Debug.Log(postActionConditions[i].ConditionType.ToString());
				return false;
			}
		}
		return true;
	}
}




