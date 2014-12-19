﻿using UnityEngine;
using System.Collections;

public class MeleeAISkill : AISkill {

    public SkillAnimation[] attackAnimations;
    public GameObject weaponCollider = null;
	public Transform hitDecal;
	public Transform impactParticles;
	public Transform impactParticleSpawnPoint;
	public float impactRadius;

	public override void Start()
	{
		base.Start();
		if(weaponCollider != null)
		{
			TriggerCollider tc = weaponCollider.GetComponent<TriggerCollider>();
			if(tc != null)
			{
				tc.status = fsm.myStatus;
				tc.masterAISkill = this;
			}
			weaponCollider.SetActive(false);
		}

		if(hitDecal)
			AddPrefabToPool(hitDecal);
		if(impactParticles)
			AddPrefabToPool(impactParticles);
	}
	// Use this for initialization
	public override IEnumerator UseSkill ()
    {

        Debug.Log("using skill");
        int i = Random.Range(0, attackAnimations.Length);
		if(attackAnimations[i].castAnimation.clip != null)
		{
			fsm.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, attackAnimations[i].castAnimation.clip.name);
			yield return new WaitForSeconds(attackAnimations[i].castAnimation.clip.length);
		}

		fsm.myPhotonView.RPC("PlayQueuedAnimation", PhotonTargets.All, attackAnimations[i].precastAnimation.clip.name, 0);

        float totalTime = attackAnimations[i].precastAnimation.clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

        yield return new WaitForSeconds(castTime);

        if(weaponCollider != null)
        {
            weaponCollider.SetActive(true);
			Debug.Log("setting active");
        }
        
        yield return new WaitForSeconds(attackduration);

		if(impactParticles)
			fsm.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, impactParticles.name, impactParticleSpawnPoint.position);

		//play impact particles

        
        if(weaponCollider != null)
        {
            weaponCollider.SetActive(false);
			Debug.Log("setting deactive");
        }
		else
		{
			OverlapSphere(impactParticleSpawnPoint.position, impactRadius);
		}
        
		if(attackAnimations[i].followThroughAnimation.clip == null)
		{
	        yield return new WaitForSeconds(followThroughTime*0.3f);
			fsm.myPhotonView.RPC("BlendAnimation", PhotonTargets.All, attackAnimations[i].precastAnimation.clip.name, 0f, followThroughTime*0.7f);
	        //fsm.BlendAnimation(attackAnimations[i].clip, 0f, followThroughTime*0.7f);
	        
	        yield return new WaitForSeconds(followThroughTime * 0.7f);
		}

		else
		{
			fsm.myPhotonView.RPC("PlayQueuedAnimation", PhotonTargets.All, attackAnimations[i].followThroughAnimation.clip.name, 0);
			yield return new WaitForSeconds(attackAnimations[i].followThroughAnimation.clip.length);
		}


		Reset();
    }



}
