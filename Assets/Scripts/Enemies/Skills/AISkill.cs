using UnityEngine;
using System.Collections;

public class AISkill : MonoBehaviour {

    public string skillName;
    public SkillType skillType;
    public float weighting = 0f;
    public SimpleFSM fsm;
    public Animation animator;

    public bool requiresTarget;
    public float skillRange;

	// Use this for initialization
    public virtual void Start()
    {
        fsm = GetComponent<SimpleFSM>();
        animator = GetComponent<Animation>();
    }

    public virtual void SetupAnimations()
    {
    }

    public virtual IEnumerator UseSkill()
    {
        yield return null;
    }

    public virtual IEnumerator CancelSkill()
    {
        yield return null;
    }

}
