using UnityEngine;
using System.Collections;

public class TradeGUIController : BasicGUIController {

	public GameObject toolTip;
	public bool isToolTipDisplayed;
	public UIPlayTween toolTipTween;



	public void OnSearchPressed()
	{

	}

	public void DisplayToolTip()
	{
		toolTipTween.Play(true);
		isToolTipDisplayed = true;
	}

	public void HideToolTip()
	{
		toolTipTween.Play(false);
		isToolTipDisplayed = false;
	}

	public void OnPlayerShopPressed()
	{
		//display playershop
	}

	public void OnBackButtonPressed()
	{
	}

	public override void Disable()
	{
		if(isToolTipDisplayed)
			HideToolTip();
		base.Disable();
	}
}
