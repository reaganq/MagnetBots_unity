using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NormalMelee : ArmorSkill {


    /***** set up in inspector *****/
    public SkillAnimation[] attackAnimations;
    public TargetDetectionMethod detectionMethod;
    public Transform weaponLocation;
    public GameObject weaponCollider;
    public float detectionRange;
    //public float collisionRange;
    //can hit multiple enemies or heal multiple allies?
    public bool canHitMultipleTargets;


    /***************/

    #region skill state

    #endregion

    #region setup and unequip
    public override void Initialise(Transform character,CharacterActionManager manager, int index)
    {
        base.Initialise(character,manager, index);
        TransferAnimations();
        if(weaponCollider != null)
        {
            TriggerCollider tc = weaponCollider.GetComponent<TriggerCollider>();
            if(tc != null)
            {
                tc.status = myStatus;
				tc.masterArmor = this;
            }
            weaponCollider.SetActive(false);
        }
    }

    public void TransferAnimations()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            TransferSkillAnimation(attackAnimations[i]);
        }
    }

    public override void UnEquip()
    {
        for (int i = 0; i < attackAnimations.Length; i++) {
            RemoveSkillAnimation(attackAnimations[i]);
        }
    }
    #endregion

	// Update is called once per frame
	void Update () {

	}

    public override bool CanPressDown()
    {
        if(attackAnimations.Length == 0 || armorState != ArmorState.ready)
        {
            return false;

        }
        else
            return true;
    }


    public override IEnumerator PressDown()
    {
		if(disableMovement)
		{
			myManager.DisableMovement();
		}

		ActivateSkill(true);
        armorState = ArmorState.casting;

        int i = Random.Range(0, attackAnimations.Length);
        //Debug.Log("i = "+i);
        myAnimation[attackAnimations[i].clip.name].time = 0;
        //characterAnimation.CrossFade(attackAnimations[i].clip.name, 0.05f);
		myManager.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, attackAnimations[i].clip.name, (float)0.05f);

        float totalTime = attackAnimations[i].clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

       /* Debug.Log(totalTime);
        Debug.Log(castTime);
        Debug.Log(attackduration);
        Debug.Log(followThroughTime);*/

        yield return new WaitForSeconds(castTime);
        armorState = ArmorState.onUse;
        if(weaponCollider != null)
        {
            weaponCollider.SetActive(true);
        }
        

        yield return new WaitForSeconds(attackduration);
        armorState = ArmorState.followThrough;
        

        if(weaponCollider != null)
        {
            weaponCollider.SetActive(false);
        }

		if(disableMovement)
			myManager.EnableMovement();

        yield return new WaitForSeconds(followThroughTime*0.3f);
        //characterAnimation.Blend(attackAnimations[i].clip.name, 0, followThroughTime*0.7f);
		myManager.myPhotonView.RPC("BlendAnimation", PhotonTargets.All, attackAnimations[i].clip.name, (float)0.0f , (float)(followThroughTime*0.7f));

        yield return new WaitForSeconds(followThroughTime * 0.7f);

        Reset();

    }

    public override void Reset()
    {
        armorState = ArmorState.ready;
        HitEnemies.Clear();
		HitAllies.Clear();
		ActivateSkill(false);
    }
}
