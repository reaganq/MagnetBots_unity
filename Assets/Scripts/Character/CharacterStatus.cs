using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStatus : MonoBehaviour {

    public int MaxHealth = 0;
    public int CurrentHealth = 0;
    public CharacterActionManager ActionManager;
    public CharacterMotor Motor;
    public float movementSpeed;
	public float rotationSpeed;
    //can't be disjointed
    public bool Invulnerable = false;
    //cant be disjointed or damaged
    public bool Invincinble = false;
    public bool canMove = true;

	public List<Collider> hitboxes;

	// Use this for initialization
	void Start () {
        CurrentHealth = MaxHealth;
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


    public void DealDamage(int hp)
    {
        CurrentHealth -= hp;
        if(CurrentHealth <= 0)
        {
            Die();
        }
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
        Destroy(gameObject);
    }

    public void EnableMovement(bool state)
    {
        canMove = state;
    }



}
