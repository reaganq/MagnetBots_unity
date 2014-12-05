using UnityEngine;
using System;
using System.Text;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable]
public class NPCService : NPCActivity {

	public NPCService()
	{
		Name = string.Empty;
		SystemDescription = string.Empty;
		Description = string.Empty;
		preffix = "SERVICE";
		activityType = NPCActivityType.Service;
	}
}
