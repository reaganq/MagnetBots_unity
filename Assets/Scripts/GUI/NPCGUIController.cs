using UnityEngine;
using System.Collections;

public class NPCGUIController : BasicGUIController {
 
    public UILabel textLabel = null;
    public GameObject enterShopButton = null;
    public GameObject confirmButton = null;
    public GameObject arenaButton = null;
	public GameObject activityButton = null;
	public GameObject minigameButton = null;
    public UILabel arenaLabel = null;
	public UILabel minigameLabel = null;
	public UILabel activityLabel = null;

	public GameObject root;
	public float offset;
    
	// Use this for initialization
	// Update is called once per frame
	public override void Enable()
    {
		root.SetActive(true);
		//textLabel.gameObject.SetActive(true);
        textLabel.text = PlayerManager.Instance.ActiveNPC.character.Name;

		int numberOfButtons = 0;
        if(PlayerManager.Instance.ActiveNPC.thisShop != null)
        {
            enterShopButton.SetActive(true);
			numberOfButtons += 1;
        }
        else
        {
            enterShopButton.SetActive(false);
        }
        if(PlayerManager.Instance.ActiveNPC.arena != null )
        {
            SetupArenaButton();
            arenaButton.SetActive(true);
			numberOfButtons += 1;
        }
        else
        {
            arenaButton.SetActive(false);
        }
		if(PlayerManager.Instance.ActiveNPC.activity != null )
		{
			SetupArenaButton();
			activityButton.SetActive(true);
			numberOfButtons += 1;
		}
		else
		{
			arenaButton.SetActive(false);
		}
		if(PlayerManager.Instance.ActiveNPC.miniGame != null )
		{
			SetupArenaButton();
			minigameButton.SetActive(true);
			numberOfButtons += 1;
		}
		else
		{
			arenaButton.SetActive(false);
		}


        confirmButton.SetActive(true);
		numberOfButtons +=1;

		Debug.Log("enable npc gui");
    }
    
    public void OnConfirmButtonPressed()
    {
        GUIManager.Instance.HideNPC();
		GUIManager.Instance.DisplayMainGUI();
    }
    
    public void OnEnterShopButton()
    {
        GUIManager.Instance.DisplayShop();
    }

    public void OnEnterArenaButton()
    {
		PlayerManager.Instance.SelectedArena = PlayerManager.Instance.ActiveNPC.arena;
		GUIManager.Instance.DisplayEnemieslist();
        //Application.LoadLevel(PlayerManager.Instance.ActiveNPC.character.LevelName);
		//PlayerManager.Instance.GoToArena(PlayerManager.Instance.ActiveWorld.GetAvailableArena("Gym"));
		GUIManager.Instance.HideNPC();
		//GUIManager.Instance.DisplayMainGUI();
		/*if(PlayerManager.Instance.ActiveArena != null)
		{
			Debug.Log("we have an arena");
			PlayerManager.Instance.ActiveArena.gameObject.GetComponent<PhotonView>().RPC("Initialise", PhotonTargets.MasterClient, "Jim");
		}*/
    }

    public void SetupArenaButton()
    {
        arenaLabel.text = PlayerManager.Instance.ActiveNPC.arena.Name;
    }

	public void SetupMiniGameButton()
	{
		minigameLabel.text = PlayerManager.Instance.ActiveNPC.miniGame.Name;
	}

	public void SetupActivityButton()
	{
		activityLabel.text = PlayerManager.Instance.ActiveNPC.activity.Name;
	}

	public override void Disable()
	{
		textLabel.gameObject.SetActive(false);
		root.SetActive(false);
	}
}
