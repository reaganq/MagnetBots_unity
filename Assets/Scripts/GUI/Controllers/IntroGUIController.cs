using UnityEngine;
using System.Collections;
using System;
using Parse;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class IntroGUIController : BasicGUIController {

    public UILabel loadingLabel;
    public GameObject startButton;
	public GameObject signInButton;
	public GameObject registerButton;
	public GameObject buttonMenu;
	public GameObject accountDetailsBox;

	public UIInput usernameInput;
	public UIInput passwordInput;

	public bool loading;
	public bool newAcct;
	public bool quickStartClicked;
	public bool registerMode;

	public UIPlayTween playTween;

	public void Awake()
	{
		loading = false;
		newAcct = true;
		quickStartClicked = false;
	}
	public override void Enable()
	{
		ShowButtonMenu();
		base.Enable();
	}

	public void ShowButtonMenu()
	{
		accountDetailsBox.SetActive(false);
		playTween.tweenTarget = buttonMenu;
		playTween.Play(true);
	}
	
	public void OnStartPressed()
	{
		if (!PhotonNetwork.insideLobby) 
		{
			loadingLabel.text = "Not connected to lobby yet";
			return;
		}

		if(!quickStartClicked)
		{
			GameManager.Instance.teststate = true;
			quickStartClicked = true;
			NetworkManager.Instance.usingParse = false;
			GameManager.Instance.newGame = true;
			StartGame();
		}
	}

	public void OnQuickLoadPressed()
	{
		if (!PhotonNetwork.insideLobby) 
		{
			loadingLabel.text = "Not connected to lobby yet";
			return;
		}
		
		if(!quickStartClicked)
		{
			GameManager.Instance.teststate = true;
			quickStartClicked = true;
			NetworkManager.Instance.usingParse = false;
			GameManager.Instance.newGame = false;
			StartGame();
		}
	}

	public void OnRegisterPressed()
	{
		if (!PhotonNetwork.insideLobby) 
		{
			loadingLabel.text = "Not connected to lobby yet";
			return;
		}

		buttonMenu.SetActive(false);
		playTween.tweenTarget = accountDetailsBox;
		playTween.Play(true);
		registerMode = true;
	}

	public void OnLoginPressed()
	{
		if (!PhotonNetwork.insideLobby) 
		{
			loadingLabel.text = "Not connected to lobby yet";
			return;
		}

		buttonMenu.SetActive(false);
		playTween.tweenTarget = accountDetailsBox;
		playTween.Play(true);
		registerMode = false;
	}

	public void OnBackButtonPressed()
	{
		Debug.Log("back button pressed");
		ShowButtonMenu();
		loadingLabel.text = "";
		quickStartClicked = false;
	}

	//int world id
    public void StartGame()
    {
		//PlayerManager.Instance.StartNewGame();
		loadingLabel.text = "loading";
		//TODO replace 1 with the desired level id;
		GameManager.Instance.loadNewLevel(1);
    }

    public string levelName;
    AsyncOperation async;
    
    IEnumerator offlineload() {
        //startButton.SetActive(false);
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
	
    public void ActivateScene() {
        async.allowSceneActivation = true;
        GameManager.Instance.GameIsPaused = false;
        //GUIManager.Instance.StartGame();
    }

	public void OnConfirmpressed()
	{
		if(!quickStartClicked)
		{
			if(registerMode)
			{
				Debug.Log(usernameInput.value + passwordInput.value);
				CreateNewUser(usernameInput.value, passwordInput.value);
			}
			else
			{
				Debug.Log(usernameInput.value + passwordInput.value);
				authenticateUser(usernameInput.value, passwordInput.value);
				loadingLabel.text = "signing in";
			}
			quickStartClicked = true;
		}
	}

	void Update()
	{
		if(NetworkManager.Instance.usingParse && !loading && PhotonNetwork.insideLobby)
		{
			if(ParseUser.CurrentUser != null)
			{
				Debug.Log("true");
				if(newAcct)
				{
					//PlayerManager.Instance.StartNewGame();
					//start a raw game because this is a new account
					StartGame();
					GameManager.Instance.newGame = true;
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
		Debug.Log ("logging in");
		ParseUser.LogInAsync(username, password).ContinueWith(t =>
		                                                      {
			if (t.IsFaulted || t.IsCanceled)
			{
				// The login failed. Check t.Exception to see why.
				//isAuthenticated = false;
				loadingLabel.text = "username does not exist";
				quickStartClicked = false;
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
		var user = new ParseUser ()
		{
			Username = userName,
			Password = password,
		};
			//Email = "reaganqiu@gmail.com"

		
        user.SignUpAsync().ContinueWith(t => 
                                        {
            if(t.IsFaulted || t.IsCanceled)
            {
                //Debug.Log(t.Exception);
				loadingLabel.text = "WTF";
				quickStartClicked = false;
            }

        });

		loadingLabel.text = "creating new account";
		newAcct = true;
		GameManager.Instance.newGame = true;
        //Application.LoadLevel("Demo");
    }
}
