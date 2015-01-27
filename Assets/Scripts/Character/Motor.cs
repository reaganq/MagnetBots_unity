using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Motor : MonoBehaviour {

	public CharacterController controller;
	public Transform _myTransform;
	public List<impactData> impacts = new List<impactData>();
	public Vector3 impact = Vector3.zero;
	public Vector3 characterVelocity;
	public CharacterStatus myStatus;
	public Animation myAnimation;
	public Vector3 rotationTarget;
	public Vector3 cachedRotation;
	public bool disableMovement;

	public Vector3 horizontalVelocity;
	public float currentSpeed
	{
		get{
			return myStatus.curMovementSpeed;
		}
	}

	public float acceleration;

	public virtual void Awake()
	{
		controller=GetComponent<CharacterController>();
		characterVelocity = Vector3.zero;
		myStatus = GetComponent<CharacterStatus>();
		_myTransform = transform;
	}

	public void AddImpact(Vector3 dir, float force, float duration, float acceleration)
	{
		impactData data = new impactData();
		dir.Normalize();
		dir = new Vector3(dir.x, 0, dir.z);
		data.dir = dir;
		data.force = force;
		data.duration = duration;
		data.accelration = acceleration;
		data.timer = 0;
		impacts.Add(data);
	}

	public void ManualRotate()
	{
		Vector3 _direction = (rotationTarget - _myTransform.position).normalized;
		_direction.y = 0;
		Quaternion _lookrotation = Quaternion.LookRotation(_direction);
		_myTransform.rotation = Quaternion.Slerp(_myTransform.rotation, _lookrotation, Time.deltaTime*myStatus.rotationSpeed);
		if(Vector3.Angle( _myTransform.forward, _direction) < 1)
		{
			rotationTarget = Vector3.zero;
		}
	}

	public virtual void LateUpdate()
	{
		impact = Vector3.zero;
		
		for (int i = 0; i < impacts.Count; i++) {
			impact += impacts[i].dir * impacts[i].force;
			impacts[i].timer += Time.deltaTime;
		}
		
		if(impact.magnitude > 0.1f)
		{
			controller.Move(impact*Time.deltaTime);
		}
		
		for (int i = 0; i < impacts.Count; i++) {
			if(impacts[i].timer >= impacts[i].duration)
				impacts[i].force -= impacts[i].accelration * Time.deltaTime;
			if(impacts[i].force <= 0)
				impacts.RemoveAt(i);
		}
	}
}
