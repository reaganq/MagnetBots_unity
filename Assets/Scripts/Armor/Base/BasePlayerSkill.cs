using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePlayerSkill : BaseSkill {

	[HideInInspector]
	public CharacterActionManager ownerCAM;
	public Job pressDownJob;
	public Job pressUpJob;
	public int equipmentSlotIndex;
	public bool hasSkill = true;
	public bool hasPressDownEvent;
	public bool hasPressUpEvent;
	public bool resetAfterDown;
	public bool resetAfterUp;
	public bool canUseWhileBusy;
	//player can't move AT ALL while using this skill
	//player has limited movement when using this skill. Think charge attacks
	//public bool continuousUse;
	public string skillButtonSpritePath;
	public string skillButtonAtlasPath;

	public float baseDamage;

	[HideInInspector]
	public Avatar ownerAvatar;
	public bool isPressedDown = false;
	public bool isPressedUp = false;


	public virtual void Initialise(PlayerCharacter status, int skillIndex)
	{
		ownerStatus = status;
		ownerManager = ownerStatus.actionManager;
		ownerCAM = (CharacterActionManager)ownerStatus.actionManager;
		ownerAnimation = ownerCAM.myAnimation;
		ownerTransform = ownerStatus._myTransform;
		skillID = skillIndex;
		ownerAvatar = ownerCAM.myAvatar;
		TransferSkillAnimations();
		BasicSetup();
	}

	public virtual void TransferSkillAnimations()
	{
		if(baseSkillAnimation != null)
		{
			TransferSkillAnimation(baseSkillAnimation);
		}
	}

	public virtual void UnEquip()
	{
		RemoveSkillAnimations();
		ownerCAM.armorSkills.Remove(this);
	}

	public virtual void RemoveSkillAnimations()
	{
		if(baseSkillAnimation != null)
		{
			RemoveSkillAnimation(baseSkillAnimation);
		}
	}

	public virtual bool CanPressDown()
	{
		if(hasPressDownEvent && (!isBusy || (isBusy && canUseWhileBusy)))
			return true;
		else
			return false;
	}
	
	public virtual void PressDown(int randomNumber)
	{
		if(pressDownJob != null)
			pressDownJob.kill();
		if(pressUpJob != null)
			pressUpJob.kill();
		pressDownJob = Job.make(PressDownSequence(randomNumber), true);
		pressDownJob.jobComplete += (wasKilled) =>
		{
			if(resetAfterDown)
				ResetSkill();
		};
	}

	public virtual bool CanPressUp()
	{
		if(hasPressUpEvent && (!isBusy ||(isBusy && canUseWhileBusy)))
			return true;
		else
			return false;
	}
	
	public virtual void PressUp(int randomNumber)
	{
		//if(pressDownJob != null)
		//	pressDownJob.kill();
		if(pressUpJob != null)
			pressUpJob.kill();
		pressUpJob = Job.make(PressUpSequence(randomNumber));
		pressUpJob.jobComplete += (wasKilled) =>
		{
			if(resetAfterUp)
				ResetSkill();
		};
	}
	
	//main action sequence when attack button is pressed down
	public virtual IEnumerator PressDownSequence(int randomNumber)
	{
		yield return null;
	}

	//main action sequence when attack button is pressed up
	public virtual IEnumerator PressUpSequence(int randomNumber)
	{
		yield return null;
	}
	
	public void SetupSkillButtons()
	{
		GUIManager.Instance.MainGUI.EnableActionButton(equipmentSlotIndex, skillID );
	}

	#region Animation Setup
	
	public void TransferSkillAnimation(SkillAnimation anim)
	{
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
			yield return null;
			if(!anim.useWholeBody)
			{
				if(anim.useArmLBones)
				{
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.clavicleL, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.shoulderL, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.shoulderGuardL, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.elbowL, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.forearmL, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.handL, false);
				}
				
				if(anim.useArmRBones)
				{
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.clavicleR, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.shoulderR, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.shoulderGuardR, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.elbowR, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.forearmR, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.handR, false);
				}
				
				if(anim.useVerticalBones)
				{
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.spine2, false);
					ownerAnimation[anim.clip.name].AddMixingTransform(ownerAvatar.neckHorizontal, false);
				}
			}
		}
		yield return null;
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
		for (int i = 0; i < kids.Length; i++) {
			if(kids[i].name == bonename)
			{
				return kids[i];
			}
		}
		return null;
	}
	
	#endregion

	public override void ResolveHit (CharacterStatus targetCS, Vector3 hitPos, Vector3 targetPos)
	{
		//redundant check
		if(targetCS.myPhotonView.isMine)
		{
			List<StatusEffectData> outgoingSEs = new List<StatusEffectData>();
			if(ownerStatus.enemyCharacterType == targetCS.characterType)
				targetCS.ReceiveHit(PhotonNetwork.player.ID, skillID, outgoingEnemyStatusEffects);
			else
				targetCS.ReceiveHit(PhotonNetwork.player.ID, skillID, outgoingAllyStatusEffects);
		}
	}
}
