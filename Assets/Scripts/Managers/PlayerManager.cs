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
    public int activeNPC;
    public string ActiveNPCName;
    public NPC ActiveNPC;
    public Shop ActiveShop;
	public WorldManager ActiveWorld;
	public Zone ActiveZone;

    public Transform SpawnPoint;

    public GameObject avatarPrefab;
    public GameObject avatarObject;
    public Avatar avatar;
    public InputController avatarInput;
    public CharacterStatus avatarStatus;
	public NetworkCharacterMovement avatarNetworkMovement;

 
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
		ActiveWorld = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
		ActiveZone = ActiveWorld.DefaultZone;
		SpawnPoint = ActiveZone.spawnPoint;

		avatarObject = PhotonNetwork.Instantiate("PlayerAvatar", SpawnPoint.position , Quaternion.identity, 0) as GameObject;
		//avatarObject = GameObject.Instantiate(Resources.Load("PlayerAvatar"), SpawnPoint.position, Quaternion.identity) as GameObject;
		//avatarObject.AddComponent<DontDestroy>();
		avatarStatus = avatarObject.GetComponent<CharacterStatus>();
		avatarInput = avatarObject.GetComponent<InputController>();
		CharacterMotor cm = avatarObject.GetComponent<CharacterMotor>();
		cm.enabled = true;
		avatarInput.enabled = true;
		avatarNetworkMovement = avatarObject.GetComponent<NetworkCharacterMovement>();
		UICamera.fallThrough = avatarObject;
		avatar = avatarObject.GetComponent<Avatar>();
		PlayerCamera.Instance.targetTransform = avatarObject.transform;
		LoadCharacterParts();
    }

    public void RefreshAvatar()
    {
		Debug.Log("refreshavatar");
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
		ActiveWorld = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
		ActiveZone = ActiveWorld.DefaultZone;
		SpawnPoint = ActiveZone.spawnPoint;
        avatarObject.transform.position = SpawnPoint.position;
        LoadCharacterParts();
        EnableAvatarInput();
    }

    void LoadCharacterParts()
    {
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedBody.rpgItem).FBXName, Hero.Equip.EquippedBody.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedLegs.rpgItem).FBXName, Hero.Equip.EquippedLegs.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedHead.rpgItem).FBXName, Hero.Equip.EquippedHead.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmL.rpgItem).FBXName, Hero.Equip.EquippedArmL.Slot);
        //avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmR.rpgItem).FBXName, Hero.Equip.EquippedArmR.Slot);
		avatar.LoadAllBodyParts(((RPGArmor)Hero.Equip.EquippedHead.rpgItem).FBXName, 
		                        ((RPGArmor)Hero.Equip.EquippedBody.rpgItem).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedArmL.rpgItem).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedArmR.rpgItem).FBXName,
		                        ((RPGArmor)Hero.Equip.EquippedLegs.rpgItem).FBXName);
    }

    public void EnableAvatarInput()
    {
        avatarInput.enabled = true;
    }

    public void DisableAvatarInput()
    {
        avatarInput.enabled = false;
    }
 
}

public enum ContainerType
{
 Container = 1,
 CharacterBody = 2
}