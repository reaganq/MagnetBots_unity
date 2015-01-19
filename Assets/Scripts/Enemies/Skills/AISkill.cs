using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PathologicalGames;

public class AISkill : BaseSkill {

    
    public float weighting = 0f;
    public SimpleFSM fsm;
	public int skillIndex;
    public Animation _animator;

    public bool requiresTarget;
	public bool requiresTargetLock;
	public bool requiresLineOfSight;
	public float angleTolerance = 15f;
    public float skillMaxRange;
	public float skillMinRange;
	public float damage;

	public List<StatusEffectData> onUseSkillEffects;
	public List<StatusEffectData> onHitSkillEffects;
	public List<StatusEffectData> onReceiveHitSkillEffects;

	// Use this for initialization
    public virtual void Start()
    {
        fsm = GetComponent<SimpleFSM>();
        _animator = GetComponent<Animation>();
    }
	
    public virtual void SetupAnimations()
    {
    }

	public void AddParticlesToPool()
	{
		
	}
	
	public void AddPrefabToPool(Transform prefab)
	{
		//if(characterManager.MakeSpawnPool())
		//{
		if(fsm.effectsPool.GetPrefabPool(prefab) == null)
		{
			PrefabPool prefabPool = new PrefabPool(prefab);;
			prefabPool.preloadAmount = 3;
			prefabPool.preloadFrames = 5;
			Debug.Log("here");
			fsm.effectsPool.CreatePrefabPool(prefabPool);
		}
		//}
	}

    public virtual IEnumerator UseSkill()
    {
        yield return null;
    }

    public virtual IEnumerator CancelSkill()
    {
        yield return null;
    }

	public virtual void ResetSkill()
	{
		HitEnemies.Clear();
		HitAllies.Clear();
	}

	public virtual void HitTarget(HitBox target, bool isAlly)
	{
		HitInfo newHit = new HitInfo();
		
		if(!isAlly)
		{
			newHit.sourceName = fsm.myStatus.characterName;
			newHit.damage = damage;
			newHit.hitPosX = fsm._transform.position.x;
			newHit.hitPosY = fsm._transform.position.y;
			newHit.hitPosZ = fsm._transform.position.z;
			newHit.skillEffects = new List<StatusEffectData>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].affectEnemy) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
				}
			}
			Debug.Log("no. of skill effects = " + newHit.skillEffects.Count);

			//TODO apply self buffs from characterstatus
			//TODO apply hitbox local buffs
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();
			b.Serialize(m, newHit);
			
			target.ownerCS.myPhotonView.RPC("ReceiveHit", PhotonTargets.All, m.GetBuffer());
			
			//target.ReceiveHit(newHit);
			//Debug.Log(finalhit.sourceName);
			Debug.Log("hitenemy");
		}
		else
		{
			Debug.Log("hitally");
		}
		//hb.ReceiveHit(newHit);
	}

	public virtual void HitTarget(HitBox target, bool isAlly, Vector3 originPos)
	{
		HitInfo newHit = new HitInfo();

		if(!isAlly)
		{
			newHit.sourceName = fsm.myStatus.characterName;
			newHit.damage = damage;
			newHit.hitPosX = originPos.x;
			newHit.hitPosY = originPos.y;
			newHit.hitPosZ = originPos.z;
			newHit.skillEffects = new List<StatusEffectData>();
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].affectEnemy) {
					newHit.skillEffects.Add(onHitSkillEffects[i]);
				}
			}
			
			//TODO apply self buffs from characterstatus
			//TODO apply hitbox local buffs
			BinaryFormatter b = new BinaryFormatter();
			MemoryStream m = new MemoryStream();
			b.Serialize(m, newHit);
			
			target.ownerCS.myPhotonView.RPC("ReceiveHit", PhotonTargets.All, m.GetBuffer());
			
			//target.ReceiveHit(newHit);
			//Debug.Log(finalhit.sourceName);
			Debug.Log("hitenemy");
		}
		else
		{
			Debug.Log("hitally");
		}
		//hb.ReceiveHit(newHit);
	}

	public void OverlapSphere(Vector3 location, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(location, radius);
		for (int i = 0; i < hitColliders.Length; i++) 
		{
			HitBox hb = hitColliders[i].gameObject.GetComponent<HitBox>();
			//ContactPoint contact = other.contacts[0];
			if(hb != null)
			{
				CharacterStatus cs = hb.ownerCS;
				if(cs != fsm.myStatus)
				{
						if(!HitEnemies.Contains(cs) && !HitAllies.Contains(cs))
						{
							//determine if friend or foe
							if(cs.characterType == CharacterType.AI)
							{
								HitAllies.Add(cs);
								HitTarget(hb, true);
							}
							else
							{
								HitEnemies.Add(cs);
								HitTarget(hb, false);
								//masterAISkill.fsm.myPhotonView.RPC("SpawnParticle", PhotonTargets.All, hitDecal.name, hitPos);
							}
							//Debug.Log("I JUST HIT SOMETHING");
						}
				}
			}
		}
	}
}
