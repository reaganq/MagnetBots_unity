using UnityEngine;
using System.Collections;

public class ConstantRotation : MonoBehaviour {

	public Vector3 rotationVector = Vector3.zero;
	private Transform _myTransform;
	public bool isWorld = false;
	// Use this for initialization
	void Start () {
		_myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(isWorld)
			_myTransform.Rotate(rotationVector*Time.deltaTime, Space.World);
		else
		{
			_myTransform.Rotate(rotationVector*Time.deltaTime, Space.Self);
		}
	}
}
