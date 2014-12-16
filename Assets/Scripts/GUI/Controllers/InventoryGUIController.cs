using UnityEngine;
using System.Collections;

public class InventoryGUIController : BasicGUIController {

	public GameObject itemTilePrefab;
	public UIGrid gridPanel;
	public GameObject gridPanelRoot;

	public override void Enable ()
	{
		base.Enable ();
	}

	public override void Disable ()
	{
		base.Disable ();
	}
}
