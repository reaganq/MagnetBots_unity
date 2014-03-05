using UnityEngine;
using System.Collections;

public class MeleeAISkill : AISkill {

    public SkillAnimation[] attackAnimations;
    public GameObject weaponCollider = null;

	// Use this for initialization
	public override IEnumerator UseSkill ()
    {
        Debug.Log("using skill");
        int i = Random.Range(0, attackAnimations.Length);

		fsm.myPhotonView.RPC("CrossFadeAnimation", PhotonTargets.All, attackAnimations[i].clip.name);

        float totalTime = attackAnimations[i].clip.length;
        float castTime = attackAnimations[i].castTime * totalTime;
        float attackduration = (attackAnimations[i].followThroughTime * totalTime) - castTime;
        float followThroughTime = totalTime - attackduration - castTime;

        yield return new WaitForSeconds(castTime);

        if(weaponCollider != null)
        {
            weaponCollider.SetActive(true);
        }
        
        yield return new WaitForSeconds(attackduration);
        
        if(weaponCollider != null)
        {
            weaponCollider.SetActive(false);
        }
        
        yield return new WaitForSeconds(followThroughTime*0.3f);
		fsm.myPhotonView.RPC("BlendAnimation", PhotonTargets.All, attackAnimations[i].clip.name, 0f, followThroughTime*0.7f);
        //fsm.BlendAnimation(attackAnimations[i].clip, 0f, followThroughTime*0.7f);
        
        yield return new WaitForSeconds(followThroughTime * 0.7f);

    }

}
