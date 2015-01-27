using UnityEngine;
using System.Collections;
using System;
using Parse;


public class IntroGUIController : MonoBehaviour {

    public UILabel loadingLabel;
    public GameObject startButton;
	public GameObject signInButton;
	public GameObject registerButton;
	public GameObject panel;

	public UIInput usernameInput;
	public UIInput passwordInput;

	public bool loading;
	public bool newAcct;
	public bool quickStartClicked;

	public void Awake()
	{
		loading = false;
		newAcct = true;
		quickStartClicked = false;
	}

	public void Enable()
	{
		panel.SetActive(true);
	}

	public void Disable()
	{
		panel.SetActive(false);
	}

	public void OnStartPressed()
	{
		if(!PhotonNetwork.insideLobby)
			return;

		if(!quickStartClicked)
		{
			quickStartClicked = true;
			NetworkManager.Instance.usingParse = false;
			PlayerManager.Instance.StartNewGame();
			StartGame();
		}
	}

    public void StartGame()
    {
		loadingLabel.text = "loading";
		load();
		GameManager.Instance.GameHasStarted = true;
    }

    public string levelName;
    AsyncOperation async;
    
    IEnumerator offlineload() {
        startButton.SetActive(false);
		NGUITools.SetActive(startButton, false);
        //loadingLabel.SetActive(true);
		
        async = Application.LoadLevelAsync(1);
        async.allowSceneActivation = false;
        Debug.LogWarning("ASYNC LOAD STARTED - " +
                         "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        while(async.progress < 0.9f)
        {
            Debug.Log(async.progress);
            yield return null;
        }

        Debug.Log("loading complete");
        yield return new WaitForSeconds(2);
        ActivateScene();
    }

	public void load()
	{
		//startButton.SetActive(false);
		//loadingLabel.SetActive(true);
		//NetworkManager.Instance.Connect();
		PhotonNetwork.JoinRandomRoom();
		//yield return null;
		/*while(!NetworkManager.Instance.isConnectedToServer)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1);
		GameManager.Instance.GameHasStarted = true;
		GameManager.Instance.GameIsPaused = false;
		GUIManager.Instance.StartGame();
		PhotonNetwork.LoadLevel(1);*/
	}
    
    public void ActivateScene() {
        async.allowSceneActivation = true;
        GameManager.Instance.GameIsPaused = false;
        GUIManager.Instance.StartGame();
    }

	public void Register()
	{
		Debug.Log(usernameInput.value + passwordInput.value);
		CreateNewUser(usernameInput.value, passwordInput.value);
	}

	public void Login()
	{
		Debug.Log(usernameInput.value + passwordInput.value);
		authenticateUser(usernameInput.value, passwordInput.value);
		loadingLabel.text = "signing in";
	}

	void Update()
	{
		if(NetworkManager.Instance.usingParse && !loading)
		{
			if(ParseUser.CurrentUser != null)
			{
				Debug.Log("true");
				if(newAcct)
				{
					PlayerManager.Instance.StartNewGame();
					StartGame();
					loading = true;
				}
				else
				{
					PlayerManager.Instance.LoadGame();
					loading = true;
				}


			}
		}
	}

	public void authenticateUser(string username, string password)
	{
		
		ParseUser.LogInAsync(username, password).ContinueWith(t =>
		                                                      {
			if (t.IsFaulted || t.IsCanceled)
			{
				// The login failed. Check t.Exception to see why.
				//isAuthenticated = false;
				loadingLabel.text = "username does not exist";
			}
			else
			{

				// Login was successful.
				//isAuthenticated = true;
			}
		});
		loadingLabel.text = "signing in";
		newAcct = false;
	}
	
	public void CreateNewUser(string userName, string password)
	{
		var user = new ParseUser()
		{
			Username = userName,
			Password = password,
			//Email = "reaganqiu@gmail.com"
		};
		
        user.SignUpAsync().ContinueWith(t => 
                                        {
            if(t.IsFaulted || t.IsCanceled)
            {
                //Debug.Log(t.Exception);
				loadingLabel.text = "account already exists";
            }

        });

		loadingLabel.text = "creating new account";
		newAcct = true;
        //Application.LoadLevel("Demo");
    }
}
