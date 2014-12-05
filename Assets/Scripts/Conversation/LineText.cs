using UnityEngine;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;

[XmlInclude(typeof(Condition))]
[XmlInclude(typeof(ActionEvent))]
public class LineText : IItem {
	
	public string Text;
	
	public List<ActionEvent> Events;
	public List<Condition> Conditions;
	
	public LineText()
	{
		Text = string.Empty;
		Events = new List<ActionEvent>();
		Conditions = new List<Condition>();
	}
	
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
	
	protected string preffix = "LINETEXT";
	public string Preffix
	{
		get
		{
			return description;
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
	
	public void DoEvents()
	{		
		if (Events == null || Events.Count == 0)
			return;
		
		foreach(ActionEvent action in Events)
		{
			action.DoAction();
		}
	}
	
	public bool CanYouDisplay()
	{
		foreach(Condition condition in Conditions)
		{
			if (condition.Validate() == false)
				return false;
		}
		return true;
	}
}
