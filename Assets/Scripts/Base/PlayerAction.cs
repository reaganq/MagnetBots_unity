using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {

	public ArmorAnimation actionAnimation;
	public CharacterActionManager cam;
	public Animation myAnimation;
	public int rewardCurrencyID;
	public float rewardAmount;
	public float rewardWaitTime;
	public GameObject externalObject;

	public void Enable(CharacterActionManager actionManager)
	{
		cam = actionManager;
		if(actionAnimation.clip != null)
		{
			StartCoroutine(TransferAnimation(actionAnimation));
			cam.PlayAnimation(actionAnimation.clip.name);
		}
		StartCoroutine(MainActionSequence());
	}
	
	public IEnumerator TransferAnimation(ArmorAnimation anim)
	{
		if(anim.clip != null)
		{
			cam.myAnimation.AddClip(anim.clip, anim.clip.name);
			cam.myAnimation[anim.clip.name].layer = anim.animationLayer;
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

	public IEnumerator MainActionSequence()
	{
		yield return null;
		if(externalObject != null)
		{
			GameObject obj = GameObject.Instantiate(externalObject, cam._myTransform.position, cam._myTransform.rotation) as GameObject;

		}
		if(myAnimation != null)
			myAnimation.Play(actionAnimation.clip.name);
		yield return new WaitForSeconds(actionAnimation.clip.length);
		RPGCurrency currency = Storage.LoadById<RPGCurrency>(rewardCurrencyID, new RPGCurrency());
		cam.EarnStatusRewards(currency, rewardAmount);
		cam.ResetActionState();
		Destroy(this);
	}
}
