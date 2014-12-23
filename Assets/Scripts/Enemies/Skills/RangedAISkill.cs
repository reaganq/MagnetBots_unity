using UnityEngine;
using System.Collections;

public class RangedAISkill : AISkill {

	
	public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;

    public Transform bulletLocation;
	public Transform bulletPrefab;
	public Transform projectileCollisionDecal;

    public float bulletSpeed;
	public float maxShotsPerSession;
	public float minShotsPerSession;
	public float targetShotsPerSession;

	public float cooldown;
	public float maxAmmoCount;
	public float fireSpeed;

	public bool isReloading;
	public float cooldownTimer;
	public float fireSpeedTimer;

	public bool isSkillActive;
	public float currentShotsFired;
	public float currentAmmoCount = 0;
	public float totalShotsFired = 0;

	// Use this for initialization
	public override void Start () {
		base.Start();
		currentAmmoCount = maxAmmoCount;
		_animator[recoilAnimation.clip.name].layer = 2;
		AddPrefabToPool(bulletPrefab);
		AddPrefabToPool(projectileCollisionDecal);
		ResetSkill();
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
				FireOneShot();
		}
		if(requiresTargetLock)
		{
			//if
		}

	}

	public override IEnumerator UseSkill ()
	{

		//Debug.Log("huh?");
		targetShotsPerSession = Random.Range((int)minShotsPerSession, (int)maxShotsPerSession);
		//if(requiresTargetLock)
			//fsm.aimAtTarget = true;
		fsm.fireObject = bulletLocation;
		//fsm.PlayAnimation(castAnimation.clip);
		fsm.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, castAnimation.clip.name);
		yield return new WaitForSeconds(castAnimation.clip.length);
		while(fsm.targetAngleDifference > angleTolerance)
		{
			yield return new WaitForEndOfFrame();
		}
		//Debug.Log("cast to fire");
		//fsm.PlayAnimation(durationAnimation.clip);
		fsm.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);
		isSkillActive = true;
		while(isSkillActive)
		{
			yield return null;
		}
	}

	public override IEnumerator CancelSkill ()
	{
		while(isReloading)
			yield return null;
		isSkillActive = false;
		if(requiresTargetLock)
			fsm.aimAtTarget = false;
		//fsm.CrossFadeAnimation(followThroughAnimation.clip);
		fsm.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, followThroughAnimation.clip.name);
		yield return new WaitForSeconds(followThroughAnimation.clip.length);
		ResetSkill();

	}

	public override void ResetSkill()
	{
		currentShotsFired = 0f;
	}
	
	public void FireOneShot()
	{
		//fire bullet
		//Debug.Log("fire one shot");
		totalShotsFired ++;
		currentAmmoCount --;
		currentShotsFired ++;
		fireSpeedTimer = fireSpeed;
		fsm.myPhotonView.RPC("SpawnProjectile", PhotonTargets.All, bulletPrefab.name, bulletLocation.position, bulletLocation.rotation, bulletSpeed, fsm.targetObject.transform.position, skillIndex);
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
		fsm.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, recoilAnimation.clip.name);

		Invoke("CheckAmmo", recoilAnimation.clip.length);
	}

	public void CheckAmmo()
	{
		if(currentAmmoCount <= 0)
		{
			cooldownTimer = cooldown + recoilAnimation.clip.length;
			StartCoroutine(Reload(recoilAnimation.clip.length));
		}
		else
		{
			if(currentShotsFired >= targetShotsPerSession)
				StartCoroutine(CancelSkill());
		}
	}
	
	/*public void ActivateSkill(bool state)
	{
		isSkillActive = state;
	}*/
	
	public IEnumerator Reload(float sec)
	{
		//Debug.Log("reloading");
		isReloading = true;
		yield return new WaitForSeconds(sec);
		//characterAnimation.Play(reloadAnimation.clip.name);
		fsm.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, reloadAnimation.clip.name);
		yield return new WaitForSeconds(reloadAnimation.clip.length);
		fsm.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);
		//Debug.Log("back to fire mode");
		currentAmmoCount = maxAmmoCount;
		
	}
	
}
