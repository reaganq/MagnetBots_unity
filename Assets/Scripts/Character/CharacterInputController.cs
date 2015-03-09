using UnityEngine;
using System.Collections;

public class CharacterInputController : MonoBehaviour {
 
    /// <summary>
    /// SUPER TEMPORARY GAME START HACK
    /// </summary>
    public InputType inputType;
    public CharacterController character = null;
    private Transform _myTransform;
    public PlayerMotor motor = null;
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
	public int characterLayerMask = 1<<9;
	private Vector2 lastPressDownPos;
	public Vector3 targetWayPoint;
	public bool hasWayPoint;
	
    
    //wasd controls
    
    void Awake()
    {
        _myTransform = transform;
        character = gameObject.GetComponent<CharacterController>();
        motor = gameObject.GetComponent<PlayerMotor>();
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
        SkillButton.onPress += onPress;
        SkillButton.onRelease += onRelease;
    }
    
    void OnDisable()
    {
        SkillButton.onPress -= onPress;
        SkillButton.onRelease -= onRelease;
    }

    public void onPress(int slot)
    {
		Debug.Log("pressed button: " );
		actionManager.UseSkill(InputTrigger.OnPressDown, slot);
    }
    
    public void onRelease(int slot)
    {
		Debug.Log("released button: ");
		actionManager.UseSkill(InputTrigger.OnPressUp, slot);
    }
	
	// Update is called once per frame
	void Update () {

		var direction = Vector3.zero;
		var forward = Quaternion.AngleAxis(-90,Vector3.up) * cameraTransform.right;

	    if(inputType == InputType.WASDInput && !GUIManager.Instance.IsUIBusy())
	    {
	        if(Input.GetKey(KeyCode.W))
	        direction += forward;
	        if(Input.GetKey(KeyCode.S))
	        direction -= forward;
	        if(Input.GetKey(KeyCode.A))
	        direction -= cameraTransform.right;
	        if(Input.GetKey(KeyCode.D))
	        direction += cameraTransform.right;  
	    }

	    else if(inputType == InputType.TouchInput)
	    {
	        if(joystick.JoystickAxis.y > 0.1f)
	            direction += forward * Mathf.Abs(joystick.JoystickAxis.y);
	        if(joystick.JoystickAxis.y < -0.1f)
	            direction -= forward * Mathf.Abs(joystick.JoystickAxis.y);
	        if(joystick.JoystickAxis.x > 0.1f)
	            direction += cameraTransform.right* Mathf.Abs(joystick.JoystickAxis.x);
	        if(joystick.JoystickAxis.x < -0.1f)
	            direction -= cameraTransform.right* Mathf.Abs(joystick.JoystickAxis.x);

			if(Input.GetKey(KeyCode.W))
				direction += forward;
			if(Input.GetKey(KeyCode.S))
				direction -= forward;
			if(Input.GetKey(KeyCode.A))
				direction -= cameraTransform.right;
			if(Input.GetKey(KeyCode.D))
				direction += cameraTransform.right;  
	    }

		if(direction.magnitude > 0)
			hasWayPoint = false;
		
		if(hasWayPoint && Vector3.Distance(targetWayPoint, _myTransform.position) >0.5f)
		{
			direction = targetWayPoint - _myTransform.position;
			direction.y = 0;
		}
		
		direction.Normalize();
		motor.Move(direction);

		if(hasWayPoint && Vector3.Distance(targetWayPoint, _myTransform.position) < 0.5f)
			hasWayPoint = false;
    }

	public void SetWayPoint(Vector3 target)
	{
		targetWayPoint = target;
		hasWayPoint = true;
	}

    /*void OnPress(bool isDown)
    {
		if(inputType == InputType.WASDInput)
		{
			if(!GUIManager.Instance.IsUIBusy())
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
    }*/

    /*void FaceMovementDirection()
    {    
        Vector3 horizontalVelocity = character.velocity;
        horizontalVelocity.y = 0; 

        if ( horizontalVelocity.magnitude > 0.1 )
            _myTransform.forward = horizontalVelocity.normalized;

    }*/
}

