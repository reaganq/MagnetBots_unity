using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletProjectile : MonoBehaviour {

    public ArmorSkill masterArmor;
	public AISkill masterAISkill;
    public float timer;
	public CharacterStatus status;


	// Use this for initialization
	void Start () {
        Invoke("suicide", timer);
		//IgnoreCollisions();
	}
	
	public void IgnoreCollisions()
	{
		List<Collider> cols = status.hitboxes;
		for (int i = 0; i < cols.Count; i++) 
		{
			Physics.IgnoreCollision(collider, cols[i]);
		}
	}
	
    public void OnCollisionEnter(Collision other)
    {
		//Debug.Log("trigger enter" + other.collider.gameObject.name);

		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;
			if(cs != status)
			{
				if(masterAISkill != null)
				{
					//if(!masterAISkill.HitEnemies.Contains(cs) && !masterAISkill.HitAllies.Contains(cs))
		            //{
						//determine if friend or foe
						//masterAISkill.HitEnemies.Add(cs);
						masterAISkill.HitTarget(hb, false);
		                //Debug.Log("I JUST HIT SOMETHING");
		            //}
				}
				if(masterArmor != null)
				{
					//if(!masterArmor.HitEnemies.Contains(cs) && !masterArmor.HitAllies.Contains(cs))
					//{
						//determine if friend or foe
						//masterArmor.HitEnemies.Add(cs);
						masterArmor.HitTarget(hb, false);
						//Debug.Log("I JUST HIT SOMETHING");
					//}
				}
			}
		}
		suicide();
    }

    void suicide()
    {
        //Debug.LogWarning("destroyed projectile");
        Destroy(this.gameObject);
    }

	public void OnDisable()
	{
	}

}
