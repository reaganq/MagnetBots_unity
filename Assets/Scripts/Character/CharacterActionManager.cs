using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterActionManager : MonoBehaviour {

    public ArmorSkill[] armorControllers = new ArmorSkill[5];
    public PassiveArmorAnimationController[] armorAnimControllers = new PassiveArmorAnimationController[5];
    public Animation animationTarget;
	public CharacterStatus myStatus;
    public CharacterMotor motor;
	public PhotonView myPhotonView;

    private Job leftJob;
    private Job rightJob;
    private Job leftEndJob;
    private Job rightEndJob;

    public bool isLocked()
    {
        if(actionState == ActionState.dead || actionState == ActionState.stunned)
        {
            return true;
        }
        else
            return false;
    }

    public bool isBusy()
    {
        if(actionState == ActionState.leftAction || actionState == ActionState.rightAction || actionState == ActionState.specialAction)
            return true;
        else
            return false;
    }
   
    // Use this for initialization
    void Start () 
    {
        //animationTarget.Play("Default_Idle");
		myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Idle");
    }
    
    public void AddArmorcontroller(ArmorSkill controller, PassiveArmorAnimationController animController, int index)
    {
        armorControllers[index] = controller;
        armorAnimControllers[index] = animController;
    }

    #region action states

    public ActionState _actionState;
    public ActionState actionState
    {
        get{ return _actionState; }
        set
        {
            ExitActionState(_actionState);
            _actionState = value;
            EnterActionState(_actionState);
        }
    }

    public void EnterActionState(ActionState state)
    {
        switch(state)
        {
        case ActionState.idle:
            break;
        case ActionState.leftAction:
            break;
        case ActionState.rightAction:
            break;
        case ActionState.specialAction:
            break;
        case ActionState.stunned:
            break;
        case ActionState.dead:
            break;
        }
    }
    
    public void ExitActionState(ActionState state)
    {
        switch(state)
        {
        case ActionState.idle:
            break;
        case ActionState.leftAction:
            break;
        case ActionState.rightAction:
            break;
        case ActionState.specialAction:
            break;
        case ActionState.stunned:
            break;
        case ActionState.dead:
            break;
        }
    }

    #endregion action states

    #region action functions

    public void LeftAction(InputTrigger trigger)
    {
        if(armorControllers[2] == null || actionState == ActionState.rightAction)
        {
            //Debug.LogWarning("returning left");
            return;
        }
        if(trigger == InputTrigger.OnPressDown && armorControllers[2].CanPressDown())
        {
            if(leftEndJob != null)
            {
                leftEndJob.kill();
                leftEndJob = null;
            }

            //Debug.LogWarning("left action state");
            if(leftJob != null) leftJob.kill();
            leftJob = Job.make( armorControllers[2].PressDown() );
            actionState = ActionState.leftAction;
        }

        if(trigger == InputTrigger.OnPressUp && armorControllers[2].CanPressUp())
        {
            leftEndJob = Job.make( armorControllers[2].PressUp() );
            leftEndJob.jobComplete += (waskilled) => 
            {
                actionState = ActionState.idle;
                //Debug.Log("job ended, was killed = " + waskilled);
            };
        }
    }

    public void RightAction(InputTrigger trigger)
    {
        //Debug.Log("here");
        if(armorControllers[3] == null || actionState == ActionState.leftAction)
        {
            //Debug.LogWarning("returning");

            return;
        }
        if(trigger == InputTrigger.OnPressDown && armorControllers[3].CanPressDown())
        {
            if(rightEndJob != null)
            {
                rightEndJob.kill();
                //Debug.LogWarning("right job killed");
                rightEndJob = null;
            }

            //Debug.Log("right action state");
            if(rightJob != null) rightJob.kill();
            rightJob = Job.make( armorControllers[3].PressDown() );
            actionState = ActionState.rightAction;
        }

        if(trigger == InputTrigger.OnPressUp && armorControllers[3].CanPressUp())
        {
                //Debug.LogError("right action end state");
            rightEndJob = Job.make(armorControllers[3].PressUp());
            rightEndJob.jobComplete += (waskilled) => 
            {
                if(!waskilled)
                    actionState = ActionState.idle;
                //Debug.LogWarning("job ended, was killed = " + waskilled);
            };
        }
    }

    public void specialAction(InputTrigger trigger)
    {

    }

    #endregion

    #region movement states

    private MovementState _movementState;
    public MovementState movementState
    {
        get{ return _movementState; }
        set
        {
            ExitMovementState(_movementState);
            _movementState = value;
            EnterMovementState(_movementState);
        }
    }

    public void EnterMovementState(MovementState state)
    {
        switch(state)
        {
        case MovementState.idle:
            break;
        case MovementState.locked:
            break;
        case MovementState.moving:
            break;
        }
    }

    public void ExitMovementState(MovementState state)
    {
        switch(state)
        {
        case MovementState.idle:
            break;
        case MovementState.locked:
            break;
        case MovementState.moving:
            break;
        }
    }

    #endregion

    #region movement functions

    public void AnimateToIdle()
    {
        if(movementState != MovementState.idle)
        {
            animationTarget["Default_Idle"].time = 0;
            //animationTarget.CrossFade("Default_Idle");
			myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Idle");
            for (int i = 0; i < armorControllers.Length ; i++)
            {
                if(armorAnimControllers[i] != null )
                {
                    if(armorAnimControllers[i].idleOverrideAnim.clip != null)
                    {
                        animationTarget[armorAnimControllers[i].idleOverrideAnim.clip.name].time = 0;
                        animationTarget.CrossFade(armorAnimControllers[i].idleOverrideAnim.clip.name);
                    }
                    //if(movementState == MovementState.moving && armorAnimControllers[i].runningOverrideAnim.clip != null)
                        //animationTarget.Blend(armorAnimControllers[i].runningOverrideAnim.clip.name, 0, 0.1f);
                    //if(movementState == MovementState.moving && armorAnimControllers[i].walkingOverrideAnim.clip != null)
                        //animationTarget.Blend(armorAnimControllers[i].walkingOverrideAnim.clip.name, 0, 0.1f);
                }
            }
            movementState = MovementState.idle;
        }
    }
    
    public void AnimateToRunning()
    {
        if(movementState != MovementState.moving)
        {
            animationTarget["Default_Run"].time = 0;
            //animationTarget.CrossFade("Default_Run");
			myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Run");
            for (int i = 0; i < armorControllers.Length ; i++) 
            {
                if(armorAnimControllers[i] != null && armorAnimControllers[i].runningOverrideAnim.clip != null)
                {
                    animationTarget[armorAnimControllers[i].runningOverrideAnim.clip.name].time = 0;
                    animationTarget.CrossFade(armorAnimControllers[i].runningOverrideAnim.clip.name);
                }
            }
            movementState = MovementState.moving;
        }
    }

    public void UpdateRunningSpeed(float t)
    {
        animationTarget["Default_Run"].speed = Mathf.Lerp( 1f, 2f, t);

        for (int i = 0; i < armorControllers.Length ; i++) {
            if(armorAnimControllers[i] != null && armorAnimControllers[i].runningOverrideAnim.clip != null)
                animationTarget[armorAnimControllers[i].runningOverrideAnim.clip.name].speed = Mathf.Lerp(1f, 2f, t);
        }
    }

    #endregion

	[RPC]
	public void PlayAnimation(string name)
	{
		Debug.Log("play animation");
		animationTarget.Play(name);
	}

	[RPC]
	public void CrossFadeAnimation(string name)
	{
		animationTarget.CrossFade(name);
	}

	[RPC]
	public void CrossFadeAnimation(string name, float timer)
	{
		animationTarget.CrossFade(name, timer);
	}

	[RPC]
	public void BlendAnimation(string name, float target, float timer)
	{
		animationTarget.Blend(name, target, timer);
	}


}

public enum MovementState
{
    idle,
    moving,
    locked,
}

public enum ActionState
{
    idle,
    stunned,
    dead,
    specialAction,
    leftAction,
    rightAction,
}