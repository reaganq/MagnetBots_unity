using UnityEngine;
using System.Collections;

public class NetworkAIMovement : Photon.MonoBehaviour {

	private Transform _transform;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(photonView.isMine)
		{
		}
		else
		{
			if(Vector3.Distance(_transform.position, realPosition) < 2)
				_transform.position = Vector3.Lerp(_transform.position, realPosition, 0.1f);
			else
				_transform.position = realPosition;
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
