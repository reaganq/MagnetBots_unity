using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCollider : Detector {

	public override void Initialise (BaseSkill skill)
	{
		base.Initialise (skill);
		IgnoreOwnCollisions();
	}

	public void IgnoreOwnCollisions()
	{
		for (int i = 0; i < ownerSkill.ownerStatus.hitboxes.Count; i++) 
		{
			Physics.IgnoreCollision(collider, ownerSkill.ownerStatus.hitboxes[i]);
		}
    }

	public void OnTriggerEnter(Collider other)
    {

			return;
		//Debug.Log("COLLISION" + other.gameObject.name);
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		Vector3 hitPos = other.collider.ClosestPointOnBounds(ownerSkill.ownerTransform.position);
		//ContactPoint contact = other.contacts[0];
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;

			if(cs == ownerSkill.ownerStatus || currentNumberOfTargets >= ownerSkill.targetLimit)
				return;

			/*int allyFlag;
			if(ownerSkill.ownerStatus.enemyCharacterType == cs.characterType)
				allyFlag = 2;
			else
				allyFlag = 1;
				*/

			/*if(isFoe)
			{
				if(!ownerSkill.HitEnemies.Contains(cs))
				{
					ownerSkill.HitEnemies.Add(cs);
				//masterAISkill.HitTarget(hb, true);
				}
			}
			else
			{
				if(!ownerSkill.HitAllies.Contains(cs))
				{
					ownerSkill.HitAllies.Add(cs);
				}
			}*/

			if(!ownerSkill.HitTargets.Contains(cs))
			{
				ownerSkill.HitTarget(cs, this.transform.position, cs._myTransform.position);
			}
        }
    }
    
}
