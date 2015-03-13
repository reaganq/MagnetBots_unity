using UnityEngine;
using System.Collections;

public class ArenaExit : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerManager.Instance.avatarObject)
        {
			PlayerManager.Instance.LeaveArena();
            //GameManager.Instance.LoadWorld(targetSceneName);
        }
    }
}
