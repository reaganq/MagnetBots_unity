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
	public List<RPGEnemy> enemies;
	public UILabel partyListText;

	public GameObject Panel;
	public int selectedCardIndex;

	public void Start()
	{
		for (int i = 0; i < EnemyCards.Count; i++) 
		{
			EnemyCardObjects.Add(EnemyCards[i].gameObject);
			EnemyCards[i].controller = this;
		}
	}

	public override void Enable()
	{
		Panel.SetActive(true);
		backButton.SetActive(true);
		DetailsBox.SetActive(false);
		ScrollView.SetActive(true);
		enemies = PlayerManager.Instance.SelectedArena.Enemies;

		for (int i = 0; i < EnemyCards.Count; i++) 
		{
			if(i < enemies.Count)
			{
				EnemyCardObjects[i].SetActive(true);
				EnemyCards[i].LoadEnemy(enemies[i].Name, enemies[i].PortraitAtlas, enemies[i].PortraitIcon, enemies[i].Description, i);
			}
			else
			{
				EnemyCardObjects[i].SetActive(false);
			}

		}

	}

	public override void Disable()
	{
		Panel.SetActive(false);
	}

	public void DisplayDetailsBox(int index)
	{
		//enemycardobjects[index]. move to the left side
		DetailsBox.SetActive(true);
		UpdateDetailsBox();
		selectedCardIndex = index;
		ScrollView.SetActive(false);
	}

	public void UpdateDetailsBox()
	{
		partyListText.text = string.Empty;
		for (int i = 0; i < PlayerManager.Instance.partyMembers.Count; i++) 
		{
			partyListText.text += PhotonPlayer.Find(PlayerManager.Instance.partyMembers[i]).name + "\n";
		}
	}

	public void SoloChallenge()
	{
		Debug.Log("solo challenge");
		LaunchArena(true);
	}

	public void TeamChallenge()
	{
		Debug.Log("team challenge");
		LaunchArena(false);
	}

	public void LaunchArena(bool solo)
	{
		int newid = PhotonNetwork.AllocateViewID();
		EnterArenaData data = new EnterArenaData();
		data.EnemyID = enemies[selectedCardIndex].ID;
		data.NewViewID = newid;
		Debug.Log(data.NewViewID);

		if(PlayerManager.Instance.partyMembers.Count == 0)
		{
			data.partyList.Add(PhotonNetwork.player.ID);
			data.PartyLeaderID = PhotonNetwork.player.ID;
		}
		else if(PlayerManager.Instance.partyMembers.Count > 0)
		{
			data.partyList = PlayerManager.Instance.partyMembers;
			data.PartyLeaderID = PhotonNetwork.player.ID;
		}

		BinaryFormatter b = new BinaryFormatter();
		MemoryStream m = new MemoryStream();
		b.Serialize(m, data);

		PlayerManager.Instance.ActiveWorld.myPhotonView.RPC("GetAvailableArena", PhotonTargets.MasterClient, PlayerManager.Instance.SelectedArena.Name, m.GetBuffer());
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
	public List<int> partyList = new List<int>();
	public int PartyLeaderID;
}
