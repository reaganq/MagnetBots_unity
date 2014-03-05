using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

    public CharacterStatus ownerCS;
	//hitbox local buffs;

	public virtual void ReceiveHit(HitInfo hit)
	{
		//ownerCS.ReceiveHit(hit);
	}
}
