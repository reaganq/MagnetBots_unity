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
    
	public InventoryGUIController InventoryGUI;
    public QuickInventoryGUIController QuickInventoryGUI;
    public MainUIManager MainGUI;
	public PartyGUIController PartyGUI;
    public PlayerShopGUIController PlayerShopGUI;
    public NPCGUIController NPCGUI;
    public IntroGUIController IntroGUI;
	public LoadScreenController loadingGUI;
	public RewardsGUIController rewardsGUI;
	public ChatGUIController chatGUI;
	public HoverPopupGUIController hoverPopupGUI;
	public MainSpeechBubble mainNPCSpeechBubble;

	public Transform minigameUIRoot;
    
	//npc stuff
	public RPGConversation activeConversation;
	public NPCActivity activeActivity;
    //public GameObject Joystick = null;
    
    //public GameObject ActionButtons = null;
    
	public UIState _uistate = UIState.idle;
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
			MainGUI.Enable();
			break;
		case UIState.npc:
			NPCGUI.Enable();
			break;
		case UIState.loading:
			break;
		case UIState.playerShop:
			PlayerShopGUI.Enable();
			break;
		case UIState.inventory:
			InventoryGUI.Enable();
			break;
		case UIState.quickInventory:
			break;
		}
		if(IsUIBusy())
			HideHoverPopup();
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
		case UIState.loading:
			break;
		case UIState.playerShop:
			PlayerShopGUI.Disable();
			break;
		case UIState.inventory:
			InventoryGUI.Disable();
			break;
		case UIState.quickInventory:
			QuickInventoryGUI.Disable();
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
		uiState = UIState.main;
    }
    
    public void HideMainGUI()
    {
    }

	public void DisplayInventory()
	{
		uiState = UIState.inventory;
	}

	public void HideInventory()
	{
		uiState = UIState.main;
	}
    
    public void DisplayQuickInventory (ItemCategories category)
    {
		uiState = UIState.quickInventory;
		QuickInventoryGUI.Enable(category);
    }
    
    public void HideQuickInventory ()
    {
		uiState = UIState.main;
    }

	public void DisplayNPC()
	{
		DisplayNPC(PlayerManager.Instance.ActiveNPC);
	}
    
    public void DisplayNPC(NPC npc)
    {
		PlayerManager.Instance.ActiveNPC = npc;
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
		PlayerManager.Instance.ActiveNPC = null;
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

	public void DisplayHoverPopup(CharacterStatus cs)
	{
		if(IsUIBusy())
			return;
		hoverPopupGUI.SelectPlayer(cs);
		Debug.Log("display hover");
	}

	public void HideHoverPopup()
	{
		if(hoverPopupGUI.isDisplayed)
			hoverPopupGUI.Disable();
	}
    
	public void DisplayLoadingScreen()
	{
		loadingGUI.gameObject.SetActive(true);
	}

	public void HideLoadingScreen()
	{
		loadingGUI.gameObject.SetActive(false);
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

	public void DisplayItemDetails()
	{
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
        if(uiState == UIState.main)
			return false;
        else
            return true;
    }

	public void DisplayNPCConversationhBubble(string text)
	{
		mainNPCSpeechBubble.DisplaySpeechBubble(text);
	}

	public void HideNPCConversationBubble()
	{
		mainNPCSpeechBubble.HideSpeechBubble();
	}
}

public enum UIState
{
	idle,
	main,
	playerShop,
	inventory,
	quickInventory,
	loading,
	login,
	npc,
	settings,
	friends,
	rewards
}
