using UnityEngine;
using System.Collections;

public class RangedAISkill : AISkill {

	public Transform fireObject;
	//skill cooldown
	public float cooldown;
	public float fireSpeed;
	public float damage;

    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;

    public Transform bulletLocation;
	public Transform bulletPrefab;
	public Transform projectileCollisionDecal;

    public float bulletSpeed;
	public float maxShotsPerSession;
	public float minShotsPerSession;
	public float targetShotsPerSession;

	public bool isReloading;
	public float cooldownTimer;
	public float fireSpeedTimer;

	public bool isSkillActive;
	public float currentShotsFired;
	public float totalShotsFired = 0;

	// Use this for initialization
	public override void InitialiseAISkill(CharacterStatus status, int skillIndex)
	{
		base.InitialiseAISkill(status, skillIndex);
		//currentAmmoCount = maxAmmoCount;
		AddPrefabToPool(bulletPrefab);
		AddPrefabToPool(projectileCollisionDecal);
		ResetSkill();
	}

	public override void SetupAnimations()
	{
		base.SetupAnimations();
		AddAnimation(recoilAnimation);
	}
	
	// Update is called once per frame
	void Update () {
		if(cooldownTimer >= 0)
		{
			cooldownTimer -= Time.deltaTime;
		}
		
		if(fireSpeedTimer >= 0)
		{
			fireSpeedTimer -= Time.deltaTime;
		}
		
		if(cooldownTimer <= 0 && isReloading)
		{
			isReloading = false;
		}
		
		if(isSkillActive)
		{
			if(fireSpeedTimer <= 0 && cooldownTimer <= 0)
				ownerFSM.AISkillFireOneShot(skillID);
		}
		if(requiresTargetLock)
		{
			//if
		}
	}

	public override void StartFulfillSkillConditions()
	{
		if(fireObject != null)
			ownerFSM.fireObject = fireObject;
		base.StartFulfillSkillConditions();
	}

	public override IEnumerator UseSkillSequence ()
	{

		//Debug.Log("huh?");
		targetShotsPerSession = Random.Range((int)minShotsPerSession, (int)maxShotsPerSession);
		//if(requiresTargetLock)
			//fsm.aimAtTarget = true;
		//ownerFSM.fireObject = bulletLocation;
		//fsm.PlayAnimation(castAnimation.clip);
		ownerFSM.PlayAnimation(baseSkillAnimation.castAnimation.clip.name, false);
		//ownerFSM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, baseSkillAnimation.castAnimation.clip.name);
		yield return new WaitForSeconds(baseSkillAnimation.castAnimation.clip.length);
		while(ownerFSM.targetAngleDifference > angleTolerance)
		{
			yield return new WaitForEndOfFrame();
		}
		//Debug.Log("cast to fire");
		//fsm.PlayAnimation(durationAnimation.clip);
		ownerFSM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, baseSkillAnimation.loopAnimation.clip.name);
		isSkillActive = true;
		while(isSkillActive)
		{
			yield return null;
		}
	}

	public override IEnumerator CancelSkillSequence ()
	{
		while(isReloading)
			yield return null;
		isSkillActive = false;
		ownerFSM.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, baseSkillAnimation.followThroughAnimation.clip.name);
		yield return new WaitForSeconds(baseSkillAnimation.followThroughAnimation.clip.length);
		ResetSkill();

	}

	public override void ResetSkill()
	{
		currentShotsFired = 0f;
	}
	
	public override void FireOneShot()
	{
		//fire bullet
		//Debug.Log("fire one shot");
		totalShotsFired ++;
		//currentAmmoCount --;
		currentShotsFired ++;
		fireSpeedTimer = fireSpeed;
		ownerFSM.myPhotonView.RPC("SpawnProjectile", PhotonTargets.All, bulletPrefab.name, bulletLocation.position, bulletLocation.rotation, bulletSpeed, ownerFSM.targetObject.transform.position, skillID);
		//GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
		//Physics.IgnoreCollision(bullet.collider, characterCollider);
		/*if(bullet.rigidbody != null)
		{
			Vector3 dir = bulletLocation.forward;
			dir.y = (fsm.targetObject.position - bulletLocation.position).normalized.y * 0.5f;
			bullet.rigidbody.AddForce(dir * bulletSpeed);
		}
		BulletProjectile src = bullet.GetComponent<BulletProjectile>();
		if(src != null)
		{
			src.masterAISkill = this;
			src.status = fsm.myStatus;
			src.IgnoreCollisions();
		}*/
		//
			//src.masterScript = this;
		
		
		//Debug.Log("ammo: " + currentAmmoCount);
		//play recoil animation
		//fsm.PlayAnimation(recoilAnimation.clip);
		ownerFSM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, recoilAnimation.clip.name);

		//Invoke("CheckAmmo", recoilAnimation.clip.length);
	}

	/*public void CheckAmmo()
	{
		if(currentAmmoCount <= 0)
		{
			cooldownTimer = cooldown + recoilAnimation.clip.length;
			StartCoroutine(Reload(recoilAnimation.clip.length));
		}
		else
		{
			if(currentShotsFired >= targetShotsPerSession)
				StartCoroutine(CancelSkillSequence());
		}
	}*/
	
	/*public void ActivateSkill(bool state)
	{
		isSkillActive = state;
	}*/
	
	/*public IEnumerator Reload(float sec)
	{
		//Debug.Log("reloading");
		isReloading = true;
		yield return new WaitForSeconds(sec);
		//characterAnimation.Play(reloadAnimation.clip.name);
		ownerFSM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, reloadAnimation.clip.name);
		yield return new WaitForSeconds(reloadAnimation.clip.length);
		ownerFSM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, baseSkillAnimation.loopAnimation.clip.name);
		//Debug.Log("back to fire mode");
		currentAmmoCount = maxAmmoCount;
		
	}*/
	
}
