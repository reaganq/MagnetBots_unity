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
		if(currentNumberOfTargets >= ownerSkill.targetLimit)
			return;
		
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		ContactPoint contact = other.contacts[0];
		if(hb != null)
		{
			CharacterStatus cs = hb.ownerCS;
			if(cs == ownerSkill.ownerStatus)
				return;

			ownerSkill.HitTarget(cs, contact.point, cs._myTransform.position);
		}
		ownerSkill.ownerManager.SpawnParticle(hitDecal.name, contact.point, false);
		if(destroyOnCollision)
		{
			Debug.Log(other.gameObject.name);
			ownerSkill.ownerManager.effectsPool.Despawn(this.transform);
		}
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
