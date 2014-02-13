using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JimFSM : SimpleFSM {
   
    public AIState _state;
    public MeshRenderer indicator;
    public float skillRange;
    public float maxMovementTimer;
    public float curMovementTimer;

    public AIState state
    {
        get
        {
            return _state;
        }
        set
        {
            ExitState(_state);
            _state = value;
            EnterState(_state);
        }
    }

	// Use this for initialization
	public override void Start () {
        base.Start();
        state = AIState.SelectSkill;

	}

    void EnterState(AIState stateEntered)
    {
        switch(stateEntered)
        {
        case AIState.Searching:
            indicator.material.color = Color.green;
            searchTargetJob = new Job(SearchForTarget(), true);
            break;
        case AIState.SelectSkill:
            indicator.material.color = Color.white;
            if(skills.Length >0)
            {
                selectedSkill = ChooseSkill();
                Debug.Log("selected skill: "+selectedSkill.skillName);
                if(selectedSkill.requiresTarget)
                    state = AIState.Searching;
                else
                {
                    state = AIState.UsingSkill;
                }
            }
            else
                state = AIState.Idling;
            break;
        case AIState.MovingTowardsTarget:
            indicator.material.color = Color.blue;
            curMovementTimer = maxMovementTimer;
            break;
        case AIState.UsingSkill:
            indicator.material.color = Color.red;
            usingSkillJob = Job.make(selectedSkill.UseSkill(), true);
            usingSkillJob.jobComplete += (waskilled) => 
            {
                if(!waskilled)
                {
                    state = AIState.Resting;
                    Debug.Log("ended attack");
                }
            };
            break;
        }
    }

    void ExitState(AIState stateExited)
    {
        switch(stateExited)
        {
        case AIState.UsingSkill:
            if(usingSkillJob != null) usingSkillJob.kill();
            break;
        case AIState.MovingTowardsTarget:
            if(moveToTargetJob != null) moveToTargetJob.kill();
            break;
        case AIState.Searching:
            if(searchTargetJob != null) searchTargetJob.kill();
            break;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	    switch(state)
        {
        case AIState.MovingTowardsTarget:
            curMovementTimer -= Time.deltaTime;
            Debug.Log("update movement");
            Debug.Log("is in range = " + TargetIsInSkillRange().ToString());

            if(curMovementTimer <= 0 && !TargetIsInSkillRange())
            {
                state = AIState.SelectSkill;
            }

            if(TargetIsInSkillRange())
            {
                Debug.Log("in skill range!");
                state = AIState.UsingSkill;
            }
            else
            {

                //rotation
                Debug.Log("move?");
                _transform.forward += (targetObject.position - _transform.position) * rotationSpeed * Time.deltaTime;
                //move
                Vector3 moveDirection = targetObject.position - _transform.position;
                moveDirection.y = 0;
                _controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
               // _transform.position = Vector3.MoveTowards (_transform.position, targetObject.position, movementSpeed * Time.deltaTime);
            }
            break;
        }
	}

    public IEnumerator SearchForTarget()
    {
        Debug.Log("searching");
        //GameObject[] alltargets;
        alltargets = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < alltargets.Length; i++) {
            CharacterStatus cs = alltargets[i].GetComponent<CharacterStatus>();
            if(cs != null)
            {
                if(cs.CurrentHealth >0 && !availableTargets.Contains(alltargets[i]))
                {
                    availableTargets.Add(alltargets[i]);
                    Debug.Log("adding player");
                }
                if(cs.CurrentHealth <=0 && availableTargets.Contains(alltargets[i]))
                {
                    availableTargets.Remove(alltargets[i]);
                    Debug.Log("removing player");
                }
            }
        }
        Debug.Log(availableTargets.Count);
        if(availableTargets.Count >0)
        {
            targetObject = availableTargets[Random.Range(0, availableTargets.Count)].transform;
        }
        else
            targetObject = null;

        yield return new WaitForSeconds(3);
        if(targetObject != null)
        {
            if(TargetIsInSkillRange())
                state = AIState.UsingSkill;
            else
            {
                Debug.Log("time to move");
                state = AIState.MovingTowardsTarget;
            }
        }
    }

    public bool TargetIsInSkillRange()
    {
        if(targetObject != null)
        {
            Debug.Log(Vector3.Distance(_transform.position, targetObject.position));
            if(Vector3.Distance(_transform.position, targetObject.position) < selectedSkill.skillRange)
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
