using UnityEngine;
using System.Collections;

public class NPCPOITrigger : MonoBehaviour {

	public NPC npc;

	public void ActivatePOI()
	{
		if(npc)
		{
			Debug.Log("OPEN NPC");
			npc.StartCoroutine(npc.ShowNPC());
		}
	}
}
