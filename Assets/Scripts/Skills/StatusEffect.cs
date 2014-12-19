using UnityEngine;
using System.Collections;

public class StatusEffect : MonoBehaviour {

	public StatusEffectData statusEffect;
	public BaseSkill ownerSkill;
	public CharacterStatus owner;
	public int ID;
	public bool effectIsActive = false;
	public bool isPaused = false;
	public float duration;
	
	public virtual void StartEffect(CharacterStatus ownerCS, int id)
	{
		effectIsActive = true;
		ID = id;
		owner = ownerCS;
		duration = statusEffect.effectDuration;
	}
	
	public void Update()
	{
		if(!effectIsActive || statusEffect == null || isPaused)
			return;
		
		if(statusEffect.effectFormat == SkillEffectFormat.timed)
		{
			duration -= Time.deltaTime;
			if(duration <= 0)
				EndEffect();
		}
	}
	
	public virtual void EndEffect()
	{
		effectIsActive = false;
		owner.RemoveStatusEffect(this);
		Destroy(this);
	}
	
	public virtual void PauseEffect()
	{
	}

}
