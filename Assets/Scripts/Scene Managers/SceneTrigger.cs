using UnityEngine;
using System.Collections;

public class SceneTrigger : MonoBehaviour {

    public string targetSceneName;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerManager.Instance.avatarObject)
        {
            Debug.Log("hit scene trigger");
            //GameManager.Instance.LoadWorld(targetSceneName);
        }
    }
}
