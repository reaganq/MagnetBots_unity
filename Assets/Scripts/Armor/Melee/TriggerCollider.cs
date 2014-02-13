using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerCollider : MonoBehaviour {

    public ArmorSkill masterArmor;
	public AISkill masterAISkill;
	public CharacterStatus status;

	public void Start()
	{
		IgnoreCollisions();
	}

	public void IgnoreCollisions()
	{
		if(status != null)
		{
			List<Collider> cols = status.hitboxes;
			for (int i = 0; i < cols.Count; i++) 
			{
				Physics.IgnoreCollision(collider, cols[i]);
			}
	        Debug.Log("ignore collision");
		}
    }

	public void OnTriggerEnter(Collider other)
    {
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;
			if(cs != status)
			{
				if(masterAISkill != null)
				{
					if(!masterAISkill.HitEnemies.Contains(cs) && !masterAISkill.HitAllies.Contains(cs))
					{
						//determine if friend or foe
						masterAISkill.HitEnemies.Add(cs);
						masterAISkill.HitEnemy(hb);
						Debug.Log("I JUST HIT SOMETHING");
					}
				}
				if(masterArmor != null)
				{
					if(!masterArmor.HitEnemies.Contains(cs) && !masterArmor.HitAllies.Contains(cs))
                    {
                        //determine if friend or foe
                        masterArmor.HitEnemies.Add(cs);
                        masterArmor.HitEnemy(hb);
                        Debug.Log("I JUST HIT SOMETHING");
                    }
                }
            }
        }
    }
    
}
