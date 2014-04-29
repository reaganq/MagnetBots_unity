using UnityEngine;
using System.Collections;

public class CharacterMotor : MonoBehaviour {
 
    //public ArmorController[] ArmorControllers = new ArmorController[5];
    
    public CharacterController controller;
    public CharacterActionManager animationManager;
    public CharacterStatus myStatus;
    public Animation animationTarget;
    public Avatar avatar;
    public InputController input;
	public bool disableMovement;
    
    public Transform _myTransform;
    //private Transform rigidbodyTransform;
    
    //public bool canRun = true;
    //public bool isRunning = false;
    public bool canControl = true;
    public bool canRotate = true;
    
    public Vector3 dir;
    //public Vector3[] speed = new Vector3[2];
    
    //public float Speed;
    //public float rotSpeed = 11.5f;
    public float rotAngle;
    
    public float rollSpeed;
    public float rollCounter = 0f;
    public float rollSlowTime;
    public float acceleration;
    //public float runSpeed;

    //----- DON'T TOUCH -----//
    public Vector3 characterVelocity;
    public Vector3 horizontalVelocity;
    public float currentSpeed;
	public Vector3 impact = Vector3.zero;
	
    //public Vector3 velocity = Vector3.zero;
    
	// Use this for initialization

    
	void Awake () {
	    controller=GetComponent<CharacterController>();
        input = GetComponent<InputController>();
        avatar = GetComponent<Avatar>();
        myStatus = GetComponent<CharacterStatus>();
        _myTransform = transform;
        characterVelocity = Vector3.zero;
        animationManager = GetComponent<CharacterActionManager>();
        //rigidbodyTransform = rigidbody.transform;
        //SetRotAngle(_myTransform.position+(Vector3.forward * -10f));
        //UpdateAnimation(); 
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
        }
        
	}

	void LateUpdate()
	{
		if(!GameManager.Instance.GameIsPaused)
		{
			if(impact.magnitude > 0.1f)
			{
				controller.Move(impact*Time.deltaTime);
			}
			impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime * 5);
		}
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
                //Debug.Log(newRotation);
				if(!disableMovement)
                	_myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * myStatus.rotationSpeed);
            }
                direction *= 0.2f * (Vector3.Dot(_myTransform.forward,direction) + 1);

            dir = direction;
            //Debug.LogWarning(dir);
        }
    }

	public void AddImpact(Vector3 dir, float force)
	{
		dir.Normalize();
		dir = new Vector3(dir.x, 0, dir.z);
		impact += dir.normalized * force / 1;
	}
    
    void UpdateFunction()
    {
        //Debug.Log("moving");
        Vector3 velocity = characterVelocity;
        
        velocity = ApplyInputVelocityChange(velocity);
        
        velocity += Physics.gravity;

		//if(impact.magnitude > 0.1f)
			//velocity += impact;

        Vector3 lastPosition = _myTransform.position;
        Vector3 currentMovementOffset = velocity * Time.deltaTime;
        
        controller.Move(currentMovementOffset);
        
        var oldHVelocity    = new Vector3(velocity.x, 0, velocity.z);
        characterVelocity = (_myTransform.position - lastPosition) / Time.deltaTime;
        var newHVelocity    = new Vector3(characterVelocity.x, 0, characterVelocity.z);

		//if(impact.magnitude > 0.1f)
		//impact = Vector3.Lerp(impact, Vector3.zero, Time.deltaTime * 5);
        // The CharacterController can be moved in unwanted directions when colliding with things.
        // We want to prevent this from influencing the recorded velocity.
        if (oldHVelocity == Vector3.zero) {
            characterVelocity = new Vector3(0, characterVelocity.y, 0);
        }
        else {
            var projectedNewVelocity    = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
            characterVelocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + characterVelocity.y * Vector3.up;
        }
        //Debug.Log(characterVelocity);
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
        //var maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
        //return _myTransform.TransformDirection(desiredLocalDirection * maxSpeed);
        return _myTransform.TransformDirection(desiredLocalDirection * myStatus.movementSpeed);
    }
    
    float MaxSpeedInDirection (Vector3 desiredMovementDirection)
    {
        if (desiredMovementDirection == Vector3.zero)
            return 0;
        else 
        {
            var zAxisEllipseMultiplier  = (desiredMovementDirection.z > 0 ? myStatus.movementSpeed : myStatus.movementSpeed) / myStatus.movementSpeed;
            var temp    = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            var length  = new Vector3(temp.x,0,temp.z * zAxisEllipseMultiplier).magnitude * myStatus.movementSpeed;
            Debug.Log("length: "+length);
            return length;
        }
    }
        
    void UpdateMovementFixed()
    {
        
        //Debug.LogWarning("moving");
        /*rigidbody.MovePosition(new Vector3(rigidbodyTransform.position.x,0f,rigidbodyTransform.position.z)+(speed[0]*Time.deltaTime));
        horizontalVelocity = rigidbody.velocity;
        Debug.LogError(horizontalVelocity);*/
        
    }

    /*public void SetRotAngle(Vector3 destination)
    {
        dir=(destination-new Vector3(_myTransform.position.x,0f,_myTransform.position.z));
        float distTotal=dir.magnitude;
        dir/=distTotal;
        
        rotAngle = (Mathf.Atan2((destination.z - _myTransform.position.z),(destination.x-_myTransform.position.x)))*(180f/Mathf.PI);
        rotAngle=(-rotAngle+90f);
        
        //Debug.Log("rotAngle: "+rotAngle);
    }*/

    #endregion

    public void AnimationUpdate()
    {
        horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
		Vector3 localVel = _myTransform.InverseTransformDirection(horizontalVelocity);
        currentSpeed = horizontalVelocity.magnitude;
        //Debug.Log(currentSpeed);
        if(localVel.z > 0)
        {
            float t = 0.0f;
            //Debug.Log(speed);
            t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.movementSpeed ), 0, myStatus.movementSpeed );
            animationManager.UpdateRunningSpeed(t);
            animationManager.AnimateToRunning();
        }
        else
        {
            //if(animationManager.myState == 
            animationManager.AnimateToIdle();
        }
    }
}



// idle
// run
// armLpreattack
//armLattack
//armLrecoil
//armLpostattack

// armRpreattack
//armRattack
//armRrecoil
//armRpostattack

//specialattack

//stunned
//frozen
//victory
//death