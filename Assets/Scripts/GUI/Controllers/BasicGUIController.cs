using UnityEngine;
using System.Collections;

public class BasicGUIController : MonoBehaviour {

	public UIPlayTween tween;
	public GameObject Root;

	public virtual void Enable()
	{
		if(Root != null)
			Root.SetActive(true);
	}

	public virtual void Hide()
	{
		if(Root != null)
			Root.SetActive(false);
	}

	public virtual void Disable()
	{
		Disable(false);
	}

	public virtual void Disable(bool resetState)
	{
		if(resetState)
			Reset();
	}

	public virtual void Reset()
	{
	}

}
