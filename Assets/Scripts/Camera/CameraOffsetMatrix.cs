using UnityEngine;
using System.Collections;

public class CameraOffsetMatrix : MonoBehaviour {

	public Vector3 offset = new Vector3(0, 1, 0);
	void LateUpdate() {
		Vector3 camoffset = new Vector3(-offset.x, -offset.y, offset.z);
		Matrix4x4 m = Matrix4x4.TRS(camoffset, Quaternion.identity, new Vector3(1, 1, -1));
		camera.worldToCameraMatrix = m * transform.worldToLocalMatrix;
	}
}
