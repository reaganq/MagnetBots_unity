using UnityEngine;
using System.Collections;

public class PlayerMotor : Motor {
     
    public CharacterActionManager myActionManager;
    public CharacterInputController input;

    public bool canControl = true;
    public bool canRotate = true;
    public Vector3 dir;
	
	public override void Awake () {
	    
		base.Awake();
        //input = GetComponent<CharacterInputController>();
        myActionManager = GetComponent<CharacterActionManager>();
		if(!myStatus.myPhotonView.isMine)
			enabled = false;
	}
	
#region Movement
	// Update is called once per frame
	void Update () {



        if(!GameManager.Instance.GameIsPaused)
        {
			if(!disableMovement)
			{
	            UpdateFunction();
	            AnimationUpdate();

			}
			if(characterVelocity == Vector3.zero && rotationTarget != Vector3.zero)
				ManualRotate();
        }
        
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
	}
    
    public void Move(Vector3 direction)
    {
        moveDirection = direction;
    }
	
    Vector3 direction;
    private Vector3 moveDirection
    {
        get { return direction; }
        set
        {
            direction = value;
            if(direction.magnitude > 0.1f)  
            {
                Quaternion newRotation = Quaternion.LookRotation(direction);
				if(!disableMovement)
                	_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
            }
                direction *= 0.2f * (Vector3.Dot(_myTransform.forward,direction) + 1);

            dir = direction;
        }
    }
    
    void UpdateFunction()
    {
        Vector3 velocity = characterVelocity;
        
        velocity = ApplyInputVelocityChange(velocity);
        
        velocity += Physics.gravity;

        Vector3 lastPosition = _myTransform.position;
        Vector3 currentMovementOffset = velocity * Time.deltaTime;
        
        controller.Move(currentMovementOffset);
        
        var oldHVelocity    = new Vector3(velocity.x, 0, velocity.z);
        characterVelocity = (_myTransform.position - lastPosition) / Time.deltaTime;
        var newHVelocity    = new Vector3(characterVelocity.x, 0, characterVelocity.z);

        if (oldHVelocity == Vector3.zero) {
            characterVelocity = new Vector3(0, characterVelocity.y, 0);
        }
        else {
            var projectedNewVelocity    = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
            characterVelocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + characterVelocity.y * Vector3.up;
        }
    }
    
    public Vector3 ApplyInputVelocityChange(Vector3 velocity)
    {
        if(!canControl)
            dir = Vector3.zero;
        
        Vector3 desiredVelocity = GetDesiredHorizontalVelocity();
        velocity.y = 0;
        
        var maxVelocityChange = acceleration * Time.deltaTime;
        var velocityChangeVector = (desiredVelocity - velocity);
        if (velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange) {
            velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
        }
        
        velocity += velocityChangeVector;
        
        return velocity;
        
    }
    
    public Vector3 GetDesiredHorizontalVelocity()
    {
        var desiredLocalDirection = _myTransform.InverseTransformDirection(dir);
        return _myTransform.TransformDirection(desiredLocalDirection * myStatus.curMovementSpeed);
    }
    
    float MaxSpeedInDirection (Vector3 desiredMovementDirection)
    {
        if (desiredMovementDirection == Vector3.zero)
            return 0;
        else 
        {
            var zAxisEllipseMultiplier  = (desiredMovementDirection.z > 0 ? myStatus.curMovementSpeed : myStatus.curMovementSpeed) / myStatus.curMovementSpeed;
            var temp    = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            var length  = new Vector3(temp.x,0,temp.z * zAxisEllipseMultiplier).magnitude * myStatus.curMovementSpeed;
            return length;
        }
    }

    #endregion

    public void AnimationUpdate()
    {
        horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
		Vector3 localVel = _myTransform.InverseTransformDirection(horizontalVelocity);
        if(localVel.z > 0)
        {
            float t = 0.0f;
            t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );
            myActionManager.UpdateRunningSpeed(t);
            myActionManager.AnimateToRunning();
        }
        else
        {
            myActionManager.AnimateToIdle();
        }
    }
}