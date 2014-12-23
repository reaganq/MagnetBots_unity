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
	public BaseSkill activeSkill;
	public float runningAnimationSpeedMultiplier;
	public float currentRunningAnimationSpeed;
	// Use this for initialization
	public virtual void Start () {
	
		MakeSpawnPool();
		_myTransform = transform;
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
	
	public virtual void EnableMovement()
	{
		motor.disableMovement = false;
	}
	
	public virtual void DisableMovement()
	{
		motor.disableMovement = true;
	}

	public virtual void UpdateRunningSpeed(float t)
	{
	}

	#region Animations
	public void PlayAnimation(string name)
	{
		PlayAnimation(name, false);
	}
	
	public void PlayAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.Play(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkPlayAnimation", PhotonTargets.All, name);
		}
	}
	
	public void CrossfadeAnimation(string name)
	{
		CrossfadeAnimation(name, false);
	}
	
	public void CrossfadeAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.CrossFade(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkCrossfadeAnimation", PhotonTargets.All, name);
		}
	}
	
	public void BlendAnimation(string name)
	{
		BlendAnimation(name, false);
	}
	
	public void BlendAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.Blend(name, 1, 0.4f);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkBlendAnimation", PhotonTargets.All, name);
		}
	}
	
	public void FadeOutAnimation(string name)
	{
		FadeOutAnimation(name, false);
	}
	
	public void FadeOutAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.Blend(name, 0, 0.2f);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkFadeOutAnimation", PhotonTargets.All, name);
		}
	}
	
	public void StopAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.Stop(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkStopAnimation", PhotonTargets.All, name);
		}
	}
	
	[RPC]
	public void NetworkPlayAnimation(string name)
	{
		myAnimation.Play(name);
	}
	
	[RPC]
	public void NetworkCrossfadeAnimation(string name)
	{
		myAnimation.CrossFade(name);
	}
	
	[RPC]
	public void NetworkBlendAnimation(string name)
	{
		myAnimation.Blend(name, 1, 0.4f);
	}
	
	[RPC]
	public void NetworkFadeOutAnimation(string name)
	{
		myAnimation.Blend(name, 0, 0.2f);
	}
	
	[RPC]
	public void NetworkStopAnimation(string name)
	{
		myAnimation.Stop(name);
	}
	
	[RPC]
	public void NetworkPlayParticleFx(string pathName)
	{
		GameObject fx = Instantiate(Resources.Load(pathName), _myTransform.position, Quaternion.identity) as GameObject;
	}
	
	#endregion
}
