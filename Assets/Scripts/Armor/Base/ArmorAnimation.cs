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

    public void TransferAnimation(Animation target, Transform avatar)
    {
        Debug.LogWarning("here");
        target.AddClip(clip, clip.name);
        target[clip.name].layer = animationLayer;
        //StartCoroutine(MixingTransforms( addMixingTransforms, removeMixingTransforms, clip));
        //yield return null;
        
		if(useArmLBones)
		{
			for (int i = 0; i < GeneralData.armLBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.armLBones[i], avatar), false);
			}
		}
		
		if(useArmRBones)
		{
			for (int i = 0; i < GeneralData.armRBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.armRBones[i], avatar), false);
			}
		}

		if(useVerticalBones)
		{
			for (int i = 0; i < GeneralData.verticalBones.Length; i++) {
				target[clip.name].AddMixingTransform(GetBone(GeneralData.verticalBones[i], avatar), false);
			}
		}


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
    }
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
    
    public Transform GetBone(string bonename, Transform avatar)
    {
        Debug.LogWarning(avatar.name);
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
    }
}
