using UnityEngine;
using System.Collections;

public class AbilitySkill : MonoBehaviour {

	public int ID;

	public Job skillJob;
	public bool skillIsActive;
	public bool hasPressAction;
	public bool hasReleaseAction;
	public AbilityState abilityState;
	//pre-action

	//main-action

	//post-action

	public virtual void Start()
	{
		abilityState = AbilityState.idle;
	}

	//when you press skill button
	public virtual void PressSkill()
	{
		//start main sequence
	}

	//when you release skill button
	public virtual void ReleaseSkill()
	{
		//start end sequence if there is one
	}

	public virtual void ActivateSkill()
	{
		skillIsActive = true;
	}
	
	public virtual void DeactivateSkill()
	{
		skillIsActive = false;
	}
}

public enum AbilityState
{
	idle,
	preAction,
	mainAction,
	postAction,
	reload,
}