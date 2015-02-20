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
	public bool isLocal;

	public virtual void IgnoreOwnCollisions()
	{
		for (int i = 0; i < ownerSkill.ownerStatus.hitboxes.Count; i++) 
		{
			Physics.IgnoreCollision(collider, ownerSkill.ownerStatus.hitboxes[i]);
		}
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
		IgnoreOwnCollisions();
	}

	public virtual void Reset()
	{
		currentNumberOfTargets = 0;
	}
}
