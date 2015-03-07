using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class CharacterActionManager : ActionManager {

	public List<BasePlayerSkill> armorSkills = new List<BasePlayerSkill>();
    public PassiveArmorAnimationController[] armorAnimControllers = new PassiveArmorAnimationController[5];
	public PlayerMotor playerMotor;
	public NetworkCharacterMovement networkMovement;
	public override bool disableMovement{
		get{
			return _disableMovement;
		}
		set
		{
			base.disableMovement = value;
			playerMotor.disableMovement = _disableMovement;
			networkMovement.disableMovement = _disableMovement;
		}
	}

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
		MakeSpawnPool();
		myAnimation["Default_Idle"].layer = 0;
		myAnimation["Default_Run"].layer = 0;
		//myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, "Default_Idle");
		//Debug.LogWarning("FADING IDLE");
		//CrossfadeAnimation("Default_Idle", false);
    }

	public void AddPassiveAnimation(PassiveArmorAnimationController animController, int index)
	{
		armorAnimControllers[index] = animController;
	}
	
	public void AddSkill(BasePlayerSkill skill)
    {
		//Debug.LogWarning(index);
        //armorSkills[index] = controller;
		armorSkills.Add(skill);
		skill.Initialise((PlayerCharacter)myStatus, armorSkills.IndexOf(skill));
		if(myPhotonView.isMine)
		{
			skill.SetupSkillButtons();
		}
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

	public void UseSkill(InputTrigger trigger, int index, int slot)
	{
		if(isLocked())
			return;
		int skillIndex = -1;
		skillIndex = GetSkillByIDAndSlot(index, slot);
		if(skillIndex >= 0)
		{
			if(trigger == InputTrigger.OnPressDown)
			{
				if(armorSkills[skillIndex].CanPressDown())
				{
					int rng = Random.Range(armorSkills[skillIndex].lowerRNGLimit, armorSkills[skillIndex].upperRNGLimit);
					myPhotonView.RPC("PressDownAction", PhotonTargets.All, skillIndex, rng);
					//PressDownAction(skillIndex, rng);
				}
			}
			else if(trigger == InputTrigger.OnPressUp)
			{
				if(armorSkills[skillIndex].CanPressUp())
				{
					int rng = Random.Range(armorSkills[skillIndex].lowerRNGLimit, armorSkills[skillIndex].upperRNGLimit);
					myPhotonView.RPC("PressUpAction", PhotonTargets.All, skillIndex, rng);
					//PressUpAction(skillIndex, rng);
				}
			}
		}
	}

	public int GetSkillByIDAndSlot(int id, int slot)
	{
		for (int i = 0; i < armorSkills.Count; i++) {
			if(armorSkills[i].skillID == id && armorSkills[i].equipmentSlotIndex == slot)
				return i;
				}
		return -1;
	}

	public void ResetActionState()
	{
		actionState = ActionState.idle;
	}

    #endregion

    #region movement states

    public MovementState _movementState;
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

	public void DisableNetworking()
	{
		networkMovement.ExitActiveState();
	}

	public void EnableNetworking()
	{
		networkMovement.EnterActiveState();
	}

	public void TransitionToQuickArmory()
	{
		RotateTo(PlayerCamera.Instance.quickArmoryPos);
		DisableNetworking();
		myStatus.DisplayName(false);
		myStatus.DisplayHpBar(false);
	}

	public void TransitionToDefault()
	{
		RotationReset();
		EnableNetworking();
	}

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
			Debug.LogWarning("animate to idle");
            myAnimation["Default_Idle"].time = 0;
			myAnimation.CrossFade("Default_Idle");
			for (int i = 0; i < armorAnimControllers.Length ; i++)
            {
                if(armorAnimControllers[i] != null )
                {
                    if(armorAnimControllers[i].idleOverrideAnim.clip != null)
                    {
						myAnimation[armorAnimControllers[i].idleOverrideAnim.clip.name].time = 0;
						myAnimation.CrossFade(armorAnimControllers[i].idleOverrideAnim.clip.name);
                    }
                }
            }
            movementState = MovementState.idle;
        }
    }
    
    public void AnimateToRunning()
    {
        if(movementState != MovementState.moving)
        {
			Debug.LogWarning("animate to running");
			myAnimation["Default_Run"].time = 0;
			myAnimation.CrossFade("Default_Run");
            for (int i = 0; i < armorAnimControllers.Length ; i++) 
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

	public override void EnableMovement()
	{
		base.EnableMovement();
	}
	
	public override void DisableMovement()
	{
		base.DisableMovement();
		AnimateToIdle();
	}

    public override void UpdateRunningSpeed(float t)
    {
		currentRunningAnimationSpeed = Mathf.Lerp( currentRunningAnimationSpeed, t*runningAnimationSpeedMultiplier, 0.1f);
		myAnimation["Default_Run"].speed = currentRunningAnimationSpeed;
        for (int i = 0; i < armorAnimControllers.Length ; i++) {
            if(armorAnimControllers[i] != null && armorAnimControllers[i].runningOverrideAnim.clip != null)
				myAnimation[armorAnimControllers[i].runningOverrideAnim.clip.name].speed = currentRunningAnimationSpeed;
        }
    }

    #endregion

	#region food toy functions

	public void EatFood(string prefabPath)
	{

	}

	public void NetworkEatFood(string prefabPath)
	{
		if(!string.IsNullOrEmpty(prefabPath))
		{
			GameObject food = Instantiate(Resources.Load(prefabPath) as GameObject) as GameObject;
			if(myPhotonView.isMine)
			{
				PlayerAction pa = food.GetComponent<PlayerAction>();
			}
		}
	}

	#endregion

	#region rpc effect prefab functions
	[RPC]
	public void PressDownAction(int index, int rng)
	{
		armorSkills[index].PressDown(rng);
	}

	[RPC]
	public void PressUpAction(int index, int rng)
	{
		armorSkills[index].PressUp(rng);
	}

	public void SpawnProjectile(string projectileName, Vector3 pos, Vector3 direction, float speed, int skillIndex, bool acrossNetwork)
	{
		Debug.Log("spawn projectil");
		if(acrossNetwork)
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkSpawnPlayerProjectile", PhotonTargets.All, projectileName, pos, direction, speed, skillIndex);
		}
		else
			NetworkSpawnPlayerProjectile(projectileName, pos, direction, speed, skillIndex);
	}

	[RPC]
	public void NetworkSpawnPlayerProjectile(string projectileName, Vector3 pos, Vector3 rot, float speed, int index)
	{
		Transform projectile = effectsPool.prefabs[projectileName];
		Transform projectileInstance = effectsPool.Spawn(projectile, pos, Quaternion.identity, null);
		//IgnoreCollisions(projectileInstance.collider);
		projectileInstance.rotation = Quaternion.LookRotation(rot);
		if(projectileInstance.rigidbody != null)
			projectileInstance.rigidbody.AddForce(rot * speed);
		Detector src = projectileInstance.GetComponentInChildren<Detector>();
		if(src != null)
		{
			src.Initialise(armorSkills[index]);
        }
	}

	public void PlayOneShot(int index)
	{
		myPhotonView.RPC("NetworkPlayOneShot", PhotonTargets.All, index);
	}
	
	[RPC]
	public void NetworkPlayOneShot(int index)
	{
		Debug.Log("fire one shot");
		armorSkills[index].FireOneShot();
    }

	public override void DealDamage(int targetViewID, int targetOwnerID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		myPhotonView.RPC("NetworkDealPlayerDamage", PhotonPlayer.Find(targetOwnerID), targetViewID, skillID, hitPos, targetPos);
	}

	[RPC]
	public void NetworkDealPlayerDamage(int targetViewID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		//find targte CharacterStatus
		CharacterStatus targetCS = PhotonView.Find(targetViewID).gameObject.GetComponent<CharacterStatus>();
		if(targetCS != null)
		{
			armorSkills[skillID].ResolveHit(targetCS, hitPos, targetPos);
		}
	}

	public void ActivateSkillVFX(int skillID, int index)
	{

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