using UnityEngine;
using System.Collections;

public class NetworkCharacterMovement : Photon.MonoBehaviour {

	private Transform _transform;
	private PlayerMotor _motor;
	private CharacterStatus myStatus;
	Vector3 realPosition = Vector3.zero;
	float currentSpeed;
	Quaternion realRotation = Quaternion.identity;

	private bool _isActive = true;
	public CharacterActionManager actionManager;

	public bool IsActive
	{
		get{return _isActive;}
		set
		{
			ExitActiveState(_isActive);
			_isActive = value;
			EnterActiveState(_isActive);
		}
	}

	void EnterActiveState(bool state)
	{
	}

	void ExitActiveState(bool state)
	{
	}

	void Start()
	{
		_transform = transform;
		_motor = GetComponent<PlayerMotor>();
		myStatus = GetComponent<CharacterStatus>();
		actionManager = GetComponent<CharacterActionManager>();
	}

	void Update() 
	{
		if(IsActive)
		{
			if(photonView.isMine)
			{
			}
			else
			{
				if(Vector3.Distance(_transform.position, realPosition) < 10)
					_transform.position = Vector3.Lerp(_transform.position, realPosition, 0.1f);
				else
					_transform.position = realPosition;
				_transform.rotation = Quaternion.Lerp(_transform.rotation, realRotation, 0.1f);

				if(currentSpeed > 0)
				{
					float t = 0.0f;
					//Debug.Log(speed);
					t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );
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
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(IsActive)
		{
			if(stream.isWriting)
			{
				stream.SendNext(_transform.position);
				stream.SendNext(_transform.rotation);
				stream.SendNext(_motor.currentSpeed);
			}
			else
			{
				realPosition = (Vector3)stream.ReceiveNext();
				realRotation = (Quaternion)stream.ReceiveNext();
				currentSpeed = (float)stream.ReceiveNext();
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
