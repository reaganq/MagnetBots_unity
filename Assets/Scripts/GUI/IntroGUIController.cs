using UnityEngine;
using System.Collections;

public class IntroGUIController : MonoBehaviour {

    public GameObject loadingLabel;
    public GameObject startButton;
	public GameObject signInButton;
	public GameObject registerButton;


    public void OnStartPressed()
    {
        StartCoroutine("load");
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

	IEnumerator load()
	{
		//startButton.SetActive(false);
		//loadingLabel.SetActive(true);
		//NetworkManager.Instance.Connect();
		PhotonNetwork.JoinRandomRoom();
		yield return null;
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
        GameManager.Instance.GameHasStarted = true;
        GameManager.Instance.GameIsPaused = false;
        GUIManager.Instance.StartGame();
    }
}
