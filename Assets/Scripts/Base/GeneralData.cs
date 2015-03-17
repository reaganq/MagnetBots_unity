using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralData: MonoBehaviour{

	public static List<RPGQuest> quests;
	public static List<RPGNPC> NPCs;
	public static List<RPGTown> towns;
	public static float interestRate = 0.1f;
	public static int maxPartySize = 3;
	public List<ItemCategoryData> itemCategories;
	public int defaultHeadObj = 1;
	public int defaultChestObj = 2;
	public int defaultArmLObj = 3;
	public int defaultArmRObj = 4;
	public int defaultLegsObj = 5;
	public static string coinIconPath = "currency_coin";
	public static string magnetIconPath = "currency_magnet";
	public static string citizenIconPath = "currency_citizen";
	public static string[] armRBones = new string[6] {"bones:R_Clavicle", "bones:R_Shoulder", "bones:R_ShoulderGuard", "bones:R_Elbow", "bones:R_Forearm", "bones:R_Hand"};
	public static string[] armLBones = new string[6] {"bones:L_Clavicle", "bones:L_Shoulder", "bones:L_ShoulderGuard", "bones:L_Elbow", "bones:L_Forearm", "bones:L_Hand"};
	public static string[] verticalBones = new string[2] {"bones:Spine_2", "bones:Neck_Horizontal"};
	public static string characters = "abcdefghijklmnopqrstuvwxyz";
	// Use this for initialization
	public void Awake()
	{
		quests = Storage.Load<RPGQuest>(new RPGQuest());
		NPCs = Storage.Load<RPGNPC>(new RPGNPC());
		towns = Storage.Load<RPGTown>(new RPGTown());
		Debug.Log(quests.Count + "quests");
		LoadQuests();
	}

	public void LoadQuests()
	{
		for (int i = 0; i < quests.Count; i++) {
			if(quests[i].questType == QuestType.collection)
			{
				for (int j = 0; j < quests[i].questSteps.Count; j++) {
					if(quests[i].questSteps[j].isMainStep)
						quests[i].questSteps[i].GenerateRandomTasks(quests[i].allTasks,Random.Range(1,4),1);
				}
			}
		}
	}

	public static RPGNPC GetNPCByID(int ID)
	{
		for (int i = 0; i < NPCs.Count; i++) {
			if(NPCs[i].ID == ID)
				return NPCs[i];
		}
		return null;
	}

	public static RPGQuest GetQuestByID(int ID)
	{
		for (int i = 0; i < quests.Count; i++) {
			if(quests[i].ID == ID)
				return quests[i];
		}
		return null;
	}

	public RPGTown GetTownByID(int ID)
	{
		for (int i = 0; i < towns.Count; i++) {
			if(towns[i].ID == ID)
				return towns[i];
				}
		return null;
	}

	public List<ItemCategoryData> GetSubcategories(string category)
	{
		for (int i = 0; i < itemCategories.Count; i++) {
			if(itemCategories[i].name == category)
				return itemCategories[i].subcategories;
		}
		return null;
	}

	public static string GenerateRandomString(int l)
	{
		string name = "";
		for (int i = 0; i < l; i++) 
		{
			int a = Random.Range(0, characters.Length);
			name = name + characters[a];
		}
		return name;
	}
}

[System.Serializable]
public class ItemCategoryData
{
	public string name;
	public string iconName;
	public List<ItemCategoryData> subcategories;
}
