using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class StatusEffectData{

	public int ID;
	public int effect;
    public float effectValue;
	public float secondaryEffectValue;
	public float tertiaryEffectValue;
    public int effectType;
    public float effectDuration;
    public int numberOfProcs;
    public float procChance;
	[SerializeField] 
	public SkillEffectFormat effectFormat;
    public int triggerCondition;
	public bool affectAlly;
	public bool affectEnemy;
	public bool affectSelf;
}

//what does the skill affect
public enum StatusEffectType
{
	paralyse = 0,
	movementSpeed = 1,
	frozen = 2,
	damage = 3,
	leech = 4,
	force = 5,
    defense = 6,
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
public enum StatusEffectTarget
{
    all,
    self,
    target
}

//when is the skill triggered
public enum SkillEventTrigger
{
    onReady,
   	onPreCast,
	onUse,
	onFireOneShot,
    onHit,
    onHitSuccess,
    onReceiveHit,
	onFollowThrough,
	none
}

[Serializable]
public class impactData
{
	public Vector3 dir;
	public float force;
	public float duration;
	public float accelration;
	public float timer;
}

