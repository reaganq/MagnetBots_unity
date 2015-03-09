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
	public bool isProjectile = false;

	public virtual void IgnoreOwnCollisions()
	{
		for (int i = 0; i < ownerSkill.ownerStatus.hitboxes.Count; i++) 
		{
			Physics.IgnoreCollision(collider, ownerSkill.ownerStatus.hitboxes[i]);
			//Debug.Log("ignored collision: " + ownerSkill.ownerStatus.hitboxes[i].name);
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
		//Invoke("IgnoreOwnCollisions", 2f);
		if(!isProjectile)
		{

			//myFunction = Deactivate;
			//InvokeNextFrame(myFunction);
		}
		else
			Activate();
	}

	public virtual void Reset()
	{
		currentNumberOfTargets = 0;
	}

	public delegate void Function();
	Function myFunction;
	
	public void InvokeNextFrame(Function function)
	{
		try
		{
			StartCoroutine(_InvokeNextFrame(function));    
		}
		catch
		{
			Debug.Log ("Trying to invoke " + function.ToString() + " but it doesnt seem to exist");    
		}            
	}
	
	private IEnumerator _InvokeNextFrame(Function function)
	{
		yield return null;
		function();
	}
}
