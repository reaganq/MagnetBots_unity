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
		//Debug.Log("COLLISION" + other.gameObject.name);
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		Vector3 hitPos = other.collider.ClosestPointOnBounds(ownerSkill.ownerTransform.position);
		//ContactPoint contact = other.contacts[0];
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;

			if(cs != ownerSkill.ownerStatus)
			{
				bool isFoe;
				if(ownerSkill.ownerStatus.enemyCharacterType == cs.characterType)
					isFoe = true;
				else
					isFoe = false;

				if(isFoe)
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
				}
			
				/*if(masterAISkill != null)
				{
					if(!masterAISkill.HitEnemies.Contains(cs) && !masterAISkill.HitAllies.Contains(cs))
					{
						//determine if friend or foe
						if(cs.characterType == masterAISkill.fsm.myStatus.enemyCharacterType)
						{
							masterAISkill.HitAllies.Add(cs);
							masterAISkill.HitTarget(hb, true);
						}
						else
						{
							masterAISkill.HitEnemies.Add(cs);
							masterAISkill.HitTarget(hb, false);
							Debug.Log("ai hit player");
							masterAISkill.fsm.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, hitDecal.name, hitPos);
						}
						//Debug.Log("I JUST HIT SOMETHING");
					}
				}
				if(masterArmor != null)
				{
					if(!masterArmor.HitEnemies.Contains(cs) && !masterArmor.HitAllies.Contains(cs))
                    {
                        //determine if friend or foe
						if(cs.characterType == masterArmor.owner.enemyCharacterType)
						{
                        	masterArmor.HitEnemies.Add(cs);
                        	masterArmor.HitTarget(hb, false);
							masterArmor.ownerManager.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, hitDecal.name, hitPos);
						}
						else
						{
							masterArmor.HitAllies.Add(cs);
							masterArmor.HitTarget(hb, true);
							masterArmor.ownerManager.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, hitDecal.name, hitPos);
						}
                        //Debug.Log("I JUST HIT SOMETHING");
                    }
                }*/
            }
        }
    }
    
}
