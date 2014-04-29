using UnityEngine;
using System.Collections;

public class Anvil : MonoBehaviour {

	public ParticleSystem SuccessParticles;
	public ParticleSystem FailParticles;
	public PhotonView myPhotonView;

	public void Sucess()
	{
		myPhotonView.RPC("ShowSuccess", PhotonTargets.All);
	}

	public void Fail()
	{
		myPhotonView.RPC("ShowFail", PhotonTargets.All);
	}

	[RPC]
	public void ShowSuccess()
	{
		SuccessParticles.Play();
	}

	[RPC]
	public void ShowFail()
	{
		FailParticles.Play();
	}

}
