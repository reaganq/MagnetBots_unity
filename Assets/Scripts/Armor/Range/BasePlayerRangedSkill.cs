using UnityEngine;
using System.Collections;

public class BasePlayerRangedSkill : BasePlayerSkill {

    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public Transform projectilePrefab;
    public Transform projectileSpawnLocation;
	public Transform hitDecal;
    public float bulletSpeed;

    public int currentAmmoCount = 0;
    public int shotsFired = 0;
	public float cooldownTimer;
	public float fireSpeedTimer;
	public bool isReloading;

	public float cooldown;
	public int maxAmmoCount;
	public float fireSpeed;
	public float damage;

	// Use this for initialization
    #region setup and unequip
    public override void Initialise(PlayerCharacter manager, int index)
    {
        base.Initialise(manager, index);
        currentAmmoCount = maxAmmoCount;
		AddPrefabToPool(projectilePrefab);
		AddPrefabToPool(hitDecal);
		fireSpeedTimer = 0;
    }

	public override void TransferSkillAnimations()
    {
		base.TransferSkillAnimations();
        StartCoroutine(TransferAnimation(recoilAnimation));
        StartCoroutine(TransferAnimation(reloadAnimation));
    }

	public override void RemoveSkillAnimations ()
    {
		base.RemoveSkillAnimations();
        RemoveArmorAnimation(recoilAnimation);
        RemoveArmorAnimation(reloadAnimation);
    }
    #endregion

    public void Update()
    {
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

        if(isActive && isOwner)
        {
            if(fireSpeedTimer <= 0 && cooldownTimer <= 0)
			{
				Debug.Log("update firing one shot" + fireSpeedTimer);
				fireSpeedTimer += fireSpeed;	
				Debug.Log("wtf");
				ownerCAM.PlayOneShot(skillID);

			}
		}
    }

    public override bool CanPressDown()
    {
        if(skillState == SkillState.precast)
            return false;
        else
            return true;
    }

	public override IEnumerator PressDownSequence(int randomNumber)
    {
		if(disableMovement)
		{
			ownerCAM.DisableMovement();
		}
	    skillState = SkillState.precast;
			ownerCAM.CrossfadeAnimation(baseSkillAnimation.precastAnimation.clip.name, false);
			yield return new WaitForSeconds(baseSkillAnimation.precastAnimation.clip.length);
		shotsFired ++;
		currentAmmoCount --;
		fireSpeedTimer = fireSpeed;
		skillState = SkillState.onUse;
		ownerCAM.PlayAnimation(baseSkillAnimation.castAnimation.clip.name, false);
		Debug.Log("firing");
		yield return new WaitForSeconds(baseSkillAnimation.castTime);
		if(isOwner)
			ownerCAM.SpawnProjectile(projectilePrefab.name, projectileSpawnLocation.position, ownerCAM._myTransform.forward, bulletSpeed, skillID, true);
		yield return new WaitForSeconds(baseSkillAnimation.castAnimation.clip.length - baseSkillAnimation.castTime);
		skillState = SkillState.wait;
		ActivateSkill(true);
    }

	public override void FireOneShot()
	{
		StartCoroutine(FireOneShotSequence());
	}
	
	public IEnumerator FireOneShotSequence()
	{
		yield return null;
		while(skillState == SkillState.precast)
		{
			Debug.Log("waiting for precast");
			yield return new WaitForEndOfFrame();
		}
		shotsFired ++;
		currentAmmoCount --;
		fireSpeedTimer = fireSpeed;
		skillState = SkillState.onUse;
		ownerCAM.PlayAnimation(baseSkillAnimation.castAnimation.clip.name, false);
		Debug.Log("firing");
		yield return new WaitForSeconds(baseSkillAnimation.castTime);
		if(isOwner)
			ownerCAM.SpawnProjectile(projectilePrefab.name, projectileSpawnLocation.position, ownerCAM._myTransform.forward, bulletSpeed, skillID, true);
		yield return new WaitForSeconds(baseSkillAnimation.castAnimation.clip.length - baseSkillAnimation.castTime);
		skillState = SkillState.wait;

	}

    public override bool CanPressUp ()
    {
        if(skillState == SkillState.followThrough)
        {
            return false;
        }
        else
            return true;
    }

    public override IEnumerator PressUpSequence(int randomNumber)
    {
        while(skillState == SkillState.onUse || skillState == SkillState.precast)
        {
            yield return new WaitForEndOfFrame();
        }

            ActivateSkill(false);
            skillState = SkillState.followThrough;
		if(disableMovement)
			ownerCAM.EnableMovement();
			Debug.LogWarning("not here");
			ownerCAM.CrossfadeAnimation(baseSkillAnimation.followThroughAnimation.clip.name, false);
			yield return new WaitForSeconds(baseSkillAnimation.followThroughAnimation.clip.length);
    }



    /*public IEnumerator Reload(float sec)
    {
        //Debug.Log("reloading");
        isReloading = true;
        yield return new WaitForSeconds(sec);


        skillState = SkillState.reloading;
        //characterAnimation.Play(reloadAnimation.clip.name);
		ownerCAM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, reloadAnimation.clip.name);
        yield return new WaitForSeconds(reloadAnimation.clip.length);
        //characterAnimation.Play(durationAnimation.clip.name);
		ownerCAM.PlayAnimation(baseSkillAnimation.loopAnimation.clip.name);
		//ownerCAM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);
        //Debug.Log("back to fire mode");
        currentAmmoCount = maxAmmoCount;

    }*/



}
