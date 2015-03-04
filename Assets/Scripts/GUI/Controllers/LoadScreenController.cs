using UnityEngine;
using System.Collections;

public class LoadScreenController : BasicGUIController {

	public Animation loadScreenAnimation;
	public float rotationSpeed;
	public Transform centralGear;
	public GameObject tipBox;

	public override void Enable ()
	{
		base.Enable ();
		rotationSpeed = 0;
		StartCoroutine(Intro());
	}

	public void Update()
	{
		if(isDisplayed)
		{
			centralGear.Rotate(0,0,Time.deltaTime*rotationSpeed);
		}
	}

	public IEnumerator Intro()
	{
		loadScreenAnimation.Play("LoadingScreen");
		yield return new WaitForSeconds(loadScreenAnimation["LoadingScreen"].length);
		Disable();
	}

	public void DisplayLoadScreen()
	{
		StartCoroutine(Intro());
	}

	public void HideLoadScreen()
	{
		StartCoroutine(Outro());
	}

	public IEnumerator Outro()
	{
		loadScreenAnimation.Play("loadingScreen_ontro");
		yield return new WaitForSeconds(loadScreenAnimation["loadingScreen_ontro"].length);
		Disable();
	}

	public override void Disable ()
	{
		rotationSpeed = 0;
		base.Disable();
	}
}
