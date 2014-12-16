using UnityEngine;
using System.Collections;

public class BasicGUIController : MonoBehaviour {

	public UIPlayTween tween;
	public GameObject Root;
	public bool isDisplayed;

	public virtual void Enable()
	{
		if(Root != null)
			Root.SetActive(true);
		isDisplayed = true;
	}

	public virtual void Hide()
	{
		if(Root != null)
			Root.SetActive(false);
		isDisplayed = false;
	}

	public virtual void Disable()
	{
		Disable(false);
	}

	public virtual void Disable(bool resetState)
	{
		if(resetState)
			Reset();
		Hide();
	}

	public virtual void Reset()
	{
	}

}
