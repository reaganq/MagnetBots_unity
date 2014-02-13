using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

    public int MaxHealth = 0;
    public int CurrentHealth = 0;
    public CharacterActionManager ActionManager;
    public CharacterMotor Motor;
    public float runSpeed;
    //can't be disjointed
    public bool Invulnerable = false;
    //cant be disjointed or damaged
    public bool Invincinble = false;
    public bool canMove = true;

	// Use this for initialization
	void Start () {
        CurrentHealth = MaxHealth;
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
        runSpeed += change;
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
