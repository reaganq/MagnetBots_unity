using UnityEngine;
using System.Collections;

public class CustomPosition : MonoBehaviour {

	public Vector3 targetPosition;

	public void Awake()
	{
		float idealRatio = 1136f/640f;
		float curx = transform.localPosition.x;
		float curOffset = curx - targetPosition.x;
		float targetOffset = GameManager.Instance.nativeAspectRatio/idealRatio * curOffset;
		transform.localPosition = new Vector3(curx - targetOffset, transform.localPosition.y, transform.localPosition.z);
	}

}
