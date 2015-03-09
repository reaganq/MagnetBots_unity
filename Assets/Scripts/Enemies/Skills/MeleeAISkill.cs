using UnityEngine;
using System.Collections;

public class MeleeAISkill : AISkill {
	
	public Transform hitDecal;
	//public Transform impactParticles;
	//public Transform impactParticleSpawnPoint;
	//public float impactRadius;

	public override void InitialiseAISkill(CharacterStatus status, int skillIndex)
	{
		base.InitialiseAISkill(status, skillIndex);
		if(hitDecal)
			AddPrefabToPool(hitDecal);
	}
	// Use this for initialization
	public override IEnumerator UseSkillSequence ()
    {

		if(fulfillConditionsBeforeUse)
		{
			StartFulfillSkillConditions();
		}
		while(!ownerFSM.hasFulfilledSkillConditions())
		{
			yield return new WaitForEndOfFrame();
		}
		if(disableMovement)
		{
			ownerFSM.DisableMovement();
        }
		skillState = SkillState.precast;
		if(baseSkillAnimation.precastAnimation.clip != null)
		{
			ownerFSM.CrossfadeAnimation(baseSkillAnimation.precastAnimation.clip.name);
			yield return new WaitForSeconds(baseSkillAnimation.precastAnimation.clip.length);
		}

		ActivateSkill(true);
		skillState = SkillState.onUse;
		ownerFSM.PlayAnimation(baseSkillAnimation.castAnimation.clip.name, false);

        yield return new WaitForSeconds(baseSkillAnimation.castAnimation.clip.length);

		skillState = SkillState.followThrough;
		if(disableMovement)
		{
			ownerFSM.EnableMovement();
        }
    }
}
