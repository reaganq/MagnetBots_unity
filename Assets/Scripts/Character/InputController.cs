using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
 
    /// <summary>
    /// SUPER TEMPORARY GAME START HACK
    /// </summary>
    public InputType inputType;
    public CharacterController character = null;
    private Transform _myTransform;
    public CharacterMotor motor = null;
    public CharacterActionManager actionManager = null;
    public CharacterStatus characterStatus = null;
    
    //touch joy stick 
    public EasyJoystick joystick = null;
    
    public Transform cameraTransform = null;
    //public UICamera uiCamera;
    public Vector3 inputDir;
    
    //public Vector2 outputAngleVector;
    //public float outputAngle = 0.0f;
    //public float speed = 6.4f;
    //public float outputForce = 0.0f;

	public LayerMask layerMask = -1;
	public int terrainMask = 1<<16;
	public int poiMask = 1<<15;
	private Vector2 lastPressDownPos;
	public Vector3 targetWayPoint;
	public bool hasWayPoint;
	
    
    //wasd controls
    
    void Awake()
    {
        _myTransform = transform;
        character = gameObject.GetComponent<CharacterController>();
        motor = gameObject.GetComponent<CharacterMotor>();
        //uiCamera = Camera.main.gameObject.GetComponent<UICamera>();
        //UICamera.fallThrough = this.gameObject;
        //outputAngleVector = new Vector2(0,0);

    }
	// Use this for initialization
	void Start () {
	    inputType = GameManager.Instance.inputType;
        
        if(inputType == InputType.WASDInput)
        {
        }
        
        else if(inputType == InputType.TouchInput)
        {
            joystick = GameManager.Instance.joystick;
        }
        cameraTransform = PlayerCamera.Instance.childTransform;
	}

    void OnEnable()
    {
        AttackButtonMessage.onPress += onPress;
        AttackButtonMessage.onRelease += onRelease;
    }
    
    void OnDisable()
    {
        AttackButtonMessage.onPress -= onPress;
        AttackButtonMessage.onRelease -= onRelease;
    }

    public void onPress(int index)
    {
        if(index == -1)
            actionManager.LeftAction(InputTrigger.OnPressDown);
        else if(index == -2)
            actionManager.RightAction(InputTrigger.OnPressDown);
    }
    
    public void onRelease(int index)
    {
        if(index == -1)
            actionManager.LeftAction(InputTrigger.OnPressUp);
        else if(index == -2)
            actionManager.RightAction(InputTrigger.OnPressUp);
    }
	
	// Update is called once per frame
	void Update () {

	    if(inputType == InputType.WASDInput && !GUIManager.Instance.IsUIBusy())
	    {
	        /*bool dirPressed = true;
	        
	        float inputX=Input.GetAxis("Horizontal");
	        float inputY=Input.GetAxis("Vertical");
	        
	        if(inputX!=0f||inputY!=0f){
	            dirPressed=true;
	            
	            inputDir=new Vector3(inputX,0f,inputY).normalized;
	            motor.SetRotAngle(_myTransform.position+inputDir);
	            Debug.Log(inputDir);
	        }
	        
	        if(inputX == 0f && inputY == 0)
	        {
	            inputDir = Vector3.zero;
	            Debug.Log("inputs are zero");
	        }*/
	        
	        //inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	        //inputDir = _myTransform.TransformDirection(inputDir);
	        
	        var direction = Vector3.zero;
	        var forward = Quaternion.AngleAxis(-90,Vector3.up) * cameraTransform.right;
	        //Debug.Log(forward);
	         
	        if(Input.GetKey(KeyCode.W))
	        direction += forward;
	        if(Input.GetKey(KeyCode.S))
	        direction -= forward;
	        if(Input.GetKey(KeyCode.A))
	        direction -= cameraTransform.right;
	        if(Input.GetKey(KeyCode.D))
	        direction += cameraTransform.right;
	               
	        direction.Normalize();
	        motor.Move(direction);
	    }

	    if(inputType == InputType.TouchInput)
	    {
	        //Debug.Log(joystick.JoystickAxis.x);

	        var direction = Vector3.zero;
	        var forward = Quaternion.AngleAxis(-90,Vector3.up) * cameraTransform.right;
	        //Debug.Log(forward);
	        
	        if(joystick.JoystickAxis.y > 0.1f)
	            direction += forward * Mathf.Abs(joystick.JoystickAxis.y);
	        if(joystick.JoystickAxis.y < -0.1f)
	            direction -= forward * Mathf.Abs(joystick.JoystickAxis.y);
	        if(joystick.JoystickAxis.x > 0.1f)
	            direction += cameraTransform.right* Mathf.Abs(joystick.JoystickAxis.x);
	        if(joystick.JoystickAxis.x < -0.1f)
	            direction -= cameraTransform.right* Mathf.Abs(joystick.JoystickAxis.x);
	        
	        
				
			if(direction.magnitude > 0)
				hasWayPoint = false;

			if(hasWayPoint && Vector3.Distance(targetWayPoint, _myTransform.position) >0.5f)
			{
				direction = targetWayPoint - _myTransform.position;
				direction.y = 0;
			}
					
			direction.Normalize();
			motor.Move(direction);
	    }

		if(hasWayPoint && Vector3.Distance(targetWayPoint, _myTransform.position) < 0.5f)
			hasWayPoint = false;
        //}
    }

    void OnPress(bool isDown)
    {
		if(inputType == InputType.TouchInput)
		{
			if(!GUIManager.Instance.IsUIBusy())
			{
				if(isDown)
				{
					lastPressDownPos = UICamera.lastTouchPosition;
				}
				else
				{
					if(Vector2.Distance(UICamera.lastTouchPosition, lastPressDownPos) < 1.5f)
					{
						Ray ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y, 0 ));
						RaycastHit hit;

						if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
						{
							int layermsk = (1<<hit.collider.gameObject.layer);

							if(layermsk == terrainMask)
							{
								Debug.Log(hit.point);
								targetWayPoint = hit.point;
								hasWayPoint = true;
							}
							else if(layermsk == poiMask)
							{
								hit.collider.gameObject.SendMessage("ActivatePOI");
								Debug.Log("activating POI");
							}
						}
					}
				}
			}
		}

		if(inputType == InputType.WASDInput)
		{
            if(!GUIManager.Instance.IsUIBusy())
            {
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y, 0 ));
				RaycastHit hit;
				
				if(Physics.Raycast(ray, out hit, Mathf.Infinity, poiMask))
				{
					if(!isDown)
					{
						Debug.Log("hit poi");
						hit.collider.gameObject.SendMessage("ActivatePOI");
					}
					/*int layermsk = (1<<hit.collider.gameObject.layer);
                        
                        if(layermsk == terrainMask)
                        {
                            Debug.Log(hit.point);
                            targetWayPoint = hit.point;
                            hasWayPoint = true;
                        }
                        if(layermsk == poiMask)
                        {

                            Debug.Log("activating POI");
                        }*/
				}
				else
				{
					if(!actionManager.isLocked())
					{
		                if(isDown)
		                {	                   
	                        if(UICamera.currentTouchID == -1)
	                            actionManager.LeftAction(InputTrigger.OnPressDown);
	                        if(UICamera.currentTouchID == -2)
	                            actionManager.RightAction(InputTrigger.OnPressDown);
		                }
		                else
						{
	                        if(UICamera.currentTouchID == -1)
	                            actionManager.LeftAction(InputTrigger.OnPressUp);
	                        if(UICamera.currentTouchID == -2)
	                            actionManager.RightAction(InputTrigger.OnPressUp);
		                }
					}
				}
            }
		}

    }

    /*void OnClick()
    {
		Debug.Log("click");
        if(GameManager.Instance.GameHasStarted)
        {
            if(!GUIManager.Instance.IsUIBusy() && inputType == InputType.WASDInput)
            {
                if(!actionManager.isLocked())
                {
                    if(UICamera.currentTouchID == -1)
                        actionManager.LeftAction(InputTrigger.OnClick);
                    if(UICamera.currentTouchID == -2)
                        actionManager.RightAction(InputTrigger.OnClick);
                }
                //Debug.Log("Onclick "+ UICamera.currentTouchID);
            }
        }
    }*/
   
    
    void FaceMovementDirection()
    {    
        Vector3 horizontalVelocity = character.velocity;
        horizontalVelocity.y = 0; // Ignore vertical movement
     
     // If moving significantly in a new direction, point that character in that direction
        if ( horizontalVelocity.magnitude > 0.1 )
            _myTransform.forward = horizontalVelocity.normalized;
         //{
         //var newDir = Vector3.RotateTowards
         //}
    }
    
    /*void OnEnable(){
        EasyJoystick.On_JoystickMove += On_JoystickMove;
        EasyJoystick.On_JoystickMoveEnd += On_JoystickMoveEnd;
    }
 
    void OnDisable(){
        EasyJoystick.On_JoystickMove -= On_JoystickMove ;
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }
     
    void OnDestroy(){
        EasyJoystick.On_JoystickMove -= On_JoystickMove;    
        EasyJoystick.On_JoystickMoveEnd -= On_JoystickMoveEnd;
    }
    
    void On_JoystickMove( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
        {
            outputAngleVector = move.joystickAxis;
            outputForce = Vector2.Distance(move.joystickAxis, Vector2.zero);
            Debug.Log(outputForce + " outputforce");
            Debug.Log(move.joystickValue);
        }
    }
    
    void On_JoystickMoveEnd( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
        {
            outputAngleVector = new Vector2(0,0);
            outputForce = 0;
        }
    }*/
}

