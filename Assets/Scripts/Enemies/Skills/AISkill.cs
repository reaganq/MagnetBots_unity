using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PathologicalGames;

public class AISkill : BaseSkill {

	//root position of range object


	//how many times in total can this skill be used?
	public float skillUsageLimit = Mathf.Infinity;
	//how many times have we already used this skill this fight?
	private float usageCount;

	//reset values after use?
	public bool resetAfterUse;
	//how likely we should use this attack
    public float weighting = 0f;
	public List<AISkillUseCondition> UseConditions;
	public bool fulfillConditionsBeforeUse;
	public List<AISkillRequirements> UseRequirements;

	//how long we do try to fulfkill skill use requirements before giving up and selecting new skill?
	public float patienceTimer;

	//do we need a target for this skill? 0 = no target
    public int targetRequirement;
	public float skillRangeMax;
	public float skillRangeMin;

	//base stuff
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
		TransferSkillAnimation(baseSkillAnimation);
    }

	public void TransferSkillAnimation(SkillAnimation anim)
	{
		if(baseSkillAnimation.precastAnimation.clip != null)
		{
			AddAnimation(baseSkillAnimation.precastAnimation);
		}
		if(baseSkillAnimation.castAnimation.clip != null)
		{
			AddAnimation(baseSkillAnimation.castAnimation);
		}
		if(baseSkillAnimation.followThroughAnimation.clip != null)
		{
			AddAnimation(baseSkillAnimation.followThroughAnimation);
		}
		if(baseSkillAnimation.loopAnimation.clip != null)
		{
			AddAnimation(baseSkillAnimation.loopAnimation);
		}
	}

	public void AddAnimation(ArmorAnimation anim)
	{
		ownerAnimation.AddClip(anim.clip, anim.clip.name);
		if(anim.animationLayer < 2)
			anim.animationLayer = 2;
		ownerAnimation[anim.clip.name].layer = anim.animationLayer;
	}

	//check if can useskill
	public virtual bool CanUseSkill()
	{
		if(usageCount > skillUsageLimit)
			return false;
		if(ownerFSM.NumTargetsInRange(skillRangeMin, skillRangeMax) < targetRequirement)
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
		Debug.Log("start fulfilling conditions");
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
	}

	public void UseSkill()
	{
		useSkillJob = Job.make(UseSkillSequence());
		useSkillJob.jobComplete += (wasKilled) =>
		{
			if(resetAfterUse)
				ResetSkill();
			ownerFSM.EndSkill();
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
	}
}

[System.Serializable]
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

[System.Serializable]
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