using UnityEngine;
using System.Collections;

public class BasePlayerSkill : BaseSkill {

	public Job pressDownJob;
	public Job pressUpJob;

	public bool hasPressDownEvent;
	public bool hasPressUpEvent;
	public bool resetAfterDown;
	public bool resetAfterUp;
	public bool canUseWhileBusy;
	public bool continuousUse;
	public int equipmentSlotIndex;
	public Avatar myAvatar;

	public virtual void Initialise(PlayerCharacter ownerStatus, int index)
	{
		owner = ownerStatus;
		ownerManager = ownerStatus.actionManager;
		ownerAnimation = ownerManager.myAnimation;
		ownerTransform = ownerStatus._myTransform;
		equipmentSlotIndex = index;
		TransferSkillAnimation(baseSkillAnimation);
		myAvatar = ownerStatus.avatar;
	}

	public virtual void UnEquip()
	{
		RemoveSkillAnimation(baseSkillAnimation);
	}
	
	public virtual void PressDown()
	{
		isPressedDown = true;
		
		if(hasPressDownEvent)
		{
			if(!isBusy || (isBusy && canUseWhileBusy))
			{
				if(pressDownJob != null)
					pressDownJob.kill();
				if(pressUpJob != null)
					pressUpJob.kill();
				pressDownJob = Job.make(PressDownSequence(), true);
				pressDownJob.jobComplete += (wasKilled) =>
				{
					if(resetAfterDown)
						ResetSkill();
				};
			}
		}
	}
	
	public virtual void PressUp()
	{
		isPressedDown = false;
		if(!hasPressDownEvent && isBusy && !canUseWhileBusy)
			return;
		
		if(hasPressUpEvent)
		{
			if(pressDownJob != null)
				pressDownJob.kill();
			pressUpJob = Job.make(PressUpSequence());
			pressUpJob.jobComplete += (wasKilled) =>
			{
				if(resetAfterUp)
					ResetSkill();
			};
		}
	}
	
	
	public virtual IEnumerator PressDownSequence()
	{
		yield return null;
	}
	
	public virtual IEnumerator PressUpSequence()
	{
		yield return null;
	}

	#region Animation Setup
	
	public void TransferSkillAnimation(SkillAnimation anim)
	{
		/*if(anim.precastAnimation.clip != null)
			anim.precastAnimation.TransferAnimation(ownerAnimation, myAvatar);
		if(anim.castAnimation.clip != null)
			anim.castAnimation.TransferAnimation(ownerAnimation, myAvatar);
		if(anim.followThroughAnimation.clip != null)
			anim.followThroughAnimation.TransferAnimation(ownerAnimation, myAvatar);
		if(anim.loopAnimation.clip != null)
			anim.loopAnimation.TransferAnimation(ownerAnimation, myAvatar);*/
		//StartCoroutine(TransferAnimation(anim.precastAnimation));
		if(anim.precastAnimation.clip != null)
			StartCoroutine(TransferAnimation(anim.precastAnimation));
		if(anim.castAnimation.clip != null)
			StartCoroutine(TransferAnimation(anim.castAnimation));
		if(anim.followThroughAnimation.clip != null)
			StartCoroutine(TransferAnimation(anim.followThroughAnimation));
		if(anim.loopAnimation.clip != null)
			StartCoroutine(TransferAnimation(anim.loopAnimation));
	}
	
	public IEnumerator TransferAnimation(ArmorAnimation anim)
	{
		if(anim.clip != null)
		{
			ownerAnimation.AddClip(anim.clip, anim.clip.name);
			ownerAnimation[anim.clip.name].layer = anim.animationLayer;
			//StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
			yield return null;
			
			/*if(anim.addMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.addMixingTransforms.Count; i++) {
                    ownerAnimation[anim.clip.name].AddMixingTransform(GetBone(anim.addMixingTransforms[i]), false);
                }
            }

            if(anim.removeMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.removeMixingTransforms.Count; i++) 
                {
                    ownerAnimation[anim.clip.name].RemoveMixingTransform(GetBone(anim.removeMixingTransforms[i]));
                }
            }*/
		}
	}
	
	public void RemoveSkillAnimation(SkillAnimation anim)
	{
		if(anim.precastAnimation.clip != null)
			RemoveAnimation(anim.precastAnimation.clip);
		if(anim.castAnimation.clip != null)
			RemoveAnimation(anim.castAnimation.clip);
		if(anim.followThroughAnimation.clip != null)
			RemoveAnimation(anim.followThroughAnimation.clip);
		if(anim.loopAnimation.clip != null)
			RemoveAnimation(anim.loopAnimation.clip);
	}
	
	public void RemoveArmorAnimation(ArmorAnimation anim)
	{
		if(anim.clip != null)
			RemoveAnimation(anim.clip);
	}
	
	public void RemoveAnimation(AnimationClip clip)
	{
		ownerAnimation.RemoveClip(clip.name);
	}
	
	public Transform GetBone(string bonename)
	{
		Transform[] kids = ownerTransform.GetComponentsInChildren<Transform>();
		//Debug.Log(kids.Length);
		for (int i = 0; i < kids.Length; i++) {
			if(kids[i].name == bonename)
			{
				//Debug.Log(kids[i].name);
				return kids[i];
			}
		}
		return null;
	}
	
	#endregion
}
