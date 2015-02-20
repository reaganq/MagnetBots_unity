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
	public bool fulfillConditionsBeforeUse;
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
		StartFulfillSkillConditions();
	}

	public virtual void StartFulfillSkillConditions()
	{
		for (int i = 0; i < UseConditions.Count; i++) {
			if(UseConditions[i].conditionType == AISkillUseConditionType.rangeOfTarget)
			{
				ownerFSM.SetTargetDistance(UseConditions[i].targetValue1);
			}
			if(UseConditions[i].conditionType == AISkillUseConditionType.direction)
			{
				ownerFSM.SetTargetAngle(UseConditions[i].targetValue1);
			}
			if(UseConditions[i].conditionType == AISkillUseConditionType.position)
			{
				ownerFSM.SetTargetPosition(ownerFSM.targetObject.position);
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
		base.ResetSkill();
		ownerFSM.moveToTarget = false;
		ownerFSM.aimAtTarget = false;
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
	rangeOfTarget,
	direction,
	position
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