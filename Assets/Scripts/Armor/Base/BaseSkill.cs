using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;

public class BaseSkill : MonoBehaviour {

	//automated stuff
	public int skillID;
	[HideInInspector]
	public CharacterStatus ownerStatus;
	[HideInInspector]
	public Animation ownerAnimation;
	[HideInInspector]
	public Transform ownerTransform;
	[HideInInspector]
	public ActionManager ownerManager;
	//all the enemies hit during the usage of this skill
	public List<CharacterStatus> HitTargets;

	public bool isBusy = false;
	public bool isActive = false;
	public string skillName;
	public SkillType skillType;
    
	//player can't move AT ALL while using this skill
	public bool disableMovement;
	public bool disableRotation;
	//player has limited movement when using this skill. Think charge attacks
	public bool restrictedMovement;
	//can this skill affect the same target multiple times? true = no, false = yes
	public bool limitDamageInstance = true;
	//default skill animation set
	public SkillAnimation baseSkillAnimation;
	//max number of unique targets we can hit with each attack
	public float targetLimit = Mathf.Infinity;
	public int lowerRNGLimit = 0;
	public int upperRNGLimit = 0;
	
    /*** attribute ***/

    //public ArmorAttribute[] armorAttributes;
    public List<StatusEffectData> skillStatusEffects;
	public List<StatusEffectData> outgoingEnemyStatusEffects;
	public List<StatusEffectData>outgoingAllyStatusEffects;
	public List<Detector> skillDetectors;
	public List<SkillVFX> skillVFXs;

	public SkillState _skillState;
	public SkillState skillState
	{
		get{return _skillState;}
		set
		{
			ExitState(_skillState);
			_skillState = value;
			EnterState(_skillState);
		}
	}

	public bool isOwner;

	public void BasicSetup()
	{
		for (int i = 0; i < skillDetectors.Count; i++) {
			skillDetectors[i].Initialise(this);
		}
		outgoingAllyStatusEffects.Clear();
		outgoingEnemyStatusEffects.Clear();
		for (int i = 0; i < skillStatusEffects.Count; i++) {
			if(skillStatusEffects[i].affectAlly)
				outgoingAllyStatusEffects.Add(skillStatusEffects[i]);
			else if(skillStatusEffects[i].affectEnemy)
				outgoingEnemyStatusEffects.Add(skillStatusEffects[i]);
		}
		isOwner = ownerManager.myPhotonView.isMine;
		skillState = SkillState.ready;
	}

	public void TriggerSkillEvents(SkillEventTrigger trigger)
	{
		for (int i = 0; i < skillVFXs.Count; i++) {
			if(!isOwner && skillVFXs[i].isLocal)
			{
				Debug.Log("catch local skill vfx");
				continue;
			}
			if(skillVFXs[i].activationEvent == trigger)
				skillVFXs[i].Activate();
			if(skillVFXs[i].deactivationEvent == trigger)
				skillVFXs[i].Deactivate();
		}
		for (int i = 0; i < skillDetectors.Count; i++) {
			if(!isOwner && skillDetectors[i].isLocal)
			{
				Debug.Log("catch");
				continue;
			}
			if(skillDetectors[i].activationEvent == trigger)
			{
				Debug.Log("activate skill vfx");
				skillDetectors[i].Activate();
			}
			if(skillDetectors[i].deactivationEvent == trigger)
				skillDetectors[i].Deactivate();
		}
		ApplySelfStatusEffects(trigger);
	}

	public virtual void EnterState(SkillState state)
	{
		switch(state)
		{
		case SkillState.ready:
			TriggerSkillEvents(SkillEventTrigger.onReady);
			break;
		case SkillState.precast:
			TriggerSkillEvents(SkillEventTrigger.onPreCast);
			break;
		case SkillState.onUse:
			TriggerSkillEvents(SkillEventTrigger.onUse);
			break;
		case SkillState.followThrough:
			TriggerSkillEvents(SkillEventTrigger.onFollowThrough);
			break;
		}
	}
	public virtual void ExitState(SkillState state)
	{
	}

	public virtual void ResetSkill()
	{
		ownerStatus.RemoveStatusEffect(this);
		isBusy = false;
		ActivateSkill(false);
		HitTargets.Clear();
		skillState = SkillState.ready;
	}

	public void ActivateSkill(bool state)
	{
		isActive = state;
	}

	public virtual void FireOneShot()
	{
		TriggerSkillEvents(SkillEventTrigger.onFireOneShot);
	}

	#region pooling

	public void AddParticlesToPool()
	{
	}
	
	public void AddPrefabToPool(Transform prefab)
	{
		//if(characterManager.MakeSpawnPool())
		//{
		if(ownerManager.effectsPool.GetPrefabPool(prefab) == null)
		{
			PrefabPool prefabPool = new PrefabPool(prefab);;
			prefabPool.preloadAmount = 1;
			prefabPool.preloadFrames = 5;
			Debug.Log("here");
			ownerManager.effectsPool.CreatePrefabPool(prefabPool);
		}
	}

	#endregion
        
	#region apply status effects

	public virtual void ApplySelfStatusEffects(SkillEventTrigger condition)
	{
		ApplyStatusEffects(condition, null, 0);
	}

