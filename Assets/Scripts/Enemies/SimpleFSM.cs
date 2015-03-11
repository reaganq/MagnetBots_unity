using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SimpleFSM : ActionManager {

	//checks for all players in arena
    public List<CharacterStatus> alltargets;
	//checks for all valid targets
    public List<CharacterStatus> availableTargets;
    public AISkill[] skills;
	//the currently used skill
    public AISkill activeSkill;
	//not all skills are available at all times
	public List<AISkill> availableSkills;
	public List<AITaunt> taunts;
	public CharacterController controller;

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
	
	//should we aim at the target?
	public bool aimAtTarget = false;
	public bool moveToTargetRange = false;
	//how close should we get to the target?

	public float targetDistance;
	//how accurate should the aim be?
	public float targetAngle;

	public bool trackTargetObject;
	public Vector3 targetPosition;

	public bool isMoving = false;
	public bool canMove = true;
	public bool canRotate = true;

	//base animations
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip deathAnim;
    public AnimationClip gotHitAnim;

	//setup through initialise
	public ArenaManager arena;
	public int ownerID;
	public int InitViewID;
	private Vector3 moveVector = Vector3.zero;

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
		if(myPhotonView.isMine)
		{
			switch(state)
			{
			case AIState.preInitialised:
				myStatus.Invulnerable = true;
				break;
			case AIState.initialised:
				break;
			case AIState.ready:
				for (int i = 0; i <skills.Length; i++) 
				{
					skills[i].InitialiseAISkill(myStatus, i);		
				}
				myStatus.Invulnerable = false;
				//check for any ready taunts
				//if not, select skill
				Invoke("ReadyUp", CheckTaunts(state));
				break;
			case AIState.selectingSkill:
				//Invoke("SelectSkill", CheckTaunts(state));
				break;
			case AIState.executingSkill:
				Invoke("UseActiveAISkill", CheckTaunts(state));
				break;
			case AIState.rest:
				Invoke("Rest", CheckTaunts(state));
				break;
			case AIState.victory:
				if(restJob != null && restJob.running)
					restJob.kill();

				Debug.LogWarning("win");
				//play win animation
				break;
			case AIState.death:
				//die();
				//cancel current skill
				if(restJob != null && restJob.running)
					restJob.kill();
				PlayAnimation(deathAnim.name, true);
				Debug.LogWarning("die");
				arena.CheckDeathStatus();
				break;
			}
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
			//SelectSkill();
			break;
		case AIState.rest:
			if(restJob != null && restJob.running)
				restJob.kill();
			break;
		case AIState.executingSkill:
			if(activeSkill != null && activeSkill.useSkillJob.running)
				activeSkill.useSkillJob.kill();
			ResetPhysicalMovementStatus();
			break;
		case AIState.taunting:
			break;
		case AIState.victory:
			break;
		}
	}

	public override void Start ()
	{
		base.Start ();
		state = AIState.preInitialised;
	}
	// setting up some skills, get to know its own components
	public virtual void Initialise (ArenaManager ownerArena, int newViewId) {

		Debug.LogWarning("initialising ai");
		arena = ownerArena;
		myPhotonView = GetComponent<PhotonView>();
		myPhotonView.viewID = newViewId;
		ownerID = myPhotonView.ownerId;
		_myTransform = transform;
		MakeSpawnPool();

        //skillChances = new float[skills.Length];

		
		foreach(AnimationState anim in myAnimation)
		{
			if(anim.name == idleAnim.name)
			{
				anim.layer = 0;
			}
			else if(anim.name == deathAnim.name)
			{
				anim.layer = 5;
			}
			else if(anim.name == runningAnim.name)
				anim.layer = 0;
		}
		EnterAIState(AIState.initialised);

	}

	public void StartBattle()
	{
		EnterAIState(AIState.ready);
	}

	public virtual void Update()
	{
		if(!myPhotonView.isMine)
			return;

		moveVector = Vector3.zero;
		if(state == AIState.executingSkill)
			{
			if(moveToTargetRange && targetObject != null)
			{
				targetPosition = targetObject.position;
			}
			if(canMove)
			{
				MoveToPosition();
			}
			else
			{
				if(canRotate)
					AimAtTarget();
			}
		}

		controller.Move(moveVector * myStatus.curMovementSpeed * Time.deltaTime);
		if(controller.velocity.magnitude > 0.2)
		{
			AnimateToRunning();
		}
		else
			AnimateToIdle();
	}

	public override void EnableMovement()
	{
		canMove = true;
	}

	public override void DisableMovement()
	{
		canMove = false;
	}

	public void EnableRotation()
	{
		canRotate = false;
	}

	public void DisableRotation()
	{
		canRotate = false;
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
		//_myTransform.position = Vector3.MoveTowards(_myTransform.position, targetPosition, myStatus.curMovementSpeed * Time.deltaTime);
		moveVector += (targetPosition - _myTransform.position).normalized;
		//controller.Move((targetPosition - _myTransform.position).normalized * myStatus.curMovementSpeed * Time.deltaTime);
			
	}

	public void AnimateToIdle()
	{
		if(movementState != MovementState.idle)
		{
			CrossfadeAnimation(idleAnim.name, true);
			movementState = MovementState.idle;
		}
	}

	public void AnimateToRunning()
	{
		if(movementState != MovementState.moving)
		{
			CrossfadeAnimation(runningAnim.name, true);
			movementState = MovementState.moving;
		}
	}

	public float CheckTaunts(AIState curState)
	{
		for (int i = 0; i < taunts.Count; i++) {
			if(taunts[i].showState == curState)
			{
				float chance = Random.Range(0.0f, 1.0f);
				if(chance < taunts[i].showChance)
				{
					PlayTaunt(i);
					return taunts[i].duration;
				}
			}
		}
		return 0.0f;
	}


	public void PlayTaunt(int newTauntID)
	{
		myPhotonView.RPC("NetworkPlayTaunt", PhotonTargets.All, newTauntID);
	}

	[RPC]
	public void NetworkPlayTaunt(int newTauntID)
	{
		//play taunt animation
		//show taunt message
	}

	public void ReadyUp()
	{
		EnterAIState(AIState.selectingSkill);
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
		restJob = Job.make(RestSequence(2.0f));
	}

	public IEnumerator RestSequence(float timer)
	{
		yield return new WaitForSeconds(timer);
		EnterAIState(AIState.selectingSkill);
	}


	public override void Die()
	{
		//play death animation
		EnterAIState(AIState.death);
	}

	public void Win()
	{
		EnterAIState(AIState.victory);
	}

	public void RefreshPotentialTargets()
	{
		alltargets = arena.playerCharacterStatuses;
		availableTargets.Clear();
		
		for (int i = 0; i < alltargets.Count; i++) {
			if(alltargets[i] != null && alltargets[i].curHealth > 0)
			{
				availableTargets.Add(alltargets[i]);
				Debug.Log("add player");
			}
		}
	}

	public override void DealDamage(int targetViewID, int targetOwnerID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		myPhotonView.RPC("NetworkDealAIDamage", PhotonPlayer.Find(targetOwnerID), targetViewID, skillID, hitPos, targetPos);
	}

	[RPC]
	public void NetworkDealAIDamage(int targetViewID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		CharacterStatus targetCS = PhotonView.Find(targetViewID).gameObject.GetComponent<CharacterStatus>();
		if(targetCS != null)
		{
			skills[skillID].ResolveHit(targetCS, hitPos, targetPos);

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
			Debug.Log("we have found a skill to use!");
			myPhotonView.RPC("NetworkSelectSkill", PhotonTargets.Others, activeSkill.skillID);
			if(activeSkill.targetLimit >0)
			{
				SelectTarget(activeSkill.skillRangeMin, activeSkill.skillRangeMax);
			}
			EnterAIState(AIState.executingSkill);
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

	public int NumTargetsInRange(float minDistance, float maxDistance)
	{
		int num = 0;
		for (int i = 0; i < availableTargets.Count; i++) {
			if(DistanceToTargetCS(availableTargets[i]) <= maxDistance && DistanceToTargetCS(availableTargets[i]) >= minDistance)
				num ++;
        }
        return num;
    }
    
    #region movement conditions

	public void SetTargetDistance(float distance)
	{
		moveToTargetRange = true;
		targetDistance = distance;
	}

	public void SetTargetAngle(float angle)
	{
		aimAtTarget = true;
		targetAngle = angle;
	}

	public void SetTargetPosition(Vector3 targetPos)
	{
		targetPosition = targetPos;
	}

	public virtual bool hasFulfilledSkillConditions()
	{
		if(aimAtTarget && targetAngleDifference < targetAngle)
			return false;
		if(moveToTargetRange && (Vector3.Distance(_myTransform.position, targetPosition) > targetDistance))
			return false;
		return true;
	}

	#endregion
	#region skill usage
	public void UseActiveAISkill()
	{
		myPhotonView.RPC("NetworkUseActiveAISkill", PhotonTargets.All, activeSkill.skillID);
	}

	[RPC]
	public void NetworkUseActiveAISkill(int index)
	{
		skills[index].UseSkill();
	}
	
	public void AISkillFireOneShot(int skillIndex)
	{
		myPhotonView.RPC("NetworkFireOneShot", PhotonTargets.All, skillIndex);
	}

	[RPC]
	public void NetworkAISkillFireOneShot(int skillIndex)
	{
		skills[skillIndex].FireOneShot();
	}

	public void EndSkill()
	{
		if(myPhotonView.isMine && state == AIState.executingSkill)
			EnterAIState(AIState.rest);
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
		PoolManager.Pools.Destroy(effectsPool.poolName);
	}

	public void EnterAIState(AIState newState)
	{
		myPhotonView.RPC("NetworkEnterAIState", PhotonTargets.All, (int)newState);
	}

	[RPC]
	public void NetworkEnterAIState(int i)
	{
		state = (AIState)i;
		//Debug.Log("entering state: " + state.ToString());
		
	}

	public float DistanceToTargetCS(CharacterStatus cs)
	{
		float distance = Vector3.Distance(cs.transform.position, _myTransform.position);
		return distance;
	}

	public void ResetPhysicalMovementStatus()
	{
		aimAtTarget = false;
		moveToTargetRange = false;
		targetObject = null;
		targetCharacterController = null;
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

[System.Serializable]
public class AITaunt{
	public AnimationClip tauntAnimation;
	public float showChance;
	public AIState showState;
	public float duration;
}
