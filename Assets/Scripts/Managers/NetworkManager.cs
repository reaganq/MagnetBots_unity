using UnityEngine;
using System.Collections;

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

	
	}

	public void Connect () 
	{

		PhotonNetwork.ConnectUsingSettings("Magnet Bots v0.0.1");

	}

	void OnGUI()
	{
		if(!offlineMode)
		{
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
		}
	}

	void OnJoinedLobby()
	{
		Debug.Log("joined room");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() 
	{
		Debug.Log("join room failed");
		PhotonNetwork.CreateRoom( null );
	}

	void OnJoinedRoom()
	{
		//PhotonNetwork.load
		isConnectedToServer = true;
		Debug.Log("OnJoinedRoom");
		GameManager.Instance.GameIsPaused = false;
		GUIManager.Instance.StartGame();
		if(!offlineMode)
		{
			Debug.Log("load level 1");
			PhotonNetwork.LoadLevel(1);

		}
		else
		{
			GameManager.Instance.OnLevelWasLoaded(Application.loadedLevel);
		}
		//PlayerManager.Instance.StartCoroutine("RefreshAvatar", 0);
	}

	public void OnConnectionFail(DisconnectCause cause)
	{
		Debug.LogError(cause);
	}


	
}
