using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DummyAvatar : MonoBehaviour {

	public Transform PelvisBone;
	public Transform BodyRoot;
	public Transform ArmLRoot;
	public Transform ArmRRoot;
	public Transform HeadRoot;
	public Transform FaceRoot;
	public Transform LegsRoot;

	public bool face;
	public bool body;
	public bool armL;
	public bool armR;
	public bool legs;
	
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
	public Animation animationTarget;

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

	void Awake()
	{
		LoadBones();
	}

	public void EquipBodyPart(string objectpath, EquipmentSlots slot)
	{
		switch(slot)
		{
		case EquipmentSlots.Head:
			NetworkSpawnHead(objectpath);
			//SpawnHead(objectpath);
			break;
		case EquipmentSlots.Body:
			//
			if(!body)
				StartCoroutine(SpawnBody(objectpath));
			else
				NetworkSpawnBody(objectpath);
			break;
		case EquipmentSlots.ArmL:
			//NetworkSpawnArmL(objectpath);
			StartCoroutine(SpawnArmL(objectpath));
			break;
		case EquipmentSlots.ArmR:
			//NetworkSpawnArmR(objectpath);
			StartCoroutine(SpawnArmR(objectpath));
			break;
		case EquipmentSlots.Legs:
			//NetworkSpawnLegs(objectpath);
			//SpawnLegs(objectpath);
			if(!legs)
				StartCoroutine(SpawnLegs(objectpath));
			else
				NetworkSpawnLegs(objectpath);
			break;
		case EquipmentSlots.Face:
			//SpawnFace(objectpath);
			StartCoroutine(SpawnFace(objectpath));
			break;
		}
	}

	public IEnumerator SpawnFace(string objectpath)
	{
		NetworkSpawnFace(objectpath);
		animationTarget.Play("head_reaction");
		animationTarget.Play("head_split");
		yield return new WaitForSeconds(animationTarget["head_reaction"].length);
		//yield return null;
	}

	public void NetworkSpawnFace(string objectpath)
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

	public void NetworkSpawnHead(string objectPath)
	{
		if(HeadObjects.Count > 0)
		{
			
			for (int i = 0; i < HeadObjects.Count; i++) {
				Destroy(HeadObjects[i]); 
			}
			HeadObjects.Clear();
		}
		
		GameObject temp = Instantiate(Resources.Load(objectPath) as GameObject) as GameObject;
		temp.transform.parent = HeadRoot;
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localRotation = Quaternion.Euler(-90, 90, 0);
		HeadObjects.Add(temp);
	}

	public IEnumerator SpawnBody(string objectpath)
	{
		animationTarget.Play("body_reaction");
		animationTarget.Play("body_split");
		yield return new WaitForSeconds(animationTarget["body_reaction"].length * 0.5f);
		NetworkSpawnBody(objectpath);
	}

	public void NetworkSpawnBody(string objectpath)
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

	public IEnumerator SpawnArmL(string objectpath)
	{
		animationTarget.Play("armL_split");
		animationTarget.Play("armL_reaction");
		yield return new WaitForSeconds(animationTarget["armL_split"].length*0.5f);
		NetworkSpawnArmL(objectpath);
	}

	public void NetworkSpawnArmL(string objectpath)
	{
		if(ArmLObjects.Count > 0)
		{
			for (int i = 0; i < ArmLObjects.Count; i++) {
				Destroy(ArmLObjects[i]);
			}
			ArmLObjects.Clear();
		}
		
		GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
		temp.transform.parent = ArmLRoot;
		temp.transform.localPosition = Vector3.zero;
		
		GameObject arml = temp.transform.FindChild(LShoulderRootName).gameObject;
		arml.transform.parent = ArmLRoot;
		arml.transform.localPosition = Vector3.zero;
		arml.transform.localRotation = Quaternion.identity;
		arml.transform.parent = ArmLRoot.parent;
		
		ArmLObjects.Add(temp);
		ArmLObjects.Add(arml);
		LoadBones();
	}

	public IEnumerator SpawnArmR(string objectpath)
	{
		animationTarget.Play("armR_split");
		animationTarget.Play("armR_reaction");
		yield return new WaitForSeconds(animationTarget["armR_split"].length*0.5f);
		NetworkSpawnArmR(objectpath);
	}

	public void NetworkSpawnArmR(string objectpath)
	{
		if(ArmRObjects.Count > 0)
		{
			for (int i = 0; i < ArmRObjects.Count; i++) {
				Destroy(ArmRObjects[i]);
			}
			ArmRObjects.Clear();
		}
		
		GameObject temp = Instantiate(Resources.Load(objectpath) as GameObject) as GameObject;  
		temp.transform.parent = ArmRRoot;
		temp.transform.localPosition = Vector3.zero;
		
		GameObject armr = temp.transform.FindChild(RShoulderRootName).gameObject;
		armr.transform.parent = ArmRRoot;
		armr.transform.localPosition = Vector3.zero;
		armr.transform.localRotation = Quaternion.identity;
		armr.transform.parent = ArmRRoot.parent;
		
		ArmRObjects.Add(temp);
		ArmRObjects.Add(armr);
		
		LoadBones();
	}

	public IEnumerator SpawnLegs(string objectpath)
	{
		animationTarget.Play("legs_reaction");
		animationTarget.Play("legs_split");
		yield return new WaitForSeconds(animationTarget["legs_reaction"].length * 0.5f);
		NetworkSpawnLegs(objectpath);
	}

	public void NetworkSpawnLegs(string objectpath)
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
		LegObjects.Add(temp);
	}
}
