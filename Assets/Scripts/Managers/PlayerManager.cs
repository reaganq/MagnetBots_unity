using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
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
	public NPCMinigame ActiveMinigame;
	public GameObject ActiveMinigameObject;
	public WorldManager ActiveWorld;
	public Zone _activeZone;
	public Zone ActiveZone
	{
		get{ return _activeZone; }
		set{
			if(value != _activeZone)
			{
				if(_activeZone != null)
				{
					_activeZone.LeaveZone();
					if(_activeZone.zoneType == ZoneType.town)
						cachedTownPosition = avatarActionManager._myTransform.position;
				}

				_activeZone = value;
				_activeZone.EnterZone();
				if(avatarStatus != null)
					avatarStatus.DisplayInfoByZone();
				GUIManager.Instance.DisplayMainGUI();
			}
		}
	}
	public Vector3 cachedTownPosition;
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

	public List<PartyMemberData> _partyMembers = new List<PartyMemberData>();
	public List<bool> partyChallengeReplies = new List<bool>();
	public List<PartyMemberData> partyMembers {
		get {
			return _partyMembers;
		}
		set {
			_partyMembers = value;
			GUIManager.Instance.MainGUI.UpdatePartyMembers();
			Debug.Log("updated partymembers");
		}
	}
	public bool haveAllTeamReplies()
	{
		if(partyChallengeReplies.Count == partyMembers.Count)
			return true;
		else
			return false;
	}
	public bool shouldStartPartyChallenge()
	{
		for (int i = 0; i < partyChallengeReplies.Count; i++) {
			if(partyChallengeReplies[i] == false)
				return false;
				}
		return true;
	}
	public bool startedPartyChallenge;

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
		if(NetworkManager.Instance.offlineMode && !GameManager.Instance.newGame)
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
		Invoke("EquipStartupItems", 0.1f);
    }

	public void EquipStartupItems()
	{
		Hero.EquipItem(Hero.ArmoryInventory.Items[15]);
		Hero.EquipItem(Hero.ArmoryInventory.Items[16]);
		Hero.EquipItem(Hero.ArmoryInventory.Items[17]);
	}

	public void ChangeWorld()
	{
		ActiveWorld = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();

		SpawnPoint = ActiveWorld.DefaultZone.spawnPoint;
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
			ActiveZone = ActiveWorld.DefaultZone;
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

	public void GoToZone(Zone newZone)
	{
		StartCoroutine(GotoZoneSequence(newZone));
	}

	public IEnumerator GotoZoneSequence(Zone newZone)
	{
		Debug.Log("go to new arena");
		GUIManager.Instance.loadingGUI.Enable();
		yield return new WaitForSeconds(1.5f);
		ActiveZone = newZone;

		if(ActiveZone.zoneType == ZoneType.arena)
		{
			ActiveArena = ActiveZone.gameObject.GetComponent<ArenaManager>();
			if(ActiveArena == null)
			{
				Debug.LogError("no bloody arena");
			}
			activityState = PlayerActivityState.arena;
			avatarObject.transform.position = SpawnPoint.position;
		}
		else if(ActiveZone.zoneType == ZoneType.town)
		{
			activityState = PlayerActivityState.idle;
			avatarObject.transform.position = cachedTownPosition;
		}
	}

	public void LeaveArena()
	{
		StartCoroutine(GotoZoneSequence(ActiveWorld.DefaultZone));
	}

	public void UseItem(InventoryItem item)
	{
		if(item.rpgItem.ItemCategory == ItemType.Food)
		{
			EatFood(item.rpgItem.FBXName[0]);
		}
		else if(item.rpgItem.ItemCategory == ItemType.Toys)
		{
			PlayToy(item.rpgItem.FBXName[0]);
		}
		if (item.rpgItem.isLimitedUse) 
		{
			Debug.Log("removing 1 of this item");
			Hero.RemoveItem (item, 1);
		}
	}

	public void PlayToy(string prefabPath)
	{
		avatarActionManager.PlayToy(prefabPath);
		//increase happiness;
		//spawn the toy prefab. toy has a self timer
		//play correct animation
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
		ActiveMinigame = (NPCMinigame)activeActivity;
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
		                        Hero.Equip.EquippedLegs.rpgArmor.FBXName[Mathf.Min(Hero.Equip.EquippedLegs.Level, Hero.Equip.EquippedLegs.rpgArmor.FBXName.Count) - 1],
		                        Hero.Equip.EquippedHead == null? Hero.Equip.EquippedFace.rpgArmor.headPortraitPath : Hero.Equip.EquippedHead.rpgArmor.headPortraitPath);
    }
	
    public void EnableAvatarInput()
    {
        avatarInput.enabled = true;
    }

    public void DisableAvatarInput()
    {
        avatarInput.enabled = false;
    }
	
	//party leader send to party members when they join
	public LootItemList GiveRewards(List<LootItem> allLoots, int maxNumberOfLoots)
	{
		LootItemList lootItemList = new LootItemList();  
		int num = 0;
		for (int i = 0; i < allLoots.Count; i++) { 
			if(!allLoots[i].Validate())
				continue;
			float chance = UnityEngine.Random.Range(0.0f, 1.0f);
			if(chance <= allLoots[i].dropRate)
			{
				int index = UnityEngine.Random.Range(0, allLoots[i].itemID.Count);
				if(allLoots[i].itemType == ItemType.Currency)
				{
					RPGCurrency currency = Storage.LoadById<RPGCurrency>(allLoots[i].itemID[index], new RPGCurrency());
					currency.amount = UnityEngine.Random.Range(allLoots[i].minQuantity, allLoots[i].maxQuantity);
					lootItemList.currencies.Add(currency);
				}
				else if(allLoots[i].itemType == ItemType.Badge)
				{
					RPGBadge badge = Storage.LoadById<RPGBadge>(allLoots[i].itemID[index], new RPGBadge());
					lootItemList.badges.Add(badge);
				}
				else 
				{
					InventoryItem newItem = new InventoryItem();
					if(allLoots[i].itemType == ItemType.Armor)
					{
						RPGArmor armor = Storage.LoadById<RPGArmor>(allLoots[i].itemID[index], new RPGArmor());
						newItem.rpgItem = armor;
					}
					else
					{
						RPGItem item = Storage.LoadById<RPGItem>(allLoots[i].itemID[index], new RPGItem());
						newItem.rpgItem = item;
					}
					newItem.CurrentAmount = UnityEngine.Random.Range(allLoots[i].minQuantity, allLoots[i].maxQuantity);
					newItem.UniqueItemId = newItem.rpgItem.UniqueId;
					newItem.Level = UnityEngine.Random.Range(1, allLoots[i].itemLevel);
					lootItemList.items.Add(newItem);
				}
				if(!allLoots[i].definiteDrop)
					num ++;
			}
			if(maxNumberOfLoots > 0 && num >= maxNumberOfLoots)
				break;
		}
		GUIManager.Instance.DisplayArenaRewards(lootItemList);
		for (int i = 0; i < lootItemList.items.Count; i++) {
			Hero.AddItem(lootItemList.items[i]);
		}
		for (int i = 0; i < lootItemList.currencies.Count; i++) {
			Hero.AddRPGCurrency(lootItemList.currencies[i]);
		}
		for (int i = 0; i < lootItemList.badges.Count; i++) {
			Hero.AddBadge(lootItemList.badges[i]);
		}

		Debug.Log("gave rewards");
		return lootItemList;
	}
}

public enum PlayerActivityState
{
	idle,
	minigame,
	shop,
	arena
}

[Serializable]
public class PartyMemberData
{
	public int playerID;
	public int viewID;
}

public enum ContainerType
{
 Container = 1,
 CharacterBody = 2
}