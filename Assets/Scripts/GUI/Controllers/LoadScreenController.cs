using UnityEngine;
using System.Collections;

public class LoadScreenController : BasicGUIController {

	public Animation loadScreenAnimation;
	public float rotationSpeed;
	public Transform centralGear;
	public GameObject tipBox;
	public bool isLoading;

	public void Enable(bool auto)
	{
		Debug.Log("Starting load screen" + Time.realtimeSinceStartup);
		base.Enable ();
		rotationSpeed = 0;
		StartCoroutine(Intro(auto));
	}

	public void Update()
	{
		if(isDisplayed)
		{
			centralGear.Rotate(0,0,Time.deltaTime*rotationSpeed);
		}
	}

	public IEnumerator Intro(bool auto)
	{
		loadScreenAnimation.Play("LoadingScreen");
		isLoading = true;
		if(auto)
		{
			yield return new WaitForSeconds(1f);
			StartCoroutine(Outro());
		}

		yield return null;
	}

	public void HideLoadScreen()
	{
		StartCoroutine(Outro());
	}

	public IEnumerator Outro()
	{
		Debug.Log(Time.realtimeSinceStartup);
		if(isLoading)
		{
		loadScreenAnimation.Play("LoadingScreen_Outro");
		yield return new WaitForSeconds(loadScreenAnimation["LoadingScreen_Outro"].length);
		Disable();
		}
		else
			yield return null;
	}

	public override void Disable ()
	{
		rotationSpeed = 0;
		base.Disable();
	}
}
