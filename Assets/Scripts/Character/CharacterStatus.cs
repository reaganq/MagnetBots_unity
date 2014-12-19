using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CharacterStatus : CharacterAttributes {

	public string characterName;
    public Motor motor;
	public ActionManager actionManager;
	public PhotonView myPhotonView;
	public CharacterType enemyCharacterType;
	public List<Collider> hitboxes;
	public Transform _myTransform;
	public List<StatusEffect> statusEffects;
	
	public bool Invulnerable = false;
	public bool canMove = true;

	private string characters = "abcdefghijklmnopqrstuvwxyz";

	public virtual void Awake () 
	{
		curHealth = maxHealth;
		curMovementSpeed = maxMovementSpeed;

		_myTransform = this.transform;
		actionManager = GetComponent<ActionManager>();
		myPhotonView = GetComponent<PhotonView>();

		UpdateHitBoxes();

	}

	public void UpdateHitBoxes()
	{
		HitBox[] hbs = GetComponentsInChildren<HitBox>();
		for (int i = 0; i < hbs.Length; i++) 
		{
			hbs[i].ownerCS = this;
			if(!hitboxes.Contains(hbs[i].collider))
				hitboxes.Add(hbs[i].collider);
		}
	}
	
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

	public bool isAlive()
	{
		if(curHealth <= 0)
			return false;
		else
			return true;
	}

	/*public void ProcessHitEffects(List<StatusEffectData> effects, Vector3 originPos)
	{
		for (int i = 0; i < effects.Count; i++) {
			switch(effects[i].effectType)
			{
			case((int)StatusEffectCategory.knockback):
				Debug.Log("KNOCKING ME BACK");
				//if(motor)
					//motor.AddImpact(_myTransform.position - originPos, effects[i].effectValue);
			break;
			}
		}
	}*/

	public void AddImpact(Vector3 dir, float force, float duration, float acceleration)
	{
		motor.AddImpact(dir, force, duration, acceleration);
    }
	
    public void ReceiveDamage(float damage)
    {
		if(curHealth >0)
		{
			curHealth -= damage;
			Debug.Log("currentHP: "+curHealth);
			if(curHealth <= 0)
	        {
            	Die();
	        }
		}
    }

    public void Heal(int hp)
    {
        curHealth += hp;
		if(curHealth > maxHealth)
        {
			curHealth = maxHealth;
        }
    }

    public virtual void ChangeMovementSpeed(float change)
    {
        curMovementSpeed += change;
    }
	
    public virtual void Die()
    {
        Debug.Log("died");
		if(GetComponent<PhotonView>().isMine)
		{
			PhotonNetwork.Destroy(this.gameObject);
		}
    }

	public void AddStatusEffect(StatusEffect effect)
	{
		if(effect.statusEffect.effect == 1)
		{
			//speedModifiers.Add(effect);
			effect.StartEffect(this, statusEffects.Count - 1);
		}
		else if (effect.statusEffect.effect == 3) 
		{
			//attackBonusModifiers.Add(effect);
			effect.StartEffect(this, statusEffects.Count - 1);
		}
		else if (effect.statusEffect.effect == 6) 
		{
			//defenseBonusModifiers.Add(effect);
			effect.StartEffect(this, statusEffects.Count - 1);
        }
    }

	public void RemoveStatusEffect(BaseSkill ownerSkill)
	{
		for (int i = statusEffects.Count - 1; i > -1; i--) 
		{
			if(statusEffects[i].ownerSkill == ownerSkill && statusEffects[i].statusEffect.effectFormat == SkillEffectFormat.timed)
			{
				statusEffects[i].EndEffect();
			}
		}
    }

	public void RemoveStatusEffect(StatusEffect effect)
	{
		if(statusEffects.Contains(effect))
		{
			statusEffects.Remove(effect);
			return;
		}
    }
    
    
    public void EnableMovement(bool state)
    {
        canMove = state;
    }

	[RPC]
	public void ReceiveHit(byte[] hit)
	{
		HitInfo receivedHit = new HitInfo();
		
		BinaryFormatter bb = new BinaryFormatter();
		MemoryStream mm = new MemoryStream(hit);
		receivedHit = (HitInfo)bb.Deserialize(mm);
		Debug.Log(receivedHit.sourceName + receivedHit.damage);
		Vector3 origin = new Vector3(receivedHit.hitPosX, receivedHit.hitPosY, receivedHit.hitPosZ);
		//TODO apply self buffs/debuffs to calculate final hit results
		//TODO apply hit effects
		if(myPhotonView.isMine)
		{
			Debug.Log("received Damage");
			myPhotonView.RPC("NetworkSyncHealth", PhotonTargets.All, receivedHit.damage);
			//ReceiveDamage(receivedHit.damage);
			//if(receivedHit.skillEffects.Count > 0)
				//ProcessHitEffects(receivedHit.skillEffects, origin);
		}

		if(myPhotonView.owner == null)
		{
			ReceiveDamage(receivedHit.damage);
		}
		//myPhotonView.RPC("ApplyReceivedHitEffects", PhotonTargets.All, 
	}

	//others
	[RPC]
	public void NetworkSyncHealth(float damage)
	{
		if(curHealth >0)
		{
			curHealth -= damage;
			Debug.Log("currentHP: "+curHealth);
			if(myPhotonView.isMine)
			{
				if(curHealth <= 0)
				{
					Die();
				}
			}
		}
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
	public List<StatusEffectData> skillEffects;
}

