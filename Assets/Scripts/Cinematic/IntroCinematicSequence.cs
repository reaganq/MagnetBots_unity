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
		GUIManager.Instance.EnterGUIState(UIState.cinematic);
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
		yield return new WaitForSeconds(2f);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Woah, you're finally here!");
		yield return new WaitForSeconds(2);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("I've been expecting you!");
		yield return new WaitForSeconds(2.5f);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("Welcome to the world of Magnetbots.");
		yield return new WaitForSeconds(3);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("We have so much to do, right this way!");
		yield return new WaitForSeconds(2.5f);
		GUIManager.Instance.nakedArmorGUI.HideSubtitle();
		yield return new WaitForSeconds(1.5f);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Now you'll be needing a companion");
		yield return new WaitForSeconds(1.5f);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("Let us create your first Magnetbot together.");
		yield return new WaitForSeconds(1.9f);
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

		TeslaAnimation.CrossFade("waitingLoop2");
		yield return new WaitForSeconds(0.1f);
		GUIManager.Instance.nakedArmorGUI.EnableSetNameUI();
	}

	public IEnumerator PhaseTwoSubtitles()
	{
		yield return new WaitForSeconds(0.7f);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Nicely done!");
		yield return new WaitForSeconds(1.2f);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("Now how about a name?");
		yield return new WaitForSeconds(1.8f);
		GUIManager.Instance.nakedArmorGUI.HideSubtitle();
	}

	public IEnumerator EnterPhaseThree()
	{
		TeslaAnimation.Play("outro");
		PlayerManager.Instance.RefreshAvatar();
		StartCoroutine(PhaseThreeSubtitles());
		yield return new WaitForSeconds(TeslaAnimation["outro"].length - 0.3f);

		GUIManager.Instance.FlashScreen(0.2f, Color.white);
		yield return new WaitForSeconds(0.8f);
		PlayerManager.Instance.ActiveZone = PlayerManager.Instance.ActiveWorld.DefaultZone;
		Debug.Log("destroy shit");
		PlayerManager.Instance.StartNewGame();
		//start blank game
		//talk to npc to get starting items
		Destroy(gameObject);
		//fade
	}

	public IEnumerator PhaseThreeSubtitles()
	{
		yield return new WaitForSeconds(0.5f);
		GUIManager.Instance.nakedArmorGUI.DisplaySubtitle("Excellent choice!");
		yield return new WaitForSeconds(1.2f);
		GUIManager.Instance.nakedArmorGUI.UpdateSubtitle("You are now ready to explore Magnet Central.");
		yield return new WaitForSeconds(1.8f);
		GUIManager.Instance.nakedArmorGUI.HideSubtitle();
	}

}
