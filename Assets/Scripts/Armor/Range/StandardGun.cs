using UnityEngine;
using System.Collections;

public class StandardGun : BaseSkill {

    public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;
    public Transform bulletPrefab;
    public Transform bulletLocation;
	public Transform projectileCollisionDecal;
    public float bulletSpeed;

    public bool canHitMultipleTargets;

    public int currentAmmoCount = 0;
    public int shotsFired = 0;
	public float cooldownTimer;
	public float fireSpeedTimer;
	public bool isReloading;

	// Use this for initialization
    #region setup and unequip
    public override void Initialise(CharacterStatus manager, int index)
    {
        base.Initialise(manager, index);
        TransferAnimations();
        currentAmmoCount = maxAmmoCount;
		AddPrefabToPool(bulletPrefab);
		AddPrefabToPool(projectileCollisionDecal);
    }

    public void TransferAnimations()
    {
        StartCoroutine(TransferAnimation(castAnimation));
        StartCoroutine(TransferAnimation(durationAnimation));
        StartCoroutine(TransferAnimation(recoilAnimation));
        StartCoroutine(TransferAnimation(reloadAnimation));
        StartCoroutine(TransferAnimation(followThroughAnimation));
    }

    public override void UnEquip()
    {
        RemoveArmorAnimation(castAnimation);
        RemoveArmorAnimation(durationAnimation);
        RemoveArmorAnimation(recoilAnimation);
        RemoveArmorAnimation(reloadAnimation);
        RemoveArmorAnimation(followThroughAnimation);
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

        if(isPressedDown)
        {
            if(fireSpeedTimer <= 0 && cooldownTimer <= 0)
                FireOneShot();
        }
    }

    public override bool CanPressDown()
    {
        //if(armorState == ArmorState.ready || armorState == ArmorState.onUse || armorState == ArmorState.followThrough || )
        if(armorState == SkillState.casting)
            return false;
        else
            return true;
        //else
            //return false;
    }

	public override IEnumerator PressDownSequence()
    {

        if(armorState == SkillState.ready || armorState == SkillState.followThrough)
        {
            armorState = SkillState.casting;
            //characterAnimation.CrossFade(castAnimation.clip.name, 0.05f);

            //characterAnimation.Play(castAnimation.clip.name);
			ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, castAnimation.clip.name);
            yield return new WaitForSeconds(castAnimation.clip.length);

            //characterAnimation.Play(durationAnimation.clip.name);
			ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);

            if(fireSpeedTimer >0)
                yield return new WaitForSeconds(fireSpeedTimer);
            FireOneShot();
            ActivateSkill(true);
            //FireOneShot();

        }
        else if(armorState == SkillState.onUse || armorState == SkillState.wait || armorState == SkillState.recoiling || armorState == SkillState.reloading)
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
            Debug.Log("DOUBLE HIT" + armorState.ToString());
            yield break;
        }
    }

    public override bool CanPressUp ()
    {
        if(armorState == SkillState.ready)
        {
            return false;
        }
        else
            return true;
    }

    public override IEnumerator PressUpSequence()
    {
        while(armorState == SkillState.casting)
        {
            yield return new WaitForEndOfFrame();
        }
        /*if(armorState == ArmorState.onUse && shotsFired == 0)
        {
            yield return new WaitForEndOfFrame();
        }


        if(isReloading)
        {
            yield return new WaitForEndOfFrame();
        }

*/
        //if(armorState == ArmorState.onUse || armorState == ArmorState.recoiling || armorState == ArmorState.reloading )
        if(isPressedDown)
        {
            ActivateSkill(false);
			//ownerManager.ResetActionState();
            while(isReloading)
            {
                yield return new WaitForEndOfFrame();
            }

            armorState = SkillState.wait;
            yield return new WaitForSeconds(1f);
            armorState = SkillState.followThrough;
			Debug.LogWarning("not here");
			ownerManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, followThroughAnimation.clip.name);
            yield return new WaitForSeconds(followThroughAnimation.clip.length);

            //Reset();
            /*}
            else
            {
                Debug.Log("bypassed");
                yield break;
            }*/
        }
        else 
        {
            ActivateSkill(false);
            yield break;
        }
    }

    public override void ResetSkill()
    {
		Debug.Log("resetting" + armorState.ToString());
		//if(armorState != ArmorState.casting || armorState == ArmorState.reloading)
		//{
			//armorState = ArmorState.followThrough;
			Debug.Log("here");
			ownerManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, followThroughAnimation.clip.name);
		//}


		HitAllies.Clear();
		HitEnemies.Clear();
		ActivateSkill(false);
        /*myAnimation[castAnimation.clip.name].time = 0;
        myAnimation[durationAnimation.clip.name].time = 0;
        myAnimation[recoilAnimation.clip.name].time = 0;
        myAnimation[reloadAnimation.clip.name].time = 0;
        myAnimation[followThroughAnimation.clip.name].time = 0;*/
        armorState = SkillState.ready;

    }

    public void FireOneShot()
    {
        //fire bullet
        //Debug.Log("fire one shot");
        armorState = SkillState.onUse;
        shotsFired ++;
        currentAmmoCount --;
        fireSpeedTimer = fireSpeed;
        //GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
		ownerManager.myPhotonView.RPC("SpawnProjectile", PhotonTargets.All, bulletPrefab.name, bulletLocation.position, bulletLocation.rotation, bulletSpeed, equipmentSlotIndex);
        /*if(bullet.rigidbody != null)
            bullet.rigidbody.AddForce(characterTransform.forward * bulletSpeed);
        BulletProjectile src = bullet.GetComponent<BulletProjectile>();
        if(src != null)
		{
            src.masterArmor = this;
			src.status = myStatus;
			src.IgnoreCollisions();
		}*/


        //Debug.Log("ammo: " + currentAmmoCount);
        //play recoil animation
        armorState = SkillState.recoiling;
        //characterAnimation.Play(recoilAnimation.clip.name);
		ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, recoilAnimation.clip.name);
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


        armorState = SkillState.reloading;
        //characterAnimation.Play(reloadAnimation.clip.name);
		ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, reloadAnimation.clip.name);
        yield return new WaitForSeconds(reloadAnimation.clip.length);
        //characterAnimation.Play(durationAnimation.clip.name);
		ownerManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);
        //Debug.Log("back to fire mode");
        currentAmmoCount = maxAmmoCount;

    }



}
