using UnityEngine;
using System.Collections;

public class RangedAISkill : AISkill {

    public ArmorAnimation castAnimation;
    public ArmorAnimation durationAnimation;
    public ArmorAnimation recoilAnimation;
    public ArmorAnimation reloadAnimation;
    public ArmorAnimation followThroughAnimation;
    public Transform bulletLocation;
    public float bulletSpeed;

    public int currentAmmoCount = 0;
    public int shotsFired = 0;

	// Use this for initialization
	public override void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}
}
