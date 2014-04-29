using UnityEngine;
using System.Collections;

public class StandardGun : ArmorSkill {

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

	// Use this for initialization
    #region setup and unequip
    public override void Initialise(Transform character, CharacterActionManager manager, int index)
    {
        base.Initialise(character, manager, index);
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

        if(_skillActive)
        {
            if(fireSpeedTimer <= 0 && cooldownTimer <= 0)
                FireOneShot();
        }
    }

    public override bool CanPressDown()
    {
        //if(armorState == ArmorState.ready || armorState == ArmorState.onUse || armorState == ArmorState.followThrough || )
        if(armorState == ArmorState.casting)
            return false;
        else
            return true;
        //else
            //return false;
    }

    public override IEnumerator PressDown()
    {

        if(armorState == ArmorState.ready || armorState == ArmorState.followThrough)
        {
            armorState = ArmorState.casting;
            //characterAnimation.CrossFade(castAnimation.clip.name, 0.05f);

            //characterAnimation.Play(castAnimation.clip.name);
			myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, castAnimation.clip.name);
            yield return new WaitForSeconds(castAnimation.clip.length);

            //characterAnimation.Play(durationAnimation.clip.name);
			myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);

            if(fireSpeedTimer >0)
                yield return new WaitForSeconds(fireSpeedTimer);
            FireOneShot();
            ActivateSkill(true);
            //FireOneShot();

        }
        else if(armorState == ArmorState.onUse || armorState == ArmorState.wait || armorState == ArmorState.recoiling || armorState == ArmorState.reloading)
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
        if(armorState == ArmorState.ready)
        {
            return false;
        }
        else
            return true;
    }

    public override IEnumerator PressUp()
    {
        while(armorState == ArmorState.casting)
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
        if(_skillActive)
        {
            ActivateSkill(false);
			myManager.ResetActionState();
            while(isReloading)
            {
                yield return new WaitForEndOfFrame();
            }

            armorState = ArmorState.wait;
            yield return new WaitForSeconds(1f);
            armorState = ArmorState.followThrough;
			Debug.LogWarning("not here");
			myManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, followThroughAnimation.clip.name);
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

    public override void Reset()
    {
		Debug.Log("resetting" + armorState.ToString());
		//if(armorState != ArmorState.casting || armorState == ArmorState.reloading)
		//{
			//armorState = ArmorState.followThrough;
			Debug.Log("here");
			myManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, followThroughAnimation.clip.name);
		//}


		HitAllies.Clear();
		HitEnemies.Clear();
		ActivateSkill(false);
        /*myAnimation[castAnimation.clip.name].time = 0;
        myAnimation[durationAnimation.clip.name].time = 0;
        myAnimation[recoilAnimation.clip.name].time = 0;
        myAnimation[reloadAnimation.clip.name].time = 0;
        myAnimation[followThroughAnimation.clip.name].time = 0;*/
        armorState = ArmorState.ready;

    }

    public void FireOneShot()
    {
        //fire bullet
        //Debug.Log("fire one shot");
        armorState = ArmorState.onUse;
        shotsFired ++;
        currentAmmoCount --;
        fireSpeedTimer = fireSpeed;
        //GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
		myManager.myPhotonView.RPC("SpawnProjectile", PhotonTargets.All, bulletPrefab.name, bulletLocation.position, bulletLocation.rotation, bulletSpeed, equipmentSlotIndex);
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
        armorState = ArmorState.recoiling;
        //characterAnimation.Play(recoilAnimation.clip.name);
		myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, recoilAnimation.clip.name);
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


        armorState = ArmorState.reloading;
        //characterAnimation.Play(reloadAnimation.clip.name);
		myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, reloadAnimation.clip.name);
        yield return new WaitForSeconds(reloadAnimation.clip.length);
        //characterAnimation.Play(durationAnimation.clip.name);
		myManager.myPhotonView.RPC("PlayAnimation", PhotonTargets.All, durationAnimation.clip.name);
        //Debug.Log("back to fire mode");
        currentAmmoCount = maxAmmoCount;

    }



}
