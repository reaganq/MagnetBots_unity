using UnityEngine;
using System.Collections;

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

    public ArmoryGUIController ArmoryGUI = null;
    public MainUIManager MainGUI = null;
	public PartyGUIController PartyGUI = null;
	public ArenaGUIController ArenaGUI = null;
    public ShopGUIController ShopGUI = null;
    public NPCGUIController NPCGUI = null;
    public IntroGUIController IntroGUI = null;
	public LoadScreenController loadingScreen = null;
    
    //public GameObject Joystick = null;
    
    //public GameObject ActionButtons = null;
    
	private UIState _uistate;
	public UIState uiState
	{
		get
		{
			return _uistate;
		}
		set
		{
			ExitState(_uistate);
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
			//IntroGUI
			break;
		case UIState.main:
			MainGUI.Enable();
			break;
		case UIState.npc:
			NPCGUI.Enable();
			break;
		case UIState.arena:
			break;
		case UIState.loading:
			break;
		case UIState.shop:
			break;
		case UIState.bank:
			break;
		case UIState.inventory:
			break;
		case UIState.armory:
			ArmoryGUI.Enable();
			break;
		}
	}
	
	public virtual void ExitState(UIState state)
	{
		switch(state)
		{
		case UIState.login:
			break;
		case UIState.main:
			MainGUI.Disable();
			break;
		case UIState.npc:
			NPCGUI.Disable();
			break;
		case UIState.arena:
			break;
		case UIState.loading:
			break;
		case UIState.shop:
			break;
		case UIState.bank:
			break;
		case UIState.inventory:
			break;
		case UIState.armory:
			ArmoryGUI.Disable();
			break;
		}
	}

    public void DisplayIntroGUI()
    {
        if(!IsIntroGUIDisplayed)
        {
            //IntroGUI.SetActive(true);
            IsIntroGUIDisplayed = true;
            //Debug.LogWarning("intro displayed");
        }
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
        HideIntroGUI();
		uiState = UIState.main;
        //DisplayMainGUI();
    }
    
    public void DisplayMainGUI()
    {
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

	public void DisplayEnemieslist()
	{
		//HideNPC();
		ArenaGUI.Enable();
		IsEnemiesListDisplayed = true;
		HideMainGUI();
	}

	public void HideEnemiesList()
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

	public void DisplayLoadingScreen()
	{
		loadingScreen.gameObject.SetActive(true);
	}

	public void HideLoadingScreen()
	{
		loadingScreen.gameObject.SetActive(false);
	}
    
    public void TurnOffAllOtherUI()
    {
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
        if(IsInventoryDisplayed)
            return true;
        else if(IsShopDisplayed)
            return true;
        else if(IsNPCGUIDisplayed)
            return true;
		else if(IsEnemiesListDisplayed)
			return true;
        else
            return false;
    }
}

public enum UIState
{
	main,
	shop,
	inventory,
	armory,
	equipment,
	loading,
	login,
	bank,
	upgrading,
	npc,
	arena,
	settings,
	friends
}
