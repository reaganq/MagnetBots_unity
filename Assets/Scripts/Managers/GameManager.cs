using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
 
    private static GameManager instance = null;
    
    private GameManager() {}
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                if (instance == null)
                {
                    GameManager prefab = Resources.Load("Managers/GameManager", typeof(GameManager)) as GameManager;
                    instance = Instantiate(prefab) as GameManager;
                }
            }
            return instance;
        }
    }

    //public static GameManager Instance { get; private set; }

    public InputType inputType;
    public GameObject characterJoyStick;
    public EasyJoystick joystick;

    public bool GameIsPaused = true;
    public bool GameHasStarted = false;
    public bool teststate = false;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            teststate = true;
            GameObject.DestroyImmediate(this.gameObject);
            return;
        }

        else
        {
            DontDestroyOnLoad(this);
        }
        //joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();

        #if UNITY_IPHONE 
        inputType = InputType.TouchInput;
        characterJoyStick = Instantiate(Resources.Load("Managers/CharacterJoystick")) as GameObject;
        joystick = characterJoyStick.GetComponent<EasyJoystick>();
        joystick.showDebugRadius = false;

        joystick.enable = false;

        #endif
		#if UNITY_WEBPLAYER || UNITY_EDITOR || UNITY_STANDALONE
        	inputType = InputType.WASDInput;
            Debug.LogWarning("wasd");
            //joystick.gameObject.SetActive(false);
        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
            inputType = InputType.WASDInput;
        #endif
        
    }

    void Start()
    {
        GameHasStarted = true;
        GameIsPaused = false;
        OnLevelWasLoaded(Application.loadedLevel);
    }

    public void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            GUIManager.Instance.DisplayIntroGUI();
            GameHasStarted = false;
            GameIsPaused = true;
            Debug.Log("0");

            //login
            //check for last town
            //load town level async
            //join room

        }
        else
        {


            if(level == 1)
            {
                Debug.Log("loaded scene 1");
                PlayerManager.Instance.RefreshAvatar();
                GUIManager.Instance.TurnOffAllOtherUI();
                GUIManager.Instance.DisplayMainGUI();
            }

            else
            {
                Debug.Log("loaded scene " + Application.loadedLevel);
                PlayerManager.Instance.RefreshAvatar();
                GUIManager.Instance.TurnOffAllOtherUI();
                GUIManager.Instance.DisplayMainGUI();
            }
            if(inputType == InputType.TouchInput)
            {
                joystick.enable = true;
            }
        }


    }

    public void LoadLevel(string name)
    {
        if(inputType == InputType.TouchInput)
        {
            joystick.enable = false;
        }
        PlayerManager.Instance.DisableAvatarInput();
        Application.LoadLevel(name);
    }


}

public enum InputType
{
    TouchInput = 0,
    WASDInput = 1
}
