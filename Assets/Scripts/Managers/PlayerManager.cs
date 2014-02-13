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
                    PlayerManager prefab = Resources.Load("Managers/PlayerManager", typeof(PlayerManager)) as PlayerManager;
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

    public Transform SpawnPoint;

    public GameObject avatarPrefab;
    public GameObject avatarObject;
    public Avatar avatar;
    public InputController avatarInput;
    public CharacterStatus avatarStatus;
    public GameObject mainCamera;
    public PlayerCamera playerCamera;
 
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
        Debug.Log("started new game");
        Hero.StartNewGame();
        LoadAvatar();

    }

    public void LoadAvatar()
    {
     //cursorImage = (Texture2D)Resources.Load("Icon/Cross");
     //Data = new GeneralData();
        
        //spawnPoints = new List<SpawnPoint>();
        //sounds = new GeneralPlayerSounds();
     //LoadScene();
        SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        //avatarObject = PhotonNetwork.Instantiate("PlayerAvatar", SpawnPoint.position, Quaternion.identity, 0) as GameObject;
		avatarObject = GameObject.Instantiate(Resources.Load("PlayerAvatar"), SpawnPoint.position, Quaternion.identity) as GameObject;
        avatarObject.AddComponent<DontDestroy>();
        avatarStatus = avatarObject.GetComponent<CharacterStatus>();
        avatarInput = avatarObject.GetComponent<InputController>();
        avatar = avatarObject.GetComponent<Avatar>();
        RefreshAvatar();
    }

    public void RefreshAvatar()
    {
        if(avatarObject == null)
        {
            StartNewGame();
            return;
        }

        //mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //playerCamera = mainCamera.GetComponent<PlayerCamera>();
        //if(playerCamera != null)
            //playerCamera.targetTransform = avatarObject.transform;
        PlayerCamera.Instance.targetTransform = avatarObject.transform;
        SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        avatarObject.transform.position = SpawnPoint.position;
        LoadCharacterParts();
        EnableAvatarInput();
    }

    void LoadCharacterParts()
    {
        avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedBody.rpgItem).FBXName, Hero.Equip.EquippedBody.Slot);
        avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedLegs.rpgItem).FBXName, Hero.Equip.EquippedLegs.Slot);
        avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedHead.rpgItem).FBXName, Hero.Equip.EquippedHead.Slot);
        avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmL.rpgItem).FBXName, Hero.Equip.EquippedArmL.Slot);
        avatar.EquipBodyPart(((RPGArmor)Hero.Equip.EquippedArmR.rpgItem).FBXName, Hero.Equip.EquippedArmR.Slot);
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