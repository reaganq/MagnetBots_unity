using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JimFSM : SimpleFSM {
   
    public MeshRenderer indicator;
    public float skillRange;
    public float maxMovementTimer;
    public float curMovementTimer;
	
	// Update is called once per frame
	public override void Update () 
    {
		base.Update();
		/*if(state == AIState.fulfillingSkillConditions)
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
