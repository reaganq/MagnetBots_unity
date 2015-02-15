using UnityEngine;
using System.Collections;

public class CharacterAttributes: MonoBehaviour{

	public float maxHealth;
	public float curHealth;
	//determined by the legs
	public float maxMovementSpeed;
	public float rotationSpeed;
	public CharacterType characterType;
}

public enum CharacterType
{
	Playable,
	AI,
}