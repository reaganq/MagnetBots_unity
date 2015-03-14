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

	public DummyAvatar dummy;


	public bool face;
	public bool body;
	public bool armL;
	public bool armR;
	public bool legs;
	// Use this for initialization
	void Start () {

		StartCoroutine(EnterPhaseOne());
		GUIManager.Instance.IntroGUI.Disable();
		GUIManager.Instance.nakedArmorGUI.introCutscene = this;
		GUIManager.Instance.nakedArmorGUI.Enable();
	}
	
	// Update is called once per frame

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
		//townTrackingCameraAnimation.Play();
		StartCoroutine(PhaseOneSubtitles());
		//yield return townTrackingCameraAnimation["clipname"].length;
		//townTrackingCamera.enabled = false;
		interiorCameraAnimation.Play();
		assistantAnimation.Play();
		machineAnimation.Play();
		TeslaAnimation.Play();
		//yield return new WaitForSeconds(3);

		yield return new WaitForSeconds(interiorCameraAnimation["Monologue 1"].length);
		Debug.Log("waiting for end" + interiorCameraAnimation["Monologue 1"].length);
		GUIManager.Instance.nakedArmorGUI.LoadNakedArmors();
		//TeslaAnimation.CrossFade("waitingLoop");

		stepCounter ++;
	}

	public IEnumerator PhaseOneSubtitles()
	{
		yield return new WaitForSeconds(3);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Woah, you're finally here!");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("I've been expecting you!");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("Welcome to the world of Magnetbots.");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("We have so much to do, right this way!");
		yield return new WaitForSeconds(1.5f);
		GUIManager.Instance.nakedArmorGUI.HideSubtitle();
	}

	public void EquipNakedArmor(InventoryItem item)
	{
		RPGArmor armor = (RPGArmor)item.rpgItem;
		dummy.EquipBodyPart(armor.FBXName[0], armor.EquipmentSlotIndex);
		if(armor.EquipmentSlotIndex == EquipmentSlots.Face)
			face = true;
		else if(armor.EquipmentSlotIndex == EquipmentSlots.Body)
			body = true;
		else if(armor.EquipmentSlotIndex == EquipmentSlots.ArmL)
			armL = true;
		else if(armor.EquipmentSlotIndex == EquipmentSlots.ArmR)
			armR = true;
		else if(armor.EquipmentSlotIndex == EquipmentSlots.Legs)
			legs = true;
		CheckDummyStatus();
	}

	public void CheckDummyStatus()
	{
		if(face && body && armL && armR && legs)
			GUIManager.Instance.nakedArmorGUI.DisplayAvatarConfirmation();
	}

	//finish build magnetbot and enter Name Typing phase
	public IEnumerator EnterPhaseTwo()
	{
		GUIManager.Instance.nakedArmorGUI.HideNakedArmors();
		interiorCameraAnimation.Play("buildFinish");
		assistantAnimation.Play("Monologue 2");
		//yield return new WaitForSeconds(1);
		StartCoroutine(PhaseTwoSubtitles());
		TeslaAnimation.CrossFade("happyBuild");
		yield return new WaitForSeconds(TeslaAnimation["happyBuild"].length);
		stepCounter ++;
		GUIManager.Instance.nakedArmorGUI.EnableSetNameUI();
		TeslaAnimation.CrossFade("waitingLoop2");
	}

	public IEnumerator PhaseTwoSubtitles()
	{
		yield return new WaitForSeconds(1);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Woah, you're finally here!");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("I've been expecting you!");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("Welcome to the world of Magnetbots.");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("We have so much to do, right this way!");
		yield return new WaitForSeconds(1.5f);
		GUIManager.Instance.nakedArmorGUI.HideSubtitle();
	}

	public IEnumerator EnterPhaseThree()
	{
		TeslaAnimation.Play("outro");
		PlayerManager.Instance.RefreshAvatar();
		StartCoroutine(PhaseThreeSubtitles());
		yield return new WaitForSeconds(TeslaAnimation["outro"].length);
		PlayerManager.Instance.ActiveZone = PlayerManager.Instance.ActiveWorld.DefaultZone;
		Debug.Log("destroy shit");
		Destroy(gameObject);
		//fade
	}

	public IEnumerator PhaseThreeSubtitles()
	{
		yield return null;
	}

}
