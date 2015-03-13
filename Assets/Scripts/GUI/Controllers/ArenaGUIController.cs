using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ArenaGUIController : BasicGUIController {

	public List<EnemyCard> EnemyCards;
	public List<GameObject> EnemyCardObjects;
	public GameObject DetailsBox;
	public GameObject ScrollView;
	public UISprite DetailsPortrait;
	public GameObject backButton;
	public GameObject teamButton;
	public List<RPGEnemy> enemies;
	public NPCArena activeArena;
	public UILabel partyListText;
	
	public int selectedCardIndex;

	public void Start()
	{
		for (int i = 0; i < EnemyCards.Count; i++) 
		{
			EnemyCardObjects.Add(EnemyCards[i].gameObject);
			EnemyCards[i].controller = this;
		}
	}

	public void Enable(NPCArena newArena)
	{
		Enable();
		activeArena = newArena;
		backButton.SetActive(true);
		DetailsBox.SetActive(false);
		ScrollView.SetActive(true);
		//enemies = PlayerManager.Instance.SelectedArena.Enemies;

		for (int i = 0; i < EnemyCards.Count; i++) 
		{
			if(i < activeArena.Enemies.Count)
			{
				EnemyCardObjects[i].SetActive(true);
				EnemyCards[i].LoadEnemy(activeArena.Enemies[i].Name, activeArena.Enemies[i].PortraitAtlas, activeArena.Enemies[i].PortraitIcon, activeArena.Enemies[i].Description, i);
			}
			else
			{
				EnemyCardObjects[i].SetActive(false);
			}

		}

	}

	public override void Disable()
	{
		base.Disable();
	}

	public void DisplayDetailsBox(int index)
	{
		//enemycardobjects[index]. move to the left side
		DetailsBox.SetActive(true);
		UpdateDetailsBox();
		selectedCardIndex = index;
		ScrollView.SetActive(false);
		if(PlayerManager.Instance.isInParty())
			teamButton.SetActive(true);
		else
			teamButton.SetActive(false);
	}

	public void UpdateDetailsBox()
	{
		/*partyListText.text = string.Empty;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) 
		{
			partyListText.text += PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]).name + "\n";
		}*/
	}

	public void SoloChallenge()
	{
		Debug.Log("solo challenge");
		LaunchArena(true);
	}

	public void TeamChallenge()
	{
		Debug.Log("team challenge");
		PlayerManager.Instance.ActiveWorld.SendPartyChallenge(activeArena.Enemies[selectedCardIndex].ID);
		//LaunchArena(false);
	}

	public void LaunchArena(bool solo)
	{
		PlayerManager.Instance.ActiveWorld.RequestArenaEntry(activeArena.Name, activeArena.Enemies[selectedCardIndex].ID, solo);
		//GUIManager.Instance.DisplayMainGUI();
	}

	public void OnBackButtonPressed()
	{
		Disable();
		GUIManager.Instance.DisplayNPC();
	}

}

[Serializable]
public class EnterArenaData
{
	public int EnemyID;
	public int NewViewID;
	public List<PartyMemberData> partyList = new List<PartyMemberData>();
	public int PartyLeaderID;
}
