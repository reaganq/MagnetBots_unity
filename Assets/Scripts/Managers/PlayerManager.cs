using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

    /*public static PlayerManager Instance
    {
        get;
        private set;
    }*/

    public PlayerInformation Hero;
 //public GeneralPlayerSounds sounds;
 //public RPGScene scene;
 //public static bool isChangingScene;
 //public bool doEvents;
 
 //public static ContainerType Container;
 //public static RPGContainer rpgContainer;
 
 //public static float sellPriceModifier;
 
    public UsableItem currentItem;
    public NPC ActiveNPC;
    public Shop ActiveShop;
	public RPGArena SelectedArena;
	public WorldManager ActiveWorld;
	public Zone ActiveZone;
	public ArenaManager ActiveArena;
	public PlayerActivityState activityState;

    public Transform SpawnPoint;
    public GameObject avatarPrefab;

    public GameObject avatarObject;
    public Avatar avatar;
    public InputController avatarInput;
    public CharacterStatus avatarStatus;
	public NetworkCharacterMovement avatarNetworkMovement;
	public PhotonView avatarPhotonView;

	public List<int> partyMembers = new List<int>();
	public bool isPartyLeader = false;

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
		GameManager.Instance.GameHasStarted = true;
        Hero.StartNewGame();
		LoadAvatar();
    }
	
    public void LoadAvatar()
    {
		avatarObject = PhotonNetwork.Instantiate("PlayerAvatar", SpawnPoint.position , Quaternion.identity, 0) as GameObject;
		//avatarObject = GameObject.Instantiate(Resources.Load("PlayerAvatar"), SpawnPoint.position, Quaternion.identity) as GameObject;
		//avatarObject.AddComponent<DontDestroy>();
		avatarStatus = avatarObject.GetComponent<CharacterStatus>();
		avatarInput = avatarObject.GetComponent<InputController>();
		CharacterMotor cm = avatarObject.GetComponent<CharacterMotor>();
		cm.enabled = true;
		//avatarInput.enabled = true;
		avatarNetworkMovement = avatarObject.GetComponent<NetworkCharacterMovement>();
		UICamera.fallThrough = avatarObject;
		avatar = avatarObject.GetComponent<Avatar>();
		avatarPhotonView = avatarObject.GetComponent<PhotonView>();
		RefreshAvatar();
		//PlayerCamera.Instance.targetTransform = avatarObject.transform;
		//LoadCharacterParts();
    }

    public void RefreshAvatar()
    {
		//TODO OK THERE IS A PROBLEM HERE
        if(avatarObject == null)
        {
            StartNewGame();
			return;
        }

		if(avatarObject == null)
		{
			LoadAvatar();
			return;
		}

		Debug.Log(PhotonNetwork.isMessageQueueRunning);

        PlayerCamera.Instance.targetTransform = avatarObject.transform;

        avatarObject.transform.position = SpawnPoint.position;
        LoadCharacterParts();
        EnableAvatarInput();

		//TODO hacky party list refresh
		partyMembers.Clear();
		isPartyLeader = false;
    }

	public void ChangeWorld()
	{
		ActiveWorld = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
		ActiveZone = ActiveWorld.DefaultZone;
		SpawnPoint = ActiveZone.spawnPoint;
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
		if(newZone != null)
		{
			Zone oldzone = ActiveZone;
			newZone.gameObject.transform.GetChild(0).gameObject.SetActive(true);
			ActiveZone = newZone;
			SpawnPoint = ActiveZone.spawnPoint;
			avatarObject.transform.position = SpawnPoint.position;
			oldzone.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			ActiveArena = ActiveZone.gameObject.GetComponent<ArenaManager>();
			if(ActiveArena == null)
			{
				
				Debug.LogError("no bloody arena");
			}
			//int newid = PhotonNetwork.AllocateViewID();
			ActiveWorld.myPhotonView.RPC("AddPlayer", PhotonTargets.AllBuffered, ActiveArena.ID, avatarPhotonView.viewID);
			//TODO make the selected enemy dynamic
			//ActiveArena.gameObject.GetComponent<PhotonView>().RPC("Initialise", PhotonTargets.MasterClient, enemyID, avatarPhotonView.viewID, newid);
			GUIManager.Instance.TurnOffAllOtherUI();
			GUIManager.Instance.DisplayMainGUI();
			ResetNPC();
			activityState = PlayerActivityState.arena;
		}
	}

	public void LeaveArena(Zone newZone)
	{
		if(newZone != null)
		{
			Zone oldzone = ActiveZone;
			newZone.gameObject.transform.GetChild(0).gameObject.SetActive(true);
			ActiveZone = newZone;
			SpawnPoint = ActiveZone.spawnPoint;
			avatarObject.transform.position = SpawnPoint.position;
			oldzone.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			
			if(ActiveArena)
			{
				ActiveWorld.EndSession(ActiveArena);
				//ActiveArena.EndSession(avatarPhotonView.viewID);
				ActiveArena = null;
			}
			activityState = PlayerActivityState.idle;
		}
	}

	public void ResetNPC()
	{
		if(ActiveNPC != null)
			ActiveNPC.Reset();
	}

    void LoadCharacterParts()
    {
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedBody.rpgItem).FBXName, Hero.Equip.EquippedBody.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedLegs.rpgItem).FBXName, Hero.Equip.EquippedLegs.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedHead.rpgItem).FBXName, Hero.Equip.EquippedHead.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmL.rpgItem).FBXName, Hero.Equip.EquippedArmL.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmR.rpgItem).FBXName, Hero.Equip.EquippedArmR.Slot);
		avatar.LoadAllBodyParts(((RPGArmor)Hero.Equip.EquippedHead.rpgArmor).FBXName, 
		                        ((RPGArmor)Hero.Equip.EquippedBody.rpgArmor).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedArmL.rpgArmor).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedArmR.rpgArmor).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedLegs.rpgArmor).FBXName);
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