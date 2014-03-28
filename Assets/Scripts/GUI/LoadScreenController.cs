using UnityEngine;
using System.Collections;

public class LoadScreenController : BasicGUIController {

	public GameObject Root;

	public override void Enable ()
	{
		base.Enable ();
		Root.SetActive(true);
	}

	public override void Disable ()
	{
		base.Disable ();
		Root.SetActive(false);
	}
}
