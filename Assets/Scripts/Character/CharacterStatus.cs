using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CharacterStatus : MonoBehaviour {

	public string characterName;
	public bool isAI = false;
    public float MaxHealth = 0f;
    public float CurrentHealth = 0f;
    public CharacterActionManager ActionManager;
	public SimpleFSM fsm;
    public CharacterMotor Motor;
    public float movementSpeed;
	public float rotationSpeed;
    //can't be disjointed
    public bool Invulnerable = false;
    //cant be disjointed or damaged
    public bool Invincinble = false;
    public bool canMove = true;
	public PhotonView myPhotonView;

	public List<Collider> hitboxes;

	private string characters = "abcdefghijklmnopqrstuvwxyz";

	public bool isAlive()
	{
		if(CurrentHealth <= 0)
			return false;
		else
			return true;
	}

	void Awake()
	{
		characterName = GenerateRandomString(6);
		if(isAI)
			fsm = GetComponent<SimpleFSM>();
		else
			ActionManager = GetComponent<CharacterActionManager>();
	}
	// Use this for initialization
	void Start () 
	{
        CurrentHealth = MaxHealth;
		myPhotonView = GetComponent<PhotonView>();
		Collider[] colliders = GetComponentsInChildren<Collider>();
		for (int i = 0; i <	colliders.Length; i++) 
		{
			if(colliders[i].gameObject.layer == 13)
			{
				hitboxes.Add(colliders[i]);
			}
		}
		HitBox[] hbs = GetComponentsInChildren<HitBox>();
		for (int i = 0; i < hbs.Length; i++) 
		{
			hbs[i].ownerCS = this;
		}
	}

	/*public void Update()
	{
		if(myPhotonView.owner == null)
			Debug.Log("wtf no owner");
	}*/

	public string GenerateRandomString(int l)
	{
		string name = "";
		for (int i = 0; i < l; i++) 
		{
			int a = Random.Range(0, characters.Length);
			name = name + characters[a];
		}
		return name;
	}

	[RPC]
	public void ReceiveHit(byte[] hit)
	{
		HitInfo receivedHit = new HitInfo();
		
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(hit);
		receivedHit = (HitInfo)bb.Deserialize(mm);
		Debug.Log(receivedHit.sourceName + receivedHit.damage);
		//TODO apply self buffs/debuffs to calculate final hit results
		//TODO apply hit effects
		if(myPhotonView.isMine)
		{
			Debug.Log("received Damage");
			myPhotonView.RPC("NetworkSyncHealth", PhotonTargets.Others, receivedHit.damage);
			ReceiveDamage(receivedHit.damage);
		}

		if(myPhotonView.owner == null)
		{
			ReceiveDamage(receivedHit.damage);
		}
		//myPhotonView.RPC("ApplyReceivedHitEffects", PhotonTargets.All, 
	}
	
    public void ReceiveDamage(float damage)
    {
		if(CurrentHealth >0)
		{
	        CurrentHealth -= damage;
			Debug.Log("currentHP: "+CurrentHealth);
	        if(CurrentHealth <= 0)
	        {
				if(isAI)
				{
					DieAI();
				}
				else
				{
	            	Die();
				}
	        }
		}
    }

	//others
	[RPC]
	public void NetworkSyncHealth(float damage)
	{
		CurrentHealth -= damage;
	}

    public void Heal(int hp)
    {
        CurrentHealth += hp;
        if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void ChangeMovementSpeed(float change)
    {
        movementSpeed += change;
        if(ActionManager != null)
        {
            ActionManager.motor.AnimationUpdate();
        }
    }
	
    public void Die()
    {
        Debug.Log("died");
		if(GetComponent<PhotonView>().isMine)
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
    }

	[RPC]
	public void DieAI()
	{
		fsm.state = SimpleFSM.AIState.Dead;
	}

    public void EnableMovement(bool state)
    {
        canMove = state;
    }
}

[System.Serializable]
public class HitInfo
{
	public string sourceName;
	public float hitPosX;
	public float hitPosY;
	public float hitPosZ;
	public float damage;
	public List<SkillEffect> skillEffects;
}

