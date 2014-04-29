using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour {
 
    private static GUIManager instance;
    
    private GUIManager() {}
    
    public static GUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(GUIManager)) as GUIManager;
                if (instance == null)
                {
                    GUIManager prefab = Resources.Load("Managers/_GUIManager", typeof(GUIManager)) as GUIManager;
                    instance = Instantiate(prefab) as GUIManager;
                }
            }
            return instance;
        }
    }
	
    //public static GUIManager Instance { get; private set; }

    public Camera mainCamera = null;
    
    public Transform UICameraRoot = null;
    
    public bool CanShowInventory = true;
    public bool CanShowShop = true;
    public bool CanShowNPC = true;

	public AnvilGUIController AnvilGUI = null;
    public ArmoryGUIController ArmoryGUI = null;
    public MainUIManager MainGUI = null;
	public PartyGUIController PartyGUI = null;
	public ArenaGUIController ArenaGUI = null;
    public ShopGUIController ShopGUI = null;
    public NPCGUIController NPCGUI = null;
    public IntroGUIController IntroGUI = null;
	public LoadScreenController loadingGUI = null;
	public MiniGameGUIController minigameGUI = null;
	public RewardsGUIController rewardsGUI = null;

	public Transform minigameUIRoot;
    
    //public GameObject Joystick = null;
    
    //public GameObject ActionButtons = null;
    
	private UIState _uistate = UIState.idle;
	public UIState cachedState = UIState.idle;
	public UIState uiState
	{
		get
		{
			return _uistate;
		}
		set
		{
			ExitState(_uistate);
			cachedState = _uistate;
			_uistate = value;
			EnterState(_uistate);
		}
	}
	
    public bool IsInventoryDisplayed = false;
    public bool IsShopDisplayed = false;
    public bool IsMainGUIDisplayed = false;
    public bool IsNPCGUIDisplayed = false;
    public bool IsIntroGUIDisplayed = false;
	public bool IsEnemiesListDisplayed = false;
    
    public Camera Inventory3DCamera = null;
    
    public void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        //Instance = this;
        
        DontDestroyOnLoad(this);
		//Debug.Log(_uistate);
        //UICameraRoot = GameObject.FindGameObjectWithTag("UICamera").transform;
        //mainCamera = Camera.main;
        //DisplayIntroGUI();
    }

    /*public void OnLevelWasLoaded(int level)
    {
        if(level == 0)
            CanShowInventory = true;
        else if (level == 1)
            CanShowInventory = false;
        
        //DisplayMainGUI();
        //DisplayIntroGUI();

    }*/
    
	public virtual void EnterState(UIState state)
	{
		switch(state)
		{
		case UIState.login:
			IntroGUI.Enable();
			//IntroGUI
			break;
		case UIState.main:
			Debug.Log("show main");
			MainGUI.Enable();
			break;
		case UIState.npc:
			Debug.Log("show npc");
			NPCGUI.Enable();
			break;
		case UIState.arena:
			ArenaGUI.Enable();
			break;
		case UIState.loading:
			break;
		case UIState.shop:
			ShopGUI.Enable();
			break;
		case UIState.bank:
			break;
		case UIState.inventory:
			break;
		case UIState.armory:
			ArmoryGUI.Enable();
			PlayerCamera.Instance.TransitionToInventory();
			break;
		case UIState.anvil:
			AnvilGUI.Enable();
			break;
		case UIState.minigame:
			minigameGUI.Enable();
			break;
		case UIState.rewards:
			rewardsGUI.Enable();
			break;
		}
	}
	
	public virtual void ExitState(UIState state)
	{
		switch(state)
		{
		case UIState.login:
			IntroGUI.Disable();
			break;
		case UIState.main:
			MainGUI.Disable();
			break;
		case UIState.npc:
			NPCGUI.Disable();
			break;
		case UIState.arena:
			ArenaGUI.Disable();
			break;
		case UIState.loading:
			break;
		case UIState.shop:
			ShopGUI.Disable();
			break;
		case UIState.bank:
			break;
		case UIState.inventory:
			break;
		case UIState.armory:
			ArmoryGUI.Disable();
			PlayerCamera.Instance.TransitionToDefault();
			break;
		case UIState.anvil:
			AnvilGUI.Disable();
			PlayerCamera.Instance.TransitionToDefault();
			break;
		case UIState.minigame:
			minigameGUI.Disable();
			break;
		case UIState.rewards:
			rewardsGUI.Disable();
			break;
		}
	}

    public void DisplayIntroGUI()
    {
		uiState = UIState.login;
    }

    public void HideIntroGUI()
    {
        if(IsIntroGUIDisplayed)
        {
            //IntroGUI.SetActive(false);
            IsIntroGUIDisplayed = false;
        }
    }
	
    public void StartGame()
    {
		uiState = UIState.main;
        //DisplayMainGUI();
    }
    
    public void DisplayMainGUI()
    {
		Debug.Log("hmmmm");
		uiState = UIState.main;
        //if(!IsMainGUIDisplayed)
        //{
			//Debug.Log(" display main gui");
            //MainGUI.gameObject.SetActive(true);
			//MainGUI.Enable();
            //IsMainGUIDisplayed = true;
        //}
    }
    
    public void HideMainGUI()
    {
        /*if(IsMainGUIDisplayed)
        {
            if(GameManager.Instance.inputType == InputType.TouchInput)
            {
                //Joystick.SetActive(false);
                //GameManager.Instance.joystick.resetFingerExit();
                GameManager.Instance.joystick.enable = false;
            }
            MainGUI.gameObject.SetActive(false);
            IsMainGUIDisplayed = false;
			Debug.Log("hide main gui");
            //Debug.Log("hide main");
        }*/
    }
    
    public void DisplayInventory ()
    {
		uiState = UIState.armory;
        //if(!IsInventoryDisplayed && CanShowInventory)
        //{
            //Inventory3DCamera = GameObject.FindGameObjectWithTag("UI3DCamera").GetComponent<Camera>();
            //HideMainGUI();
            //TurnOffAllOtherUI();
            //ArmoryGUI.SetActive(true);
            //IsInventoryDisplayed = true;
           // Inventory3DCamera.gameObject.SetActive(true);
            //Inventory3DCamera.enabled = true;
            //mainCamera.enabled = false;
            //ArmoryGUI.SendMessage("Enable");
        //}
    }
    
    public void HideInventory ()
    {
		uiState = UIState.main;
        /*if(IsInventoryDisplayed)
        {
            ArmoryGUI.SetActive(false);
            IsInventoryDisplayed = false;
            //Debug.Log("hiding inventory");
            DisplayMainGUI();
            //mainCamera.enabled = true;
           // Inventory3DCamera.gameObject.SetActive(false);
           // Inventory3DCamera.enabled = false;
            
        }*/
    }
    
    public void DisplayNPC()
    {
		uiState = UIState.npc;
        /*if(!IsNPCGUIDisplayed)
        {
            HideMainGUI();
            TurnOffAllOtherUI();
            //NPCGUI.SetActive(true);

            IsNPCGUIDisplayed = true;
        }*/
    }
    
    public void HideNPC()
    {
		uiState = UIState.main;
        /*if(IsNPCGUIDisplayed)
        {
            NPCGUI.SetActive(false);
            IsNPCGUIDisplayed = false;
			Debug.Log(" hide npc ui");
            //DisplayMainGUI();
        }*/
    }

	public void DisplayPartyNotification(PhotonPlayer player)
	{
		PartyGUI.DisplayNotificationBox(player);
	}

	public void HidePartyNotification()
	{
		PartyGUI.HideNotificationBox();
	}

	public void DisplayCharacterPopUp(GameObject character)
	{
		PartyGUI.DisplayHoverBox(character);
	}

	public void HideCharacterPopUp()
	{
		PartyGUI.HideHoverBox();
	}

	public void DisplayArenaUI()
	{
		//HideNPC();
		/*ArenaGUI.Enable();
		IsEnemiesListDisplayed = true;
		HideMainGUI();*/
		uiState = UIState.arena;
	}

	public void HideArenaUI()
	{
		ArenaGUI.Disable();
		IsEnemiesListDisplayed = false;
		DisplayNPC();
	}
    
    public void DisplayShop ()
    {
        /*if(!IsShopDisplayed && CanShowShop)
        {
            HideMainGUI();
            TurnOffAllOtherUI();
            ShopGUI.SetActive(true);
            //mainCamera.enabled = false;
            IsShopDisplayed = true;
            ShopGUI.SendMessage("Enable");
        }*/
		uiState = UIState.shop;
    }
    
    public void HideShop()
    {
        /*if(IsShopDisplayed)
        {
            //mainCamera.enabled = true;
            ShopGUI.SetActive(false);
            IsShopDisplayed = false;
            DisplayMainGUI();
        }*/
		uiState = UIState.npc;
    }

	public void DisplayAnvil(Anvil anvil)
	{
		uiState = UIState.anvil;
		AnvilGUI.anvil = anvil;
	}

	public void HideAnvil()
	{
		uiState = cachedState;
	}

	public void DisplayLoadingScreen()
	{
		loadingGUI.gameObject.SetActive(true);
	}

	public void HideLoadingScreen()
	{
		loadingGUI.gameObject.SetActive(false);
	}

	public void DisplayMinigame()
	{
		uiState = UIState.minigame;
	}

	public void HideMinigame()
	{
		uiState = UIState.npc;
	}

	public void DisplayRewards(List<InventoryItem> items)
	{
		rewardsGUI.items = items;
		uiState = UIState.rewards;

	}

	public void HideRewards()
	{
		uiState = cachedState;
	}
	
    public void HideAllUI()
    {
		uiState = UIState.idle;
        /*if(IsInventoryDisplayed)
        {
            ArmoryGUI.SetActive(false);
            //mainCamera.enabled = true;
            //Inventory3DCamera.gameObject.SetActive(false);
            //Inventory3DCamera.enabled = false;
            IsInventoryDisplayed = false;
        }
        if(IsShopDisplayed)
        {
            //mainCamera.enabled = true;
            ShopGUI.SetActive(false);
            IsShopDisplayed = false;
        }
        if(IsNPCGUIDisplayed)
        {
            NPCGUI.SetActive(false);
            IsNPCGUIDisplayed = false;
        }
		if(IsEnemiesListDisplayed)
		{
			ArenaGUI.Disable();
			IsEnemiesListDisplayed = false;
		}*/
    }
    
    public bool IsUIBusy()
    {
        if(uiState == UIState.main || uiState == UIState.rewards)
			return false;
        else
            return true;
    }
}

public enum UIState
{
	idle,
	anvil,
	main,
	shop,
	inventory,
	armory,
	equipment,
	minigame,
	loading,
	login,
	bank,
	upgrading,
	npc,
	arena,
	settings,
	friends,
	rewards
}
