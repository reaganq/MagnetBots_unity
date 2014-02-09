using UnityEngine;
using System.Collections;

public class TriggerCollider : MonoBehaviour {

    public ArmorSkill masterScript;
    public SimpleFSM AI;
    public Collider ownerCollider;

	public void OnTriggerEnter(Collider other)
    {
        Debug.Log("wtf");
        if(other != ownerCollider)
        {
            GameObject go = other.gameObject;
            Debug.Log("trigger enter" + go.name);
            if(!masterScript.HitEnemies.Contains(go) && !masterScript.HitAllies.Contains(go))
            {
                ShieldCollider sc = go.GetComponent<ShieldCollider>();
                if(sc != null)
                {
                    Debug.Log("hit shield");
                    // add the shield's characterstatus gameobject to hitenemies
                }
                
                CharacterStatus cs = go.GetComponent<CharacterStatus>();
                if(cs != null)
                {
                    masterScript.HitEnemies.Add(go);
                    masterScript.HitEnemy(cs);
                    
                    Debug.Log("I JUST HIT SOMETHING");
                }
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.collider != ownerCollider)
        {
            GameObject go = other.collider.gameObject;
            Debug.Log("trigger enter" + go.name);
            if(!masterScript.HitEnemies.Contains(go) && !masterScript.HitAllies.Contains(go))
            {
                ShieldCollider sc = go.GetComponent<ShieldCollider>();
                if(sc != null)
                {
                    Debug.Log("hit shield");
                    // add the shield's characterstatus gameobject to hitenemies
                }
                
                CharacterStatus cs = go.GetComponent<CharacterStatus>();
                if(cs != null)
                {
                    masterScript.HitEnemies.Add(go);
                    masterScript.HitEnemy(cs);
                    
                    Debug.Log("I JUST HIT SOMETHING");
                }
            }
        }
    }
}
