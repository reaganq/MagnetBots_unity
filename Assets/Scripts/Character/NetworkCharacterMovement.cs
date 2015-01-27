using UnityEngine;
using System.Collections;

public class NetworkCharacterMovement : Photon.MonoBehaviour {

	private Transform _transform;
	public PlayerMotor _motor;
	public CharacterStatus myStatus;
	Vector3 realPosition = Vector3.zero;
	public float currentSpeed;
	Quaternion realRotation = Quaternion.identity;
	public CharacterActionManager actionManager;

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
					//t = Mathf.Clamp( Mathf.Abs( currentSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );
					t = Mathf.Clamp( Mathf.Abs( myStatus.curMovementSpeed / myStatus.maxMovementSpeed ), 0, myStatus.maxMovementSpeed );

					actionManager.UpdateRunningSpeed(t);
					actionManager.AnimateToRunning();
					Debug.Log(t);
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
				stream.SendNext(_motor.controller.velocity.sqrMagnitude);
				stream.SendNext(myStatus.curMovementSpeed);
			}
			else
			{
				realPosition = (Vector3)stream.ReceiveNext();
				realRotation = (Quaternion)stream.ReceiveNext();
				currentSpeed = (float)stream.ReceiveNext();
				myStatus.curMovementSpeed = (float)stream.ReceiveNext();
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
