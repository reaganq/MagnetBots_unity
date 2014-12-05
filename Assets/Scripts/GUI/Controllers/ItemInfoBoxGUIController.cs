using UnityEngine;
using System.Collections;

public class ItemInfoBoxGUIController : BasicGUIController {

	public GameObject EquipButton = null;
	public GameObject DestroyButton = null;
	public UILabel nameLabel;
	public UILabel descriptionLabel;

	public void DisplayItemDetails(RPGItem item, itemGUIType type)
	{
		nameLabel.text = item.Name;
		descriptionLabel.text = item.Description;
		if(type == itemGUIType.inventory)
		{
			EquipButton.SetActive(true);

		}
	}

	public override void Enable()
	{
		base.Enable();
		TweenScale.Begin(Root, 0.1f, Vector3.one);
	}

	public override void Disable ()
	{
		base.Disable ();
		TweenScale.Begin(Root, 0.1f, Vector3.zero);
	}
}

public enum itemGUIType
{
	inventory,
	nonInventory,
}