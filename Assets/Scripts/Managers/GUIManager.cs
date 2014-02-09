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
                    GUIManager prefab = Resources.Load("Managers/GUIManager", typeof(GUIManager)) as GUIManager;
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

    public GameObject ArmoryGUI = null;
    public GameObject MainGUI = null;
    public GameObject ShopGUI = null;
    public GameObject NPCGUI = null;
    public GameObject IntroGUI = null;
    
    //public GameObject Joystick = null;
    public GameObject OpenInventoryButton = null;
    public GameObject ActionButtons = null;
    public UILabel MagnetsCounter = null;
    
    public bool IsInventoryDisplayed = false;
    public bool IsShopDisplayed = false;
    public bool IsMainGUIDisplayed = false;
    public bool IsNPCGUIDisplayed = false;
    public bool IsIntroGUIDisplayed = false;
    
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
    
    public void UpdateMagnetsCount()
    {
        MagnetsCounter.text = PlayerManager.Instance.Hero.Magnets.ToString();
    }

    public void DisplayIntroGUI()
    {
        if(!IsIntroGUIDisplayed)
        {
            IntroGUI.SetActive(true);
            IsIntroGUIDisplayed = true;
            //Debug.LogWarning("intro displayed");
        }
    }

    public void HideIntroGUI()
    {
        if(IsIntroGUIDisplayed)
        {
            IntroGUI.SetActive(false);
            IsIntroGUIDisplayed = false;
        }
    }

    public void StartGame()
    {
        HideIntroGUI();
        DisplayMainGUI();
    }
    
    public void DisplayMainGUI()
    {
        if(!IsMainGUIDisplayed)
        {
            MainGUI.SetActive(true);
            if(GameManager.Instance.inputType == InputType.TouchInput)
            {
                GameManager.Instance.joystick.enable = true;
                //Joystick.SetActive(true);
            }
            else
                ActionButtons.SetActive(false);
            if(!CanShowInventory)
                OpenInventoryButton.SetActive(false);
            IsMainGUIDisplayed = true;
        }
    }
    
    public void HideMainGUI()
    {
        if(IsMainGUIDisplayed)
        {
            if(GameManager.Instance.inputType == InputType.TouchInput)
            {
                //Joystick.SetActive(false);
                //GameManager.Instance.joystick.resetFingerExit();
                GameManager.Instance.joystick.enable = false;
            }
            MainGUI.SetActive(false);
            IsMainGUIDisplayed = false;
            //Debug.Log("hide main");
        }
    }
    
    public void DisplayInventory ()
    {
        if(!IsInventoryDisplayed && CanShowInventory)
        {
            //Inventory3DCamera = GameObject.FindGameObjectWithTag("UI3DCamera").GetComponent<Camera>();
            HideMainGUI();
            TurnOffAllOtherUI();
            ArmoryGUI.SetActive(true);
            IsInventoryDisplayed = true;
           // Inventory3DCamera.gameObject.SetActive(true);
            //Inventory3DCamera.enabled = true;
            //mainCamera.enabled = false;
            ArmoryGUI.SendMessage("Enable");
        }
    }
    
    public void HideInventory ()
    {
        if(IsInventoryDisplayed)
        {
            ArmoryGUI.SetActive(false);
            IsInventoryDisplayed = false;
            //Debug.Log("hiding inventory");
            DisplayMainGUI();
            //mainCamera.enabled = true;
           // Inventory3DCamera.gameObject.SetActive(false);
           // Inventory3DCamera.enabled = false;
            
        }
    }
    
    public void DisplayNPC()
    {
        if(!IsNPCGUIDisplayed)
        {
            HideMainGUI();
            TurnOffAllOtherUI();
            NPCGUI.SetActive(true);
            NPCGUI.SendMessage("Enable");
            IsNPCGUIDisplayed = true;
        }
    }
    
    public void HideNPC()
    {
        if(IsNPCGUIDisplayed)
        {
            NPCGUI.SetActive(false);
            IsNPCGUIDisplayed = false;
            DisplayMainGUI();
        }
    }
    
    public void DisplayShop ()
    {
        if(!IsShopDisplayed && CanShowShop)
        {
            HideMainGUI();
            TurnOffAllOtherUI();
            //ShopGUI.SetActive(true);
            mainCamera.enabled = false;
            IsShopDisplayed = true;
            ShopGUI.SendMessage("Enable");
        }
    }
    
    public void HideShop()
    {
        if(IsShopDisplayed)
        {
            //mainCamera.enabled = true;
            ShopGUI.SetActive(false);
            IsShopDisplayed = false;
            DisplayMainGUI();
        }
    }
    
    public void TurnOffAllOtherUI()
    {
        if(IsInventoryDisplayed)
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
    }
    
    public bool IsUIBusy()
    {
        if(IsInventoryDisplayed)
            return true;
        else if(IsShopDisplayed)
            return true;
        else if(IsNPCGUIDisplayed)
            return true;
        else
            return false;
    }
}
