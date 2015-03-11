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
	public RarityColor[] rarityColors;

    public Camera mainCamera = null;
	public Camera uiCamera = null;
    public Transform UICameraRoot = null;
    
	public UIPlayTween screenFlashTween;
	public UISprite screenFlash;
	public InventoryGUIController InventoryGUI;
    public QuickInventoryGUIController QuickInventoryGUI;
    public MainUIManager MainGUI;
	public NotificationGUIController NotificationGUI;
    public PlayerShopGUIController PlayerShopGUI;
    public NPCGUIController NPCGUI;
    public IntroGUIController IntroGUI;
	public LoadScreenController loadingGUI;
	public RewardsGUIController rewardsGUI;
	public ChatGUIController chatGUI;
	public HoverPopupGUIController hoverPopupGUI;
	public ItemInfoBoxGUIController itemInfoGUI;
	public ProfileGUIController profileGUI;
	public ConversationGUIController conversationGUI;
	public OpeningCinematicGUIController nakedArmorGUI;
	public Transform dragDropRoot;

	public Transform minigameUIRoot;
	public List<BasicGUIController> allUIs = new List<BasicGUIController>();
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
		allUIs.Clear();
		BasicGUIController[] guis = GetComponentsInChildren<BasicGUIController>();
		for (int i = 0; i < guis.Length; i++) {
			allUIs.Add(guis[i]);
				}

		allUIs.Remove(MainGUI);
		allUIs.Remove(chatGUI);
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
			chatGUI.Enable();
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
		case UIState.profile:
			profileGUI.Enable();
			break;
		case UIState.battle:
			MainGUI.Enable();
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
			chatGUI.Disable();
			break;
		case UIState.npc:
			NPCGUI.Disable();
			break;
		case UIState.loading:
			break;
		case UIState.profile:
			profileGUI.Disable();
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

	public Color GetRarityColor(RarityType rarity)
	{
		for (int i = 0; i < rarityColors.Length; i++) {
			if(rarityColors[i].rarity == rarity)
				return rarityColors[i].tintColor;
				}
		return Color.white;
	}

	public void EnterGUIState(UIState state)
	{
		uiState = state;
	}

    public void StartGame()
    {
		EnterGUIState(UIState.main);
        //DisplayMainGUI();
    }
    
    public void DisplayMainGUI()
    {
		EnterGUIState(UIState.main);
    }

	public void DisplayInventory()
	{
		EnterGUIState(UIState.inventory);
	}

	public void HideInventory()
	{
		EnterGUIState(UIState.main);
	}

	public void DisplayQuest()
	{
		EnterGUIState(UIState.quest);
	}

	public void HideQuest()
	{
		EnterGUIState(UIState.main);
	}
    
    public void DisplayQuickInventory (ItemCategories category)
    {
		EnterGUIState(UIState.quickInventory);
		QuickInventoryGUI.Enable(category);
    }
    
    public void HideQuickInventory ()
    {
		EnterGUIState(UIState.main);
    }

	public void DisplayNPC()
	{
		DisplayNPC(PlayerManager.Instance.ActiveNPC);
	}
    
    public void DisplayNPC(NPC npc)
    {
		PlayerManager.Instance.ActiveNPC = npc;
		EnterGUIState(UIState.npc);
        /*if(!IsNPCGUIDisplayed)
        {
            HideMainGUI();
            TurnOffAllOtherUI();
            //NPCGUI.SetActive(true);

            IsNPCGUIDisplayed = true;
        }*/
    }

	public void DisplayProfile(PhotonPlayer player)
	{
		EnterGUIState(UIState.profile);
		PlayerManager.Instance.ActiveWorld.RequestPlayerProfileData(player);
	}

	public void HideProfile()
	{
		EnterGUIState(UIState.main);
	}
    
    public void HideNPC()
    {
		PlayerManager.Instance.ActiveNPC = null;
		EnterGUIState(UIState.main);
        /*if(IsNPCGUIDisplayed)
        {
            NPCGUI.SetActive(false);
            IsNPCGUIDisplayed = false;
			Debug.Log(" hide npc ui");
            //DisplayMainGUI();
        }*/
    }

	public void DisplayPartyNotification(PhotonPlayer player, int partyleaderID)
	{
		NotificationGUI.DisplayNotificationBox(player, partyleaderID);
	}

	public void HidePartyNotification()
	{
		NotificationGUI.HideNotificationBox();
	}

	/*public void DisplayCharacterPopUp(GameObject character)
	{
		NotificationGUI.DisplayHoverBox(character);
	}

	public void HideCharacterPopUp()
	{
		NotificationGUI.HideHoverBox();
	}*/

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

	public void DisplayRewards(LootItemList items)
	{
		rewardsGUI.Enable(items);
		//rewardsGUI.items = items;
		//uiState = UIState.rewards;

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

	public void DisplayItemDetails(InventoryItem item, InventoryGUIType type, BasicGUIController gui)
	{
		itemInfoGUI.DisplayItemDetails(item, type, gui);
	}

	public void HideItemDetails()
	{
		itemInfoGUI.Disable();
	}
}

public enum UIState
{
	idle,
	main,
	playerShop,
	inventory,
	quickInventory,
	quest,
	profile,
	loading,
	login,
	npc,
	settings,
	friends,
	cinematic,
	battle
}

[System.Serializable]
public class RarityColor
{
	public RarityType rarity;
	public Color tintColor;

}
