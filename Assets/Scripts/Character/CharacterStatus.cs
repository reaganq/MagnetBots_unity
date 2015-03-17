using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CharacterStatus : CharacterAttributes {

	public UILabel nameLabel;
	public UISprite hpBar;
	public OverheadUI HUD;
	public string characterName;
    public Motor motor;
	public ActionManager actionManager;
	public PhotonView myPhotonView;
	public CharacterType enemyCharacterType;
	public List<Collider> hitboxes;
	public Transform _myTransform;
	public List<StatusEffect> statusEffects;
	public List<StatusEffect> speedModifiers;
	public float clientCurMovementSpeed;
	public float curMovementSpeed{
		get{
			if(!myPhotonView.isMine)
				return clientCurMovementSpeed;
			else
			{
				float s = maxMovementSpeed;
				for (int i = 0; i < speedModifiers.Count; i++) {
					s += speedModifiers[i].statusEffect.primaryEffectValue;
				}
				return s;
			}
		}
		set{
			curMovementSpeed = value;
		}
	}
	
	public bool Invulnerable = false;
	public bool canMove = true;

	public virtual void Awake () 
	{
		curHealth = maxHealth;
		//curMovementSpeed = maxMovementSpeed;
		_myTransform = this.transform;
		actionManager = GetComponent<ActionManager>();
		myPhotonView = GetComponent<PhotonView>();
		UpdateHitBoxes();
	}

	public void UpdateNameTag(string nameString)
	{
		characterName = nameString;
		if(nameLabel != null)
			nameLabel.text = characterName;
	}

	public void Update()
	{
		if(hpBar != null)
			hpBar.fillAmount = curHealth/maxHealth;
	}

	public void DisplayName(bool state)
	{
		if(nameLabel != null)
			nameLabel.gameObject.SetActive(state);
	}

	public void DisplayHpBar(bool state)
	{
		if(hpBar != null)
			hpBar.gameObject.SetActive(state);
	}

	public void HideInfo()
	{
		DisplayName(false);
		DisplayHpBar(false);
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

	public virtual void AddImpact(Vector3 dir, float force, float duration, float acceleration)
	{
		motor.AddImpact(dir, force, duration, acceleration);
    }

	public virtual void ReceiveHit(int perpetratorViewID, int perpetratorSkillID, List<StatusEffectData> incomingStatusEffects)
	{
		if(!isAlive() || Invulnerable)
			return;
		//process damage
		//ReceiveDamage(damage);
		Debug.Log(perpetratorViewID +" " + perpetratorSkillID);
		Debug.Log("receiving hit " + incomingStatusEffects.Count);
		for (int i = 0; i < incomingStatusEffects.Count; i++) {
			if(incomingStatusEffects[i].effect == 0)
			{
				Debug.Log("got a damage effect");
				ReceiveDamage(incomingStatusEffects[i].primaryEffectValue);
			}
		}
	}

    public void ReceiveDamage(float damage)
    {
		if(curHealth < 0)
			return;

		if(curHealth >0)
		{
			curHealth -= damage;
			Debug.LogWarning("currentHP: "+curHealth);
			myPhotonView.RPC("NetworkSyncHealth", PhotonTargets.All, curHealth, damage, true);
		}
		if(curHealth <= 0)
		{
			Die();
		}

    }

	public void ResetHealth()
	{
		myPhotonView.RPC("NetworkSyncHealth", PhotonTargets.All, maxHealth, 0.0f , false);
		actionManager.StopAnimation("Default_Death", true);
	}
	
	[RPC]
	public void NetworkSyncHealth(float newHealth, float damage, bool displayMessage)
	{
		curHealth = newHealth;
		if(HUD != null && displayMessage)
			HUD.DisplayHpMessage(-damage);
		//update healthbar
	}

	public virtual void Die()
	{
		myPhotonView.RPC("NetworkDie", PhotonTargets.All);
	}

	[RPC]
	public virtual void NetworkDie()
	{
		Debug.Log("died");
		actionManager.Die();
		//play death animation;
	}

    public void Heal(int hp)
    {
        curHealth += hp;
		if(curHealth > maxHealth)
        {
			curHealth = maxHealth;
        }
    }

    /*public virtual void ChangeMovementSpeed(float change)
    {
        curMovementSpeed += change;
    }*/
	
    

	public void AddStatusEffect(StatusEffect effect)
	{
		if(effect.statusEffect.effect == 1)
		{
			speedModifiers.Add(effect);
			effect.StartEffect(this, speedModifiers.Count - 1);
		}
		else
		{
			statusEffects.Add(effect);
			effect.StartEffect(this, statusEffects.Count - 1);
		}
	}

	public void RemoveStatusEffect(BaseSkill ownerSkill)
	{
		for (int i = statusEffects.Count - 1; i > -1; i--) 
		{
			if(statusEffects[i].ownerSkill == ownerSkill && (statusEffects[i].statusEffect.effectFormat == SkillEffectFormat.useDuration || statusEffects[i].statusEffect.effectFormat == SkillEffectFormat.instant) )
			{
				statusEffects[i].EndEffect();
			}
		}
		for (int i = speedModifiers.Count - 1; i > -1; i--) 
		{
				if(speedModifiers[i].ownerSkill == ownerSkill && (speedModifiers[i].statusEffect.effectFormat == SkillEffectFormat.useDuration || speedModifiers[i].statusEffect.effectFormat == SkillEffectFormat.instant))
			{
				speedModifiers[i].EndEffect();
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
		if(speedModifiers.Contains(effect))
		{
			speedModifiers.Remove(effect);
			return;
		}
	}

	/*[RPC]
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
	}*/

	//others

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

