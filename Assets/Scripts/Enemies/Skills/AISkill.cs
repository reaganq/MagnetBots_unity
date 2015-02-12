using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PathologicalGames;

public class AISkill : BaseSkill {

	public Transform fireObject;
	public float cooldown;
	public int maxAmmoCount;
	public float fireSpeed;
	public float damage;
	public bool isLimitedUse;
	public float skillUsageLimit = Mathf.Infinity;
	private float usageCount;
	public bool resetAfterUse;
	//how likely we should use this attack
    public float weighting = 0f;
	public List<AISkillUseCondition> UseConditions;
	public List<AISkillRequirements> UseRequirements;

	//do we need a target for this skill?
    public int targetRequirement;
	public float skillRangeMax;
	public float skillRangeMin;

	public SimpleFSM ownerFSM;
	public bool requiresTargetLock;
	public bool requiresLineOfSight;
	public float angleTolerance = 15f;

	public Job useSkillJob;

	public virtual void Start()
	{
	}
	// Use this for initialization
	public virtual void InitialiseAISkill(CharacterStatus status, int skillIndex)
    {
		usageCount = 0;
		ownerStatus = status;
		ownerManager = status.actionManager;
		ownerFSM = (SimpleFSM)ownerManager;
		ownerTransform = status._myTransform;
		ownerAnimation = ownerFSM.myAnimation;
		skillID = skillIndex;
		SetupAnimations();
		BasicSetup();
    }
	
    public virtual void SetupAnimations()
    {
    }

	public virtual bool CanUseSkill()
	{
		if(usageCount > skillUsageLimit)
			return false;
		if(ownerFSM.NumTargetsInRange(skillRangeMax) < targetRequirement)
		{
			return false;
		}
		for (int i = 0; i < UseRequirements.Count; i++) {
			if(UseRequirements[i].requirementType == AISkillRequirementType.healthGreatherThan && ownerStatus.curHealth < UseRequirements[i].targetValue)
			{
				return false;
			}
			else if(UseRequirements[i].requirementType == AISkillRequirementType.healthLessThan && ownerStatus.curHealth > UseRequirements[i].targetValue)
			{
				return false;
			}
				}
		return true;
	}

	public void FulfillSkillConditions()
	{
		FulfillSkillConditionSequence();
	}

	public virtual void FulfillSkillConditionSequence()
	{
		for (int i = 0; i < UseConditions.Count; i++) {
			if(UseConditions[i].conditionType == AISkillUseConditionType.range)
			{
				ownerFSM.SetTargetDistance(UseConditions[i].targetValue1);
			}
			if(UseConditions[i].conditionType == AISkillUseConditionType.direction)
			{
				ownerFSM.SetTargetAngle(UseConditions[i].targetValue1);
			}
		}
		if(fireObject != null)
			ownerFSM.fireObject = fireObject;
	}

	public void UseSkill()
	{
		useSkillJob = Job.make(UseSkillSequence());
		useSkillJob.jobComplete += (wasKilled) =>
		{
			if(resetAfterUse)
				ResetSkill();
		};
	}

    public virtual IEnumerator UseSkillSequence()
    {
        yield return null;
    }

	public void CancelSkill()
	{
	}

    public virtual IEnumerator CancelSkillSequence()
    {
        yield return null;
    }

	public override void ResetSkill ()
	{
		base.ResetSkill ();
		ownerFSM.moveToTarget = false;
		ownerFSM.aimAtTarget = false;
	}

	/*public override void HitTarget(HitBox target, bool isAlly)
	{
		HitInfo newHit = new HitInfo();
		
		if(!isAlly)
		{
			newHit.sourceName = fsm.myStatus.characterName;
			newHit.hitPosX = fsm._transform.position.x;
			newHit.hitPosY = fsm._transform.position.y;
			newHit.hitPosZ = fsm._transform.position.z;
			newHit.skillEffects = new List<StatusEffectData>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].affectEnemy) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
				}
			}
			Debug.Log("no. of skill effects = " + newHit.skillEffects.Count);

			//TODO apply self buffs from characterstatus
			//TODO apply hitbox local buffs
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();
			b.Serialize(m, newHit);
			
			target.ownerCS.myPhotonView.RPC("ReceiveHit", PhotonTargets.All, m.GetBuffer());
			
			//target.ReceiveHit(newHit);
			//Debug.Log(finalhit.sourceName);
			Debug.Log("hitenemy");
		}
		else
		{
			Debug.Log("hitally");
		}
		//hb.ReceiveHit(newHit);
	}*/
	/*
	public virtual void HitTarget(HitBox target, bool isAlly, Vector3 originPos)
	{
		HitInfo newHit = new HitInfo();

		if(!isAlly)
		{
			newHit.sourceName = fsm.myStatus.characterName;
			//newHit.damage = damage;
			newHit.hitPosX = originPos.x;
			newHit.hitPosY = originPos.y;
			newHit.hitPosZ = originPos.z;
			newHit.skillEffects = new List<StatusEffectData>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].affectEnemy) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
				}
			}
			
			//TODO apply self buffs from characterstatus
			//TODO apply hitbox local buffs
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();
			b.Serialize(m, newHit);
			
			target.ownerCS.myPhotonView.RPC("ReceiveHit", PhotonTargets.All, m.GetBuffer());
			
			//target.ReceiveHit(newHit);
			//Debug.Log(finalhit.sourceName);
			Debug.Log("hitenemy");
		}
		else
		{
			Debug.Log("hitally");
		}
		//hb.ReceiveHit(newHit);
	}*/

	public void OverlapSphere(Vector3 location, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(location, radius);
		for (int i = 0; i < hitColliders.Length; i++) 
		{
			HitBox hb = hitColliders[i].gameObject.GetComponent<HitBox>();
			//ContactPoint contact = other.contacts[0];
			if(hb != null)
			{
				CharacterStatus cs = hb.ownerCS;
				if(cs != ownerFSM.myStatus)
				{
					if(!HitEnemies.Contains(cs) && !HitAllies.Contains(cs))
					{
						//determine if friend or foe
						if(cs.characterType == CharacterType.AI)
						{
							HitAllies.Add(cs);
							HitTarget(hb, true);
						}
						else
						{
							HitEnemies.Add(cs);
							HitTarget(hb, false);
							//masterAISkill.fsm.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, hitDecal.name, hitPos);
						}
					}
				}
			}
		}
	}
}

public class AISkillUseCondition
{
	public AISkillUseConditionType conditionType;
	public float targetValue1;
	public float targetValue2;
}

public enum AISkillUseConditionType
{
	range,
	direction,
}

public class AISkillRequirements
{
	public AISkillRequirementType requirementType;
	public float targetValue;
}

public enum AISkillRequirementType
{
	healthLessThan,
	healthGreatherThan,
}