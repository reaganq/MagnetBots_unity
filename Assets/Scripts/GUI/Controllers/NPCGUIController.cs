using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCGUIController : BasicGUIController {
 
    public UILabel textLabel = null;
    public GameObject enterShopButton = null;
    public GameObject confirmButton = null;
    public GameObject arenaButton = null;
	public GameObject activityButton = null;
	public GameObject minigameButton = null;
    public UILabel arenaLabel = null;
	public UILabel minigameLabel = null;
	public UILabel activityLabel = null;

	public GameObject root;
	public GameObject panel;
	public float offset;
	public Transform cameraOvertakeTransform;

		
	// Use this for initialization
	// Update is called once per frame
	public override void Enable()
    {
		panel.SetActive(true);
		//textLabel.gameObject.SetActive(true);
        textLabel.text = GUIManager.Instance.activeNPC.character.Name;

		int numberOfButtons = 0;
        if(PlayerManager.Instance.ActiveNPC.thisShop != null)
        {
            enterShopButton.SetActive(true);
			enterShopButton.transform.localPosition = new Vector3(numberOfButtons*offset, -50, 0);
			numberOfButtons += 1;
			Debug.Log("shop here");
        }
        else
        {
            enterShopButton.SetActive(false);
        }
		if(PlayerManager.Instance.ActiveNPC.arena != null )
        {
            SetupArenaButton();
            arenaButton.SetActive(true);
			arenaButton.transform.localPosition = new Vector3(numberOfButtons*offset, -50, 0);
			numberOfButtons += 1;
			Debug.Log("arena here");
        }
        else
        {
            arenaButton.SetActive(false);
        }
		if(PlayerManager.Instance.ActiveNPC.activity.ID > 0 )
		{
			SetupActivityButton();
			activityButton.SetActive(true);
			activityButton.transform.localPosition = new Vector3(numberOfButtons*offset, -50, 0);
			numberOfButtons += 1;
			Debug.Log("activity here");
		}
		else
		{
			activityButton.SetActive(false);
		}
		if(PlayerManager.Instance.ActiveNPC.miniGame.ID > 0)
		{
			SetupMiniGameButton();
			minigameButton.SetActive(true);
			minigameButton.transform.localPosition = new Vector3(numberOfButtons*offset, -50, 0);
			numberOfButtons += 1;
			Debug.Log("minigame here");
		}
		else
		{
			minigameButton.SetActive(false);
		}


        confirmButton.SetActive(true);
		confirmButton.transform.localPosition = new Vector3(numberOfButtons*offset, -50, 0);
		numberOfButtons +=1;
		root.transform.localPosition = new Vector3(((numberOfButtons-1)*-0.5f*offset), 363, 0);

		Debug.Log("enable npc gui");
    }
    
    public void OnConfirmButtonPressed()
    {
        GUIManager.Instance.HideNPC();
		//GUIManager.Instance.DisplayMainGUI();
    }
    
    public void OnEnterShopButton()
    {
		PlayerManager.Instance.ActiveShop = PlayerManager.Instance.ActiveNPC.thisShop;
        GUIManager.Instance.DisplayShop();
    }

    public void OnEnterArenaButton()
    {
		PlayerManager.Instance.SelectedArena = PlayerManager.Instance.ActiveNPC.arena;
		GUIManager.Instance.DisplayArenaUI();
        //Application.LoadLevel(PlayerManager.Instance.ActiveNPC.character.LevelName);
		//PlayerManager.Instance.GoToArena(PlayerManager.Instance.ActiveWorld.GetAvailableArena("Gym"));
		//GUIManager.Instance.HideNPC();
		//GUIManager.Instance.DisplayMainGUI();
		/*if(PlayerManager.Instance.ActiveArena != null)
		{
			Debug.Log("we have an arena");
			PlayerManager.Instance.ActiveArena.gameObject.GetComponent<PhotonView>().RPC("Initialise", PhotonTargets.MasterClient, "Jim");
		}*/
    }

	public void OnEnterActivityButton()
	{
		if(PlayerManager.Instance.ActiveNPC.activity.ID == 2)
		{
			PlayerManager.Instance.ActiveActivity = PlayerManager.Instance.ActiveNPC.activity;
			GUIManager.Instance.DisplayAnvil(PlayerManager.Instance.ActiveNPC.GetComponent<Anvil>());
			PlayerCamera.Instance.TransitionTo(PlayerManager.Instance.ActiveNPC.targetCameraPos, PlayerManager.Instance.ActiveNPC.targetCameraFOV, 1);
			//PlayerCamera.Instance.targetTransform = cameraOvertakeTransform;
		}
	}

	public void OnEnterMiniGameButton()
	{
		PlayerManager.Instance.ActiveMinigame = PlayerManager.Instance.ActiveNPC.miniGame;
		GUIManager.Instance.DisplayMinigame();
	}

    public void SetupArenaButton()
    {
		Debug.Log(PlayerManager.Instance.ActiveNPC.arena.Name);
        arenaLabel.text = PlayerManager.Instance.ActiveNPC.arena.Name;
    }

	public void SetupMiniGameButton()
	{
		minigameLabel.text = PlayerManager.Instance.ActiveNPC.miniGame.Name;
	}

	public void SetupActivityButton()
	{
		activityLabel.text = PlayerManager.Instance.ActiveNPC.activity.Name;
	}

	public override void Disable()
	{
		//textLabel.gameObject.SetActive(false);
		panel.SetActive(false);
	}
}
