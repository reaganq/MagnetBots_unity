using UnityEngine;
using System.Collections;

public class ParticleCollider : Detector {

	public float lifeTime;
	public bool isProjectile;
	public bool destroyOnCollision;
	public GameObject parentObj;
	public GameObject hitDecal;

	public bool isAlive = true;

	public void OnCollisionEnter(Collision other)
	{
		if(currentNumberOfTargets >= ownerSkill.targetLimit)
			return;
		
		HitBox hb = other.collider.gameObject.GetComponent<HitBox>();
		
		if(hb != null)
		{
			/*ownerSkill.HitTarget(hb.core.ID, this.transform.position, hb.gameObject.transform.position);
			currentNumberOfTargets ++ ;
			if(destroyOnCollision)
			{
				StartCoroutine( DestroyMe());
			}*/
		}
	}

	void Update () {
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0 && isAlive)
			StartCoroutine( DestroyMe());
	}
	
	IEnumerator DestroyMe()
	{
		isAlive = false;
		collider.enabled = false;
		ParticleSystem[] ps = parentObj.GetComponentsInChildren<ParticleSystem>();
		foreach(ParticleSystem p in ps)
		{
			p.Stop();
			Debug.Log("stop particles");
		}
		yield return new WaitForSeconds(1f);
		Destroy(parentObj);
	}
}
