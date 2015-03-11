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
		skill.Initialise((PlayerCharacter)myStatus, skill.equipmentSlotIndex);
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

	public void UseSkill(InputTrigger trigger, int slot)
	{
		if(isLocked())
			return;
		//int skillIndex = -1;
		//skillIndex = GetSkillByIDAndSlot(index, slot);
		for (int i = 0; i < armorSkills.Count; i++) {
			if(armorSkills[i].equipmentSlotIndex == slot)
			{
				Debug.Log("found right armor slot skill");
				if(trigger == InputTrigger.OnPressDown)
				{
					if(armorSkills[i].CanPressDown())
					{
						Debug.Log("can press down");
						int rng = Random.Range(armorSkills[i].lowerRNGLimit, armorSkills[i].upperRNGLimit);
						myPhotonView.RPC("PressDownAction", PhotonTargets.All, slot, rng);
						//PressDownAction(skillIndex, rng);
					}
				}
				else if(trigger == InputTrigger.OnPressUp)
				{
					if(armorSkills[i].CanPressUp())
					{
						int rng = Random.Range(armorSkills[i].lowerRNGLimit, armorSkills[i].upperRNGLimit);
						myPhotonView.RPC("PressUpAction", PhotonTargets.All, slot, rng);
						//PressUpAction(skillIndex, rng);
					}
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
		if(myPhotonView.isMine)
			actionState = ActionState.idle;
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
			//Debug.LogWarning("animate to idle");
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
			//Debug.LogWarning("animate to running");
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
		myPhotonView.RPC("NetworkEatFood", PhotonTargets.All);
		actionState = ActionState.specialAction;
	}

	[RPC]
	public void NetworkEatFood(string prefabPath)
	{
		if(!string.IsNullOrEmpty(prefabPath))
		{
			GameObject food = Instantiate(Resources.Load(prefabPath) as GameObject) as GameObject;
			food.transform.position = _myTransform.position;
			PlayerAction pa = food.GetComponent<PlayerAction>();
			pa.Enable(this);
		}
	}

	public void PlayToy(string prefabPath)
	{
		myPhotonView.RPC("NetworkPlayToy", PhotonTargets.All);
		actionState = ActionState.specialAction;
	}

	[RPC]
	public void NetworkPlayToy(string prefabPath)
	{
		if(!string.IsNullOrEmpty(prefabPath))
		{
			GameObject toy = Instantiate(Resources.Load(prefabPath) as GameObject) as GameObject;
			toy.transform.position = _myTransform.position;

			PlayerAction pa = toy.GetComponent<PlayerAction>();
			pa.Enable(this);
		}
	}

	public void EarnStatusRewards(RPGCurrency currency, float amount)
	{
		//display rewards;
		myStatus.HUD.DisplayStatusReward(currency, amount);
	}

	#endregion

	#region rpc effect prefab functions
	[RPC]
	public void PressDownAction(int index, int rng)
	{
		for (int i = 0; i < armorSkills.Count; i++) {
			if(armorSkills[i].equipmentSlotIndex == index)
				armorSkills[i].PressDown(rng);
		}
	}

	[RPC]
	public void PressUpAction(int index, int rng)
	{
		for (int i = 0; i < armorSkills.Count; i++) {
			if(armorSkills[i].equipmentSlotIndex == index)
				armorSkills[i].PressUp(rng);
		}

	}

	public void SpawnProjectile(string projectileName, Vector3 pos, Vector3 direction, float speed, int skillIndex, bool acrossNetwork)
	{
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
			for (int i = 0; i < armorSkills.Count; i++) {
				if(armorSkills[i].equipmentSlotIndex == index)
					src.Initialise(armorSkills[i]);
			}
        }
	}

	public void PlayOneShot(int index)
	{
		myPhotonView.RPC("NetworkPlayOneShot", PhotonTargets.All, index);
	}
	
	[RPC]
	public void NetworkPlayOneShot(int index)
	{
		for (int i = 0; i < armorSkills.Count; i++) {
			if(armorSkills[i].equipmentSlotIndex == index)
				armorSkills[i].FireOneShot();
		}
    }

	public override void DealDamage(int targetViewID, int targetOwnerID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		myPhotonView.RPC("NetworkDealPlayerDamage", PhotonPlayer.Find(targetOwnerID), targetViewID, skillID, hitPos, targetPos);
	}

	[RPC]
	public void NetworkDealPlayerDamage(int targetViewID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
		//find targte CharacterStatus
		Debug.Log("dealing damage to ai");
		CharacterStatus targetCS = PhotonView.Find(targetViewID).gameObject.GetComponent<CharacterStatus>();
		if(targetCS != null)
		{
			for (int i = 0; i < armorSkills.Count; i++) {
				if(armorSkills[i].equipmentSlotIndex == skillID)
					armorSkills[i].ResolveHit(targetCS, hitPos, targetPos);
			}
		}
	}

	public void ActivateSkillVFX(int skillID, int index)
	{

	}
	
    #endregion

	public void OnDestroy()
	{
		Debug.Log("being destoyred");
		PoolManager.Pools.Destroy(effectsPool.poolName);
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