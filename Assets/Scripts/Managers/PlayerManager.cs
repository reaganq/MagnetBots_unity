using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;


public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    
    private PlayerManager() {}
    
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
                if (instance == null)
                {
                    PlayerManager prefab = Resources.Load("Managers/_PlayerManager", typeof(PlayerManager)) as PlayerManager;
                    instance = Instantiate(prefab) as PlayerManager;
                }
            }
            return instance;
        }
    }

    public PlayerInformation Hero;
    public NPC ActiveNPC;
	public NPCActivity activeActivity;
	//public NPCMinigame ActiveMinigame;
	public GameObject ActiveMinigameObject;
	public WorldManager ActiveWorld;
	public Zone ActiveZone;
	public ArenaManager ActiveArena;
	public PlayerActivityState activityState;
	public GeneralData data;
	public Cutscene activeCutscene;

    public Transform SpawnPoint;
    public GameObject avatarPrefab;

    public GameObject avatarObject;
	public CharacterActionManager avatarActionManager;
    public Avatar avatar;
    public CharacterInputController avatarInput;
    public PlayerCharacter avatarStatus;
	public NetworkCharacterMovement avatarNetworkMovement;
	public PhotonView avatarPhotonView;

	public List<int> _partyMembers = new List<int>();
	public List<bool> partyChallengeReplies = new List<bool>();
	public List<int> partyMembers {
		get {
			return _partyMembers;
		}
		set {
			_partyMembers = value;
			GUIManager.Instance.MainGUI.UpdatePartyMembers();
			Debug.Log("updated partymembers");
		}
	}

	public bool isInParty()
	{
		if(partyMembers.Count > 1)
			return true;
		else
			return false;
	}

 //public static GeneralData Data;
 
 //public Texture cursorImage;
 
 //public float effectTimer;
    //private float checkTimer;

    //public BaseGameObject selectedObject;


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        //Instance = this;
        //avatarObject = GameObject.FindGameObjectWithTag("Player");
        //avatar = avatarObject.GetComponent<Avatar>();
        DontDestroyOnLoad(this);
        Hero = new PlayerInformation();
        SaveLoad.content = new SavedContent();
        //Debug.Log("wtf");

        //ChangeMinimap(false);
        //gameObject.AddComponent<GUIScale>();
    }

    public void StartNewGame()
    {
		Debug.Log("start new game");
        Hero.StartNewGame();
		//GameManager.Instance.GameHasStarted = true;
		//LoadAvatar();
    }

	public void LoadGame()
	{
		StartCoroutine(Hero.RetrieveParseData());
	}
	
    public void LoadAvatar()
    {
		//SUPER HACK
		if(NetworkManager.Instance.offlineMode)
			StartNewGame();

		avatarObject = PhotonNetwork.Instantiate("PlayerAvatar", SpawnPoint.position , Quaternion.identity, 0) as GameObject;
		//avatarObject = GameObject.Instantiate(Resources.Load("PlayerAvatar"), SpawnPoint.position, Quaternion.identity) as GameObject;
		//avatarObject.AddComponent<DontDestroy>();
		avatarStatus = avatarObject.GetComponent<PlayerCharacter>();
		avatarInput = avatarObject.GetComponent<CharacterInputController>();
		avatarActionManager = avatarObject.GetComponent<CharacterActionManager>();
		//PlayerMotor cm = avatarObject.GetComponent<PlayerMotor>();
		//cm.enabled = true;
		//avatarInput.enabled = true;
		avatarNetworkMovement = avatarObject.GetComponent<NetworkCharacterMovement>();
		//UICamera.fallThrough = avatarObject;
		avatar = avatarObject.GetComponent<Avatar>();
		avatarPhotonView = avatarObject.GetComponent<PhotonView>();
		RefreshAvatar();
		//PlayerCamera.Instance.targetTransform = avatarObject.transform;
		//LoadCharacterParts();
    }

    public void RefreshAvatar()
    {
		//TODO OK THERE IS A PROBLEM HERE
        /*if(avatarObject == null)
        {
            StartNewGame();
			return;
        }*/

		if(avatarObject == null)
		{
			LoadAvatar();
			return;
		}

        PlayerCamera.Instance.targetTransform = avatarObject.transform;

        avatarObject.transform.position = SpawnPoint.position;
        LoadCharacterParts();
        EnableAvatarInput();

		//TODO hacky party list refresh
		_partyMembers.Clear();
    }

	public void ChangeWorld()
	{
		ActiveWorld = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
		ActiveZone = ActiveWorld.DefaultZone;
		SpawnPoint = ActiveZone.spawnPoint;
		SfxManager.Instance.PlaySoundtrack(ActiveWorld.soundtrack);
		GameManager.Instance.GameIsPaused = false;
		if(GameManager.Instance.newGame)
		{
			GameObject cutscene = Instantiate(Resources.Load("Cutscenes/IntroCinematic") as GameObject) as GameObject;
			Debug.Log("load cutscene");
		}
		else
		{
			RefreshAvatar();
			GUIManager.Instance.DisplayMainGUI();
		} 
	}

	/*public void ChangeZone(Zone newZone)
	{
		if(newZone != null)
		{
			Zone oldzone = ActiveZone;
			newZone.gameObject.SetActive(true);
			ActiveZone = newZone;
			SpawnPoint = ActiveZone.spawnPoint;
			avatarObject.transform.position = SpawnPoint.position;
			oldzone.gameObject.SetActive(false);

			if(ActiveArena)
			{
				activewo.EndSession(avatarPhotonView.viewID);
				ActiveArena = null;
			}
		}
	}*/

	public void GoToArena(Zone newZone, int enemyID)
	{
		StartCoroutine(GoToArenaSequence(newZone, enemyID));
	}

	public IEnumerator GoToArenaSequence(Zone newZone, int enemyID)
	{
		Debug.Log("go to new arena");
		GUIManager.Instance.loadingGUI.Enable();
		yield return new WaitForSeconds(1.5f);
		//TODO display loading screen
		Zone oldzone = ActiveZone;
		oldzone.LeaveZone();
		//	newZone.zoneObject.SetActive(true);
		ActiveZone = newZone;
		//newZone.EnterZone();
		//SpawnPoint = ActiveZone.spawnPoint;
		
		//oldzone.zoneObject.SetActive(false);
		ActiveZone.EnterZone();
		avatarObject.transform.position = SpawnPoint.position;
		if(ActiveZone.zoneType == ZoneType.arena)
		{
			ActiveArena = ActiveZone.gameObject.GetComponent<ArenaManager>();
			if(ActiveArena == null)
			{
				Debug.LogError("no bloody arena");
			}
			activityState = PlayerActivityState.arena;
		}
	}

	public void LeaveArena(Zone newZone)
	{
		if(newZone != null)
		{
			Zone oldzone = ActiveZone;
			oldzone.LeaveZone();
			//newZone.zoneObject.SetActive(true);
			ActiveZone = newZone;
			ActiveZone.EnterZone();
			//SpawnPoint = ActiveZone.spawnPoint;
			avatarObject.transform.position = SpawnPoint.position;
			//oldzone.zoneObject.SetActive(false);

			if(ActiveArena)
			{
				ActiveWorld.EndSession(ActiveArena);
				//ActiveArena.EndSession(avatarPhotonView.viewID);
				ActiveArena = null;
			}

			activityState = PlayerActivityState.idle;
			//GUIManager.Instance.DisplayMainGUI();
		}
	}

	public void PlayToy(string prefabPath)
	{
		//increase happiness;
	}

	public void EatFood(string prefabPath)
	{
		avatarActionManager.EatFood(prefabPath);
		//increase energy;
	}

	public void PlayMiniGame(NPCMinigame minigame)
	{
		avatarObject.SetActive(false);
		ActiveZone.zoneObject.SetActive(false);
		ActiveMinigameObject = Instantiate(Resources.Load(minigame.PrefabDirectory) as GameObject) as GameObject;
		activityState = PlayerActivityState.minigame;
		GUIManager.Instance.HideAllUI();
	}

	public void EndMiniGame()
	{
		ActiveZone.zoneObject.SetActive(true);
		avatarObject.SetActive(true);
		activityState = PlayerActivityState.idle;
		Destroy(ActiveMinigameObject);
		GUIManager.Instance.DisplayMainGUI();
	}

	/*public void ResetNPC()
	{
		if(ActiveNPC != null)
			ActiveNPC.Reset();
	}*/

    void LoadCharacterParts()
    {
		avatar.LoadAllBodyParts(Hero.PlayerName,
		                        Hero.Equip.EquippedFace.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedFace.Level, Hero.Equip.EquippedFace.rpgArmor.FBXName.Count) - 1], 
		                        Hero.Equip.EquippedHead == null? null : Hero.Equip.EquippedHead.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedHead.Level, Hero.Equip.EquippedHead.rpgArmor.FBXName.Count) - 1], 
		                        Hero.Equip.EquippedBody.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedBody.Level, Hero.Equip.EquippedBody.rpgArmor.FBXName.Count) - 1],
		                        Hero.Equip.EquippedArmL.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedArmL.Level, Hero.Equip.EquippedArmL.rpgArmor.FBXName.Count) - 1],
		                        Hero.Equip.EquippedArmR.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedArmR.Level, Hero.Equip.EquippedArmR.rpgArmor.FBXName.Count) - 1],
		                        Hero.Equip.EquippedLegs.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedLegs.Level, Hero.Equip.EquippedLegs.rpgArmor.FBXName.Count) - 1]);
    }
	
    public void EnableAvatarInput()
    {
        avatarInput.enabled = true;
    }

    public void DisableAvatarInput()
    {
        avatarInput.enabled = false;
    }

	public void GiveReward(LootItemList loots)
	{
		for (int i = 0; i < loots.items.Count; i++) {
			Hero.AddItem(loots.items[i]);
		}
		for (int i = 0; i < loots.currencies.Count; i++) {
			Hero.AddRPGCurrency(loots.currencies[i]);
		}
		for (int i = 0; i < loots.badges.Count; i++) {
			Hero.AddRPGCurrency(loots.currencies[i]);
		}
	}

	//party leader send to party members when they join
	public void GiveRewards(List<LootItem> loots)
	{
		List<InventoryItem> lootItems = new List<InventoryItem>();     
		for (int i = 0; i < loots.Count; i++) 
		{
			float chance = Random.Range(0.0f, 1.0f);
			if(chance <= loots[i].dropRate)
			{
				Debug.Log(loots[i].itemType.ToString() + i);
				InventoryItem newItem = new InventoryItem();
				if(loots[i].itemType == ItemType.Currency)
				{
					//RPGCurrency currency = Storage.LoadById<RPGCurrency>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGCurrency());
					//newItem.rpgItem = currency;
				}
				else if(loots[i].itemType == ItemType.Armor)
				{
					RPGArmor armor = Storage.LoadById<RPGArmor>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGArmor());
					newItem.rpgItem = armor;
				}
				else if(loots[i].itemType == ItemType.Normal || loots[i].itemType == ItemType.UpgradeMaterials)
				{
					RPGItem item = Storage.LoadById<RPGItem>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGItem());
					newItem.rpgItem = item;
				}
				newItem.CurrentAmount = Random.Range(loots[i].minQuantity, loots[i].maxQuantity);
				newItem.UniqueItemId = newItem.rpgItem.UniqueId;
				newItem.Level = Random.Range(1, loots[i].itemLevel);
				lootItems.Add(newItem);
			}
		}
		
		Debug.Log("total loot items = " + lootItems.Count);
		GUIManager.Instance.DisplayRewards(lootItems);
	}

	public void GiveRewards(int magnets, List<LootItem> loots)
	{
		List<InventoryItem> lootItems = new List<InventoryItem>();

		if(magnets > 0)
		{
			/*InventoryItem newItem = new InventoryItem();
			RPGCurrency currency = Storage.LoadById<RPGCurrency>(1, new RPGCurrency());
			newItem.rpgItem = currency;
			newItem.CurrentAmount = magnets;
			newItem.UniqueItemId = newItem.rpgItem.UniqueId;
			newItem.Level = 1;
			lootItems.Add(newItem);*/
		}
		for (int i = 0; i < loots.Count; i++) 
		{
			float chance = Random.Range(0.0f, 1.0f);
			if(chance <= loots[i].dropRate)
			{
				Debug.Log(loots[i].itemType.ToString() + i);
				InventoryItem newItem = new InventoryItem();
				if(loots[i].itemType == ItemType.Currency)
				{
					//RPGCurrency currency = Storage.LoadById<RPGCurrency>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGCurrency());
					//newItem.rpgItem = currency;
				}
				else if(loots[i].itemType == ItemType.Armor)
				{
					RPGArmor armor = Storage.LoadById<RPGArmor>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGArmor());
					newItem.rpgItem = armor;
				}
				else if(loots[i].itemType == ItemType.Normal || loots[i].itemType == ItemType.UpgradeMaterials)
				{
					RPGItem item = Storage.LoadById<RPGItem>(loots[i].itemID[Random.Range(0, loots[i].itemID.Count)], new RPGItem());
					newItem.rpgItem = item;
				}
				newItem.CurrentAmount = Random.Range(loots[i].minQuantity, loots[i].maxQuantity);
				newItem.UniqueItemId = newItem.rpgItem.UniqueId;
				newItem.Level = Random.Range(1, loots[i].itemLevel);
				lootItems.Add(newItem);
			}
		}
		
		Debug.Log("total loot items = " + lootItems.Count);
		GUIManager.Instance.DisplayRewards(lootItems);
	}
}

public enum PlayerActivityState
{
	idle,
	minigame,
	shop,
	arena
}

public enum ContainerType
{
 Container = 1,
 CharacterBody = 2
}