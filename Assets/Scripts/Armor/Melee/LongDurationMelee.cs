using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongDurationMelee : BaseSkill {

    /***** set up in inspector *****/
    public SkillAnimation skillAnimation; 
    //public ArmorAnimation castingAnimation;
    //public ArmorAnimation durationAnimation;
    //public ArmorAnimation followThroughAnimation;
    public TargetDetectionMethod detectionMethod;
    public Transform weaponLocation;
    public float detectionRange;
    //public float collisionRange;
    //can hit multiple enemies or heal multiple allies?
    public bool canHitMultipleTargets;

    /***************/

    #region setup and unequip
    public override void Initialise(CharacterStatus manager, int index)
    {
        base.Initialise(manager, index);
        TransferAnimations();
        //Debug.Log("override");

    }
    
    public void TransferAnimations()
    {
        TransferSkillAnimation(skillAnimation);
    }
    
    public override void UnEquip()
    {
        RemoveSkillAnimation(skillAnimation);
    }
    #endregion

    public override bool CanPressDown()
    {
        if(armorState != SkillState.ready)
        {
            return false;
        }
        else 
            return true;
    }
    
    // Update is called once per frame
	public override IEnumerator PressDownSequence()
    {


        //Debug.Log("start skill");

		ActivateSkill(true);
        armorState = SkillState.casting;

        ownerAnimation[skillAnimation.castAnimation.clip.name].time = 0;
        ownerAnimation[skillAnimation.castAnimation.clip.name].speed = 1;
        //ownerAnimation[skillAnimation.precastAnimation.clip.name].time = 0;
        ownerAnimation[skillAnimation.followThroughAnimation.clip.name].time = 0;

        //characterAnimation.Play(skillAnimation.castAnimation.clip.name);
		ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, skillAnimation.castAnimation.clip.name);
        yield return new WaitForSeconds(skillAnimation.castAnimation.clip.length);
  
        armorState = SkillState.onUse;
        //characterAnimation.Play(skillAnimation.clip.name);
		ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, skillAnimation.precastAnimation.clip.name);
        //yield return new WaitForSeconds(attackduration);

       //Debug.Log("end of start action");
        //ApplyOnUseSkillEffects();

        
    }

    public override bool CanPressUp()
    {
        if(armorState == SkillState.ready || armorState == SkillState.followThrough || armorState == SkillState.onCooldown || armorState == SkillState.recoiling)
            return false;
        else
            return true;
    }

    public override IEnumerator PressUpSequence()
    {


        while(armorState == SkillState.casting)
        {
            yield return new WaitForEndOfFrame();
        }

        if(armorState == SkillState.onUse)
        {
			ActivateSkill(false);
        }

        /*float t = characterAnimation[castingAnimation.clip.name].normalizedTime;

        Debug.Log("t: " + t);
        float blendTime = 1;

        if(t > 0)
        {
            characterAnimation[followThroughAnimation.clip.name].normalizedTime = (1-t);
            blendTime = followThroughAnimation.clip.length * t;
        }
        else
            blendTime = followThroughAnimation.clip.length;

        if(characterAnimation.IsPlaying(castingAnimation.clip.name))
           characterAnimation[castingAnimation.clip.name].speed = 0;
           */

        //characterAnimation.CrossFade(skillAnimation.followThroughAnimation.clip.name);
		ownerManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, skillAnimation.followThroughAnimation.clip.name);
        armorState = SkillState.followThrough;
		//actionManager.ResetActionState();

        //yield return new WaitForSeconds(followThroughAnimation.clip.length*0.3f);

        //characterAnimation.Blend(followThroughAnimation.clip.name, 0, blendTime);
        
        //yield return new WaitForSeconds(blendTime*0.9f);
        yield return new WaitForSeconds(skillAnimation.followThroughAnimation.clip.length);
        //Debug.Log("time pased: "+ (Time.realtimeSinceStartup - time));
        //Debug.Log("finish");
        //Reset();

    }
    
    public override void ResetSkill()
    {
		Debug.Log("reset");
        armorState = SkillState.ready;
		//RemoveOnUseSkillEffects();
    }
}
