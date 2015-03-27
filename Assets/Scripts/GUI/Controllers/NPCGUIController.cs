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
	public ConversationGUIController conversationGUI;
	public MiniGameGUIController minigameGUI;
	public ShopGUIController shopGUI;
	public ArenaGUIController arenaGUI;
	public CombobulatorGUIController combobulatorGUI;
	public BankGUIController bankGUI;
	public TeleporterGUIController teleporterGUI;
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
			StartCoroutine( DisplayActivityButtons());
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
		case NPCGUIState.arena:
			arenaGUI.Disable();
			break;
		case NPCGUIState.teleporter:
			teleporterGUI.Disable();
			break;
		case NPCGUIState.minigame:
			minigameGUI.Disable();
            break;
		case NPCGUIState.bank:
			bankGUI.Disable();
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
		base.Enable();
		//check for check override
		//Debug.Log("active npc id: " + PlayerManager.Instance.ActiveNPC.ID);
		activities = PlayerManager.Instance.ActiveNPC.availableActivities;
		if(DisplayQuest(PlayerManager.Instance.Hero.questLog.CheckNPCConversation(PlayerManager.Instance.ActiveNPC.ID)))
			return;


		if(PlayerManager.Instance.ActiveNPC.character.defaultConversationID <= 0)
			state = NPCGUIState.activityButtons;

    }

	public void CheckForQuestConversationOverrides()
	{
	}

	public override void Disable()
	{
		state= NPCGUIState.nothing;
		PlayerCamera.Instance.TransitionToDefault();
		//base.Disable();
		//GUIManager.Instance.HideNPCConversationBubble();
		base.Disable();
	}

	public IEnumerator DisplayActivityButtons()
	{
		textLabel.alpha = 0;
		textLabel.gameObject.SetActive(true);
		textLabel.text = PlayerManager.Instance.ActiveNPC.character.Name;
		TweenAlpha.Begin(textLabel.gameObject, 0.3f, 1.0f);
		if(activities.Count>0)
			activityButtonsParent.transform.localPosition = new Vector3(((activities.Count)*-0.5f*offset), 363, 0);
		else
			activityButtonsParent.transform.localPosition = new Vector3(0, 363, 0);
		activityButtonsParent.SetActive(true);
		confirmButton.SetActive(false);
		//check npc for override conversations
		int num = activities.Count - activityButtons.Count;
		if(num >0)
		{
			for (int i = 0; i < num; i++) {
				GameObject activitybutton = NGUITools.AddChild(activityButtonsParent, activityButtonPrefab);
				NPCActivityButton nab = activitybutton.GetComponent<NPCActivityButton>();
				activityButtons.Add(nab);
				//activitybutton.SetActive(false);
			}
		}
		for (int i = 0; i < activityButtons.Count; i++) {
			activityButtons[i].gameObject.SetActive(false);
				}

		for (int i = 0; i < activityButtons.Count; i++) {
			//if()
			//	activityButtons[i].gameObject.SetActive(false);
			if(i < PlayerManager.Instance.ActiveNPC.activities.Count)
			{
				activityButtons[i].gameObject.SetActive(true);
				activityButtons[i].LoadActivityButton(activities[i], i);
				activityButtons[i].transform.localPosition = new Vector3(i*offset, -70, 0);
				TweenPosition.Begin(activityButtons[i].gameObject, 0.1f, new Vector3(i*offset, -50, 0));
				TweenAlpha tweenA = TweenAlpha.Begin(activityButtons[i].gameObject, 0.1f, 1.0f);
				tweenA.from = 0f;
				yield return new WaitForSeconds(0.1f);
			}
		}
		confirmButton.SetActive(true);
		confirmButton.transform.localPosition = new Vector3(activities.Count*offset, -70, 0);
		TweenPosition.Begin(confirmButton, 0.1f, new Vector3(activities.Count*offset, -50, 0));
		TweenAlpha tween = TweenAlpha.Begin(confirmButton, 0.1f, 1.0f);
		tween.from = 0f;
		//numberOfButtons +=1;
		yield return null;


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
		activeActivity = activities[index];
		PlayerManager.Instance.activeActivity = activeActivity;
		switch(activeActivity.activityType)
		{
		case NPCActivityType.Shop:
			DisplayShop((Shop)activeActivity);
			break;
		case NPCActivityType.Arena:
			DisplayArena((NPCArena)activeActivity);
			break;
		case NPCActivityType.Quest:
			DisplayQuest((NPCQuest)activeActivity);
			break;
		case NPCActivityType.Construction:
			DisplayConstruction((RPGConstruction)activeActivity);
			break;

		case NPCActivityType.Minigame:
			DisplayMinigame(PlayerManager.Instance.ActiveWorld.MinigameByID(activeActivity.ID));
			break;
		case NPCActivityType.Service:
			if(activeActivity.ID == 1)
			{
				DisplayBank();
			}
			if(activeActivity.ID == 2)
			{
				DisplayCombobulator();
			}
			if(activeActivity.ID == 3)
			{
				DisplayTeleporter();
			}
			break;
		}
		/*if(activeActivity.conversation != null)
			conversationGUI.DisplayConversation(activeActivity.conversation);*/
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

	public void DisplayBank()
	{
		bankGUI.Enable();
		state = NPCGUIState.bank;
	}

	public void DisplayCombobulator()
	{
		combobulatorGUI.Enable();
		state = NPCGUIState.combobulator;
	}

	public void DisplayTeleporter()
	{
		state = NPCGUIState.teleporter;
		teleporterGUI.Enable();
	}

	public void DisplayConstruction(RPGConstruction newConstruction)
	{
		state = NPCGUIState.construction;
	}

	public void DisplayQuest(NPCQuest newQuest)
	{
		if(newQuest != null)
		{
			state = NPCGUIState.quest;
			PlayerManager.Instance.Hero.questLog.selectedQuest = newQuest.quest;
			conversationGUI.DisplayConversation(newQuest.conversation);
		}
	}

	public bool DisplayQuest(RPGQuest newQuest)
	{
		if(newQuest != null)
		{
			state = NPCGUIState.quest;
			PlayerManager.Instance.Hero.questLog.selectedQuest = newQuest;
			RPGConversation convo = Storage.LoadById<RPGConversation>(newQuest.CurrentStep.overrideNPCConversationID, new RPGConversation());
			return conversationGUI.DisplayConversation(convo);
        }
		return false;
    }

	public void DisplayMinigame(NPCMinigame newMinigame)
	{
		state = NPCGUIState.minigame;
		minigameGUI.Enable(newMinigame);
	}

	public void DisplayShop(Shop newShop)
	{
		state = NPCGUIState.shop;
		shopGUI.Enable(newShop);
	}

	public void HideShop()
	{
		if(cachedState == NPCGUIState.activityButtons)
			state = cachedState;
		else
			GUIManager.Instance.HideNPC();
	}
	
}

public enum NPCGUIState
{
	nothing,
	activityButtons,
	combobulator,
	shop,
	quest,
	arena,
	bank,
	minigame,
	teleporter,
	construction,
}
