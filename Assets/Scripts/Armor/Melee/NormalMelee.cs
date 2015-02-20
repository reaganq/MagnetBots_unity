using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalMelee : BasePlayerSkill {
	
    /***** set up in inspector *****/
    public SkillAnimation[] attackAnimations;
	public Transform hitDecal;

    /***************/

    #region skill state

    #endregion

    #region setup and unequip
    public override void Initialise(PlayerCharacter ownerStatus, int index)
    {
		base.Initialise(ownerStatus, index);
        //TransferAnimations();
        /*if(weaponCollider != null)
        {
            TriggerCollider tc = weaponCollider.GetComponent<TriggerCollider>();
            if(tc != null)
            {
                tc.status = owner;
				tc.masterArmor = this;
            }
            weaponCollider.SetActive(false);
        }*/
		if(hitDecal)
			AddPrefabToPool(hitDecal);
    }

    public override void TransferSkillAnimations()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            TransferSkillAnimation(attackAnimations[i]);
        }
    }

	public override void RemoveSkillAnimations ()
	{
		for (int i = 0; i < attackAnimations.Length; i++) {
			RemoveSkillAnimation(attackAnimations[i]);
		}
    }
    
    #endregion

	public override IEnumerator PressDownSequence(int randomNumber)
    {
		if(disableMovement)
		{
			ownerCAM.DisableMovement();
		}

		ActivateSkill(true);
		isBusy = true;
        skillState = SkillState.onUse;

        if(randomNumber > attackAnimations.Length -1)
			randomNumber = attackAnimations.Length -1;
		int i = randomNumber;
		ownerCAM.CrossfadeAnimation(attackAnimations[i].castAnimation.clip.name, 0.05f, false);

        float totalTime = attackAnimations[i].castAnimation.clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

        yield return new WaitForSeconds(castTime);

        yield return new WaitForSeconds(attackduration);
        skillState = SkillState.followThrough;
        
		if(disableMovement)
			ownerCAM.EnableMovement();
    }

	public override void HitTarget (CharacterStatus targetCS, Vector3 hitPos, Vector3 targetPos)
	{
		base.HitTarget (targetCS, hitPos, targetPos);
		ownerCAM.SpawnParticle(hitDecal.name, hitPos, true);
	}
}
