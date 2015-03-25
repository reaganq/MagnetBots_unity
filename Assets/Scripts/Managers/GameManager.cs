using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
	public float defaultScreenHeight = 1536.0f;
	public float defaultScreenWidth = 2048.0f;
	public float defaultAspectRatio
	{
		get{return 2048.0f/1536.0f;}
	}
	public float nativeAspectRatio
	{
		get{return ((float)Screen.width)/((float)Screen.height);}
	}

    public bool GameIsPaused;
    public bool GameHasStarted;
	public bool teststate;
	public bool newGame;
	public int targetLevel = 1;
	private bool isLoadingNewLevel = false;

    void Awake()
    {
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes"); 
		GameIsPaused = true;
		GameHasStarted = false;

        if(Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(this.gameObject);
            return;
        }

        else
        {
            DontDestroyOnLoad(this);
        }
        //joystick = GameObject.FindGameObjectWithTag("GameController").GetComponent<EasyJoystick>();
		#if UNITY_IPHONE || UNITY_EDITOR || UNITY_STANDALONE
        inputType = InputType.TouchInput;
        characterJoyStick = Instantiate(Resources.Load("Managers/_CharacterJoystick")) as GameObject;
        joystick = characterJoyStick.GetComponent<EasyJoystick>();
        joystick.showDebugRadius = false;

        joystick.enable = false;
		characterJoyStick.GetComponent<EasyTouch>().nGUICameras[0] = GUIManager.Instance.uiCamera;

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
		{
        	OnLevelWasLoaded(Application.loadedLevel);
			Debug.Log("started level: " + Application.loadedLevel);
		}
    }

	public void loadNewLevel(int level)
	{
		StartCoroutine(LoadNewLevelSequence(level));
	}

	public IEnumerator LoadNewLevelSequence(int level)
	{
		GUIManager.Instance.loadingGUI.Enable(false);
		targetLevel = level;
		//yield return new WaitForSeconds(0.5f);
		if(PhotonNetwork.room != null)
		{

			PhotonNetwork.LeaveRoom();
			isLoadingNewLevel = true;
			if(NetworkManager.Instance.offlineMode)
				OnJoinedLobby();
			else
				PhotonNetwork.JoinLobby();
			yield break;
		}
		Hashtable worldID = new Hashtable() {{"world", targetLevel.ToString()}};
		PhotonNetwork.JoinRandomRoom(worldID, 0);
		isLoadingNewLevel = false;
		yield return null;
	}

	public void OnJoinedLobby()
	{
		if(isLoadingNewLevel)
		{
			Hashtable worldID = new Hashtable() {{"world", targetLevel.ToString()}};
			PhotonNetwork.JoinRandomRoom(worldID, 0);
			isLoadingNewLevel = false;
		}
	}

    public void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
			//StartCoroutine("LoadStandard");
            GUIManager.Instance.EnterGUIState(UIState.login);
            GameIsPaused = true;

            //login
            //check for last town
            //load town level async
            //join room
        }
        else
        {
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


		//PlayerManager.Instance.RefreshAvatar();
		//GUIManager.Instance.DisplayMainGUI();

		if(inputType == InputType.TouchInput)
		{
			joystick.enable = true;
		}
		PlayerManager.Instance.ChangeWorld();
		yield return null;
		PlayerCamera.Instance.Reset();
	}
}

public enum InputType
{
    TouchInput = 0,
    WASDInput = 1
}
