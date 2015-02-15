using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongDurationMelee : BasePlayerSkill {
	
    // Update is called once per frame
	public override IEnumerator PressDownSequence(int randomNumber)
    {
        skillState = SkillState.precast;
		isBusy = true;
		ownerCAM.PlayAnimation(baseSkillAnimation.precastAnimation.clip.name, false);
		yield return new WaitForSeconds(baseSkillAnimation.precastAnimation.clip.length);
  
        skillState = SkillState.onUse;
		ActivateSkill(true);
		ownerCAM.PlayAnimation(baseSkillAnimation.loopAnimation.clip.name, false);
    }

	public override bool CanPressUp ()
	{
		if(skillState == SkillState.precast || skillState == SkillState.onUse)
			return true;
		else
			return false;
	}

    public override IEnumerator PressUpSequence(int randomNumber)
    {
        while(skillState == SkillState.precast)
        {
            yield return new WaitForEndOfFrame();
        }

		ActivateSkill(false);
		isBusy = false;
		skillState = SkillState.followThrough;
		ownerCAM.PlayAnimation(baseSkillAnimation.followThroughAnimation.clip.name, false);
		yield return new WaitForSeconds(baseSkillAnimation.followThroughAnimation.clip.length);
    }
}
