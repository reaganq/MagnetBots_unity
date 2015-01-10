using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ArmorAnimation{

    public AnimationClip clip;
    public int animationLayer;
	public bool useArmLBones;
	public bool useArmRBones;
	public bool useVerticalBones;
    //public List<string> addMixingTransforms;
    //public List<string> removeMixingTransforms;

    /*public void TransferAnimation(Animation target, Avatar avatar)
    {
        //Debug.LogWarning("here");
        target.AddClip(clip, clip.name);
        target[clip.name].layer = animationLayer;
        //StartCoroutine(MixingTransforms( addMixingTransforms, removeMixingTransforms, clip));
        //yield return null;
        if(useArmLBones)
		{
			target[clip.name].AddMixingTransform(avatar.clavicleL, false);
			target[clip.name].AddMixingTransform(avatar.shoulderL, false);
			target[clip.name].AddMixingTransform(avatar.shoulderGuardL, false);
			target[clip.name].AddMixingTransform(avatar.elbowL, false);
			target[clip.name].AddMixingTransform(avatar.forearmL, false);
			target[clip.name].AddMixingTransform(avatar.handL, false);
		}

		if(useArmRBones)
		{
			target[clip.name].AddMixingTransform(avatar.clavicleR, false);
			target[clip.name].AddMixingTransform(avatar.shoulderR, false);
			target[clip.name].AddMixingTransform(avatar.shoulderGuardR, false);
			target[clip.name].AddMixingTransform(avatar.elbowR, false);
			target[clip.name].AddMixingTransform(avatar.forearmR, false);
			target[clip.name].AddMixingTransform(avatar.handR, false);
		}

		if(useVerticalBones)
		{
			target[clip.name].AddMixingTransform(avatar.spine2, false);
			target[clip.name].AddMixingTransform(avatar.neckHorizontal, false);
		}

		/*if(useArmRBones)
		{
			for (int i = 0; i < GeneralData.armRBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.armRBones[i], avatar), false);
			}
		}

		if(useArmLBones)
		{
			for (int i = 0; i < GeneralData.armLBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.armLBones[i], avatar), false);
				//Debug.Log(GeneralData.armLBones[i]);
			}
		}

		if(useVerticalBones)
		{
			for (int i = 0; i < GeneralData.verticalBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.verticalBones[i], avatar), false);
			}
		}*/



        /*if(addMixingTransforms.Count>0)
        {
            for (int i = 0; i < addMixingTransforms.Count; i++) {
                target[clip.name].AddMixingTransform(GetBone(addMixingTransforms[i], avatar), false);
                //yield return null;
            }
        }
        
        if(removeMixingTransforms.Count>0)
        {
            for (int i = 0; i < removeMixingTransforms.Count; i++) 
            {
                target[clip.name].RemoveMixingTransform(GetBone(removeMixingTransforms[i], avatar));
                //yield return null;
            }
        }*/
    //}
    /*public IEnumerator MixingTransforms(List<string> bonelist, List<string> removelist, AnimationClip clip)
    {
        yield return null;
        
        if(bonelist.Count>0)
        {
            for (int i = 0; i < bonelist.Count; i++) {
                animationTarget[clip.name].AddMixingTransform(GetBone(bonename), false);
            }
        }
        
        if(removelist.Count>0)
        {
            foreach(string bonename in removelist)
            {
                animationTarget[clip.name].RemoveMixingTransform(GetBone(bonename));
            }
        }
        
    }*/
    
    /*public Transform GetBone(string bonename, Transform avatar)
    {
        //Debug.LogWarning(avatar.name);
        Transform[] kids = avatar.GetComponentsInChildren<Transform>();
        //Debug.Log(kids.Length);
        for (int i = 0; i < kids.Length; i++) {
            if(kids[i].name == bonename)
            {
                Debug.Log(kids[i].name);
                return kids[i];
            }
        }
        return null;
    }*/
}
