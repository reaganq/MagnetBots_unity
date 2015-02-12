using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : MonoBehaviour {

	// Use this for initialization
    public PlayerMotor myMotor;
    public Animation animationTarget;
    public CharacterActionManager myActionManager;
    public PlayerCharacter myStatus;
	public PhotonView myPhotonView;
    
    public Transform PelvisBone;
    public Transform BodyRoot;
    public Transform ArmLRoot;
    public Transform ArmRRoot;
    public Transform HeadRoot;
    public Transform FaceRoot;
    public Transform LegsRoot;
    
    public List<GameObject> HeadObjects;
    public List<GameObject> BodyObjects;
    public List<GameObject> ArmLObjects;
    public List<GameObject> ArmRObjects;
    public List<GameObject> LegObjects;
    public List<GameObject> FaceObjects;

    private string LShoulderRootName = "bones:L_Arm_Hover_b";
    private string RShoulderRootName = "bones:R_Arm_Hover_b";
    private string LegsRootName = "bones:LegsRoot";
    public Transform _myTransform;

	//TODO add hitboxes to playercharacter list

	#region store bones
	public Transform clavicleL;
	public Transform shoulderL;
	public Transform shoulderGuardL;
	public Transform elbowL;
	public Transform forearmL;
	public Transform handL;
	public Transform clavicleR;
	public Transform shoulderR;
	public Transform shoulderGuardR;
	public Transform elbowR;
	public Transform forearmR;
	public Transform handR;
	public Transform spine2;
	public Transform neckHorizontal;

	public void LoadBones()
	{
		Transform[] kids = GetComponentsInChildren<Transform>();
		for (int i = 0; i < kids.Length; i++) {
			if(kids[i].name == "bones:R_Clavicle")
				clavicleR = kids[i];
			else if(kids[i].name == "bones:R_Shoulder")
				shoulderR = kids[i];
			else if(kids[i].name == "bones:R_ShoulderGuard")
				shoulderGuardR = kids[i];
			else if(kids[i].name == "bones:R_Elbow")
				elbowR = kids[i];
			else if(kids[i].name == "bones:R_Forearm")
				forearmR = kids[i];
			else if(kids[i].name == "bones:R_Hand")
				handR = kids[i];
			else if(kids[i].name == "bones:L_Clavicle")
				clavicleL = kids[i];
			else if(kids[i].name == "bones:L_Shoulder")
				shoulderL = kids[i];
			else if(kids[i].name == "bones:L_ShoulderGuard")
				shoulderGuardL = kids[i];
			else if(kids[i].name == "bones:L_Elbow")
				elbowL = kids[i];
			else if(kids[i].name == "bones:L_Forearm")
				forearmL = kids[i];
			else if(kids[i].name == "bones:L_Hand")
				handL = kids[i];
			else if(kids[i].name == "bones:Spine_2")
				spine2 = kids[i];
			else if(kids[i].name == "bones:Neck_Horizontal")
				neckHorizontal = kids[i];
		}
	}

	#endregion

	void Awake () {
        myMotor = GetComponent<PlayerMotor>();
        myActionManager = GetComponent<CharacterActionManager>();
        animationTarget = PelvisBone.GetComponent<Animation>();
        _myTransform = this.transform;
        myStatus = GetComponent<PlayerCharacter>();
		LoadBones();
	}

	public void RequestInitInfo()
	{
		myPhotonView.RPC("NetworkRequestInitInfo", myPhotonView.owner);
	}

	//send to owner
	[RPC]
	public void NetworkRequestInitInfo(PhotonMessageInfo info)
	{
		myPhotonView.RPC("NetworkInitialiseAvatar", info.sender, 
		                 PlayerManager.Instance.Hero.profile.name,
		                 PlayerManager.Instance.Hero.Equip.EquippedFace.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedFace.Level, PlayerManager.Instance.Hero.Equip.EquippedFace.rpgArmor.FBXName.Count) - 1],
		                 PlayerManager.Instance.Hero.Equip.EquippedHead == null? null : PlayerManager.Instance.Hero.Equip.EquippedHead.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedHead.Level, PlayerManager.Instance.Hero.Equip.EquippedHead.rpgArmor.FBXName.Count) - 1], 
		                 PlayerManager.Instance.Hero.Equip.EquippedBody.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedBody.Level, PlayerManager.Instance.Hero.Equip.EquippedBody.rpgArmor.FBXName.Count) - 1], 
		                 PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedArmL.Level, PlayerManager.Instance.Hero.Equip.EquippedArmL.rpgArmor.FBXName.Count) - 1], 
		                 PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedArmR.Level, PlayerManager.Instance.Hero.Equip.EquippedArmR.rpgArmor.FBXName.Count) - 1], 
		                 PlayerManager.Instance.Hero.Equip.EquippedLegs.rpgArmor.FBXName[Mathf.Min(PlayerManager.Instance.Hero.Equip.EquippedLegs.Level, PlayerManager.Instance.Hero.Equip.EquippedLegs.rpgArmor.FBXName.Count) - 1]
		                 );
	}

	public void LoadAllBodyParts(string characterName, string facePath, string headPath, string bodyPath, string armLPath, string armRPath, string legsPath)
	{
		myPhotonView.RPC("NetworkInitialiseAvatar", PhotonTargets.All, characterName, facePath, headPath, bodyPath, armLPath, armRPath, legsPath);
	}

	[RPC]
	public void NetworkInitialiseAvatar(string characterName, string facePath, string headPath, string bodyPath, string armLPath, string armRPath, string legsPath)
	{
		SpawnFace(facePath);
		if(!string.IsNullOrEmpty(headPath))
			SpawnHead(headPath);
		SpawnBody(bodyPath);
		SpawnArmL(armLPath);
		SpawnArmR(armRPath);
		SpawnLegs(legsPath);
		LoadBones();
		myStatus.UpdateNameTag(characterName);
		animationTarget.Play("Default_Idle");
	}

    public void EquipBodyPart(string objectpath, EquipmentSlots slot)
    {
        switch(slot)
        {
            case EquipmentSlots.Head:
			myPhotonView.RPC("SpawnHead", PhotonTargets.All, objectpath);
            //SpawnHead(objectpath);
                break;
            case EquipmentSlots.Body:
			myPhotonView.RPC("SpawnBody", PhotonTargets.All, objectpath);
            //SpawnBody(objectpath);
                break;
            case EquipmentSlots.ArmL:
			myPhotonView.RPC("SpawnArmL", PhotonTargets.All, objectpath);
            //SpawnArmL(objectpath);
                break;
            case EquipmentSlots.ArmR:
			myPhotonView.RPC("SpawnArmR", PhotonTargets.All, objectpath);
            //SpawnArmR(objectpath);
                break;
            case EquipmentSlots.Legs:
			myPhotonView.RPC("SpawnLegs", PhotonTargets.All, objectpath);
            //SpawnLegs(objectpath);
                break;
            case EquipmentSlots.Face:
			myPhotonView.RPC("SpawnFace", PhotonTargets.All, objectpath);
                break;
        }
    }

	[RPC]
    public void SpawnFace(string objectpath)
    {
        if(FaceObjects.Count > 0)
        {
			for (int i = 0; i < FaceObjects.Count ; i++) {
				Destroy(FaceObjects[i]);
            }
            FaceObjects.Clear();
        }

        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = BodyRoot;
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localPosition = Vector3.zero;
        
        //move bones to new bones position
        SkinnedMeshRenderer[] BonedObjects = temp.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach( SkinnedMeshRenderer smr in BonedObjects )
        {
            FaceObjects.Add(ProcessBonedObject( smr ));
        }
        
        Destroy(temp);
    }

	[RPC]
    public void SpawnHead(string objectpath)
    {
        if(HeadObjects.Count > 0)
        {

            for (int i = 0; i < HeadObjects.Count; i++) {
                Destroy(HeadObjects[i]); 
            }
            HeadObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = HeadRoot;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localRotation = Quaternion.Euler(-90, 90, 0);
        HeadObjects.Add(temp);

    }
   
#region SpawnBody
	[RPC]
    public void SpawnBody(string objectpath)
    {        
        if(BodyObjects.Count > 0)
        {
            for (int i = 0; i < BodyObjects.Count; i++) {
                Destroy(BodyObjects[i]);
            }
            BodyObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = BodyRoot;
        temp.transform.localRotation = Quaternion.identity;
        temp.transform.localPosition = Vector3.zero;

        //move bones to new bones position
        SkinnedMeshRenderer[] BonedObjects = temp.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach( SkinnedMeshRenderer smr in BonedObjects )
        {
            BodyObjects.Add(ProcessBonedObject( smr ));
        }
        
        Destroy(temp);
		LoadBones();
    }
    
    private GameObject ProcessBonedObject( SkinnedMeshRenderer ThisRenderer )
    {       
     // Create the SubObject
        GameObject bodyMesh = new GameObject( ThisRenderer.gameObject.name );    
        bodyMesh.transform.parent = _myTransform;
        bodyMesh.transform.localRotation = Quaternion.identity;
        bodyMesh.transform.localPosition = Vector3.zero;
        
        // Add the renderer
        SkinnedMeshRenderer NewRenderer = bodyMesh.AddComponent( typeof( SkinnedMeshRenderer ) ) as SkinnedMeshRenderer;
        
        // Assemble Bone Structure  
        Transform[] MyBones = new Transform[ ThisRenderer.bones.Length ];

        // As clips are using bones by their names, we find them that way.
        for( int i = 0; i < ThisRenderer.bones.Length; i++ )
        {
            MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, _myTransform );
        }
    //
        //Debug.Log("mybones: "+MyBones.Length);
        NewRenderer.bones = MyBones;    
        NewRenderer.sharedMesh = ThisRenderer.sharedMesh;   
        NewRenderer.materials = ThisRenderer.materials; 
        return bodyMesh;
    }

    // Recursive search of the child by name.
    private Transform FindChildByName( string ThisName, Transform ThisGObj )    
    {   
        Transform ReturnObj;
       
        // If the name match, we're return it
        if( ThisGObj.name == ThisName ) 
            return ThisGObj.transform;
        
        // Else, we go continue the search horizontaly and verticaly
        foreach( Transform child in ThisGObj )  
        {   
            ReturnObj = FindChildByName( ThisName, child );
        
            if( ReturnObj != null ) 
                return ReturnObj;   
        }
        
        return null;    
    }
#endregion
	[RPC]
    public void SpawnArmL(string objectpath)
    {
        if(ArmLObjects.Count > 0)
        {
			for (int i = 0; i < myActionManager.armorSkills.Count; i++) {
				if(myActionManager.armorSkills[i].equipmentSlotIndex == 2)
				{
					myActionManager.armorSkills[i].UnEquip();
				}
			}
            if(myActionManager.armorAnimControllers[2] != null)
            {
                myActionManager.armorAnimControllers[2].RemoveAnimations();
                myActionManager.armorAnimControllers[2] = null;
            }
            for (int i = 0; i < ArmLObjects.Count; i++) {
                Destroy(ArmLObjects[i]);
            }
            ArmLObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
        temp.transform.parent = ArmLRoot;
        for (int i = 0; i < temp.transform.childCount; i++) {
            if(temp.transform.GetChild(i).name == LShoulderRootName)
            {
                ArmLObjects.Add(temp.transform.GetChild(i).gameObject);
                PositionArmL(temp.transform.GetChild(i));
            }
        }
		LoadBones();
		BasePlayerSkill armLcontroller = temp.GetComponent<BasePlayerSkill>();
        if(armLcontroller != null)
        {
			myActionManager.AddSkill(armLcontroller);
        }
		else
			GUIManager.Instance.MainGUI.DisableActionButton(0);

        PassiveArmorAnimationController armLAnimController = temp.GetComponent<PassiveArmorAnimationController>();
        if(armLAnimController != null)
        {
            armLAnimController.TransferAnimations(animationTarget, this);
			myActionManager.AddPassiveAnimation(armLAnimController, 2);
        }
        ArmLObjects.Add(temp);
    }
    
    public void PositionArmL(Transform obj)
    {
        obj.parent = ArmLRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmLRoot.parent;
    }
    
	[RPC]
    public void SpawnArmR(string objectpath)
    {
        if(ArmRObjects.Count > 0)
        {
            for (int i = 0; i < myActionManager.armorSkills.Count; i++) {
            	if(myActionManager.armorSkills[i].equipmentSlotIndex == 3)
				{
					myActionManager.armorSkills[i].UnEquip();
				}
            }
            if(myActionManager.armorAnimControllers[3] != null)
            {
                myActionManager.armorAnimControllers[3].RemoveAnimations();
                myActionManager.armorAnimControllers[3] = null;
            }
            for (int i = 0; i < ArmRObjects.Count; i++) {
                Destroy(ArmRObjects[i]);
            }
            ArmRObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
        temp.transform.parent = ArmRRoot;
        for (int i = 0; i < temp.transform.childCount; i++) {
            if(temp.transform.GetChild(i).name == RShoulderRootName)
            {
                ArmRObjects.Add(temp.transform.GetChild(i).gameObject);
                PositionArmR(temp.transform.GetChild(i));
            }
        }
		LoadBones();
		BasePlayerSkill armRcontroller = temp.GetComponent<BasePlayerSkill>();
        if(armRcontroller != null)
        {
            //armRcontroller.Initialise(myStatus, 3);
			myActionManager.AddSkill(armRcontroller);
            //Debug.Log("transfer animation");
        }
		else
			GUIManager.Instance.MainGUI.DisableActionButton(1);

        PassiveArmorAnimationController armRAnimController = temp.GetComponent<PassiveArmorAnimationController>();
        if(armRAnimController != null)
        {
            armRAnimController.TransferAnimations(animationTarget, this);
            //animManager.
			myActionManager.AddPassiveAnimation(armRAnimController, 3);
        }

        ArmRObjects.Add(temp);
    }
    
    public void PositionArmR(Transform obj)
    {
        obj.parent = ArmRRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmRRoot.parent;
    }
	[RPC]
    public void SpawnLegs(string objectpath)
    {
        if(LegObjects.Count > 0)
        {
            for (int i = 0; i < LegObjects.Count ; i++) {
                Destroy(LegObjects[i]);
            }
            LegObjects.Clear();
        }
        
        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = transform;
        temp.transform.localPosition = Vector3.zero;

        GameObject pelvis = temp.transform.FindChild(PelvisBone.name).gameObject;
        pelvis.transform.parent = PelvisBone;
        pelvis.transform.localPosition = Vector3.zero;
        pelvis.transform.localRotation = Quaternion.identity;

        GameObject legRoot = pelvis.transform.FindChild(LegsRootName).gameObject;
        legRoot.transform.parent = PelvisBone;
		LoadBones();

        LegObjects.Add(pelvis);
        LegObjects.Add(legRoot);
        /*for (int i = 0; i < temp.transform.childCount ; i++) {
            if(temp.transform.GetChild(i).name == PelvisBone.name)
            {
                LegObjects.Add(temp.transform.GetChild(i).gameObject);
                temp.transform.GetChild(i).parent = PelvisBone;
                temp.transform.GetChild(i).localPosition = Vector3.zero;
                temp.transform.GetChild(i).localRotation = Quaternion.identity;

                temp.transform.GetChild(i).GetChild(i).parent = PelvisBone;
                //PositionLegs(temp.transform.GetChild(i).FindChild(LegsRootName).transform);
                //Destroy(temp.transform.GetChild(i).gameObject);
            }
        }*/
        LegObjects.Add(temp);
        //Destroy(temp);
        //PositionLegs(boneObj);
        //Destroy(temp);
        //animationController.UpdateAnimation();
    }
    
    public void PositionLegs(Transform obj)
    {
        obj.parent = PelvisBone;
        //obj.parent = LegsRoot.parent;
    }
}
