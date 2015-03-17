using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class ActionManager : MonoBehaviour {
	
	//common usage
	public SpawnPool effectsPool;
	public CharacterStatus myStatus;
	public Transform _myTransform;
	public Animation myAnimation;
	public PhotonView myPhotonView;
	public Motor myMotor;
	public bool _disableMovement;
	public MovementState movementState;
	public virtual bool disableMovement
	{
		get{
			return _disableMovement;
		}
		set{
			_disableMovement = value;
		}
	}

	//exclusive for player
	public float runningAnimationSpeedMultiplier;
	public float currentRunningAnimationSpeed;
	public Avatar myAvatar;
	// Use this for initialization
	public virtual void Start () {
	
		_myTransform = transform;
		myPhotonView = GetComponent<PhotonView>();
		myAvatar = GetComponent<Avatar>();
		myMotor = GetComponent<Motor>();
		myStatus = GetComponent<CharacterStatus>();
	}

	public bool MakeSpawnPool()
	{
		if(effectsPool == null)
		{
			effectsPool = PoolManager.Pools.Create(myPhotonView.viewID.ToString());
			//Debug.Log(effectsPool.poolName);
		}
		return true;
	}
	
	public virtual void EnableMovement()
	{
		disableMovement = false;
	}
	
	public virtual void DisableMovement()
	{
		disableMovement = true;
	}

	public virtual void UpdateRunningSpeed(float t)
	{
	}

	public virtual void Die()
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
			NetworkPlayAnimation(name);
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
			NetworkCrossfadeAnimation(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkCrossfadeAnimation", PhotonTargets.All, name);
		}
	}

	public void CrossfadeAnimation(string name, float fadeTime)
	{
		CrossfadeAnimation(name, fadeTime, false);
	}
	
	public void CrossfadeAnimation(string name, float fadeTime, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			NetworkCrossfadeAnimationWithTime(name, fadeTime);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkCrossfadeAnimationWithTime", PhotonTargets.All, name);
		}
	}
	
	public void BlendAnimation(string name, float targetWeight, float fadeLength)
	{
		BlendAnimation(name, targetWeight, fadeLength, false);
	}
	
	public void BlendAnimation(string name, float targetWeight, float fadeLength, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			NetworkBlendAnimation(name, targetWeight, fadeLength);
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
			NetworkFadeOutAnimation(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkFadeOutAnimation", PhotonTargets.All, name);
		}
	}

	public void PlayQueuedAnimation(string clip, int mode, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			NetworkPlayQueuedAnimation(clip, mode);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkPlayQueuedAnimation", PhotonTargets.All, clip, mode);
		}
	}
	
	public void StopAnimation(string name, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			if(myAnimation.IsPlaying(name))
				myAnimation.Stop(name);
		}
		else
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkStopAnimation", PhotonTargets.All, name);
		}
	}

	[RPC]
	public void NetworkTalk(string text)
	{
		CancelInvoke("HideSpeechBubble");
		//speechBubbleText.text = text;
		//speechBubbleTween.Play(true);
		Invoke("HideSpeechBubble", 3);
	}

	public void HideSpeechBubble()
	{
		//speechBubbleTween.Play(false);
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
	public void NetworkCrossfadeAnimationWithTime(string name, float fadeTime)
	{
		myAnimation.CrossFade(name, fadeTime);
	}
	
	[RPC]
	public void NetworkBlendAnimation(string name, float weight, float length)
	{
		myAnimation.Blend(name, weight, length);
	}
	


	[RPC]
	public void NetworkPlayQueuedAnimation(string clip, int mode)
	{
		if(mode == 0)
			myAnimation.PlayQueued(clip, QueueMode.PlayNow);
		else if (mode == 1)
			myAnimation.PlayQueued(clip, QueueMode.CompleteOthers);
	}
	
	[RPC]
	public void NetworkFadeOutAnimation(string name)
	{
		myAnimation.Blend(name, 0, 0.2f);
	}
	
	[RPC]
	public void NetworkStopAnimation(string name)
	{
		if(myAnimation.IsPlaying(name))
			myAnimation.Stop(name);
	}
	
	[RPC]
	public void NetworkPlayParticleFx(string pathName)
	{
		GameObject fx = Instantiate(Resources.Load(pathName), _myTransform.position, Quaternion.identity) as GameObject;
	}
	
	#endregion

	public virtual void DealDamage(int targetViewID, int targetOwnerID, int skillID, Vector3 hitPos, Vector3 targetPos)
	{
	}
	
	public void SpawnParticle(string particleName, Vector3 pos, bool network)
	{
		if(network)
		{
			if(myPhotonView.isMine)
				myPhotonView.RPC("NetworkSpawnParticle", PhotonTargets.All, particleName, pos);
		}
		else
			NetworkSpawnParticle(particleName, pos);
	}
	
	[RPC]
	public void NetworkSpawnParticle(string particleName, Vector3 pos)
	{
		Transform particle = effectsPool.prefabs[particleName];
		ParticleSystem particleSys = particle.GetComponent<ParticleSystem>();
		effectsPool.Spawn(particleSys, pos, Quaternion.identity, null);
	}
}
