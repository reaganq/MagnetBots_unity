using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

	public ArmorAnimation actionAnimation;
	public CharacterActionManager cam;
	public Animation myAnimation;

	public void Enable(CharacterActionManager actionManager)
	{
		cam = actionManager;
		if(actionAnimation.clip != null)
		{
			StartCoroutine(TransferAnimation(actionAnimation));
			cam.PlayAnimation(actionAnimation.clip.name);
		}
		if(myAnimation != null)
			myAnimation.Play();
	}

	public IEnumerator TransferAnimation(ArmorAnimation anim)
	{
		if(anim.clip != null)
		{
			cam.myAnimation.AddClip(anim.clip, anim.clip.name);
			cam.myAnimation[anim.clip.name].layer = anim.animationLayer;
			yield return null;
			if(!anim.useWholeBody)
			{
				if(anim.useArmLBones)
				{
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.clavicleL, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.shoulderL, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.shoulderGuardL, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.elbowL, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.forearmL, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.handL, false);
				}
				
				if(anim.useArmRBones)
				{
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.clavicleR, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.shoulderR, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.shoulderGuardR, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.elbowR, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.forearmR, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.handR, false);
				}
				
				if(anim.useVerticalBones)
				{
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.spine2, false);
					cam.myAnimation[anim.clip.name].AddMixingTransform(cam.myAvatar.neckHorizontal, false);
				}
			}
		}
		yield return null;
	}
}
