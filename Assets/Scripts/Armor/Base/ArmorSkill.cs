using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorSkill : MonoBehaviour {

   
    public bool isLimitedUse;
    public bool isPlayerOwned;
    private string OwnerId;

    public bool hasPressDownEvent;
    public bool hasPressUpEvent;
    public EquipmentSlots equipmentSlot;

    public bool _skillActive = false;

    /*** attribute ***/

    public int itemID;
    public float cooldown;
    public int maxAmmoCount;
    public float fireSpeed;

    public bool isReloading;
    public float cooldownTimer;
    public float fireSpeedTimer;

    private ArmorAttribute[] armorAttributes;
    private SkillEffect[] skillEffects;

    public List<SkillEffect> onUseSkillEffects;
    public List<SkillEffect> onHitSkillEffects;
    public List<SkillEffect> onReceiveHitSkillEffects;

    public Transform characterTransform;
    public Animation characterAnimation;
    public CharacterStatus myStatus;
    public CharacterActionManager characterManager;
    public Collider characterCollider;
    public ArmorState armorState;

    public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

    public virtual void Initialise(Animation target, Transform character, Collider masterCollider, CharacterStatus status, CharacterActionManager manager)
    {
        characterAnimation = target;
        characterTransform = character;
        characterManager = manager;
        characterCollider = masterCollider;
        myStatus = status;
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

    public virtual void HitEnemy(HitBox target)
    {
    }

	public virtual void HitAlly(HitBox target)
    {
    }

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
            case SkillEffectCategory.speed:
                if(onUseSkillEffects[i].effectTarget == TargetType.self)
                {
                    myStatus.ChangeMovementSpeed(onUseSkillEffects[i].effectValue);
                }
                //Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
        Debug.Log("onuseskilleffects");
    }

    public void RemoveOnUseSkillEffects()
    {
        for (int i = 0; i < onUseSkillEffects.Count; i++) {
            switch(onUseSkillEffects[i].effectType)
            {
            case SkillEffectCategory.speed:
                if(onUseSkillEffects[i].effectTarget == TargetType.self && onUseSkillEffects[i].durationFormat == SkillEffectFormat.useDuration)
                {
                    myStatus.ChangeMovementSpeed((onUseSkillEffects[i].effectValue)* -1f);
                }
                //Debug.Log("sopeed = " + characterManager.motor.runSpeed);
                break;
            }
        }
    }

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

        }
    }

    public void PopulateSkillEffects()
    {
        for (int i = 0; i < skillEffects.Length ; i++) {
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onUse)
                onUseSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onHit)
                onHitSkillEffects.Add(skillEffects[i]);
            if(skillEffects[i].effectTrigger == SkillEffectTrigger.onReceiveHit)
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
            characterAnimation.AddClip(anim.clip, anim.clip.name);
            characterAnimation[anim.clip.name].layer = anim.animationLayer;
            //StartCoroutine(MixingTransforms( anim.addMixingTransforms, anim.removeMixingTransforms, anim.clip));
            yield return null;
            
            if(anim.addMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.addMixingTransforms.Count; i++) {
                    characterAnimation[anim.clip.name].AddMixingTransform(GetBone(anim.addMixingTransforms[i]), false);
                }
            }

            if(anim.removeMixingTransforms.Count>0)
            {
                for (int i = 0; i < anim.removeMixingTransforms.Count; i++) 
                {
                    characterAnimation[anim.clip.name].RemoveMixingTransform(GetBone(anim.removeMixingTransforms[i]));
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
        characterAnimation.RemoveClip(clip.name);
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
        Transform[] kids = characterTransform.GetComponentsInChildren<Transform>();
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

