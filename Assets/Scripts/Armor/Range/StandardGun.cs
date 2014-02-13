using UnityEngine;
using System.Collections;

public class StandardGun : ArmorSkill {

    public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;
    public GameObject bulletPrefab;
    public Transform bulletLocation;
    public float bulletSpeed;

    public bool canHitMultipleTargets;

    public int currentAmmoCount = 0;
    public int shotsFired = 0;

	// Use this for initialization
    #region setup and unequip
    public override void Initialise(Animation target, Transform character, Collider masterCollider, CharacterStatus status, CharacterActionManager manager)
    {
        base.Initialise(target,character, masterCollider, status, manager);
        TransferAnimations();
        currentAmmoCount = maxAmmoCount;
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
            Debug.Log("ready to cast");
            armorState = ArmorState.casting;
            //characterAnimation.CrossFade(castAnimation.clip.name, 0.05f);

            characterAnimation.Play(castAnimation.clip.name);
            yield return new WaitForSeconds(castAnimation.clip.length);

            Debug.Log("cast to fire");
            characterAnimation.Play(durationAnimation.clip.name);

            if(fireSpeedTimer >0)
                yield return new WaitForSeconds(fireSpeedTimer);
            FireOneShot();
            ActivateSkill(true);
            //FireOneShot();

        }
        else if(armorState == ArmorState.onUse || armorState == ArmorState.wait || armorState == ArmorState.recoiling || armorState == ArmorState.reloading)
        {
            Debug.Log("already firing and more firing" + armorState.ToString());
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
            Debug.Log("looping");
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
            Debug.Log("stop firing, waiting");
            while(isReloading)
            {
                yield return new WaitForEndOfFrame();
                //Debug.LogWarning("reloading wait");
            }
            armorState = ArmorState.wait;
            yield return new WaitForSeconds(1f);
            Debug.Log("withdraw");
            armorState = ArmorState.followThrough;
            characterAnimation.CrossFade(followThroughAnimation.clip.name);
            yield return new WaitForSeconds(followThroughAnimation.clip.length);
            _skillActive = false;
            Reset();
            /*}
            else
            {
                Debug.Log("bypassed");
                yield break;
            }*/
        }
        else 
        {
            Debug.Log("uh oh" + armorState.ToString());
            ActivateSkill(false);
            yield break;
        }
    }

    public void Reset()
    {
        characterAnimation[castAnimation.clip.name].time = 0;
        characterAnimation[durationAnimation.clip.name].time = 0;
        characterAnimation[recoilAnimation.clip.name].time = 0;
        characterAnimation[reloadAnimation.clip.name].time = 0;
        characterAnimation[followThroughAnimation.clip.name].time = 0;
        armorState = ArmorState.ready;

    }

    public void FireOneShot()
    {
        //fire bullet
        Debug.Log("fire one shot");
        armorState = ArmorState.onUse;
        shotsFired ++;
        currentAmmoCount --;
        fireSpeedTimer = fireSpeed;
        GameObject bullet = Instantiate(bulletPrefab, bulletLocation.position, Quaternion.identity) as GameObject;
        Physics.IgnoreCollision(bullet.collider, characterCollider);
        if(bullet.rigidbody != null)
            bullet.rigidbody.AddForce(characterTransform.forward * bulletSpeed);
        BulletProjectile src = bullet.GetComponent<BulletProjectile>();
        if(src != null)
		{
            src.masterArmor = this;
			src.status = myStatus;
			src.IgnoreCollisions();
		}


        Debug.Log("ammo: " + currentAmmoCount);
        //play recoil animation
        armorState = ArmorState.recoiling;
        characterAnimation.Play(recoilAnimation.clip.name);
        if(currentAmmoCount <= 0)
        {
            cooldownTimer = cooldown + recoilAnimation.clip.length;
            StartCoroutine(Reload(recoilAnimation.clip.length));
        }

    }

    public void ActivateSkill(bool state)
    {
        _skillActive = state;
    }

    public IEnumerator Reload(float sec)
    {
        Debug.Log("reloading");
        isReloading = true;
        yield return new WaitForSeconds(sec);


        armorState = ArmorState.reloading;
        characterAnimation.Play(reloadAnimation.clip.name);
        yield return new WaitForSeconds(reloadAnimation.clip.length);
        characterAnimation.Play(durationAnimation.clip.name);
        Debug.Log("back to fire mode");
        currentAmmoCount = maxAmmoCount;

    }

	public override void HitEnemy(HitBox target)
    {
        //package up all attack data, damage + status effects etc
        //target.receiveHit(data);
        target.DealDamage(20);
        Debug.Log("hitenemy");
    }

}
