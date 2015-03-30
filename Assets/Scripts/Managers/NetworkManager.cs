using UnityEngine;
using System.Collections;
using Parse;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviour {

	private static NetworkManager instance = null;
	
	private NetworkManager() {}
	
	public static NetworkManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
				if (instance == null)
				{
					NetworkManager prefab = Resources.Load("Managers/_NetworkManager", typeof(NetworkManager)) as NetworkManager;
					instance = Instantiate(prefab) as NetworkManager;
				}
			}
			return instance;
		}
	}

	public bool isConnectedToServer = false;
	public bool offlineMode;
	public bool usingParse;
	public bool showGUI;


	void Awake()
	{
		if(Instance != null && Instance != this)
		{
			GameObject.DestroyImmediate(this.gameObject);
			return;
		}
		
		else
		{
			DontDestroyOnLoad(this);
		}

		PhotonNetwork.playerName = "Tester";

		if(offlineMode)
		{
			//Connect();
			PhotonNetwork.Disconnect();
			PhotonNetwork.offlineMode = offlineMode;
			PhotonNetwork.CreateRoom("null");
			Debug.Log("offlinemode");
		}
		else
			Connect();

		if (usingParse) {
			if(ParseUser.CurrentUser != null )
			{
			Debug.Log("there is already a parse user in use");
			ParseUser.LogOut ();
			}
		}
	}

	public void Connect () 
	{

		PhotonNetwork.ConnectUsingSettings("Magnet Bots v0.0.1");

	}

	void OnGUI()
	{
		if(!offlineMode && showGUI)
		{
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
		}
	}

	void OnPhotonRandomJoinFailed() 
	{
		string[] roomPropsInLobby = {"world"};
		Hashtable worldID = new Hashtable() {{"world", GameManager.Instance.targetLevel.ToString()}};
		PhotonNetwork.CreateRoom( null, true, true, 0, worldID, roomPropsInLobby);
	}

	void OnJoinedRoom()
	{
		//PhotonNetwork.load
		//retrieve room properties to figure out which world to load
		isConnectedToServer = true;
		//PhotonNetwork.LoadLevel(1);
		//GUIManager.Instance.StartGame();
		if(!offlineMode)
		{
			PhotonNetwork.LoadLevel(GameManager.Instance.targetLevel);
		}
		else
		{
			PhotonNetwork.LoadLevel(GameManager.Instance.targetLevel);
			//HACK get unity editor to run OnLevelWasLoaded on the first level
			//GameManager.Instance.OnLevelWasLoaded(Application.loadedLevel);
		}
		//PlayerManager.Instance.StartCoroutine("RefreshAvatar", 0);
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		Debug.LogError(cause);
	}

	#region parse



	public void OnApplicationQuit()
	{
		if(usingParse)
			ParseUser.LogOut();
	}

	#endregion
	
}
