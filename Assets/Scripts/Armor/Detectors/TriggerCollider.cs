using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCollider : Detector {

	public void OnTriggerEnter(Collider other)
    {
		if(currentNumberOfTargets >= ownerSkill.targetLimit)
			return;
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		Vector3 hitPos = other.collider.ClosestPointOnBounds(ownerSkill.ownerTransform.position);
		//ContactPoint contact = other.contacts[0];
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;
			if(cs == ownerSkill.ownerStatus)
				return;

			ownerSkill.HitTarget(cs, this.transform.position, cs._myTransform.position);
        }
    }
}
