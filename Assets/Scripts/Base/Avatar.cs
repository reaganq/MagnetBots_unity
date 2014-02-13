using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Avatar : MonoBehaviour {

	// Use this for initialization
    public CharacterMotor characterMotor;
    public Animation animationTarget;
    public CharacterActionManager actionManager;
    public CharacterStatus myStatus;
    
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
    //private string HeadRootName = "bones:Head_Hover_b";
    public Transform _myTransform;

	void Awake () {
        characterMotor = GetComponent<CharacterMotor>();
        actionManager = GetComponent<CharacterActionManager>();
        animationTarget = PelvisBone.GetComponent<Animation>();
        _myTransform = this.transform;
        myStatus = GetComponent<CharacterStatus>();
	}

    public void EquipBodyPart(string objectpath, EquipmentSlots slot)
    {
        switch(slot)
        {
            case EquipmentSlots.Head:
            SpawnHead(objectpath);
                break;
            case EquipmentSlots.Body:
            SpawnBody(objectpath);
                break;
            case EquipmentSlots.ArmL:
            SpawnArmL(objectpath);
                break;
            case EquipmentSlots.ArmR:
            SpawnArmR(objectpath);
                break;
            case EquipmentSlots.Legs:
            SpawnLegs(objectpath);
                break;
            case EquipmentSlots.Face:
            SpawnFace(objectpath);
                break;
        }
    }

    public void SpawnFace(string objectpath)
    {
        if(FaceObjects.Count > 0)
        {
            for (int i = 0; i < HeadObjects.Count ; i++) {
                Destroy(HeadObjects[i]);
            }
            HeadObjects.Clear();
        }

        GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;
        temp.transform.parent = _myTransform;
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
        
        //Debug.Log("bone number" + ThisRenderer.bones.Length);
    
        /*foreach(Transform bone in ThisRenderer.bones)
    {
        Debug.Log(bone.ToString());
        
    }*/
        // As clips are using bones by their names, we find them that way.
        for( int i = 0; i < ThisRenderer.bones.Length; i++ )
        {
            //Debug.Log(ThisRenderer.bones[i].name);
            MyBones[ i ] = FindChildByName( ThisRenderer.bones[ i ].name, transform );
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
    
    public void SpawnArmL(string objectpath)
    {
        if(ArmLObjects.Count > 0)
        {
            if(actionManager.armorControllers[2] != null)
            {
                actionManager.armorControllers[2].UnEquip();
                actionManager.armorControllers[2] = null;
            }
            if(actionManager.armorAnimControllers[2] != null)
            {
                actionManager.armorAnimControllers[2].RemoveAnimations();
                actionManager.armorAnimControllers[2] = null;
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
        ArmorSkill armLcontroller = temp.GetComponent<ArmorSkill>();
        if(armLcontroller != null)
        {
            armLcontroller.Initialise(animationTarget, _myTransform, (Collider)characterMotor.controller, myStatus, actionManager );

            Debug.Log("transfer animation");
        }

        PassiveArmorAnimationController armLAnimController = temp.GetComponent<PassiveArmorAnimationController>();
        if(armLAnimController != null)
        {
            armLAnimController.TransferAnimations(animationTarget, _myTransform);
        }

        actionManager.AddArmorcontroller(armLcontroller, armLAnimController, 2);
        ArmLObjects.Add(temp);
        //ArmorController armLcontroller = ArmL.GetComponent<ArmorController>();

        /*if(armLcontroller != null)
        {
            armLcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[2] = armLcontroller;
        }*/
        
        //animationController.UpdateAnimation();
    }
    
    public void PositionArmL(Transform obj)
    {
        obj.parent = ArmLRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmLRoot.parent;
    }
    
    public void SpawnArmR(string objectpath)
    {
        if(ArmRObjects.Count > 0)
        {
            if(actionManager.armorControllers[3] != null)
            {
                actionManager.armorControllers[3].UnEquip();
                actionManager.armorControllers[3] = null;
            }
            if(actionManager.armorAnimControllers[3] != null)
            {
                actionManager.armorAnimControllers[3].RemoveAnimations();
                actionManager.armorAnimControllers[3] = null;
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

        ArmorSkill armRcontroller = temp.GetComponent<ArmorSkill>();
        if(armRcontroller != null)
        {
            armRcontroller.Initialise(animationTarget, _myTransform, characterMotor.controller, myStatus, actionManager);

            //Debug.Log("transfer animation");
        }

        PassiveArmorAnimationController armRAnimController = temp.GetComponent<PassiveArmorAnimationController>();
        if(armRAnimController != null)
        {
            armRAnimController.TransferAnimations(animationTarget, _myTransform);
            //animManager.
        }

        actionManager.AddArmorcontroller(armRcontroller, armRAnimController,3);
        ArmRObjects.Add(temp);
        /*if(armRcontroller != null)
        {
            armRcontroller.TransferAnimations(animationTarget, animationController);
            animationController.ArmorControllers[3] = armRcontroller;
            
        }*/

        //animationController.UpdateAnimation();
    }
    
    public void PositionArmR(Transform obj)
    {
        obj.parent = ArmRRoot;
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        obj.parent = ArmRRoot.parent;
    }
    
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
