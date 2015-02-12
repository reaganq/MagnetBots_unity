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

	public override IEnumerator PressDownSequence()
    {
		if(disableMovement)
		{
			ownerCAM.DisableMovement();
		}

		ActivateSkill(true);
		isBusy = true;
        skillState = SkillState.onUse;

        int i = Random.Range(0, attackAnimations.Length);
        //Debug.Log("i = "+i);
        //ownerAnimation[attackAnimations[i].castAnimation.clip.name].time = 0;
		ownerCAM.CrossfadeAnimation(attackAnimations[i].castAnimation.clip.name, 0.05f, false);
        //characterAnimation.CrossFade(attackAnimations[i].clip.name, 0.05f);
		//ownerManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, attackAnimations[i].precastAnimation.clip.name, (float)0.05f);

        float totalTime = attackAnimations[i].castAnimation.clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

       /* Debug.Log(totalTime);
        Debug.Log(castTime);
        Debug.Log(attackduration);
        Debug.Log(followThroughTime);*/

        yield return new WaitForSeconds(castTime);

        yield return new WaitForSeconds(attackduration);
        skillState = SkillState.followThrough;
        
		if(disableMovement)
			ownerCAM.EnableMovement();

        //yield return new WaitForSeconds(followThroughTime*0.3f);
        //characterAnimation.Blend(attackAnimations[i].clip.name, 0, followThroughTime*0.7f);
		//ownerManager.myPhotonView.RPC("BlendAnimation", PhotonTargets.All, attackAnimations[i].precastAnimation.clip.name, (float)0.0f , (float)(followThroughTime*0.7f));
		//ownerManager.FadeOutAnimation(attackAnimations[i].castAnimation.clip.name);

        //yield return new WaitForSeconds(followThroughTime * 0.7f);

        //ResetSkill();

    }
}
