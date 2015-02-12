using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JimFSM : SimpleFSM {
   
    public MeshRenderer indicator;
    public float skillRange;
    public float maxMovementTimer;
    public float curMovementTimer;

	public override void Start()
	{
		base.Start();
		state = AIState.preInitialised;
    }
    
    public override void EnterState(AIState stateEntered)
    {
		base.EnterState(stateEntered);
        switch(stateEntered)
        {
		case AIState.preInitialised:
			//restJob = Job.make(Rest(), true);
			break;
        case AIState.initialised:
            //indicator.material.color = Color.green;
            //searchTargetJob = new Job(SearchForTarget(), true);
            break;
        case AIState.ready:
			restJob = Job.make(Rest());
            /*indicator.material.color = Color.white;
            if(skills.Length >0)
            {
                activeSkill = ChooseSkill();
                Debug.Log("selected skill: "+activeSkill.skillName);
                if(activeSkill.requiresTarget)
                    state = AIState.Searching;
                else
                {
                    state = AIState.UsingSkill;
                }
            }
            else
                state = AIState.Idling;*/
            break;
        case AIState.battleTaunts:
            /*indicator.material.color = Color.blue;
			myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, runningAnim.name);
            curMovementTimer = maxMovementTimer;*/
            break;
        case AIState.selectingSkill:
            /*indicator.material.color = Color.red;
            usingSkillJob = Job.make(activeSkill.UseSkill(), true);
            usingSkillJob.jobComplete += (waskilled) => 
            {
                if(!waskilled)
                {
                    state = AIState.Resting;
                }
				else
				{
					Debug.LogWarning("killing job");
	
					cancelSkillJob = Job.make(activeSkill.CancelSkill(), true);
					cancelSkillJob.jobComplete += (killed) =>
					{
						if(state != AIState.Dead)
						{
							state = AIState.SelectSkill;
							Debug.LogWarning("selecting skill");
						}
					};
				}
            };*/
			SelectSkill();
            break;
		case AIState.checkingSkillRequirements:
			break;
		case AIState.fulfillingSkillConditions:
			activeSkill.FulfillSkillConditions();
			break;
		case AIState.executingSkill:
			UseActiveSkill();
			break;
		case AIState.rest:
			break;
		case AIState.taunting:
			break;
		case AIState.victory:
			break;
        }
    }

    public override void ExitState(AIState stateExited)
    {
        switch(stateExited)
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
			SelectSkill();
			break;
		case AIState.rest:
			if(restJob != null)
				restJob.kill();
			break;
		case AIState.checkingSkillRequirements:
			//if(moveToTargetJob != null) moveToTargetJob.kill();
			break;
		
        case AIState.fulfillingSkillConditions:
            if(usingSkillJob != null) 
			{
				usingSkillJob.kill();
				Debug.Log("killing job");
			}
			activeSkill.ResetSkill();

			Debug.Log("exit using skill");
            break;
		case AIState.executingSkill:
			if(activeSkill.useSkillJob.running)
				activeSkill.useSkillJob.kill();
			break;
		case AIState.taunting:
			break;
		case AIState.victory:
			break;
        }
    }
	
	// Update is called once per frame
	public override void Update () 
    {
		base.Update();
		if(state == AIState.fulfillingSkillConditions)
		{
			if(hasFulfilledSkillConditions())
			{
				aimAtTarget = false;
				moveToTarget = false;
			}
			EnterAIState(AIState.executingSkill);
		}
		/*
	    switch(state)
        {
        case AIState.fulfillingSkillConditions:
			if(targetObject == null)
			{
				//state = AIState.Searching;
			}

            curMovementTimer -= Time.deltaTime;
            //Debug.Log("update movement");
            //Debug.Log("is in range = " + TargetIsInSkillRange().ToString());

            if(curMovementTimer <= 0 && !TargetIsInSkillRange())
            {
                state = AIState.selectingSkill;
            }

            if(TargetIsInSkillRange())
            {
                //Debug.Log("in skill range!");
                state = AIState.executingSkill;
            }
            else
            {
                //rotation
                //_transform.forward += () * rotationSpeed * Time.deltaTime;

				Quaternion newRotation = Quaternion.LookRotation(targetObject.position - _myTransform.position);
				_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
                //move

				Vector3 moveDirection = targetObject.position - _myTransform.position;
                moveDirection.y = 0;
				Vector3 movementOffset = moveDirection.normalized * myStatus.curMovementSpeed * Time.deltaTime;
				//movementOffset += Physics.gravity;
				movementOffset += Physics.gravity;
                //myMotor.Move(movementOffset);
               // _transform.position = Vector3.MoveTowards (_transform.position, targetObject.position, movementSpeed * Time.deltaTime);
            }
            break;
		case AIState.executingSkill:

			if(activeSkill.targetRequirement > 0 && targetObject == null)
			{
				state = AIState.selectingSkill;
			}

			if(DistanceToTargetCS() < activeSkill.skillRangeMin)
			{
				//Debug.Log("too close");
				usingSkillJob.kill();
			}

			if(activeSkill.requiresTargetLock || (activeSkill.requiresLineOfSight && targetAngleDifference > activeSkill.angleTolerance))
			{
				AimAtTarget();
			}
			break;
        }
*/
	}
	
}
