using UnityEngine;
using System.Collections;

public class NetworkCharacterMovement : Photon.MonoBehaviour {

	private Transform _transform;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	void Start()
	{
		_transform = transform;
	}

	void Update() 
	{
		if(photonView.isMine)
		{
		}
		else
		{
			_transform.position = Vector3.Lerp(_transform.position, realPosition, 0.1f);
			_transform.rotation = Quaternion.Lerp(_transform.rotation, realRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(_transform.position);
			stream.SendNext(_transform.rotation);
		}
		else
		{
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
