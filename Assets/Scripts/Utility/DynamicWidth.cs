using UnityEngine;
using System;
using System.Collections;

public class DynamicWidth : MonoBehaviour {

	public DynamicSprites[] targetSprites;

	public void Start()
	{
		float offset = GameManager.Instance.nativeAspectRatio/GameManager.Instance.defaultAspectRatio;
		Debug.Log(offset);
		for (int i = 0; i < targetSprites.Length; i++) {
			if(targetSprites[i].positionType == DynamicPositioningType.adjustToWidth)
			{
				float diff = targetSprites[i].sprite.gameObject.transform.localPosition.x * offset - targetSprites[i].sprite.gameObject.transform.localPosition.x;
				//targetSprites[i].sprite.gameObject.transform.localPosition.x = targetSprites[i].sprite.gameObject.transform.localPosition.x * offset;
				targetSprites[i].sprite.gameObject.transform.localPosition = new Vector3(targetSprites[i].sprite.gameObject.transform.localPosition.x * offset, targetSprites[i].sprite.gameObject.transform.localPosition.y, targetSprites[i].sprite.gameObject.transform.localPosition.z);
				if(targetSprites[i].rescaleWidth)
					targetSprites[i].sprite.width = (int)(GameManager.Instance.defaultScreenWidth * offset - (GameManager.Instance.defaultScreenWidth - targetSprites[i].sprite.width) - diff);
			}
			else if(targetSprites[i].positionType == DynamicPositioningType.absolute)
			{
				if(targetSprites[i].rescaleWidth)
					targetSprites[i].sprite.width = (int)(GameManager.Instance.defaultScreenWidth * offset - (GameManager.Instance.defaultScreenWidth - targetSprites[i].sprite.width));
			}
		}
	}
}

[Serializable]
public class DynamicSprites
{
	public UISprite sprite;
	public DynamicPositioningType positionType;
	public bool rescaleWidth;
}

public enum DynamicPositioningType
{
	absolute,
	adjustToWidth,
}