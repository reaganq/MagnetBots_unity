using UnityEngine;
using System.Collections;

public class NetworkAIMovement : Photon.MonoBehaviour {

	private Transform _transform;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	private float fraction;
	private Vector3 latestCorrectPos;
	private Vector3 onUpdatePos;

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
			fraction = fraction + Time.deltaTime * 9;
			_transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction); 
			_transform.rotation = Quaternion.Lerp(_transform.rotation, realRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(_transform.localPosition);
			stream.SendNext(_transform.rotation);
		}
		else
		{
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			latestCorrectPos = realPosition;                 // save this to move towards it in FixedUpdate()
			onUpdatePos = _transform.localPosition;  // we interpolate from here to latestCorrectPos
			fraction = 0;  
		}

	}


}
