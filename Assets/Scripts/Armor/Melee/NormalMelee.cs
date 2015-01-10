using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalMelee : BasePlayerSkill {


    /***** set up in inspector *****/
    public SkillAnimation[] attackAnimations;
    public TargetDetectionMethod detectionMethod;
    public Transform weaponLocation;
    public GameObject weaponCollider;
    public float detectionRange;
	public Transform hitDecal;
    //public float collisionRange;
    //can hit multiple enemies or heal multiple allies?
    public bool canHitMultipleTargets;

	public TrailRenderer trail;

    /***************/

    #region skill state

    #endregion

    #region setup and unequip
    public override void Initialise(PlayerCharacter ownerStatus, int index)
    {
		base.Initialise(ownerStatus, index);
        TransferAnimations();
        if(weaponCollider != null)
        {
            TriggerCollider tc = weaponCollider.GetComponent<TriggerCollider>();
            if(tc != null)
            {
                tc.status = owner;
				tc.masterArmor = this;
            }
            weaponCollider.SetActive(false);
        }

		if(trail != null)
			trail.enabled = false;
		if(hitDecal)
			AddPrefabToPool(hitDecal);
    }

    public void TransferAnimations()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            TransferSkillAnimation(attackAnimations[i]);
        }
    }

    public override void UnEquip()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            RemoveSkillAnimation(attackAnimations[i]);
        }
    }
    #endregion


    public override bool CanPressDown()
    {
        if(attackAnimations.Length == 0 || armorState != SkillState.ready)
        {
            return false;

        }
        else
            return true;
    }


	public override IEnumerator PressDownSequence()
    {
		if(disableMovement)
		{
			ownerManager.DisableMovement();
		}

		ActivateSkill(true);
        armorState = SkillState.casting;

        int i = Random.Range(0, attackAnimations.Length);
        //Debug.Log("i = "+i);
        //ownerAnimation[attackAnimations[i].castAnimation.clip.name].time = 0;
		ownerManager.CrossfadeAnimation(attackAnimations[i].castAnimation.clip.name, 0.05f, false);
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
        armorState = SkillState.onUse;
        if(weaponCollider != null)
        {
            weaponCollider.SetActive(true);
        }
		if(trail != null)
			trail.enabled = true;

        yield return new WaitForSeconds(attackduration);
        armorState = SkillState.followThrough;
        

        if(weaponCollider != null)
        {
            weaponCollider.SetActive(false);
        }
		if(trail != null)
			trail.enabled = false;

		if(disableMovement)
			ownerManager.EnableMovement();

        //yield return new WaitForSeconds(followThroughTime*0.3f);
        //characterAnimation.Blend(attackAnimations[i].clip.name, 0, followThroughTime*0.7f);
		//ownerManager.myPhotonView.RPC("BlendAnimation", PhotonTargets.All, attackAnimations[i].precastAnimation.clip.name, (float)0.0f , (float)(followThroughTime*0.7f));
		//ownerManager.FadeOutAnimation(attackAnimations[i].castAnimation.clip.name);

        //yield return new WaitForSeconds(followThroughTime * 0.7f);

        //ResetSkill();

    }

    public override void ResetSkill()
    {
        armorState = SkillState.ready;
        HitEnemies.Clear();
		HitAllies.Clear();
		ActivateSkill(false);
    }
}
