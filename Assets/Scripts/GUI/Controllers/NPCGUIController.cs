using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCGUIController : BasicGUIController {
 
    public UILabel textLabel = null;
    public GameObject confirmButton = null;
	public List<NPCActivityButton> activityButtons;
	public GameObject activityButtonPrefab;

	public GameObject activityButtonsParent;
	public float offset;
	public Transform cameraOvertakeTransform;
	public List<NPCActivity> activities;

	public NPCActivity activeActivity;
	public ShopGUIController shopGUI;
	public ArenaGUIController arenaGUI;
	public NPCGUIState _state;
	public NPCGUIState state
	{
		get{
			return _state;
		}
		set{
			cachedState = _state;
			ExitState(_state);
			_state = value;
			EnterState(_state);
		}
	}
	public NPCGUIState cachedState;

	public void EnterState(NPCGUIState newState)
	{
		switch(newState)
		{
		case NPCGUIState.shop:

			break;
		case NPCGUIState.activityButtons:
			DisplayActivityButtons();
			break;
		}
	}

	public void ExitState(NPCGUIState oldState)
	{
		switch(oldState)
		{
		case NPCGUIState.nothing:
			break;
		case NPCGUIState.activityButtons:
			HideActivityButtons();
			break;
		case NPCGUIState.shop:
			shopGUI.Disable();
			break;
		}
	}

	void Start()
	{
		_state = NPCGUIState.nothing;
	}
		
	// Use this for initialization
	// Update is called once per frame
	public override void Enable()
    {
		activities = PlayerManager.Instance.ActiveNPC.availableActivities;
		if(PlayerManager.Instance.ActiveNPC.character.defaultConversationID <= 0)
			state = NPCGUIState.activityButtons;
		base.Enable();
    }

	public override void Disable()
	{
		state= NPCGUIState.nothing;
		//base.Disable();
		GUIManager.Instance.HideNPCConversationBubble();
		base.Disable();
	}

	public void DisplayActivityButtons()
	{
		textLabel.gameObject.SetActive(true);
		textLabel.text = PlayerManager.Instance.ActiveNPC.character.Name;

		//check npc for override conversations
		int num = activities.Count - activityButtons.Count;
		if(num >0)
		{
			for (int i = 0; i < num; i++) {
				GameObject activitybutton = NGUITools.AddChild(activityButtonsParent, activityButtonPrefab);
				NPCActivityButton nab = activitybutton.GetComponent<NPCActivityButton>();
				activityButtons.Add(nab);
			}
		}
		for (int i = 0; i < activityButtons.Count; i++) {
			if(i >= PlayerManager.Instance.ActiveNPC.activities.Count)
				activityButtons[i].gameObject.SetActive(false);
			else
			{
				activityButtons[i].gameObject.SetActive(true);
				activityButtons[i].LoadActivityButton(activities[i], i);
				activityButtons[i].transform.localPosition = new Vector3(i*offset, -50, 0);
			}
		}
		confirmButton.SetActive(true);
		confirmButton.transform.localPosition = new Vector3(activities.Count*offset, -50, 0);
		//numberOfButtons +=1;
		if(activities.Count>0)
			activityButtonsParent.transform.localPosition = new Vector3(((activities.Count)*-0.5f*offset), 363, 0);
		else
			activityButtonsParent.transform.localPosition = new Vector3(0, 363, 0);
		activityButtonsParent.SetActive(true);
	}

	public void HideActivityButtons()
	{
		textLabel.gameObject.SetActive(false);
		activityButtonsParent.SetActive(false);
	}
    
    public void OnConfirmButtonPressed()
    {
        GUIManager.Instance.HideNPC();
		//GUIManager.Instance.DisplayMainGUI();
    }

	public void OnActivityButtonPressed(int index)
	{
		Debug.Log("activity pressed" + index);
		Debug.Log(activities[index].activityType.ToString() + activities[index].Name);
		activeActivity = activities[index];
		switch(activeActivity.activityType)
		{
		case NPCActivityType.Shop:
			DisplayShop((Shop)activeActivity);
			break;
		case NPCActivityType.Arena:
			DisplayArena((NPCArena)activeActivity);
			break;
		}
		//load activity's conversation
	}
    
    /*public void OnEnterShopButton()
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
		}
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
	}*/

	public void DisplayArena(NPCArena newArena)
	{
		state = NPCGUIState.arena;
		arenaGUI.Enable(newArena);
	}

    public void SetupArenaButton()
    {
		//Debug.Log(PlayerManager.Instance.ActiveNPC.arena.Name);
        //arenaLabel.text = PlayerManager.Instance.ActiveNPC.arena.Name;
    }

	public void SetupMiniGameButton()
	{
		//minigameLabel.text = PlayerManager.Instance.ActiveNPC.miniGame.Name;
	}

	public void SetupActivityButton()
	{
		//activityLabel.text = PlayerManager.Instance.ActiveNPC.activity.Name;
	}

	public void DisplayShop(Shop newShop)
	{
		state = NPCGUIState.shop;
		shopGUI.Enable(newShop);
	}

	public void HideShop()
	{
		state = cachedState;
	}
	
}

public enum NPCGUIState
{
	nothing,
	activityButtons,
	shop,
	quest,
	arena,
	bank,
}
