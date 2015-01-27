using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveArmorAnimationController : MonoBehaviour {

   // public ArmorAnimation braceAnimation;
    public ArmorAnimation idleOverrideAnim;
    //public ArmorAnimation walkingOverrideAnim;
    public ArmorAnimation runningOverrideAnim;

    public Animation animationTarget;
    //public Avatar avatar;

    public void RemoveAnimations()
    {
        if(idleOverrideAnim.clip != null)
            animationTarget.RemoveClip(idleOverrideAnim.clip.name);
        if(runningOverrideAnim.clip != null)
            animationTarget.RemoveClip(runningOverrideAnim.clip.name);
    }

    public void TransferAnimations(Animation target, Avatar character)
    {
        animationTarget = target;
        //avatar = character;

        if(idleOverrideAnim.clip != null)
        {
            StartCoroutine(TransferAnimation(target, idleOverrideAnim, character));
			//idleOverrideAnim.TransferAnimation(target, character);
        }

        if(runningOverrideAnim.clip != null)
        {
            //animationTarget.AddClip(runningOverrideAnim.clip, runningOverrideAnim.clip.name);
            //animationTarget[runningOverrideAnim.clip.name].layer = runningOverrideAnim.animationLayer;
            //StartCoroutine(MixingTransforms( runningOverrideAnim.addMixingTransforms, runningOverrideAnim.removeMixingTransforms, runningOverrideAnim.clip));
            StartCoroutine(TransferAnimation(target, runningOverrideAnim, character));
			//runningOverrideAnim.TransferAnimation(target, character);
        }
    }

    public IEnumerator TransferAnimation(Animation target, ArmorAnimation anim, Avatar avatar)
    {
        target.AddClip(anim.clip, anim.clip.name);
        target[anim.clip.name].layer = anim.animationLayer;
        //StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
        yield return null;

		if(anim.useArmLBones)
		{
			target[anim.clip.name].AddMixingTransform(avatar.clavicleL, false);
			target[anim.clip.name].AddMixingTransform(avatar.shoulderL, false);
			target[anim.clip.name].AddMixingTransform(avatar.shoulderGuardL, false);
			target[anim.clip.name].AddMixingTransform(avatar.elbowL, false);
			target[anim.clip.name].AddMixingTransform(avatar.forearmL, false);
			target[anim.clip.name].AddMixingTransform(avatar.handL, false);
		}
		
		if(anim.useArmRBones)
		{
			target[anim.clip.name].AddMixingTransform(avatar.clavicleR, false);
			target[anim.clip.name].AddMixingTransform(avatar.shoulderR, false);
			target[anim.clip.name].AddMixingTransform(avatar.shoulderGuardR, false);
			target[anim.clip.name].AddMixingTransform(avatar.elbowR, false);
			target[anim.clip.name].AddMixingTransform(avatar.forearmR, false);
			target[anim.clip.name].AddMixingTransform(avatar.handR, false);
		}
		
		if(anim.useVerticalBones)
		{
			target[anim.clip.name].AddMixingTransform(avatar.spine2, false);
			target[anim.clip.name].AddMixingTransform(avatar.neckHorizontal, false);
		}
        
        /*if(anim.addMixingTransforms.Count>0)
        {
            for (int i = 0; i < anim.addMixingTransforms.Count; i++) {
                target[anim.clip.name].AddMixingTransform(GetBone(anim.addMixingTransforms[i], avatar), false);
            }
        }
        
        if(anim.removeMixingTransforms.Count>0)
        {
            for (int i = 0; i < anim.removeMixingTransforms.Count; i++) 
            {
                target[anim.clip.name].RemoveMixingTransform(GetBone(anim.removeMixingTransforms[i], avatar));
            }
        }*/
    }
    
    /*public Transform GetBone(string bonename, Transform avatar)
    {
        Transform[] kids = avatar.GetComponentsInChildren<Transform>();
        //Debug.Log(kids.Length);
        for (int i = 0; i < kids.Length; i++) {
            if(kids[i].name == bonename)
            {
                //Debug.Log(kids[i].name);
                return kids[i];
            }
        }
        return null;
    }*/
}
