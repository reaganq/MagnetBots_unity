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

		PhotonNetwork.offlineMode = offlineMode;
	}

	public void Connect () 
	{

		PhotonNetwork.ConnectUsingSettings("Magnet Bots v0.0.1");
	}

	void OnGUI()
	{
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );
	}

	void Update()
	{
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
		Debug.Log("OnJoinedRoom");
		//PhotonNetwork.load
		isConnectedToServer = true;

	}
	
}
