using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongDurationMelee : ArmorSkill {

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
    public override void Initialise(Transform character, CharacterActionManager manager, int index)
    {
        base.Initialise(character, manager, index);
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
        if(armorState != ArmorState.ready)
        {
            return false;
        }
        else 
            return true;
    }
    
    // Update is called once per frame
    public override IEnumerator PressDown()
    {


        //Debug.Log("start skill");

		ActivateSkill(true);
        armorState = ArmorState.casting;

        myAnimation[skillAnimation.castAnimation.clip.name].time = 0;
        myAnimation[skillAnimation.castAnimation.clip.name].speed = 1;
        myAnimation[skillAnimation.clip.name].time = 0;
        myAnimation[skillAnimation.followThroughAnimation.clip.name].time = 0;

        //characterAnimation.Play(skillAnimation.castAnimation.clip.name);
		myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, skillAnimation.castAnimation.clip.name);
        yield return new WaitForSeconds(skillAnimation.castAnimation.clip.length);
  
        armorState = ArmorState.onUse;
        //characterAnimation.Play(skillAnimation.clip.name);
		myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, skillAnimation.clip.name);
        //yield return new WaitForSeconds(attackduration);

       //Debug.Log("end of start action");
        ApplyOnUseSkillEffects();

        
    }

    public override bool CanPressUp()
    {
        if(armorState == ArmorState.ready || armorState == ArmorState.followThrough || armorState == ArmorState.onCooldown || armorState == ArmorState.recoiling)
            return false;
        else
            return true;
    }

    public override IEnumerator PressUp()
    {


        while(armorState == ArmorState.casting)
        {
            yield return new WaitForEndOfFrame();
        }

        if(armorState == ArmorState.onUse)
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
		myManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, skillAnimation.followThroughAnimation.clip.name);
        armorState = ArmorState.followThrough;
		myManager.ResetActionState();

        //yield return new WaitForSeconds(followThroughAnimation.clip.length*0.3f);

        //characterAnimation.Blend(followThroughAnimation.clip.name, 0, blendTime);
        
        //yield return new WaitForSeconds(blendTime*0.9f);
        yield return new WaitForSeconds(skillAnimation.followThroughAnimation.clip.length);
        //Debug.Log("time pased: "+ (Time.realtimeSinceStartup - time));
        //Debug.Log("finish");
        
        //Reset();

    }
    
    public override void Reset()
    {
        armorState = ArmorState.ready;
		RemoveOnUseSkillEffects();
    }
}
