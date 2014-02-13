using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISkill : MonoBehaviour {

    public string skillName;
    public SkillType skillType;
    public float weighting = 0f;
    public SimpleFSM fsm;
    public Animation animator;

    public bool requiresTarget;
	public bool requiresTargetLock;
	public bool requiresLineOfSight;
	public float angleTolerance = 5f;
    public float skillRange;

	public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

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

	public virtual void Reset()
	{
	}

	public virtual void HitEnemy(HitBox hb)
	{
	}
}
