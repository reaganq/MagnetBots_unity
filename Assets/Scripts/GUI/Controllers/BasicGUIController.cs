using UnityEngine;
using System.Collections;

public class BasicGUIController : MonoBehaviour {

	public UIPlayTween tween;
	public GameObject Root;
	public bool isDisplayed;
	public bool autoPlay;

	public virtual void Enable()
	{
		isDisplayed = true;
		if(tween != null && autoPlay)
		{
			tween.Play(isDisplayed);
		}
		else
		{
			if(Root != null)
				Root.SetActive(true);

		}

	}

	public virtual void Hide()
	{
		isDisplayed = false;
		if(tween != null && autoPlay)
		{
			tween.Play(isDisplayed);
		}
		else
		{
			if(Root != null)
				Root.SetActive(false);
		}

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

	public virtual void OnCategoryPressed(int index, int level)
	{
	}
	
	public virtual void OnItemTilePressed(int index)
	{
	}

	public virtual void OnDragDrop(int index)
	{
	}


}
