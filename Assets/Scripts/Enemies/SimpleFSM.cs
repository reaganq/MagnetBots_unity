using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleFSM : MonoBehaviour {

    public GameObject[] alltargets;
    public List<GameObject> availableTargets;
    public Transform _transform;
    public Animation _animator;
    public Collider _collider;
    public CharacterController _controller;
    public CharacterStatus _characterStatus;
    public AISkill[] skills;
    public AISkill selectedSkill;
    public int selectedSkillIndex;
    public float[] skillChances;

    public Job searchTargetJob;
    public Job moveToTargetJob;
    public Job usingSkillJob;

    public Transform targetObject;

    public float movementSpeed;
    public float rotationSpeed;
    public float visionRange;

    public bool aimAtTarget = false;

    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip deathAnim;
    public AnimationClip gotHitAnim;

    public enum AIState
    {
        Inactive,
        Waking,
        Stunned,
        SelectSkill,
        Searching,
        MovingTowardsTarget,
        Targetting,
        UsingSkill,
        Dead,
        Idling,
        Resting
    }

	// Use this for initialization
	public virtual void Start () {
        _transform = transform;
        _animator = GetComponent<Animation>();
        _collider = this.collider;
        _characterStatus = GetComponent<CharacterStatus>();
        _controller = GetComponent<CharacterController>();
        skillChances = new float[skills.Length];
        for (int i = 0; i < skillChances.Length; i++) {
            skillChances[i] = skills[i].weighting;
        }
	}

    public void PlayAnimation(AnimationClip clip)
    {
        _animator.Play(clip.name);
    }

    public void CrossFadeAnimation(AnimationClip clip)
    {
        _animator.CrossFade(clip.name);
    }

    public void BlendAnimation(AnimationClip clip, float target, float timer)
    {
        _animator.Blend(clip.name, target, timer);
    }
	
    public AISkill ChooseSkill()
    {
        float total = 0f;
        for (int i = 0; i < skillChances.Length; i++) {
            total += skillChances[i];
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < skillChances.Length; i++) {
            if(randomPoint < skillChances[i])
            {
                return skills[i];
            }
            else
                randomPoint -= skillChances[i];

        }

        return skills[skillChances.Length -1];
    }

}
