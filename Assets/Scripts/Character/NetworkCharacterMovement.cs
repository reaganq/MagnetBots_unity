using UnityEngine;
using System.Collections;

public class NetworkCharacterMovement : Photon.MonoBehaviour {

	private Transform _transform;
	public PlayerMotor _motor;
	public CharacterStatus myStatus;
	Vector3 realPosition = Vector3.zero;
	public float currentSpeed;
	Quaternion realRotation = Quaternion.identity;
	private Vector3 latestCorrectPos;
	private Vector3 onUpdatePos;
	private float fraction;
	public CharacterActionManager actionManager;
	public bool disableMovement;

	public bool IsActive;

	public void EnterActiveState()
	{
		IsActive = true;
	}

	public void ExitActiveState()
	{
		IsActive = false;
	}

	void Awake()
	{
		if (photonView.isMine)
		{
			this.enabled = false;   // due to this, Update() is not called on the owner client.
		}
		
		latestCorrectPos = transform.position;
		onUpdatePos = transform.position;

		_transform = transform;
		_motor = GetComponent<PlayerMotor>();
		myStatus = GetComponent<CharacterStatus>();
		actionManager = GetComponent<CharacterActionManager>();
	}

	void Update() 
	{
		if(IsActive)
		{
			/*if(Vector3.Distance(_transform.position, realPosition) < 10)
				_transform.position = Vector3.Lerp(_transform.position, realPosition, 0.1f);
			else
				_transform.position = realPosition;*/
			fraction = fraction + Time.deltaTime * 9;
			_transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction); 
			_transform.rotation = Quaternion.Lerp(_transform.rotation, realRotation, 0.1f);

			if(currentSpeed > 0 && !disableMovement)
			{
				float t = 0.0f;
				//t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );
				t = Mathf.Clamp( Mathf.Abs( myStatus.curMovementSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );

				actionManager.UpdateRunningSpeed(t);
				actionManager.AnimateToRunning();
			}
			else
			{
				//if(animationManager.myState == 
				actionManager.AnimateToIdle();
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(IsActive)
		{
			if(stream.isWriting)
			{
				//stream.Serialize(_transform.position);
				stream.SendNext(_transform.localPosition);
				stream.SendNext(_transform.rotation);
				stream.SendNext(_motor.controller.velocity.sqrMagnitude);
				stream.SendNext(myStatus.curMovementSpeed);
			}
			else
			{
				realPosition = (Vector3)stream.ReceiveNext();
				realRotation = (Quaternion)stream.ReceiveNext();
				currentSpeed = (float)stream.ReceiveNext();
				myStatus.clientCurMovementSpeed = (float)stream.ReceiveNext();

				latestCorrectPos = realPosition;                 // save this to move towards it in FixedUpdate()
				onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
				fraction = 0;  
			}
		}
	}

	void OnConnectionFail(DisconnectCause cause)
	{
		Debug.LogError(cause.ToString());
	}

	public void ChangeActiveState(Zone zone)
	{
		if(gameObject != PlayerManager.Instance.avatarObject)
		{
			IsActive = true;
			return;
		}
		if(zone != PlayerManager.Instance.ActiveZone)
		{
			IsActive = false;
		}
		else
			IsActive = true;

	}
}
