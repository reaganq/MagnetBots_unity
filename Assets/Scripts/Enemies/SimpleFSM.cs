using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SimpleFSM : MonoBehaviour {

    public List<CharacterStatus> alltargets;
    public List<CharacterStatus> availableTargets;
    public Transform _transform;
    public Animation _animator;
    public Collider _collider;
    public CharacterController _controller;
	public PhotonView myPhotonView;
    public CharacterStatus myStatus;
	public float targetAngleDifference;
    public AISkill[] skills;
    public AISkill selectedSkill;
    public int selectedSkillIndex;
    public float[] skillChances;

	public Job restJob;
    public Job searchTargetJob;
    public Job moveToTargetJob;
    public Job usingSkillJob;
	public Job cancelSkillJob;

    public Transform targetObject;
	public CharacterController targetCharacterController;
	public Transform fireObject;
	
    public float visionRange;

    public bool aimAtTarget = false;
		
	public SpawnPool effectsPool;

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
	}

	public virtual void ExitState(AIState state)
	{
	}



    public enum AIState
    {
        Inactive,
        Waking,
        Stunned,
        SelectSkill,
        Searching,
        MovingTowardsTarget,
        Targetting,
        UsingSkill,
        Dead,
        Idling,
        Resting
    }

	// Use this for initialization
	public virtual void Awake () {

        _transform = transform;
        _animator = GetComponent<Animation>();
        _collider = this.collider;
        myStatus = GetComponent<CharacterStatus>();
        _controller = GetComponent<CharacterController>();
        skillChances = new float[skills.Length];
		for (int i = 0; i <skills.Length; i++) 
		{
			skills[i].skillIndex = i;			
		}
		myPhotonView = GetComponent<PhotonView>();
		MakeSpawnPool();
		//myPhotonView.RPC("PlayAnimation", PhotonTargets.All, idleAnim);
	}

	/*public virtual void Start()
	{
		foreach(AISkill skill in skills)
		{
			skill.Initialise();
		}
	}*/
	
	public virtual void Start()
	{
		for (int i = 0; i < skillChances.Length; i++) {
			skillChances[i] = skills[i].weighting;
		}
		
		foreach(AnimationState anim in _animator)
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
		Debug.Log(_animator[idleAnim.name].layer);
	}

	public void MakeSpawnPool()
	{
		effectsPool = PoolManager.Pools.Create(myStatus.characterName);
		Debug.Log(effectsPool.poolName);
	}

	[RPC]
    public void PlayAnimation(string clip)
    {
        _animator.Play(clip);
    }

	[RPC]
	public void CrossFadeAnimation(string clip)
    {
        _animator.CrossFade(clip);
    }

	[RPC]
	public void BlendAnimation(string clip, float target, float timer)
    {
        _animator.Blend(clip, target, timer);
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
			state = AIState.Resting;
		else
			state = AIState.Inactive;
	}

	//all
	[RPC]
	public void RevertOwner()
	{
		myPhotonView.viewID = InitViewID;
		//ownerID = myPhotonView.ownerId;
		Debug.Log(ownerID);
		state = AIState.Resting;
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
	
	public void IgnoreCollisions(Collider collider)
	{
		List<Collider> cols = myStatus.hitboxes;
		for (int i = 0; i < cols.Count; i++) 
		{
			Physics.IgnoreCollision(collider, cols[i]);
		}
	}
	
    public AISkill ChooseSkill()
    {
        float total = 0f;
        for (int i = 0; i < skillChances.Length; i++) {
            total += skillChances[i];
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < skillChances.Length; i++) {
            if(randomPoint < skillChances[i])
            {
                return skills[i];
            }
            else
                randomPoint -= skillChances[i];
        }
        return skills[skillChances.Length -1];
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

	/*public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("disconnected: "+ player);
		
		if(player == owner)
		{
			Debug.Log("this is my owner!");
			//myPhotonView.RPC
			if(myPhotonView.owner == null)
				Debug.Log("no owner");
			state = AIState.Resting;

		}
	}*/

}
