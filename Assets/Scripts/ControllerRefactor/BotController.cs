using UnityEngine;
using System.Collections;

public class BotController : MonoBehaviour {

	//list of body parts
	public BodyPart[] bodyParts;

	//CharacterStats
	public CharacterStats myStats;

	//abilities
	public AbilitySkill activeAbility;

	//motor capabilities
	public bool canMove;
	public bool canRotate;
	public Vector3 targetWayPoint;

	public bool AbleToMove()
	{
		//if we are not stunned or doing any actions or busy, we can move
		return true;
	}

	public bool AbleToUseAbility()
	{
		//if we are not stunned or busy, we can use abilities
		return true;
	}

	void Update()
	{
		//when player can move, move to target waypoint, rotate to target waypoint
	}

	void LateUpdate()
	{
	}

	public void EnableFullMovement()
	{
		canMove = true;
		canRotate = true;
	}

	public void EnableMovement(bool state)
	{
		canMove = state;
	}

	public void EnableRotation(bool state)
	{
		canRotate = state;
	}

	//process input commands
	public void SetNewWayPoint(Vector3 target)
	{
		targetWayPoint = target;
	}


}
