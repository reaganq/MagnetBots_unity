using UnityEngine;
using System.Collections;

public class ConstructionGUIController : BasicGUIController {

	public UISlider progressBar;
	public Transform buildingTransform;
	public UILabel panelNameLabel;
	public RPGConstruction construction;

	public void Enable(Transform buildingTrans, RPGConstruction construct)
	{
		Enable();
		buildingTransform = buildingTrans;
		construction = construct;
		construction.LoadRequiredItems();
	}

	public void OnDonateButtonPressed()
	{
		StartCoroutine(EnterDonateMode());
		Disable();

	}

	public IEnumerator EnterDonateMode()
	{
		//move camera focus to building;
		yield return new WaitForSeconds(1);
		GUIManager.Instance.DisplayQuickInventory(ItemCategories.Construction);
	}
}
