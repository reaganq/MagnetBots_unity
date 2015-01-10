﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CharacterStatus : CharacterAttributes {

	public UILabel nameLabel;
	public UISprite hpBar;
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

	public virtual void Awake () 
	{
		curHealth = maxHealth;
		curMovementSpeed = maxMovementSpeed;

		_myTransform = this.transform;
		actionManager = GetComponent<ActionManager>();
		myPhotonView = GetComponent<PhotonView>();

		UpdateHitBoxes();

	}

	public void Update()
	{
		if(hpBar != null)
			hpBar.fillAmount = curHealth/maxHealth;
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
	


	public bool isAlive()
	{
		if(curHealth <= 0)
			return false;
		else
			return true;
	}

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
		statusEffects.Add(effect);
		effect.StartEffect(this, statusEffects.Count - 1);
    }

	public void RemoveStatusEffectFromSkill(BaseSkill ownerSkill)
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

