using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JimFSM : SimpleFSM {
   
    
    public MeshRenderer indicator;
    public float skillRange;
    public float maxMovementTimer;
    public float curMovementTimer;

    

	// Use this for initialization
	public override void Awake () {
        base.Awake();
	}

	public override void Start()
	{
		base.Start();
		if(myPhotonView.isMine)
		{
			Debug.LogWarning("THIS IS MINE");
			state = AIState.Resting;
        }
    }
    
    public override void EnterState(AIState stateEntered)
    {
        switch(stateEntered)
        {
		case AIState.Resting:
			restJob = Job.make(Rest(), true);
			break;
        case AIState.Searching:
            indicator.material.color = Color.green;
            searchTargetJob = new Job(SearchForTarget(), true);
			Debug.Log("aistate searching");
            break;
        case AIState.SelectSkill:
            indicator.material.color = Color.white;
            if(skills.Length >0)
            {
                selectedSkill = ChooseSkill();
                Debug.Log("selected skill: "+selectedSkill.skillName);
                if(selectedSkill.requiresTarget)
                    state = AIState.Searching;
                else
                {
                    state = AIState.UsingSkill;
                }
            }
            else
                state = AIState.Idling;
            break;
        case AIState.MovingTowardsTarget:
            indicator.material.color = Color.blue;
			myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, runningAnim.name);
            curMovementTimer = maxMovementTimer;
            break;
        case AIState.UsingSkill:
            indicator.material.color = Color.red;
            usingSkillJob = Job.make(selectedSkill.UseSkill(), true);
            usingSkillJob.jobComplete += (waskilled) => 
            {
                if(!waskilled)
                {
                    state = AIState.Resting;
                }
				else
				{
					Debug.LogWarning("killing job");
	
					cancelSkillJob = Job.make(selectedSkill.CancelSkill(), true);
					cancelSkillJob.jobComplete += (killed) =>
					{
						if(state != AIState.Dead)
						{
							state = AIState.SelectSkill;
							Debug.LogWarning("selecting skill");
						}
					};
				}
            };
            break;
		case AIState.Dead:
			//if(cancelSkillJob != null) cancelSkillJob.kill();
			myPhotonView.RPC("PlayAnimation", PhotonTargets.All, deathAnim.name);
			Debug.LogWarning("die");
			break;
        }
    }

    public override void ExitState(AIState stateExited)
    {
        switch(stateExited)
        {
		case AIState.Resting:
			if(restJob != null)
				restJob.kill();
			break;
        case AIState.UsingSkill:
            if(usingSkillJob != null) 
			{
				usingSkillJob.kill();
				Debug.Log("killing job");
			}

			selectedSkill.Reset();
			targetObject = null;
			targetCharacterController = null;
			fireObject = null;
			aimAtTarget = false;
			Debug.Log("exit using skill");
			//myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, idleAnim);
            break;
        case AIState.MovingTowardsTarget:
            if(moveToTargetJob != null) moveToTargetJob.kill();
			//myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, idleAnim);
            break;
        case AIState.Searching:
            if(searchTargetJob != null) searchTargetJob.kill();
            break;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	    switch(state)
        {
        case AIState.MovingTowardsTarget:
			if(targetObject == null)
			{
				state = AIState.Searching;
			}

            curMovementTimer -= Time.deltaTime;
            //Debug.Log("update movement");
            //Debug.Log("is in range = " + TargetIsInSkillRange().ToString());

            if(curMovementTimer <= 0 && !TargetIsInSkillRange())
            {
                state = AIState.SelectSkill;
            }

            if(TargetIsInSkillRange())
            {
                //Debug.Log("in skill range!");
                state = AIState.UsingSkill;
            }
            else
            {
                //rotation
                //_transform.forward += () * rotationSpeed * Time.deltaTime;

				Quaternion newRotation = Quaternion.LookRotation(targetObject.position - _transform.position);
				_transform.rotation  = Quaternion.Slerp(_transform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
                //move

                Vector3 moveDirection = targetObject.position - _transform.position;
                moveDirection.y = 0;
				Vector3 movementOffset = moveDirection.normalized * myStatus.movementSpeed * Time.deltaTime;
				//movementOffset += Physics.gravity;
				movementOffset += Physics.gravity;
                _controller.Move(movementOffset);
               // _transform.position = Vector3.MoveTowards (_transform.position, targetObject.position, movementSpeed * Time.deltaTime);
            }
            break;
		case AIState.UsingSkill:

			if(selectedSkill.requiresTarget && targetObject == null)
			{
				state = AIState.SelectSkill;
			}

			if(DistanceToTarget() < selectedSkill.skillMinRange)
			{
				//Debug.Log("too close");
				usingSkillJob.kill();
			}

			if(selectedSkill.requiresTargetLock || (selectedSkill.requiresLineOfSight && targetAngleDifference > selectedSkill.angleTolerance))
			{
				AimAtTarget();
			}
			break;
        }

		//update aim angle to target
		if(targetObject != null)
		{
			Vector3 targetDir = Vector3.zero;
			Vector3 forward = Vector3.zero;
			if(fireObject != null)
			{
				forward = fireObject.forward;
                targetDir = targetObject.position - fireObject.position;
			}
			else
			{
				forward = _transform.forward;
				targetDir = targetObject.position - _transform.position;
            }
            forward.y = targetDir.y = 0;

			targetAngleDifference = Vector3.Angle(targetDir, forward);
		}
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
					float scaledVal = Mathf.Clamp(DistanceToTarget()/15.0f, 0.0f, 1.0f);
					targetPos += targetCharacterController.velocity*scaledVal ;
				}

				Vector3 fireObjectPos = fireObject.position;
				targetPos.y = fireObjectPos.y = 0;
				Quaternion newRotation = Quaternion.LookRotation(targetPos - fireObjectPos);
				transform.rotation  = Quaternion.Slerp(_transform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
				//_transform.forward += (targetPos - fireObjectPos) * _characterStatus.rotationSpeed * Time.deltaTime;
			}
			else
			{
				Quaternion newRotation = Quaternion.LookRotation(targetObject.position - _transform.position);
				_transform.rotation  = Quaternion.Slerp(_transform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
				//_transform.forward += (targetObject.position - _transform.position) * _characterStatus.rotationSpeed * Time.deltaTime;
	        }
		}
    }
    
    public IEnumerator Rest()
	{
		yield return new WaitForSeconds(Random.Range (1,4));
		state = AIState.SelectSkill;
	}

    public IEnumerator SearchForTarget()
    {
        Debug.Log("searching");
        //GameObject[] alltargets;
		alltargets = arena.players;
		availableTargets.Clear();

		for (int i = 0; i < alltargets.Count; i++) {
			/*if(alltargets[i].CurrentHealth >0 && !availableTargets.Contains(alltargets[i]))
	        {
	            availableTargets.Add(alltargets[i]);
	            Debug.Log("adding player");
	        }*/
			if(alltargets[i] != null && alltargets[i].CurrentHealth > 0 )
	        {
	            //availableTargets.Remove(alltargets[i]);
				availableTargets.Add(alltargets[i]);
	            Debug.Log("add player");
	        }
        }
        if(availableTargets.Count >0)
        {
            targetObject = availableTargets[Random.Range(0, availableTargets.Count)].transform;
			targetCharacterController = targetObject.gameObject.GetComponent<CharacterController>();
        }
        else
		{
            targetObject = null;
			targetCharacterController = null;
		}

        if(targetObject != null)
        {
			if(DistanceToTarget() < selectedSkill.skillMinRange)
			{
				if(availableTargets.Count > 1)
					state = AIState.Searching;
				else
					state = AIState.SelectSkill;
			}
			else
			{
	            if(TargetIsInSkillRange())
	                state = AIState.UsingSkill;
	            else
	            {
	                Debug.Log("time to move");
	                state = AIState.MovingTowardsTarget;
	            }
			}
        }
		else
			state = AIState.Resting;

		yield return null;
    }

    public bool TargetIsInSkillRange()
    {
        if(targetObject != null)
        {
            //Debug.Log(Vector3.Distance(_transform.position, targetObject.position));
			if(DistanceToTarget() < selectedSkill.skillMaxRange)
                return true;
            else
                return false;
        }
        else
            return false;
    }

	public float DistanceToTarget()
	{
		float distance = Vector3.Distance(targetObject.position, _transform.position);
		return distance;
	}
}
