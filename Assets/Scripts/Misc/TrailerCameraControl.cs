using UnityEngine;
using System.Collections;

public class TrailerCameraControl : MonoBehaviour {

	public Camera[] cameras;
	public Animation camera7;
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKey(KeyCode.Keypad0))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[0] != null)
				cameras[0].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad1))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[1] != null)
				cameras[1].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad2))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[2] != null)
				cameras[2].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad3))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[3] != null)
				cameras[3].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad4))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[4] != null)
				cameras[4].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad5))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[5] != null)
				cameras[5].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad6))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
			if(cameras[6] != null)
				cameras[6].enabled = true;
		}
		if(Input.GetKey(KeyCode.Keypad7))
		{
			camera7.Play();
		}
		if(Input.GetKey(KeyCode.Keypad9))
		{
			foreach(Camera camera in cameras)
			{
				camera.enabled = false;
			}
		}
	}
}
