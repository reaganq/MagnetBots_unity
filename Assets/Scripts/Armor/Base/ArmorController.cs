using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorController : MonoBehaviour {
 
    public Animation rootAnimationTarget;
    public Animation myAnimation;
    public CharacterMotor controller;
    
    public UsageType usageType;
    public WeaponType weaponType;
    public bool isActive = false;
    
    public AnimationClip PreAttackClip;
    public AnimationClip AttackClip;
    public AnimationClip PostAttackClip;
    public AnimationClip RecoilClip;
    
    public List<string> AttackAnimationMixingTransforms;
    public List<string> AttackAnimationRemoveTransforms;
    
    
    public AnimationClip IdleOverrideAnimationClip;
    public bool hasIdleAnimationOverride;
	public int attackAnimationLayer;
    public int idleAnimationLayer;
    public List<string> IdleAnimationMixingTransformNames;
    public List<string> IdleAnimationRemoveTransformNames;
    
    public AnimationClip RunningOverrideAnimationClip;
    public bool hasRunningOverride;
    public int RunningOverrideLayer;
    public List<string> RunningAnimationMixingTransforms;
    public List<string> RunningAnimationRemoveTransforms;
    
    public WeaponState myState = WeaponState.idle;
    
    public enum WeaponState
    {
        idle,
        preattacking,
        preattackingend,
        attacking,
        attackingend,
        postattacking,
        postattackingend
        
    }
    
    public void Start()
    {
        myAnimation = GetComponent<Animation>();
    }
    
    #region animation transfers
    public void TransferAnimations(Animation target, CharacterMotor newcontroller)
    {
        
        rootAnimationTarget = target;
        controller = newcontroller;
        
        if(PreAttackClip != null)
        {
            rootAnimationTarget.AddClip(PreAttackClip, PreAttackClip.name);
            rootAnimationTarget[PreAttackClip.name].layer = attackAnimationLayer;
            StartCoroutine(MixingTransforms( AttackAnimationMixingTransforms, AttackAnimationRemoveTransforms, PreAttackClip ));
        }
        
        if(AttackClip != null)
        {
            rootAnimationTarget.AddClip(AttackClip, AttackClip.name);
            rootAnimationTarget[AttackClip.name].layer = attackAnimationLayer;
            StartCoroutine(MixingTransforms( AttackAnimationMixingTransforms, AttackAnimationRemoveTransforms, AttackClip ));
        }
        
        if(PostAttackClip != null)
        {
            rootAnimationTarget.AddClip(PostAttackClip, PostAttackClip.name);
            rootAnimationTarget[PostAttackClip.name].layer = attackAnimationLayer;
            StartCoroutine(MixingTransforms( AttackAnimationMixingTransforms, AttackAnimationRemoveTransforms, PostAttackClip ));
        }
        
        if(RecoilClip != null)
        {
            rootAnimationTarget.AddClip(RecoilClip, RecoilClip.name);
            rootAnimationTarget[RecoilClip.name].layer = attackAnimationLayer;
            StartCoroutine(MixingTransforms( AttackAnimationMixingTransforms, AttackAnimationRemoveTransforms, RecoilClip ));
        }
        
        
        if(hasIdleAnimationOverride)
        {
        rootAnimationTarget.AddClip(IdleOverrideAnimationClip, IdleOverrideAnimationClip.name);
        rootAnimationTarget[IdleOverrideAnimationClip.name].layer = idleAnimationLayer;
        
        StartCoroutine(MixingTransforms( IdleAnimationMixingTransformNames, IdleAnimationRemoveTransformNames, IdleOverrideAnimationClip ));
        }
        
        if(hasRunningOverride)
        {
        rootAnimationTarget.AddClip(RunningOverrideAnimationClip, RunningOverrideAnimationClip.name);
        rootAnimationTarget[RunningOverrideAnimationClip.name].layer = idleAnimationLayer;
        
        StartCoroutine(MixingTransforms( RunningAnimationMixingTransforms, RunningAnimationRemoveTransforms, RunningOverrideAnimationClip ));
        }
        
    }
    
    public IEnumerator MixingTransforms(List<string> bonelist, List<string> removelist, AnimationClip clip)
    {
        yield return null;
        
        if(bonelist.Count>0)
        {
            foreach(string bonename in bonelist)
            {
                if(bonename.IndexOf("Recursive") != -1)
                {
                    string newname = bonename.Replace("Recursive", string.Empty);
                    //Debug.Log(newname);
                    rootAnimationTarget[clip.name].AddMixingTransform(GetBone(newname), true);
                }
                else
                {
                    rootAnimationTarget[clip.name].AddMixingTransform(GetBone(bonename), false);
                    //Debug.Log("added mixing transform");
                    //yield return null;
                }
            }
        }
        
        if(removelist.Count>0)
        {
            foreach(string bonename in removelist)
            {
                rootAnimationTarget[clip.name].RemoveMixingTransform(GetBone(bonename));
                //Debug.Log("remove mixing transform: "+bonename);
                //yield return null;
            }
        }
        
    }
    
    public void UpdateMixingTransforms()
    {
        if(hasIdleAnimationOverride)
            StartCoroutine(MixingTransforms( IdleAnimationMixingTransformNames, IdleAnimationRemoveTransformNames, IdleOverrideAnimationClip ));
        if(hasRunningOverride)
            StartCoroutine(MixingTransforms( RunningAnimationMixingTransforms, RunningAnimationRemoveTransforms, RunningOverrideAnimationClip ));
    }
    
    public Transform GetBone(string bonename)
    {
        Transform[] kids = controller.transform.GetComponentsInChildren<Transform>();
        //Debug.Log(kids.Length);
        for (int i = 0; i < kids.Length; i++) {
            if(kids[i].name == bonename)
            {
                Debug.Log(kids[i].name);
                return kids[i];
            }
        }
        return null;
    }
    #endregion
    
    public void ButtonClicked()
    {
        if(usageType == UsageType.click)
        {
            //StartCoroutine("MainAction");
            rootAnimationTarget.Play(AttackClip.name);
            Debug.Log("do shit");
        }
    }
    
    public void ButtonPressed()
    {
        if(usageType != UsageType.click)
        {
            switch(weaponType)
            {
            case WeaponType.ChargedMelee:
                Debug.Log("yay");
                if(!isActive)
                StartCoroutine("MainAction");
                    break;
            case WeaponType.ChargedRangedSingleShot:
                break;
            case WeaponType.RangedMultiShot:
                break;
            }
        }
    }
    
    public void ButtonReleased()
    {
        if(usageType != UsageType.click)
        {
            if(isActive)
                StartCoroutine("EndMainAction");
            Debug.Log("released");
        }
        
    }
    
    public IEnumerator MainAction()
    {
        isActive = true;
        //if(PreAttackClip!=null)
        //{
            myState = WeaponState.preattacking;
            rootAnimationTarget.CrossFade(PreAttackClip.name);
            yield return new WaitForSeconds(PreAttackClip.length);
            
        //}
        myState = WeaponState.attacking;
        //if(AttackClip != null)
        //{
            rootAnimationTarget.CrossFade(AttackClip.name);
            if(usageType == UsageType.click)
            {
                yield return new WaitForSeconds(AttackClip.length);
                
                StartCoroutine("EndMainAction");
                
            }
        //}
        
        yield return null;
    }
    
    public IEnumerator EndMainAction()
    {
        if(!isActive)
            yield return null;
        else
        {
            StopCoroutine("MainAction");
            myState = WeaponState.postattacking;
            if(PostAttackClip != null)
            {
                rootAnimationTarget.CrossFade(PostAttackClip.name);
                isActive = false;
                yield return new WaitForSeconds(PostAttackClip.length);
                
            }
            myState = WeaponState.idle;
            isActive = false;
        }
    }
    
    public IEnumerator BeginContinuousFire()
    {
        yield return null;
    }
    
    public void Interrupt()
    {
        if(!isActive)
            return;
        else
        {
            StopAllCoroutines();
        }
    }
}

public enum UsageType
{
    pressandrelease,
    continuouspress,
    click
}

public enum WeaponType
{
    ChargedRangedSingleShot,
    ChargedMelee,
    Melee,
    RangedSingleShot,
    RangedMultiShot,
    RangedWaitForFeedback
}
