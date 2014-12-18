﻿using UnityEngine;
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

	private Transform newTransform;

	public float quickInventoryCameraRectOffset;
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
		quickInventoryCameraRectOffset = Mathf.Ceil(((GameManager.Instance.defaultAspectRatio/GameManager.Instance.nativeAspectRatio)*-1090/2048)*100) / 100;
		Debug.Log(Mathf.Ceil(((GameManager.Instance.defaultAspectRatio/GameManager.Instance.nativeAspectRatio)*-1090/2048)*100));
		Debug.Log(quickInventoryCameraRectOffset);
	}
    
	void LateUpdate () {
        
        if(targetTransform != null)
            _myTransform.position = targetTransform.position;
        
    }

	public void TransitionToQuickArmory()
	{
		TransitionTo(quickArmoryPos, 40, 0.8f, quickInventoryCameraRectOffset);
	}

	public void TransitionToQuickInventory()
	{
		TransitionTo(defaultPos, 60, 0.8f, quickInventoryCameraRectOffset);
	}

	public void TransitionToDefault()
	{
		TransitionTo(defaultPos, 60, 0.55f, 0);
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
		//Vector4 curRect = childCamera.rect;
		//Vector4 newRect = new Vector4(offset, curRect.y, curRect.z, curRect.w);
		while(Time.time < startTime + duration)
		{
			childTransform.position = Vector3.Lerp(childTransform.position, newTrans.position, (Time.time - startTime)/duration);
			childTransform.rotation = Quaternion.Lerp(childTransform.rotation, newTrans.rotation, (Time.time - startTime)/duration);
			childCamera.fieldOfView = Mathf.Lerp(childCamera.fieldOfView, fov, (Time.time - startTime)/duration);
			childCamera.rect = new Rect(Mathf.Lerp(childCamera.rect.x, offset, (Time.time - startTime)/duration), childCamera.rect.y, childCamera.rect.width, childCamera.rect.height);
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
