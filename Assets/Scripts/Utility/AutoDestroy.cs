using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

	public float timer;
	// Use this for initialization
	void Start () {
		Invoke("Destroy", timer);
	}
	
	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}
