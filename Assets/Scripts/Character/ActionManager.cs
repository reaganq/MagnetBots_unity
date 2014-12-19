using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class ActionManager : MonoBehaviour {

	public SpawnPool effectsPool;
	public CharacterStatus myStatus;
	public Transform _myTransform;
	public Animation myAnimation;
	public PhotonView myPhotonView;
	public PlayerMotor motor;
	// Use this for initialization
	public virtual void Start () {
	
		MakeSpawnPool();
		_myTransform = transform;
		myAnimation = animation;
		myPhotonView = GetComponent<PhotonView>();
	}

	public bool MakeSpawnPool()
	{
		if(effectsPool == null)
		{
			effectsPool = PoolManager.Pools.Create(myStatus.characterName);
			//Debug.Log(effectsPool.poolName);
		}
		return true;
	}

	public virtual void DisableMovement()
	{
	}

	public virtual void EnableMovement()
	{
    }
}
