using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class BulletProjectile : MonoBehaviour {

    public BaseSkill masterArmor;
	public AISkill masterAISkill;
    public float timer;
	public CharacterStatus status;
	public string pool;
	public ParticleSystem hitDecal;

	// Use this for initialization
	public void OnSpawned() {
		StartCoroutine(DeSpawn());
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
		Debug.Log("trigger enter" + other.collider.gameObject.name);

		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		ContactPoint contact = other.contacts[0];
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
						//masterAISkill.HitTarget(hb, false, transform.position);
		                //Debug.Log("I JUST HIT SOMETHING");
		            //}
				}
				if(masterArmor != null)
				{
					//if(!masterArmor.HitEnemies.Contains(cs) && !masterArmor.HitAllies.Contains(cs))
					//{
						//determine if friend or foe
						//masterArmor.HitEnemies.Add(cs);
						//masterArmor.HitTarget(hb, false, transform.position);
						//Debug.Log("I JUST HIT SOMETHING");
					//}
				}
			}
		}
		PoolManager.Pools[pool].Spawn(hitDecal, contact.point, Quaternion.identity);
		PoolManager.Pools[pool].Despawn(this.transform);
    }

    private IEnumerator DeSpawn()
    {
        //Debug.LogWarning("destroyed projectile");
		yield return new WaitForSeconds(timer);
		PoolManager.Pools[pool].Despawn(this.transform);
		//pool.Despawn(this.transform);
    }

	public void OnDespawned()
	{
		masterArmor = null;
		masterAISkill = null;
		status = null;
		pool = null;
		this.rigidbody.velocity = Vector3.zero;
	}

}
