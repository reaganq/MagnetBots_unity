using UnityEngine;
using System.Collections;

public class InstantAOEDetector : Detector {

	public float range;
	public int layerMask = 1<<10;

	// Use this for initialization
	public override void Activate ()
	{
		base.Activate ();
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, layerMask);
		int i = 0;
		while(i < hitColliders.Length)
		{
			/*Debug.Log(hitColliders[i].gameObject.name);
			HitBox hb = hitColliders[i].GetComponent<HitBox>();
			
			if(hb != null && hb.ownerCS != ownerSkill.owner)
			{
				ownerSkill.HitTarget(hb.core.ID, this.transform.position, hb.gameObject.transform.position);
				Debug.Log(hb.name + ownerSkill.owner.hitBox.name);
			}
			i++;*/
		}
		Debug.Log(hitColliders.Length);
	}
}
