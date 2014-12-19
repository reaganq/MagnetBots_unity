using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BaseSkill : MonoBehaviour {

	public int ID;
	public string skillName;
	public SkillType skillType;
	public bool _isSkillActive = false;
    public bool hasPressDownEvent;
    public bool hasPressUpEvent;
	public bool disableMovement;
    public int equipmentSlotIndex;
	public SkillAnimation baseSkillAnimation;
    /*** attribute ***/
    public float cooldown;
    public int maxAmmoCount;
    public float fireSpeed;
    //public bool isReloading;
    //public float cooldownTimer;
    //public float fireSpeedTimer;
	public float damage;

    public ArmorAttribute[] armorAttributes;
    public List<StatusEffectData> skillStatusEffects;

    public CharacterStatus owner;
	public ActionManager ownerManager;
	public Animation ownerAnimation;
	public Transform ownerTransform;
    public SkillState armorState;

    public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

    public virtual void Initialise(CharacterStatus ownerStatus, int index)
    {
        owner = ownerStatus;
		ownerManager = ownerStatus.actionManager;
		ownerAnimation = ownerManager.myAnimation;
		ownerTransform = ownerStatus._myTransform;
		equipmentSlotIndex = index;
    }

    public virtual IEnumerator PressUp()
    {
        yield return null;
    }

    public virtual IEnumerator PressDown()
    {
        yield return null;
    }

    public virtual bool CanPressDown()
    {
        return false;
    }

    public virtual bool CanPressUp()
    {
        return false;
    }

	public virtual void Reset()
	{
	}

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
			prefabPool.preloadAmount = 3;
			prefabPool.preloadFrames = 5;
			Debug.Log("here");
			ownerManager.effectsPool.CreatePrefabPool(prefabPool);
		}
		//}
	}

	public void ActivateSkill(bool state)
	{
		_isSkillActive = state;
	}
        
        #region skill effects

	//give generic status effects to target
	public virtual void ProcessStatusEffects(int condition, CharacterStatus target, int allyFlag)
	{
		if(!owner.myPhotonView.isMine)
			return;
		
		foreach(StatusEffectData effect in skillStatusEffects)
		{
			if(effect.triggerCondition == condition)
			{
				//myself
				if(effect.affectSelf && allyFlag == 0)
				{
					if(effect.effect == 5)
					{
						owner.motor.AddImpact(ownerTransform.forward, effect.effectValue, effect.secondaryEffectValue, effect.tertiaryEffectValue);
					}
					//attribute change
					if(effect.effect == 3 || effect.effect == 1 || effect.effect == 6)
					{
						StatusEffect newEffect = owner.gameObject.AddComponent<StatusEffect>();
						newEffect.statusEffect = effect;
						newEffect.ownerSkill = this;
						owner.AddStatusEffect(newEffect);
						Debug.Log("adding status effect to myself");
					}
				}
				else if(effect.affectAlly && allyFlag == 1 || effect.affectEnemy && allyFlag == 2)
				{
					if(effect.effect == 5)
					{
						target.AddImpact(target._myTransform.forward, effect.effectValue, effect.secondaryEffectValue, effect.tertiaryEffectValue);
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
				//target
				
			}
		}
	}

	#endregion

	#region skill effects and attributes setup

    public void InitializeAttributesStats(ArmorAttribute[] attributes, StatusEffectData[] effects)
    {
        //characterTransform = Player.Instance.avatarObject.transform;
        //skillEffects = effects;
        //armorAttributes = attributes;
    }

    public void InitializeAttributesStats()
    {
        for (int i = 0; i < armorAttributes.Length ; i++) {
            if(armorAttributes[i].attributeName == ArmorAttributeName.cooldown)
                cooldown = armorAttributes[i].attributeValue;
			if(armorAttributes[i].attributeName == ArmorAttributeName.damage)
				damage = armorAttributes[i].attributeValue;
        }
    }

    public void PopulateSkillEffects()
    {
        /*for (int i = 0; i < skillEffects.Length ; i++) {
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onUse)
                onUseSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onHit)
                onHitSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onReceiveHit)
                onReceiveHitSkillEffects.Add(skillEffects[i]);
        }*/
    }

    public float GetAttributeValue(ArmorAttributeName name)
    {
        for (int i = 0; i < armorAttributes.Length; i++) {
            if(armorAttributes[i].attributeName == name)
                return armorAttributes[i].attributeValue;
        }
        return 0;
    }

	#endregion

    #region Animation Setup
   
    public void TransferSkillAnimation(SkillAnimation anim)
    {
        if(anim.precastAnimation.clip != null)
            StartCoroutine(TransferAnimation(anim.precastAnimation));
        if(anim.castAnimation.clip != null)
            StartCoroutine(TransferAnimation(anim.castAnimation));
        if(anim.followThroughAnimation.clip != null)
            StartCoroutine(TransferAnimation(anim.followThroughAnimation));
    }

    public IEnumerator TransferAnimation(ArmorAnimation anim)
    {
        if(anim.clip != null)
        {
            ownerAnimation.AddClip(anim.clip, anim.clip.name);
            ownerAnimation[anim.clip.name].layer = anim.animationLayer;
            //StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
            yield return null;
            
            if(anim.addMixingTransforms.Count>0)
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
            }
        }
    }

    public virtual void UnEquip()
    {

    }

    public void RemoveSkillAnimation(SkillAnimation anim)
    {
        if(anim.precastAnimation.clip != null)
            RemoveAnimation(anim.precastAnimation.clip);
        if(anim.castAnimation.clip != null)
            RemoveAnimation(anim.castAnimation.clip);
        if(anim.followThroughAnimation.clip != null)
            RemoveAnimation(anim.followThroughAnimation.clip);
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

	public virtual void HitTarget(HitBox target, bool isAlly)
	{
		HitInfo newHit = new HitInfo();
		
		if(!isAlly)
		{
			newHit.sourceName = owner.characterName;
			newHit.damage = damage;
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
	}

	public virtual void HitTarget(HitBox target, bool isAlly, Vector3 originPos)
	{
		HitInfo newHit = new HitInfo();
		
		if(!isAlly)
		{
			newHit.sourceName = owner.characterName;
			newHit.damage = damage;
			newHit.hitPosX = originPos.x;
			newHit.hitPosY = originPos.y;
			newHit.hitPosZ = originPos.z;
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
	}

}

public enum SkillState
{
    ready,
    casting,
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

