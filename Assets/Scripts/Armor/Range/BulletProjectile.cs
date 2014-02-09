using UnityEngine;
using System.Collections;

public class BulletProjectile : MonoBehaviour {

    public ArmorSkill masterScript;
    public float timer;


	// Use this for initialization
	void Start () {
        Invoke("suicide", timer);
	}
	
    public void OnCollisionEnter(Collision other)
    {
        //if(other.collider != ownerCollider)
        //{
        if(!other.gameObject.CompareTag("Terrain"))
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
                    //masterScript.HitEnemies.Add(go);
                    masterScript.HitEnemy(cs);
                    
                    Debug.Log("I JUST HIT SOMETHING");
                }
            }
            suicide();
        }

    }

    void suicide()
    {
        Debug.LogWarning("destroyed projectile");
        Destroy(this.gameObject);
    }

}
