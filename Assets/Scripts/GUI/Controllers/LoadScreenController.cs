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
	}

	public void Update()
	{
		if(isDisplayed)
		{
			centralGear.Rotate(Time.deltaTime*rotationSpeed, 0,0);
		}
	}

	public IEnumerator Intro()
	{
		Enable();
		loadScreenAnimation.Play("loadingScreen_intro");
		yield return new WaitForSeconds(loadScreenAnimation["loadingScreen_intro"].length);
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
