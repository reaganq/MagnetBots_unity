using UnityEngine;
using System.Collections;

public class LifeTime : MonoBehaviour {

	public float life;

	public void Start()
	{
		Invoke("Destroy", life);
	}

	public void Destroy()
	{
		GameObject.Destroy(this);
	}
}
