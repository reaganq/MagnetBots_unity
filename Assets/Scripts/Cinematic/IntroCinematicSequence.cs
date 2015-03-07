using UnityEngine;
using System.Collections;

public class IntroCinematicSequence : Cutscene {

	public int stepCounter;
	public GameObject interiorlab;
	public Animation machineAnimation;
	public Animation assistantAnimation;
	public Animation TeslaAnimation;
	public Camera interiorCamera;
	public Animation interiorCameraAnimation;
	public Camera townTrackingCamera;
	public Animation townTrackingCameraAnimation;

	// Use this for initialization
	void Start () {

		StartCoroutine(EnterPhaseOne());
		GUIManager.Instance.IntroGUI.Disable();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void ProceedNextStep()
	{
		if(stepCounter == 2)
			StartCoroutine(EnterPhaseTwo());
		else if(stepCounter == 3)
			StartCoroutine(EnterPhaseThree());
	}

	//get from world tour to building magnetbot
	public IEnumerator EnterPhaseOne()
	{
		stepCounter = 1;
		townTrackingCameraAnimation.Play();
		yield return townTrackingCameraAnimation["clipname"].length;
		townTrackingCamera.enabled = false;
		interiorCameraAnimation.Play();
		assistantAnimation.Play();
		machineAnimation.Play();
		TeslaAnimation.Play();
		yield return interiorCameraAnimation["intro"].length;
		TeslaAnimation.CrossFade("waitingLoop");
		GUIManager.Instance.nakedArmorGUI.Enable();
		stepCounter ++;
	}

	//finish build magnetbot and enter Name Typing phase
	public IEnumerator EnterPhaseTwo()
	{
		GUIManager.Instance.nakedArmorGUI.Disable();
		interiorCameraAnimation.Play("buildFinish");
		yield return new WaitForSeconds(1);
		TeslaAnimation.CrossFade("happyBuild");
		yield return new WaitForSeconds(TeslaAnimation["happyBuild"].length);
		stepCounter ++;
		GUIManager.Instance.nakedArmorGUI.EnableSetNameUI();
		TeslaAnimation.CrossFade("waitingLoop2");
	}

	public IEnumerator EnterPhaseThree()
	{
		TeslaAnimation.Play("outro");
		yield return new WaitForSeconds(TeslaAnimation["outro"].length);

	}
}
