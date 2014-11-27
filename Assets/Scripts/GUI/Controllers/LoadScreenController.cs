using UnityEngine;
using System.Collections;

public class LoadScreenController : BasicGUIController {

	public override void Enable ()
	{
		base.Enable ();
		Root.SetActive(true);
	}

	public override void Disable (bool resetState)
	{
		Root.SetActive(false);
		base.Disable(resetState);
	}
}
