using UnityEngine;
using System.Collections;

public class HitBox : MonoBehaviour {

    public CharacterStatus ownerCS;

	public void DealDamage(int val)
	{

		ownerCS.DealDamage(val);
	}
}
