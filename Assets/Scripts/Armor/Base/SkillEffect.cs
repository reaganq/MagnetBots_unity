using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SkillEffect{

    public float effectValue;
    public int effectType;
    public int effectFormat;
    public float effectDuration;
    public int numberOfProcs;
    public float procChance;
    public int effectTarget;
    public int effectTrigger;
    public bool stackable;
    public bool refreshable;

}

//what does the skill affect
public enum SkillEffectCategory
{
    stun = 0 ,
    slow = 1,
    armor = 2,
    damage = 3,
    critPercent = 4,
    invulnerability = 5,
    damageBlock = 6,
    lifeDrain = 7,
    knockback = 8,
    moveTarget =9,
    heal = 10,
    speed = 11,
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

