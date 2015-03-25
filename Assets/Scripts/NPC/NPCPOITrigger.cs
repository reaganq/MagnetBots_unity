using UnityEngine;
using System.Collections;

public class NPCPOITrigger : MonoBehaviour {

	public NPC npc;
	public Construction construction;

	public void ActivatePOI()
	{
		if(npc)
		{
			npc.StartCoroutine(npc.ShowNPC());
			Debug.Log("hit");
			return;
		}
		if(construction)
			construction.StartCoroutine(construction.ShowConstruction());
	}
}