	//give generic status effects to target
	public virtual void ApplyStatusEffects(SkillEventTrigger condition, CharacterStatus target, int allyFlag)
	{
		foreach(StatusEffectData effect in skillStatusEffects)
		{
			if(effect.triggerCondition == condition)
			{
				if(!isOwner && effect.isLocal)
				{
					continue;
				}
				//myself
				if(effect.affectSelf && allyFlag == 0)
				{
					if(effect.effect == 5)
					{
						ownerStatus.AddImpact(ownerTransform.forward, effect.primaryEffectValue, effect.secondaryEffectValue, effect.tertiaryEffectValue);
					}
					//attribute change
					if(effect.effect == 3 || effect.effect == 1 || effect.effect == 6)
					{
						StatusEffect newEffect = ownerStatus.gameObject.AddComponent<StatusEffect>();
						newEffect.statusEffect = effect;
						newEffect.ownerSkill = this;
						ownerStatus.AddStatusEffect(newEffect);
						Debug.Log("adding status effect to myself");
					}
				}
				//currently this bit never gets used since we are passing through outgoing targeted status effects directly to targetCS;
				else if(effect.affectAlly && allyFlag == 1 || effect.affectEnemy && allyFlag == 2)
				{
					if(effect.effect == 5)
					{
						target.AddImpact(target._myTransform.forward, effect.primaryEffectValue, effect.secondaryEffectValue, effect.tertiaryEffectValue);
					}
					//attribute change
					if(effect.effect == 3 || effect.effect == 1 || effect.effect == 6)
					{
						StatusEffect newEffect = target.gameObject.AddComponent<StatusEffect>();
						newEffect.statusEffect = effect;
						newEffect.ownerSkill = this;
						target.AddStatusEffect(newEffect);
					}
				}	
			}
		}
	}

	#endregion

	#region hitting targets

	/*public virtual void HitTarget(HitBox target, bool isAlly)
	{
		HitInfo newHit = new HitInfo();
		
		if(!isAlly)
		{
			newHit.sourceName = ownerStatus.characterName;
			//newHit.damage = damage;
			newHit.hitPosX = ownerTransform.position.x;
			newHit.hitPosY = ownerTransform.position.y;
			newHit.hitPosZ = ownerTransform.position.z;
			newHit.skillEffects = new List<StatusEffectData>();
			for (int i = 0; i < skillStatusEffects.Count; i++) 
			{
				if (skillStatusEffects[i].affectEnemy) {
					newHit.skillEffects.Add(skillStatusEffects[i]);
				}
			}
			
			//TODO apply self buffs from characterstatus
			//TODO apply hitbox local buffs
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();
			b.Serialize(m, newHit);
			
			target.ownerCS.myPhotonView.RPC("ReceiveHit", PhotonTargets.All, m.GetBuffer());
			
			//target.ReceiveHit(newHit);
			//Debug.Log(finalhit.sourceName);
			Debug.Log("hitenemy");
		}
		else
		{
			Debug.Log("hitally");
		}
	}*/

	public virtual void HitTarget(CharacterStatus targetCS, Vector3 hitPos, Vector3 targetPos)
	{
		//HitInfo newHit = new HitInfo();
		if(HitTargets.Contains(targetCS))
		{
			if(limitDamageInstance)
				return;
		}
		else
			HitTargets.Add(targetCS);

		TriggerSkillEvents(SkillEventTrigger.onHit);
		if(ownerManager.myPhotonView.isMine)
		{
			ownerManager.DealDamage(targetCS.myPhotonView.viewID, targetCS.myPhotonView.ownerId, skillID, hitPos, targetPos);
			//action manager deal damage
		}
	}

	public virtual void ResolveHit(CharacterStatus targetCS, Vector3 hitPos, Vector3 targetPos)
	{
	}

	#endregion

}

public enum SkillState
{
    ready,
    precast,
    onUse,
    followThrough,
    recoiling,
    onCooldown,
    reloading,
    wait,
}

public enum TargetDetectionMethod
{
    directionalRange,
    customColliderHit,
    weaponRange,
    characterRange,
    projectileCollider,
    notNeeded
}

public enum InputTrigger
{
    OnPressUp,
    OnPressDown,
    OnClick
}

public enum SkillType
{
    Melee,
    Ranged,
    JumpingMelee,
    Buff
}

[Serializable]
public class SkillAnimation
{
	public ArmorAnimation precastAnimation;
	public ArmorAnimation castAnimation;
	public ArmorAnimation followThroughAnimation;
	public ArmorAnimation loopAnimation;
	public float castTime;
	public float followThroughTime;
}

[Serializable]
public class SkillVFX
{
	public TrailRenderer vfxTrail;
	public GameObject obj;
	public SkillEventTrigger activationEvent;
	public SkillEventTrigger deactivationEvent;
	public bool isLocal;

	public void Activate()
	{
		if(vfxTrail != null)
			vfxTrail.enabled = true;
		if(obj != null)
			obj.SetActive(true);
	}

	public void Deactivate()
	{
		if(vfxTrail != null)
			vfxTrail.enabled = false;
		if(obj != null)
			obj.SetActive(false);
	}
}

