using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ArmorSkill : MonoBehaviour {

   
    public bool isLimitedUse;

    public bool hasPressDownEvent;
    public bool hasPressUpEvent;
	public bool disableMovement;
    public int equipmentSlotIndex;

    public bool _skillActive = false;

    /*** attribute ***/

    public int itemID;
    public float cooldown;
    public int maxAmmoCount;
    public float fireSpeed;

    public bool isReloading;
    public float cooldownTimer;
    public float fireSpeedTimer;
	public float damage;

    public ArmorAttribute[] armorAttributes;
    private SkillEffect[] skillEffects;

    public List<SkillEffect> onUseSkillEffects;
    public List<SkillEffect> onHitSkillEffects;
    public List<SkillEffect> onReceiveHitSkillEffects;

    public Transform myTransform;
    public Animation myAnimation;
    public CharacterStatus myStatus;
	public CharacterMotor myMotor;
    public CharacterActionManager myManager;
    public Collider myCharacterCollider;
    public ArmorState armorState;

    public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

    public virtual void Initialise(Transform character, CharacterActionManager manager, int index)
    {
		myManager = manager;
        myAnimation = myManager.animationTarget;
        myTransform = character;
        myStatus = myManager.myStatus;
		myMotor = myManager.motor;
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
		if(myManager.effectsPool.GetPrefabPool(prefab) == null)
		{
			PrefabPool prefabPool = new PrefabPool(prefab);;
			prefabPool.preloadAmount = 3;
			prefabPool.preloadFrames = 5;
			Debug.Log("here");
			myManager.effectsPool.CreatePrefabPool(prefabPool);
		}
		//}
	}

	public void ActivateSkill(bool state)
	{
		_skillActive = state;
	}
        
        #region skill effects

    public void ApplyOnHitSkillEffects()
    {
    }

    public void ApplyOnReceiveHitSkillEffects()
    {
    }

    public void ApplyOnUseSkillEffects()
    {
        for (int i = 0; i < onUseSkillEffects.Count; i++) {
            switch(onUseSkillEffects[i].effectType)
            {
            case (int)SkillEffectCategory.speed:
                if(onUseSkillEffects[i].effectTarget == (int)TargetType.self)
                {
                    myStatus.ChangeMovementSpeed(onUseSkillEffects[i].effectValue);
                }
                //Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
        //Debug.Log("onuseskilleffects");
    }

    public void RemoveOnUseSkillEffects()
    {
        for (int i = 0; i < onUseSkillEffects.Count; i++) {
            switch(onUseSkillEffects[i].effectType)
            {
            case ((int)SkillEffectCategory.speed):
                if(onUseSkillEffects[i].effectTarget == (int)TargetType.self && onUseSkillEffects[i].effectFormat == (int)SkillEffectFormat.useDuration)
                {
                    myStatus.ChangeMovementSpeed((onUseSkillEffects[i].effectValue)* -1f);
					Debug.Log("movement speed = " + myStatus.movementSpeed);
                }
                //Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
    }

	#endregion

	#region skill effects and attributes setup

    public void InitializeAttributesStats(ArmorAttribute[] attributes, SkillEffect[] effects)
    {
        //characterTransform = Player.Instance.avatarObject.transform;
        skillEffects = effects;
        armorAttributes = attributes;
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
        for (int i = 0; i < skillEffects.Length ; i++) {
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onUse)
                onUseSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onHit)
                onHitSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == (int)SkillEffectTrigger.onReceiveHit)
                onReceiveHitSkillEffects.Add(skillEffects[i]);
        }
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
        if(anim.clip != null)
            StartCoroutine(TransferAnimation(anim));
        if(anim.castAnimation.clip != null)
            StartCoroutine(TransferAnimation(anim.castAnimation));
        if(anim.followThroughAnimation.clip != null)
            StartCoroutine(TransferAnimation(anim.followThroughAnimation));
    }

    public IEnumerator TransferAnimation(ArmorAnimation anim)
    {
        if(anim.clip != null)
        {
            myAnimation.AddClip(anim.clip, anim.clip.name);
            myAnimation[anim.clip.name].layer = anim.animationLayer;
            //StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
            yield return null;
            
            if(anim.addMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.addMixingTransforms.Count; i++) {
                    myAnimation[anim.clip.name].AddMixingTransform(GetBone(anim.addMixingTransforms[i]), false);
                }
            }

            if(anim.removeMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.removeMixingTransforms.Count; i++) 
                {
                    myAnimation[anim.clip.name].RemoveMixingTransform(GetBone(anim.removeMixingTransforms[i]));
                }
            }
        }
    }

    public virtual void UnEquip()
    {

    }

    public void RemoveSkillAnimation(SkillAnimation anim)
    {
        if(anim.clip != null)
            RemoveAnimation(anim.clip);
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
        myAnimation.RemoveClip(clip.name);
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
        Transform[] kids = myTransform.GetComponentsInChildren<Transform>();
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
			newHit.sourceName = myStatus.characterName;
			newHit.damage = damage;
			newHit.hitPosX = myTransform.position.x;
			newHit.hitPosY = myTransform.position.y;
			newHit.hitPosZ = myTransform.position.z;
			newHit.skillEffects = new List<SkillEffect>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].effectTarget == (int)TargetType.hitEnemies || onHitSkillEffects[i].effectTarget == (int)TargetType.allEnemies || onHitSkillEffects[i].effectTarget == (int)TargetType.all) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
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
			newHit.sourceName = myStatus.characterName;
			newHit.damage = damage;
			newHit.hitPosX = originPos.x;
			newHit.hitPosY = originPos.y;
			newHit.hitPosZ = originPos.z;
			newHit.skillEffects = new List<SkillEffect>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].effectTarget == (int)TargetType.hitEnemies || onHitSkillEffects[i].effectTarget == (int)TargetType.allEnemies || onHitSkillEffects[i].effectTarget == (int)TargetType.all) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
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

public enum ArmorState
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

