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
	public UILabel DetailsLabel;
	public GameObject backButton;
	public List<RPGEnemy> enemies;
	public NPCArena activeArena;
	public bool isBusy = false;
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
		isBusy = false;
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
				EnemyCards[i].LoadEnemy(activeArena.Enemies[i].PortraitAtlas, activeArena.Enemies[i].PortraitIcon, i, activeArena.Enemies[i].isAvailable, !activeArena.Enemies[i].Validate());
			}
			else
			{
				//display locked instead
				EnemyCardObjects[i].SetActive(false);
			}

		}

	}

	public override void Disable()
	{
		isBusy = false;
		base.Disable();
	}

	public void DisplayDetailsBox(int index)
	{
		//enemycardobjects[index]. move to the left side
		DetailsBox.SetActive(true);
		selectedCardIndex = index;
		UpdateDetailsBox();

		ScrollView.SetActive(false);
	}

	public void UpdateDetailsBox()
	{
		DetailsPortrait.spriteName = activeArena.Enemies[selectedCardIndex].PortraitIcon;
		DetailsLabel.text = activeArena.Enemies[selectedCardIndex].Description;
		/*partyListText.text = string.Empty;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) 
		{
			partyListText.text += PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]).name + "\n";
		}*/

	}

	public void Challenge()
	{
		if(isBusy)
			return;

		isBusy = true;
		Debug.Log("solo challenge");
		if(!PlayerManager.Instance.isInParty())
			LaunchArena(true);
		else
			PlayerManager.Instance.ActiveWorld.SendPartyChallenge(activeArena.Enemies[selectedCardIndex].ID);
	}

	public void LaunchArena(bool solo)
	{
		PlayerManager.Instance.ActiveWorld.RequestArenaEntry(activeArena.Name, activeArena.Enemies[selectedCardIndex].ID, solo);
		//GUIManager.Instance.DisplayMainGUI();
	}

	public void OnBackButtonPressed()
	{
		if(isBusy)
			return;
		selectedCardIndex = -1;
		DetailsBox.SetActive(false);
		ScrollView.SetActive(true);
	}

	public void OnExitButtonPressed()
	{
		if(isBusy)
			return;
		//Disable();
		GUIManager.Instance.NPCGUI.HideShop();
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
