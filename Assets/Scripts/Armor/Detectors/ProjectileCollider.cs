using UnityEngine;
using System.Collections;

public class ProjectileCollider : Detector {

	public float lifeTime;
	public bool destroyOnCollision;
	public GameObject hitDecal;

	//public bool isAlive = true;

	public void OnSpawned() {
		StartCoroutine(DeSpawn());
		//IgnoreCollisions();
    } 	

	public void OnCollisionEnter(Collision other)
	{
		if(!isActive)
			return;
		if(currentNumberOfTargets >= ownerSkill.targetLimit)
			return;
		
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		Debug.LogWarning(other.collider.gameObject.name);
		ContactPoint contact = other.contacts[0];
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;
			if(cs == ownerSkill.ownerStatus)
				return;

			ownerSkill.HitTarget(cs, contact.point, cs._myTransform.position);
			Debug.Log("hit target with projectile");
		}
		ownerSkill.ownerManager.SpawnParticle(hitDecal.name, contact.point, false);
		isActive = false;
		if(destroyOnCollision)
		{
			StartCoroutine(DeSpawnAfterOneframe());
		}
	}

	private IEnumerator DeSpawnAfterOneframe()
	{
		yield return new WaitForEndOfFrame();
		ownerSkill.ownerManager.effectsPool.Despawn(this.transform);
	}
	
	private IEnumerator DeSpawn()
	{
		yield return new WaitForSeconds(lifeTime);
		ownerSkill.ownerManager.effectsPool.Despawn(this.transform);
	}

	public void OnDespawned()
	{
		this.rigidbody.velocity = Vector3.zero;
	}
}
