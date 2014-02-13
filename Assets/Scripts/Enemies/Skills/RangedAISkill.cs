using UnityEngine;
using System.Collections;

public class RangedAISkill : AISkill {

    public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;

    public Transform bulletLocation;
	public GameObject bulletPrefab;
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
		currentAmmoCount = maxAmmoCount;

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
	}

	public override IEnumerator UseSkill ()
	{
		fsm.CrossFadeAnimation(castAnimation.clip);
		yield return new WaitForSeconds(castAnimation.clip.length);
		
		Debug.Log("cast to fire");
		fsm.PlayAnimation(durationAnimation.clip);
		isSkillActive = true;
	}

	public override IEnumerator CancelSkill ()
	{
		isSkillActive = false;
		fsm.CrossFadeAnimation(followThroughAnimation.clip);
		yield return new WaitForSeconds(followThroughAnimation.clip.length);
		Reset();
	}

	public void Reset()
	{
		currentShotsFired = 0f;
	}
	
	public void FireOneShot()
	{
		//fire bullet
		Debug.Log("fire one shot");
		totalShotsFired ++;
		currentAmmoCount --;
		fireSpeedTimer = fireSpeed;
		GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
		//Physics.IgnoreCollision(bullet.collider, characterCollider);
		if(bullet.rigidbody != null)
			bullet.rigidbody.AddForce(this.transform.forward * bulletSpeed);
		BulletProjectile src = bullet.GetComponent<BulletProjectile>();
		//if(src != null)
			//src.masterScript = this;
		
		
		Debug.Log("ammo: " + currentAmmoCount);
		//play recoil animation
		fsm.PlayAnimation(recoilAnimation.clip);
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
		Debug.Log("reloading");
		isReloading = true;
		yield return new WaitForSeconds(sec);
		//characterAnimation.Play(reloadAnimation.clip.name);
		fsm.PlayAnimation(reloadAnimation.clip);
		yield return new WaitForSeconds(reloadAnimation.clip.length);
		fsm.PlayAnimation(durationAnimation.clip);
		Debug.Log("back to fire mode");
		currentAmmoCount = maxAmmoCount;
		
	}
}
