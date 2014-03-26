using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PathologicalGames;

public class AISkill : MonoBehaviour {

    public string skillName;
    public SkillType skillType;
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

	public List<SkillEffect> onUseSkillEffects;
	public List<SkillEffect> onHitSkillEffects;
	public List<SkillEffect> onReceiveHitSkillEffects;

	public List<CharacterStatus> HitEnemies;
	public List<CharacterStatus> HitAllies;

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

	public virtual void Reset()
	{
	}

	public virtual void HitTarget(HitBox target, bool isAlly)
	{
		HitInfo newHit = new HitInfo();

		if(!isAlly)
		{
			newHit.sourceName = fsm.myStatus.characterName;
			newHit.damage = damage;
			newHit.hitPosX = 1;
			newHit.hitPosY = 1;
			newHit.hitPosZ = 1;
			for (int i = 0; i < onHitSkillEffects.Count; i++) 
			{
				if (onHitSkillEffects[i].effectTarget == TargetType.hitEnemies || onHitSkillEffects[i].effectTarget == TargetType.allEnemies || onHitSkillEffects[i].effectTarget == TargetType.all) {
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
}
