using UnityEngine;
using System.Collections;

public class CharacterAttributes: MonoBehaviour{

	public float maxHealth;
	public float curHealth;
	public float maxMovementSpeed;
	public float curMovementSpeed;
	public CharacterType characterType;
}

public enum CharacterType
{
	Playable,
	AI,
}