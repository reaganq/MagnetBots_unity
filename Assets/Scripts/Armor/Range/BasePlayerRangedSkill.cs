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

        if(isActive)
        {
            if(fireSpeedTimer <= 0 && cooldownTimer <= 0)
				ownerCAM.PlayOneShot(skillID);
        }
    }

    public override bool CanPressDown()
    {
        //if(armorState == ArmorState.ready || armorState == ArmorState.onUse || armorState == ArmorState.followThrough || )
        if(skillState == SkillState.precast)
            return false;
        else
            return true;
        //else
            //return false;
    }

	public override IEnumerator PressDownSequence(int randomNumber)
    {

        if(skillState == SkillState.ready || skillState == SkillState.followThrough)
        {
            skillState = SkillState.onUse;
            //characterAnimation.CrossFade(castAnimation.clip.name, 0.05f);

            //characterAnimation.Play(castAnimation.clip.name);

			ownerCAM.PlayAnimation(baseSkillAnimation.castAnimation.clip.name, false);
			//ownerCAM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, baseSkillAnimation.castAnimation.clip.name);
			yield return new WaitForSeconds(baseSkillAnimation.castAnimation.clip.length);

            //characterAnimation.Play(durationAnimation.clip.name);
			ownerCAM.PlayAnimation(baseSkillAnimation.loopAnimation.clip.name, false);
			//ownerCAM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, baseSkillAnimation.loopAnimation.clip.name);
            ActivateSkill(true);
            //FireOneShot();

        }
        else if(skillState == SkillState.onUse || skillState == SkillState.wait || skillState == SkillState.recoiling || skillState == SkillState.reloading)
        {
            ActivateSkill(true);
            /*armorState = ArmorState.onUse;
            //while(_skillActive)
            //{
                if(fireSpeedTimer <= 0 && !isReloading)
                {
                    armorState = ArmorState.onUse;
                    FireOneShot();
                }
                //FireOneShot();
                //armorState = ArmorState.onUse;
                //Debug.Log("loop firing");
            //}*/

        }
        else
        {
            Debug.Log("DOUBLE HIT" + skillState.ToString());
            yield break;
        }
    }

    public override bool CanPressUp ()
    {
        if(skillState == SkillState.ready)
        {
            return false;
        }
        else
            return true;
    }

    public override IEnumerator PressUpSequence(int randomNumber)
    {
        while(skillState == SkillState.onUse)
        {
            yield return new WaitForEndOfFrame();
        }
        if(isPressedDown)
        {
            ActivateSkill(false);
			//ownerManager.ResetActionState();
            while(isReloading)
            {
                yield return new WaitForEndOfFrame();
            }

            skillState = SkillState.wait;
            yield return new WaitForSeconds(1f);
            skillState = SkillState.followThrough;
			Debug.LogWarning("not here");
			ownerCAM.CrossfadeAnimation(baseSkillAnimation.followThroughAnimation.clip.name, false);
			yield return new WaitForSeconds(baseSkillAnimation.followThroughAnimation.clip.length);
        }
        else 
        {
            ActivateSkill(false);
            yield break;
        }
    }

    public override void FireOneShot()
    {
        shotsFired ++;
        currentAmmoCount --;
        fireSpeedTimer = fireSpeed;
        //GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
		ownerCAM.myPhotonView.RPC("SpawnProjectile", PhotonTargets.All, projectilePrefab.name, projectileSpawnLocation.position, projectileSpawnLocation.rotation, bulletSpeed, skillID);
        skillState = SkillState.recoiling;
        //characterAnimation.Play(recoilAnimation.clip.name);
		ownerCAM.PlayAnimation(recoilAnimation.clip.name, false);
		//ownerCAM.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, recoilAnimation.clip.name);
        if(currentAmmoCount <= 0)
        {
            cooldownTimer = cooldown + recoilAnimation.clip.length;
            StartCoroutine(Reload(recoilAnimation.clip.length));
        }
    }

    public IEnumerator Reload(float sec)
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

    }



}
