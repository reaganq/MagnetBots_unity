using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

	public Collider collider;
	public BaseSkill ownerSkill;
	public bool isActive = false;
	[HideInInspector]
	public float currentNumberOfTargets;
	public SkillEventTrigger activationEvent;
	public SkillEventTrigger deactivationEvent;

	public virtual void IgnoreOwnCollisions(Collider ownerHitBox)
	{
		Physics.IgnoreCollision(collider, ownerHitBox);
	}

	public virtual void Activate()
	{
		isActive = true;
		this.gameObject.SetActive(true);
	}
	
	public virtual void Deactivate()
	{
		isActive = false;
		this.gameObject.SetActive(false);
	}

	public virtual void Initialise(BaseSkill skill)
	{
		ownerSkill = skill;
		currentNumberOfTargets = 0;
	}

	public virtual void Reset()
	{
		currentNumberOfTargets = 0;
	}
}
