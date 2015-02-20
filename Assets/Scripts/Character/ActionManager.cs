using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class ActionManager : MonoBehaviour {

	//ui shit
	public GameObject speechBubble;
	public UILabel speechBubbleText;
	public UIPlayTween speechBubbleTween;

	//common usage
	public SpawnPool effectsPool;
	public CharacterStatus myStatus;
	public Transform _myTransform;
	public Animation myAnimation;
	public PhotonView myPhotonView;
	public Motor myMotor;

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
		myMotor.disableMovement = false;
	}
	
	public virtual void DisableMovement()
	{
		myMotor.disableMovement = true;
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

	public void CrossfadeAnimation(string name, float fadeTime)
	{
		CrossfadeAnimation(name, fadeTime, false);
	}
	
	public void CrossfadeAnimation(string name, float fadeTime, bool acrossNetwork)
	{
		if(!acrossNetwork)
		{
			myAnimation.CrossFade(name, fadeTime);
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
			myAnimation.Blend(name, targetWeight, fadeLength);
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
	public void NetworkTalk(string text)
	{
		CancelInvoke("HideSpeechBubble");
		speechBubbleText.text = text;
		speechBubbleTween.Play(true);
		Invoke("HideSpeechBubble", 3);
	}

	public void HideSpeechBubble()
	{
		speechBubbleTween.Play(false);
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
	public void PlayQueuedAnimation(string clip, int mode)
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
	
	
	public void IgnoreCollisions(Collider collider)
	{
		for (int i = 0; i < myStatus.hitboxes.Count; i++) 
		{
			Physics.IgnoreCollision(collider, myStatus.hitboxes[i]);
		}
	}
}
