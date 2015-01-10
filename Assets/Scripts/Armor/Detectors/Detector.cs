using UnityEngine;
using System.Collections;

public class Detector : MonoBehaviour {

	public Collider collider;
	public BaseSkill ownerSkill;
	public bool isActive = false;
	public float currentNumberOfTargets;

	public virtual void IgnoreCollisions(Collider ownerHitBox)
	{
		Physics.IgnoreCollision(collider, ownerHitBox);
	}

	public virtual void Activate()
	{
		isActive = true;
	}
	
	public virtual void Deactivate()
	{
		isActive = false;
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
