using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BaseSkill : MonoBehaviour {

	//automated stuff
	public int ID;
	public bool isPressedDown = false;
	public CharacterStatus owner;
	public ActionManager ownerManager;
	public Animation ownerAnimation;
	public Transform ownerTransform;
	public SkillState armorState;
	public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

	public bool isBusy = false;

	public string skillName;
	public SkillType skillType;
    
	public bool disableMovement;
	public bool restrictedMovement;
    
	public SkillAnimation baseSkillAnimation;
	public float targetLimit = Mathf.Infinity;


    /*** attribute ***/
    public float cooldown;
    public int maxAmmoCount;
    public float fireSpeed;
	public float damage;

    public ArmorAttribute[] armorAttributes;
    public List<StatusEffectData> skillStatusEffects;

    



    public virtual bool CanPressDown()
    {
        return false;
    }

    public virtual bool CanPressUp()
    {
        return false;
    }

	public virtual void ResetSkill()
	{
		isBusy = false;
	}

	public void ActivateSkill(bool state)
	{
		isPressedDown = state;
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
			prefabPool.preloadAmount = 3;
			prefabPool.preloadFrames = 5;
			Debug.Log("here");
			ownerManager.effectsPool.CreatePrefabPool(prefabPool);
		}
		//}
	}

	#endregion
        
	#region apply status effects

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

    

	#region hitting targets

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

	public virtual void HitTarget(int targetID, bool isAlly, Vector3 detectorPos, Vector3 targetPos)
	{
		//HitInfo newHit = new HitInfo();
		
		/*if(!isAlly)
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
		}*/
	}

	#endregion

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

