using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class CharacterActionManager : ActionManager {

    public BaseSkill[] armorSkillsArray = new BaseSkill[5];
    public PassiveArmorAnimationController[] armorAnimControllers = new PassiveArmorAnimationController[5];
	public PlayerMotor playerMotor;
    
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
   
    public override void Start () 
    {
        //animationTarget.Play("Default_Idle");
		base.Start();
		myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Idle");
    }


    
    public void AddArmorcontroller(BaseSkill controller, PassiveArmorAnimationController animController, int index)
    {
		//Debug.LogWarning(index);
        armorSkillsArray[index] = controller;
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
		if(isLocked())
			return;

		if(armorSkillsArray[3] != null )
		{
			if(!armorSkillsArray[3].CanPressDown())
				return;
		}
        if(armorSkillsArray[2] == null)
        {
            return;
        }
        if(trigger == InputTrigger.OnPressDown && armorSkillsArray[2].CanPressDown())
        {
			if(rightEndJob != null)
			{
				rightEndJob.kill();
				//rightEndJob = null;
				Debug.Log("htere");
			}

            if(leftEndJob != null)
            {
                leftEndJob.kill();
                leftEndJob = null;
            }
            if(leftJob != null) leftJob.kill();
            leftJob = Job.make( armorSkillsArray[2].PressDown(), true );
            actionState = ActionState.leftAction;
        }

        if(trigger == InputTrigger.OnPressUp && armorSkillsArray[2].CanPressUp())
        {
            leftEndJob = Job.make( armorSkillsArray[2].PressUp(), true );
            leftEndJob.jobComplete += (waskilled) => 
            {
                actionState = ActionState.idle;
				armorSkillsArray[2].Reset();
            };
        }
    }

    public void RightAction(InputTrigger trigger)
    {
		if(isLocked())
			return;

		if(armorSkillsArray[2] != null)
		{
			if(!armorSkillsArray[2].CanPressDown())
				return;
		}
        //Debug.Log("here");
        if(armorSkillsArray[3] == null)
        {
            return;
        }
        if(trigger == InputTrigger.OnPressDown && armorSkillsArray[3].CanPressDown())
        {
			if(leftEndJob != null)
			{
				leftEndJob.kill();
				leftEndJob = null;
			}

            if(rightEndJob != null)
            {
                rightEndJob.kill();
                rightEndJob = null;
            }
            if(rightJob != null) rightJob.kill();
            rightJob = Job.make( armorSkillsArray[3].PressDown(), true);
            actionState = ActionState.rightAction;
        }

        if(trigger == InputTrigger.OnPressUp && armorSkillsArray[3].CanPressUp())
        {
            rightEndJob = Job.make(armorSkillsArray[3].PressUp(), true);
            rightEndJob.jobComplete += (waskilled) => 
            {
				if(waskilled)
					Debug.Log(waskilled);
                actionState = ActionState.idle;
				armorSkillsArray[3].Reset();
            };
        }
    }

	public void ResetActionState()
	{
		actionState = ActionState.idle;
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

	public void RotateTo(Transform target)
	{
		playerMotor.cachedRotation = _myTransform.forward + _myTransform.position;
		playerMotor.rotationTarget = target.position;
	}
	
	public void RotationReset()
	{
		playerMotor.rotationTarget = playerMotor.cachedRotation;
    }

    public void AnimateToIdle()
    {
        if(movementState != MovementState.idle)
        {
            myAnimation["Default_Idle"].time = 0;
			myAnimation.CrossFade("Default_Idle");
			//myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Idle");
            for (int i = 0; i < armorSkillsArray.Length ; i++)
            {
                if(armorAnimControllers[i] != null )
                {
                    if(armorAnimControllers[i].idleOverrideAnim.clip != null)
                    {
						myAnimation[armorAnimControllers[i].idleOverrideAnim.clip.name].time = 0;
						myAnimation.CrossFade(armorAnimControllers[i].idleOverrideAnim.clip.name);
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
			myAnimation["Default_Run"].time = 0;
			myAnimation.CrossFade("Default_Run");
			//myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Run");
            for (int i = 0; i < armorSkillsArray.Length ; i++) 
            {
                if(armorAnimControllers[i] != null && armorAnimControllers[i].runningOverrideAnim.clip != null)
                {
					myAnimation[armorAnimControllers[i].runningOverrideAnim.clip.name].time = 0;
					myAnimation.CrossFade(armorAnimControllers[i].runningOverrideAnim.clip.name);
                }
            }
            movementState = MovementState.moving;
        }
    }

	public void EnableMovement()
	{

		motor.disableMovement = false;
	}
	
	public void DisableMovement()
	{
		motor.disableMovement = true;
		AnimateToIdle();
	}

    public void UpdateRunningSpeed(float t)
    {
		myAnimation["Default_Run"].speed = Mathf.Lerp( 1f, 2f, t);

        for (int i = 0; i < armorSkillsArray.Length ; i++) {
            if(armorAnimControllers[i] != null && armorAnimControllers[i].runningOverrideAnim.clip != null)
				myAnimation[armorAnimControllers[i].runningOverrideAnim.clip.name].speed = Mathf.Lerp(1f, 2f, t);
        }
    }

    #endregion

	#region rpc animation functions
	[RPC]
	public void PlayAnimation(string name)
	{
		myAnimation.Play(name);
	}

	[RPC]
	public void CrossFadeAnimation(string name)
	{
		myAnimation.CrossFade(name);
	}

	[RPC]
	public void CrossFadeAnimation(string name, float timer)
	{
		myAnimation.CrossFade(name, timer);
	}

	[RPC]
	public void BlendAnimation(string name, float target, float timer)
	{
		myAnimation.Blend(name, target, timer);
	}
	#endregion

	#region rpc effect prefab functions

	[RPC]
	public void SpawnParticleEffect()
	{

	}

	[RPC]
	public void SpawnProjectile(string projectileName, Vector3 pos, Quaternion rot, float speed, int index)
	{
		Transform projectile = effectsPool.prefabs[projectileName];
		Transform projectileInstance = effectsPool.Spawn(projectile, pos, rot, null);
		IgnoreCollisions(projectileInstance.collider);
		if(projectileInstance.rigidbody != null)
			projectileInstance.rigidbody.AddForce( _myTransform.forward * speed);
		BulletProjectile src = projectileInstance.GetComponent<BulletProjectile>();
		if(src != null)
		{
			src.masterArmor = armorSkillsArray[index];
			src.status = myStatus;
			src.pool = effectsPool.poolName;
        }
    }

	[RPC]
	public void SpawnParticle(string particleName, Vector3 pos)
	{
		Transform particle = effectsPool.prefabs[particleName];
		ParticleSystem particleSys = particle.GetComponent<ParticleSystem>();
		effectsPool.Spawn(particleSys, pos, Quaternion.identity, null);
	}

    
    public void IgnoreCollisions(Collider collider)
	{
		List<Collider> cols = myStatus.hitboxes;
		for (int i = 0; i < cols.Count; i++) 
		{
			Physics.IgnoreCollision(collider, cols[i]);
        }
    }
    
    #endregion

	public void OnDestroy()
	{
		Debug.Log("being destoyred");
		//PoolManager.Pools.Destroy(effectsPool.poolName);
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