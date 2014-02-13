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
    
    private Transform _myTransform;
    //private Transform rigidbodyTransform;
    
    //public bool canRun = true;
    //public bool isRunning = false;
    public bool canControl = true;
    public bool canRotate = true;
    
    public Vector3 dir;
    //public Vector3[] speed = new Vector3[2];
    
    //public float Speed;
    public float rotSpeed = 11.5f;
    public Quaternion rotTarget;
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
        SetRotAngle(_myTransform.position+(Vector3.forward * -10f));
        SetRotTarget();
        //UpdateAnimation(); 
	}
	
#region Movement
	// Update is called once per frame
	void Update () {
        
        /*characterVelocity = controller.velocity;
	    horizontalVelocity = characterVelocity;
        horizontalVelocity.y = 0;
        currentSpeed = horizontalVelocity.magnitude;*/
        //UpdateMovement();
        if(!GameManager.Instance.GameIsPaused)
        {
            //if(input.inputType == InputType.WASDInput)
            //{
                UpdateFunction();
            //}
            AnimationUpdate();
        }
        
	}
    
    void FixedUpdate()
    {
        //UpdateMovementFixed();
        //AnimationUpdate();
    }
        
    void UpdateMovement()
    {
        /*if(input.inputDir.magnitude > 0)
        {
            /*speed[1] = _myTransform.TransformDirection((dir*runSpeed));
            speed[1] += Physics.gravity;
            speed[1] *= Time.deltaTime;
            Debug.Log("speed 1: " + speed[1]);
            
            speed[1] = dir*runSpeed;
            
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.Normalize();
            
            moveDirection *= runSpeed;
            moveDirection *= Time.deltaTime;
            Debug.LogWarning("move direction: " + moveDirection);
        //controller.Move(moveDirection);
            //Debug.Log("speed 0: " + speed[0]);
            //Vector3 moveDirection = input.inputDir.Normalize();
            //moveDirection *= runSpeed * 0.5f * (Vector3.Dot(gameObject.transform.forward, moveDirection) + 1);
        }
        
        else
        {
            speed[1] = Vector3.zero;
            Debug.Log(speed[0]);
            Debug.LogWarning("speed1" + speed[1]);
            Debug.Log("speed 1 is 0");
        }
        
        if(canRotate)
            {
                SetRotTarget();
                
                _myTransform.rotation = Quaternion.Slerp (_myTransform.rotation, rotTarget, (Time.deltaTime*rotSpeed));
            }
        
        speed[0] = Vector3.MoveTowards(speed[0], speed[1],(Time.deltaTime * acceleration));*/
        
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
                _myTransform.rotation  = Quaternion.Slerp(_myTransform.rotation,newRotation,Time.deltaTime * rotSpeed);
            }
                direction *= 0.2f * (Vector3.Dot(_myTransform.forward,direction) + 1);

            dir = direction;
            //Debug.LogWarning(dir);
        }
    }
    
    void UpdateFunction()
    {
        //Debug.Log("moving");
        Vector3 velocity = characterVelocity;
        
        velocity = ApplyInputVelocityChange(velocity);
        
        velocity += Physics.gravity;
        Vector3 lastPosition = _myTransform.position;
        Vector3 currentMovementOffset = velocity * Time.deltaTime;
        
        controller.Move(currentMovementOffset);
        
        var oldHVelocity    = new Vector3(velocity.x, 0, velocity.z);
        characterVelocity = (_myTransform.position - lastPosition) / Time.deltaTime;
        var newHVelocity    = new Vector3(characterVelocity.x, 0, characterVelocity.z);


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
        return _myTransform.TransformDirection(desiredLocalDirection * myStatus.runSpeed);
    }
    
    float MaxSpeedInDirection (Vector3 desiredMovementDirection)
    {
        if (desiredMovementDirection == Vector3.zero)
            return 0;
        else 
        {
            var zAxisEllipseMultiplier  = (desiredMovementDirection.z > 0 ? myStatus.runSpeed : myStatus.runSpeed) / myStatus.runSpeed;
            var temp    = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
            var length  = new Vector3(temp.x,0,temp.z * zAxisEllipseMultiplier).magnitude * myStatus.runSpeed;
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

    public void SetRotAngle(Vector3 destination)
    {
        dir=(destination-new Vector3(_myTransform.position.x,0f,_myTransform.position.z));
        float distTotal=dir.magnitude;
        dir/=distTotal;
        
        rotAngle = (Mathf.Atan2((destination.z - _myTransform.position.z),(destination.x-_myTransform.position.x)))*(180f/Mathf.PI);
        rotAngle=(-rotAngle+90f);
        
        //Debug.Log("rotAngle: "+rotAngle);
    }
    
    public void SetRotTarget()
    {
        rotTarget = Quaternion.Euler(new Vector3(0f, rotAngle, 0f));
    }

    #endregion

    public void AnimationUpdate()
    {
        horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;
        currentSpeed = horizontalVelocity.magnitude;
        //Debug.Log(currentSpeed);
        if(currentSpeed > 0)
        {
            float t = 0.0f;
            //Debug.Log(speed);
            t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.runSpeed ), 0, myStatus.runSpeed );
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