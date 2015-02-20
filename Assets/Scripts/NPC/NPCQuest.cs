using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class NPCQuest : NPCActivity {

	public int questID;

	[XmlIgnore]
	public RPGQuest quest{
		get{
			if(questID > 0)
			{
				return PlayerManager.Instance.data.GetQuestByID(questID);
			}
			else
				return null;
		}
	}

	public NPCQuest()
	{
		Name = string.Empty;
		SystemDescription = string.Empty;
		Description = string.Empty;
		preffix = "NPCQuest";
		activityType = NPCActivityType.Quest;
	}
}
