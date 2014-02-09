using UnityEngine;
using System.Collections;

public class UI3DCamera : MonoBehaviour {

	void Awake()
    {
        this.gameObject.SetActive(false);
    }
    // Use this for initialization
	void Start () {
	    GUIManager.Instance.Inventory3DCamera = this.camera;
	}
}
