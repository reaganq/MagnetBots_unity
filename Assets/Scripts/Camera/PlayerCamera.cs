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
                    PlayerCamera prefab = Resources.Load("Managers/_GameCamera", typeof(PlayerCamera)) as PlayerCamera;
                    instance = Instantiate(prefab) as PlayerCamera;
                }
            }
            return instance;
        }
    }

    public Transform targetTransform = null;
    public Transform childTransform = null;
	public Camera childCamera;
    private Transform _myTransform;
	public Transform defaultPos;
	public Transform quickArmoryPos;
	private Job movementJob;
	public float defaultFOV = 60;
	private Transform newTransform;

	public float quickInventoryCameraRectOffset;
	public float quickSingleInventoryCameraRectOffset;
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

        _myTransform = this.transform;


	}

	void Start()
	{
		quickInventoryCameraRectOffset = Mathf.Ceil(((GameManager.Instance.defaultAspectRatio/GameManager.Instance.nativeAspectRatio)*-700/2048)*100) / 100;
		quickSingleInventoryCameraRectOffset = Mathf.Ceil(((GameManager.Instance.defaultAspectRatio/GameManager.Instance.nativeAspectRatio)*-390/2048)*100) / 100;
	}
    
	void LateUpdate () {
        
        if(targetTransform != null)
            _myTransform.position = targetTransform.position;
        
    }

	public void TransitionToQuickArmory()
	{
		TransitionTo(quickArmoryPos, defaultFOV, 0.3f, quickInventoryCameraRectOffset);
	}

	public void TransitionToQuickInventory()
	{
		TransitionTo(defaultPos, defaultFOV, 0.3f, quickInventoryCameraRectOffset);
	}
	
	public void TransitionToDefault()
	{
		TransitionTo(defaultPos, defaultFOV, 0.2f, 0);
		//StartCoroutine(MoveTo(defaultPos, 60, 1));
	}

	public void TransitionTo(Transform newTrans, float fov, float duration, float offset)
	{
		if(movementJob != null)
			movementJob.kill();
		
		movementJob = Job.make(MoveTo(newTrans, fov, duration, offset), true);
        //StartCoroutine(MoveTo(newTrans, fov, duration));
	}

	public IEnumerator MoveTo(Transform newTrans, float fov, float duration, float offset)
	{
		float startTime = Time.time;
		float timer = 0;
		Vector3 origPos = childTransform.position;
		Quaternion origRot = childTransform.rotation;
		float origFov = childCamera.fieldOfView;
		float origX = childCamera.rect.x;
		//Vector4 curRect = childCamera.rect;
		//Vector4 newRect = new Vector4(offset, curRect.y, curRect.z, curRect.w);
		while(Time.time < startTime + duration)
		{
			childTransform.position = Vector3.Lerp(origPos, newTrans.position, timer);
			childTransform.rotation = Quaternion.Lerp(origRot, newTrans.rotation, timer);
			childCamera.fieldOfView = Mathf.Lerp(origFov, fov, timer);
			childCamera.rect = new Rect(Mathf.Lerp(origX, offset, timer), childCamera.rect.y, childCamera.rect.width, childCamera.rect.height);
			timer += Time.deltaTime/duration;
			yield return null;
		}
		childTransform.position = newTrans.position;
		childTransform.rotation = newTrans.rotation;
		childCamera.fieldOfView = fov;
		childCamera.rect = new Rect(offset, childCamera.rect.y, childCamera.rect.width, childCamera.rect.height);
		yield return null;

	}

	public void Reset()
	{
		childTransform.position = defaultPos.position;
		childTransform.rotation = defaultPos.rotation;
		childCamera.fieldOfView = 60;
	}

}
