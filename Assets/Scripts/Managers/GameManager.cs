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
                    GameManager prefab = Resources.Load("Managers/_GameManager", typeof(GameManager)) as GameManager;
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
	public float defaultAspectRatio;
	public float nativeAspectRatio;

    public bool GameIsPaused;
    public bool GameHasStarted;
	public bool teststate;

    void Awake()
    {
		GameIsPaused = true;
		GameHasStarted = false;
		teststate = false;

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
		Debug.LogWarning("WTF");
		#if UNITY_IPHONE || UNITY_STANDALONE
        inputType = InputType.TouchInput;
        characterJoyStick = Instantiate(Resources.Load("Managers/_CharacterJoystick")) as GameObject;
        joystick = characterJoyStick.GetComponent<EasyJoystick>();
        joystick.showDebugRadius = false;

        joystick.enable = false;
		characterJoyStick.GetComponent<EasyTouch>().nGUICameras[0] = GameObject.FindGameObjectWithTag("UICamera").camera;

        #endif
		#if UNITY_WEBPLAYER 
        	inputType = InputType.WASDInput;
            //Debug.LogWarning("wasd");
            //joystick.gameObject.SetActive(false);
        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
            inputType = InputType.WASDInput;
        #endif

    }

    void Start()
    {
		if(Application.loadedLevel == 0)
        	OnLevelWasLoaded(Application.loadedLevel);
		defaultAspectRatio = 2048.0f/1536.0f;
		nativeAspectRatio = ((float)Screen.width)/((float)Screen.height);
    }

    public void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
			//StartCoroutine("LoadStandard");
            GUIManager.Instance.DisplayIntroGUI();
            GameHasStarted = false;
            GameIsPaused = true;

            //login
            //check for last town
            //load town level async
            //join room
        }
        else
        {
			//Debug.Log("onlevelwasloaded");
			StartCoroutine("LoadStandard");
        }
    }

	public IEnumerator LoadStandard()
	{
		while(!NetworkManager.Instance.isConnectedToServer)
		{
			//Debug.Log("not connected");
			yield return null;
		}
		PlayerCamera.Instance.Reset();
		PlayerManager.Instance.ChangeWorld();
		//yield return new WaitForSeconds(1);
		PlayerManager.Instance.RefreshAvatar();
		//GUIManager.Instance.TurnOffAllOtherUI();
		GUIManager.Instance.DisplayMainGUI();
		Debug.Log("load standard");
		
		if(inputType == InputType.TouchInput)
		{
			joystick.enable = true;
		}
		yield return null;
	}

    public void LoadWorld(string name)
    {
        if(inputType == InputType.TouchInput)
        {
            joystick.enable = false;
        }
        //PlayerManager.Instance.DisableAvatarInput();
        Application.LoadLevel(name);
    }
}

public enum InputType
{
    TouchInput = 0,
    WASDInput = 1
}
