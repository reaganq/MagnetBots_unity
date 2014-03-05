using UnityEngine;
using System.Collections;

public class NPCPOITrigger : MonoBehaviour {

	public NPC npc;

	public void ActivatePOI()
	{
		if(npc)
		{
			npc.StartCoroutine(npc.ShowNPC());
		}
	}
}
