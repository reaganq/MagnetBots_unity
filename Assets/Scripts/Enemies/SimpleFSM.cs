using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SimpleFSM : ActionManager {

	//checks for all players in arena
    public List<CharacterStatus> alltargets;
	//checks for all valid targets
    public List<CharacterStatus> availableTargets;

	public Transform _myTransform;
	
    public AISkill[] skills;
	//the currently used skill
    public AISkill activeSkill;
	//not all skills are available at all times
	public List<AISkill> availableSkills;

	public Job restJob;
    public Job searchTargetJob;
    public Job moveToTargetJob;
    public Job usingSkillJob;
	public Job cancelSkillJob;

	//target set depending on needs of skill
    public Transform targetObject;

	public float distanceToTargetObject{
		get
		{
			float distance = Vector3.Distance(targetObject.position, _myTransform.position);
			return distance;
		}
	}
	//check if i'm pointing at my target
	//don't touch
	public float targetAngleDifference{
		get{
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
			forward.y = 0;
			targetDir.y = 0;
			return Vector3.Angle(targetDir, forward);
		}
	}
	public CharacterController targetCharacterController;
	//position of gun if there is one
	public Transform fireObject;

	//should we move towards target?
	public bool moveToTarget = false;
	//how close should we get to the target?
	public float targetDistance;
	//should we aim at the target?
    public bool aimAtTarget = false;
	//how accurate should the aim be?
	public float targetAngle;
	public bool moveToPosition = false;
	public Vector3 targetPosition;

	//base animations
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip deathAnim;
    public AnimationClip gotHitAnim;

	//setup through initialise
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
		if(state == AIState.death)
		{
		//if(cancelSkillJob != null) cancelSkillJob.kill();
			PlayAnimation(deathAnim.name, true);
			Debug.LogWarning("die");
			arena.CheckDeathStatus();
		}
	}

	public virtual void ExitState(AIState state)
	{

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
	public virtual void Initialise () {

		_myTransform = transform;
		MakeSpawnPool();
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
		state = AIState.preInitialised;

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
			if(moveToPosition)
			{
				MoveToPosition();
			}
		}
	}

	public void MoveToTarget()
	{
		Quaternion newRotation = Quaternion.LookRotation(targetObject.position - _myTransform.position);
		_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
		//move
		
		//Vector3 moveDirection = targetObject.position - _myTransform.position;
		//moveDirection.y = 0;
		//Vector3 movementOffset = moveDirection.normalized * myStatus.curMovementSpeed * Time.deltaTime;
		//movementOffset += Physics.gravity;
		//movementOffset += Physics.gravity;
		//myMotor.Move(movementOffset);
		_myTransform.position = Vector3.MoveTowards(_myTransform.position, targetObject.position, myStatus.curMovementSpeed * Time.deltaTime);
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

	public void MoveToPosition()
	{
		Quaternion newRotation = Quaternion.LookRotation(targetPosition - _myTransform.position);
		_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
		_myTransform.position = Vector3.MoveTowards(_myTransform.position, targetPosition, myStatus.curMovementSpeed * Time.deltaTime);
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
		EnterState(AIState.rest);
	}

	public virtual void Rest()
	{
		myPhotonView.RPC("NetworkRest", PhotonTargets.All, 2);
	}

	[RPC]
	public void NetworkRest(float timer)
	{
		StartCoroutine(Rest(timer));
	}

	public IEnumerator Rest(float timer)
	{
		yield return new WaitForSeconds(timer);
		EnterAIState(AIState.selectingSkill);
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

	public void SelectTarget(float minSkillRange, float skillRange)
	{
		RefreshPotentialTargets();
		for (int i = availableTargets.Count -1; i >= 0; i--) {
			if(DistanceToTargetCS(availableTargets[i]) > skillRange || DistanceToTargetCS(availableTargets[i]) < minSkillRange)
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

	public void SelectSkill()
	{
		activeSkill = ChooseSkill();
		if(activeSkill != null)
		{
			myPhotonView.RPC("NetworkSelectSkill", PhotonTargets.Others, activeSkill.skillID);
			if(activeSkill.targetLimit >0)
			{
				SelectTarget(activeSkill.skillRangeMin, activeSkill.skillRangeMax);
			}
			EnterState(AIState.executingSkill);
        }
        else
            EnterAIState(AIState.rest);
    }
	
    public AISkill ChooseSkill()
    {
		RefreshPotentialTargets();
		RefreshAvailableSkillsList();
		if(availableSkills.Count < 1)
			return null;
        float total = 0f;
		float[] skillChances = new float[availableSkills.Count];

        for (int i = 0; i < availableSkills.Count; i++) {
			skillChances[i] = availableSkills[i].weighting;
			total += skillChances[i];
        }

        float randomPoint = Random.value * total;

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
    
    #region movement conditions

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

	public void SetTargetPosition(Vector3 targetPos)
	{
		moveToPosition = true;
		targetPosition = targetPos;
	}

	public virtual bool hasFulfilledSkillConditions()
	{
		if(moveToTarget && distanceToTargetObject < targetDistance)
			return false;
		if(aimAtTarget && targetAngleDifference < targetAngle)
			return false;
		if(moveToPosition && (Vector3.Distance(_myTransform.position, targetPosition) > 1))
			return false;
		return true;
	}

	#endregion
	#region skill usage
	public void UseActiveSkill()
	{
		myPhotonView.RPC("NetworkUseActiveSkill", PhotonTargets.All, activeSkill.skillID);
	}

	[RPC]
	public void NetworkUseActiveSkill(int index)
	{
		skills[index].UseSkill();
	}
	
	public void FireOneShot(int skillIndex)
	{
		myPhotonView.RPC("NetworkFireOneShot", PhotonTargets.All, skillIndex);
	}

	[RPC]
	public void NetworkFireOneShot(int skillIndex)
	{
		skills[skillIndex].FireOneShot();
	}

	[RPC]
	public void SpawnProjectile(string projectileName, Vector3 pos, Quaternion rot, float speed, Vector3 targetPos, int index)
	{
		Transform projectile = effectsPool.prefabs[projectileName];
		Transform projectileInstance = effectsPool.Spawn(projectile, pos, Quaternion.identity, null);
		//IgnoreCollisions(projectileInstance.collider);
		if(projectileInstance.rigidbody != null)
		{
			Vector3 dir = rot * Vector3.forward;
			dir.y = (targetPos - pos).normalized.y * 0.5f;
			projectileInstance.rigidbody.AddForce(dir * speed);
			//projectileInstance.rigidbody.AddForce( _transform.forward * speed);
		}
		ProjectileCollider src = projectileInstance.GetComponent<ProjectileCollider>();
		if(src != null)
		{
			src.Initialise(skills[index]);
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

	#endregion

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
		myPhotonView.RPC("NetworkEnterAIState", PhotonTargets.All, (int)state);
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
