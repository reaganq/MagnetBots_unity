using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SimpleFSM : ActionManager {

    public List<CharacterStatus> alltargets;
    public List<CharacterStatus> availableTargets;

	//check if i'm pointing at my target
	public float targetAngleDifference;
    public AISkill[] skills;
    public AISkill activeSkill;
	public List<AISkill> availableSkills;

	public Job restJob;
    public Job searchTargetJob;
    public Job moveToTargetJob;
    public Job usingSkillJob;
	public Job cancelSkillJob;

    public Transform targetObject;
	public float distanceToTargetObject{
		get
		{
			float distance = Vector3.Distance(targetObject.position, _myTransform.position);
			return distance;
		}
	}
	public CharacterController targetCharacterController;
	public Transform fireObject;

	public bool moveToTarget = false;
	public float targetDistance;
    public bool aimAtTarget = false;
	public float targetAngle;
	public SpawnPool effectsPool;

	//base animations
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip deathAnim;
    public AnimationClip gotHitAnim;

	public ArenaManager arena;
	public int ownerID;
	public int InitViewID;

	public AIState _state;
	public AIState state
	{
		get
		{
			return _state;
		}
		set
		{
			ExitState(_state);
			_state = value;
			EnterState(_state);
		}
	}

	public virtual void EnterState(AIState state)
	{
		switch(state)
		{
		case AIState.preInitialised:
			break;
			//has owner arena and all intended targets
		case AIState.initialised:
			break;
			//all players have loaded into arena
		case AIState.ready:
			break;
			//play battle taunts
		case AIState.battleTaunts:
			break;
		case AIState.selectingSkill:
			break;
		case AIState.rest:
			break;
		case AIState.checkingSkillRequirements:
			break;
		case AIState.fulfillingSkillConditions:
			break;
		case AIState.executingSkill:
			break;
		case AIState.taunting:
			break;
		case AIState.victory:
			break;
		case AIState.death:
		//if(cancelSkillJob != null) cancelSkillJob.kill();
			myPhotonView.RPC("PlayAnimation", PhotonTargets.All, deathAnim.name);
			Debug.LogWarning("die");
			arena.CheckDeathStatus();
			break;
		}
	}

	public virtual void ExitState(AIState state)
	{
		switch(state)
		{
			//just instantiated
		case AIState.preInitialised:
			break;
			//has owner arena and all intended targets
		case AIState.initialised:
			break;
			//all players have loaded into arena
		case AIState.ready:
			break;
			//play battle taunts
		case AIState.battleTaunts:
			break;
		case AIState.selectingSkill:
			break;
		case AIState.rest:
			break;
		case AIState.checkingSkillRequirements:
			break;
		case AIState.fulfillingSkillConditions:
			break;
		case AIState.executingSkill:
			break;
		case AIState.taunting:
			break;
		case AIState.victory:
			break;
		case AIState.death:
			break;
		}
	}

    public enum AIState
    {
        preInitialised,
		initialised,
		ready,
		battleTaunts,
		selectingSkill,
		checkingSkillRequirements,
		fulfillingSkillConditions,
		executingSkill,
		rest,
		taunting,
		victory,
		death
    }

	// setting up some skills, get to know its own components
	public override void Start () {

		base.Start();
        //skillChances = new float[skills.Length];
		for (int i = 0; i <skills.Length; i++) 
		{
			skills[i].InitialiseAISkill(myStatus, i);		
		}
		
		foreach(AnimationState anim in myAnimation)
		{
			if(anim.name == idleAnim.name)
			{
				anim.layer = 0;
			}
			else if(anim.name == deathAnim.name)
			{
				anim.layer = 3;
			}
			else
				anim.layer = 1;
		}
	}

	public virtual void Update()
	{
		if(targetObject != null)
		{
			if(moveToTarget)
			{
				MoveToTarget();
			}
			if(aimAtTarget)
			{
				AimAtTarget();
			}
			
			Vector3 targetDir = Vector3.zero;
			Vector3 forward = Vector3.zero;
			if(fireObject != null)
			{
				forward = fireObject.forward;
				targetDir = targetObject.position - fireObject.position;
			}
			else
			{
				forward = _myTransform.forward;
				targetDir = targetObject.position - _myTransform.position;
			}
			forward.y = targetDir.y = 0;
			
			targetAngleDifference = Vector3.Angle(targetDir, forward);
		}
	}

	public void MoveToTarget()
	{
	}
	
	public void AimAtTarget()
	{
		if(targetObject != null)
		{
			if(fireObject != null)
			{
				Vector3 targetPos = targetObject.position;
				if(targetCharacterController != null)
				{
					float scaledVal = Mathf.Clamp(distanceToTargetObject/15.0f, 0.0f, 1.0f);
					targetPos += targetCharacterController.velocity*scaledVal ;
				}
				
				Vector3 fireObjectPos = fireObject.position;
				targetPos.y = fireObjectPos.y = 0;
				Quaternion newRotation = Quaternion.LookRotation(targetPos - fireObjectPos);
				_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
				//_transform.forward += (targetPos - fireObjectPos) * _characterStatus.rotationSpeed * Time.deltaTime;
			}
			else
			{
				Quaternion newRotation = Quaternion.LookRotation(targetObject.position - _myTransform.position);
				_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
				//_transform.forward += (targetObject.position - _transform.position) * _characterStatus.rotationSpeed * Time.deltaTime;
			}
		}
	}

	//load in owner arena and list of all intended targets
	public void InitialiseAI()
	{
	}

	[RPC]
	public void NetworkInitialiseAI()
	{
	}

	public void StartBattle()
	{
	} 

	[RPC]
	public void NetworkStartBattle()
	{
	}

	[RPC]
	public void PlayQueuedAnimation(string clip, int mode)
	{
		if(mode == 0)
			myAnimation.PlayQueued(clip, QueueMode.PlayNow);
		else if (mode == 1)
			myAnimation.PlayQueued(clip, QueueMode.CompleteOthers);
	}

	public void UseAISkill(int index)
	{
		myPhotonView.RPC("NetworkUseAISkill", PhotonTargets.All, index);
	}

	[RPC]
	public void NetworkUseAISkill(int index)
	{

	}

	//All
	[RPC]
	public void SetupArena(int id, int viewid)
	{
		myPhotonView = GetComponent<PhotonView>();
		Debug.Log("VIEW ID: "+ myPhotonView.viewID);
		InitViewID = myPhotonView.viewID;
		arena = PlayerManager.Instance.ActiveWorld.ArenaManagers[id];
		arena.enemyFSMs.Add(this);
		myPhotonView.viewID = viewid;
		ownerID = myPhotonView.ownerId;
	}

	//all
	[RPC]
	public void ChangeOwner(int viewid)
	{
		myPhotonView.viewID = viewid;
		ownerID = myPhotonView.ownerId;
		Debug.Log(ownerID);
		if(myPhotonView.isMine)
			EnterAIState(AIState.rest);
	}

	//all
	[RPC]
	public void RevertOwner()
	{
		myPhotonView.viewID = InitViewID;
		Debug.Log(ownerID);
		state = AIState.rest;
	}

	[RPC]
	public void SpawnParticleEffect()
	{
	}
	
	[RPC]
	public void SpawnProjectile(string projectileName, Vector3 pos, Quaternion rot, float speed, Vector3 targetPos, int index)
	{
		Transform projectile = effectsPool.prefabs[projectileName];
		Transform projectileInstance = effectsPool.Spawn(projectile, pos, Quaternion.identity, null);
		IgnoreCollisions(projectileInstance.collider);
		if(projectileInstance.rigidbody != null)
		{
			Vector3 dir = rot * Vector3.forward;
			dir.y = (targetPos - pos).normalized.y * 0.5f;
			projectileInstance.rigidbody.AddForce(dir * speed);
			//projectileInstance.rigidbody.AddForce( _transform.forward * speed);
		}
		BulletProjectile src = projectileInstance.GetComponent<BulletProjectile>();
		if(src != null)
		{
			src.masterAISkill = skills[index];
			src.status = myStatus;
			src.pool = effectsPool.poolName;
		}
	}

	[RPC]
	public void SpawnParticle(string particleName, Vector3 pos)
	{
		Debug.Log("spawning particle: " + particleName);
		Transform particle = effectsPool.prefabs[particleName];
		ParticleSystem particleSys = particle.GetComponent<ParticleSystem>();
		effectsPool.Spawn(particleSys, pos, Quaternion.identity, null);
	}
	
	public void IgnoreCollisions(Collider collider)
	{
		for (int i = 0; i < myStatus.hitboxes.Count; i++) 
		{
			Physics.IgnoreCollision(collider, myStatus.hitboxes[i]);
		}
	}

	public void SelectSkill()
	{
		activeSkill = ChooseSkill();
		if(activeSkill != null)
		{
			myPhotonView.RPC("NetworkSelectSkill", PhotonTargets.Others, activeSkill.skillID);
			if(activeSkill.targetLimit >0)
			{
				SelectTarget(activeSkill.skillRangeMax);
			}
			EnterState(AIState.fulfillingSkillConditions);
		}
		else
			EnterAIState(AIState.rest);
	}

	public virtual IEnumerator Rest()
	{
		yield return new WaitForSeconds(Random.Range (1,4));
		//play taunts
		state = AIState.selectingSkill;
	}

	[RPC]
	public void NetworkSelectSkill(int id)
	{
		for (int i = 0; i < skills.Length; i++) {
			if(skills[i].skillID == id)
			{
				activeSkill = skills[i];
				return;
			}
		}
	}

	public void RefreshPotentialTargets()
	{
		alltargets = arena.players;
		availableTargets.Clear();
		
		for (int i = 0; i < alltargets.Count; i++) {
			if(alltargets[i] != null && alltargets[i].curHealth > 0)
			{
				availableTargets.Add(alltargets[i]);
				Debug.Log("add player");
			}
		}
	}

	public void SelectTarget(float skillRange)
	{
		RefreshPotentialTargets();
		for (int i = availableTargets.Count -1; i >= 0; i--) {
			if(DistanceToTargetCS(availableTargets[i]) > skillRange)
				availableTargets.Remove(availableTargets[i]);
		}
		if(availableTargets.Count > 0)
		{
			targetObject = availableTargets[Random.Range(0, availableTargets.Count)].transform;
			targetCharacterController = targetObject.gameObject.GetComponent<CharacterController>();
		}
		else
		{
			targetObject = null;
			targetCharacterController = null;
		}
	}

	[RPC]
	public void NetworkSelectTarget(float skillRange)
	{
	}
	
    public AISkill ChooseSkill()
    {
		RefreshPotentialTargets();
		RefreshAvailableSkillsList();
        float total = 0f;
		float[] skillChances = new float[availableSkills.Count];

        for (int i = 0; i < availableSkills.Count; i++) {
			skillChances[i] = availableSkills[i].weighting;
			total += skillChances[i];
        }

        float randomPoint = Random.value * total;
		if(availableSkills.Count < 1)
			return null;

		for (int i = 0; i < availableSkills.Count; i++) {
            if(randomPoint < skillChances[i])
            {
				return availableSkills[i];
			}
            else
                randomPoint -= skillChances[i];
        }
        return availableSkills[skillChances.Length -1];
    }

	public void RefreshAvailableSkillsList()
	{
		availableSkills.Clear();
		for (int i = 0; i < skills.Length; i++) {
			if(skills[i].CanUseSkill())
				availableSkills.Add(skills[i]);
		}
	}

	public int NumTargetsInRange(float distance)
	{
		int num = 0;
		for (int i = 0; i < availableTargets.Count; i++) {
			if(DistanceToTargetCS(availableTargets[i]) <= distance)
				num ++;
				}
		return num;
	}

	public void SetTargetDistance(float distance)
	{
		moveToTarget = true;
		targetDistance = distance;
	}

	public void SetTargetAngle(float angle)
	{
		aimAtTarget = true;
		targetAngle = angle;
	}

	public virtual bool hasFulfilledSkillConditions()
	{
		if(moveToTarget && distanceToTargetObject < targetDistance)
			return false;
		if(aimAtTarget && targetAngleDifference < targetAngle)
			return false;
		return true;
	}

	public void UseActiveSkill()
	{
		myPhotonView.RPC("NetworkUseActiveSkill", PhotonTargets.All);
	}

	[RPC]
	public void NetworkUseActiveSkill()
	{
		activeSkill.UseSkill();
	}

	public void FireOneShot()
	{
		myPhotonView.RPC("NetworkFireOneShot", PhotonTargets.All);
	}

	[RPC]
	public void NetworkFireOneShot()
	{
		activeSkill.FireOneShot();
	}

	public void OnDestroy()
	{
		if(searchTargetJob != null && searchTargetJob.running)
			searchTargetJob.kill();
		if(moveToTargetJob != null && moveToTargetJob.running)
			moveToTargetJob.kill();
		if(usingSkillJob != null && usingSkillJob.running)
			usingSkillJob.kill();
		if(cancelSkillJob != null && cancelSkillJob.running)
			cancelSkillJob.kill();
	}

	public void EnterAIState(AIState state)
	{
		myPhotonView.RPC("NetworkEnterState", PhotonTargets.All, (int)state);
	}

	[RPC]
	public void NetworkEnterAIState(int i)
	{
		state = (AIState)i;
	}

	public float DistanceToTargetCS(CharacterStatus cs)
	{
		float distance = Vector3.Distance(cs.transform.position, _myTransform.position);
		return distance;
	}
}
