using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	public AbilitySkill[] abilitySkills;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < abilitySkills.Length; i++) {
			abilitySkills[i].ID = i;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
