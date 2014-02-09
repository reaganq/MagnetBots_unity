using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SkillEffect{

    public float effectValue;
    public SkillEffectCategory effectType;
    public SkillEffectFormat durationFormat;
    public float effectDuration;
    public int numberOfProcs;
    public float procChance;
    public TargetType effectTarget;
    public SkillEffectTrigger effectTrigger;
    public bool stackable;
    public bool refreshable;

}

public class SkillEffectMultiplier
{
    public int skillIndex;
    public float skillMultiplier;
}

public struct EquippedSkillEffect
{
    public SkillEffect skillEffect;
    public float skillMultiplier;

    public EquippedSkillEffect(SkillEffect sE, float multiplier)
    {
        skillEffect = sE;
        skillMultiplier = multiplier;
    }
}

//what does the skill affect
public enum SkillEffectCategory
{
    stun,
    slow,
    armor,
    damage,
    critPercent,
    invulnerability,
    damageBlock,
    lifeDrain,
    knockback,
    moveTarget,
    heal,
    speed
}

//format of skill
public enum SkillEffectFormat
{
    timed,
    useDuration,
    instant,
    forever
}

//what does the skill target
public enum TargetType
{
    none,
    all,
    hitEnemies,
    allEnemies,
    allAllies,
    hitAllies,
    self,
    others,
    selfAllies
}

//when is the skill triggered
public enum SkillEffectTrigger
{
    onEquip,
    onUse,
    onHit,
    onHitSuccess,
    onReceiveHit
}

