using UnityEngine;
using System.Collections;

public class NPCGUIController : MonoBehaviour {
 
    public UILabel textLabel = null;
    public GameObject enterShopButton = null;
    public GameObject confirmButton = null;
    public GameObject arenaButton = null;
    public UILabel arenaLabel = null;
    
	// Use this for initialization
	// Update is called once per frame
	public void Enable()
    {
        textLabel.text = PlayerManager.Instance.ActiveNPC.character.Name;
        if(PlayerManager.Instance.ActiveShop != null)
        {
            enterShopButton.SetActive(true);
        }
        else
        {
            enterShopButton.SetActive(false);
        }
        if(!string.IsNullOrEmpty(PlayerManager.Instance.ActiveNPC.character.LevelName))
        {
            Debug.Log("1");
            SetupArenaButton();
            arenaButton.SetActive(true);
        }
        else
        {
            Debug.Log("2");
            arenaButton.SetActive(false);
        }


        confirmButton.SetActive(true);
    }
    
    public void OnConfirmButtonPressed()
    {
        GUIManager.Instance.HideNPC();
    }
    
    public void OnEnterShopButton()
    {
        GUIManager.Instance.DisplayShop();
    }

    public void OnEnterArenaButton()
    {
        Application.LoadLevel(PlayerManager.Instance.ActiveNPC.character.LevelName);
    }

    public void SetupArenaButton()
    {
        arenaLabel.text = PlayerManager.Instance.ActiveNPC.character.LevelName;
    }
}
