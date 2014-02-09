using UnityEngine;
using System.Collections;

public class PlayerCamera: MonoBehaviour {
 
    private static PlayerCamera instance = null;
    
    private PlayerCamera() {}
    
    public static PlayerCamera Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(PlayerCamera)) as PlayerCamera;
                if (instance == null)
                {
                    PlayerCamera prefab = Resources.Load("Managers/GameCamera", typeof(PlayerCamera)) as PlayerCamera;
                    instance = Instantiate(prefab) as PlayerCamera;
                }
            }
            return instance;
        }
    }

    public Transform targetTransform = null;
    public Transform childTransform = null;
    private Transform _myTransform;
    //private Vector3 offsetPosition;
    
    //public float rotationSpeed = 50;
    //public EasyJoystick joystick = null;
    
    //private Transform _myTransform = null;
    
    //private Vector2 outputAngleVector = Vector2.zero;
    //private float outputAngle = 0.0f;
    
    //private Vector2 camRotation;
    
	// Usae this for initialization
	void Awake () {

        //DontDestroyOnLoad(gameObject.transform.parent.gameObject);
	    //_myTransform = transform;
        //camRotation = new Vector2(0,0);
        
        //offsetPosition = pivotTransform.position;
        
        if(Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(this.gameObject);
            return;
        }
        
        else
        {
            DontDestroyOnLoad(this);
        }

        childTransform = transform.GetChild(0).transform;
        _myTransform = this.transform;
        //outputAngleVector = new Vector2(0,0);
        //joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();
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
            //move.joystickAxis
            outputAngleVector = move.joystickAxis;
        }
    }
    
    void On_JoystickMoveEnd( MovingJoystick move )
    {
        if(move.joystickName == "CharacterJoystick")
            outputAngleVector = new Vector2(0,0);
    }*/
    
	// Update is called once per frame
	void LateUpdate () {
        
        //outputAngle = Mathf.Atan2(joystick.JoystickAxis.x, joystick.JoystickAxis.y);
        
        if(targetTransform != null)
            _myTransform.position = targetTransform.position;
        //camRotation = new Vector2(Mathf.Sin(outputAngle), Mathf.Cos(outputAngle));
        //camRotation.x *= rotationSpeed;
        //camRotation *= Time.deltaTime;
        
        
        //pivotTransform.Rotate(0, camRotation.x, 0, Space.World );
        
        
        /*var camRotation = new Vector2(Mathf.Sin(joystickCircle.outputAngleRad) , Mathf.Cos(joystickCircle.outputAngleRad));
 camRotation.x *= rotationSpeed.x;
 camRotation.y *= rotationSpeed.y;
 camRotation *= Time.deltaTime;
 
 // Rotate around the character horizontally in world, but use local space
 // for vertical rotation
 cameraPivot.Rotate( 0, camRotation.x, 0, Space.World );
 cameraPivot.Rotate( camRotation.y * -5, 0, 0 );*/
        
    }

}
