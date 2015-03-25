using UnityEngine;
using System;
using System.Collections;

public class DynamicWidth : MonoBehaviour {

	public DynamicSprites[] targetSprites;
	public DynamicTransforms[] targetTransforms;

	public void Start()
	{
		float offset = GameManager.Instance.nativeAspectRatio/GameManager.Instance.defaultAspectRatio;
		for (int i = 0; i < targetSprites.Length; i++) {
			Transform tran = targetSprites[i].sprite.gameObject.transform;
			float oldX = tran.localPosition.x;
			if(targetSprites[i].positionType == DynamicPositioningType.adjustToWidth)
			{

				float diff = tran.localPosition.x * offset - tran.localPosition.x;
				//targetSprites[i].sprite.gameObject.transform.localPosition.x = targetSprites[i].sprite.gameObject.transform.localPosition.x * offset;
				tran.localPosition = new Vector3(tran.localPosition.x * offset, tran.localPosition.y, tran.localPosition.z);
				if(targetSprites[i].rescaleWidth)
				{
					if(targetSprites[i].widthScaleType == DynamicWidthType.proportional)
						targetSprites[i].sprite.width = (int)(targetSprites[i].sprite.width * offset);
					else if(targetSprites[i].widthScaleType == DynamicWidthType.absolute)
					{
						float oldGap = 1024 - oldX - targetSprites[i].sprite.width;
						targetSprites[i].sprite.width = (int)(1024 * offset - tran.localPosition.x - oldGap);
					}
					else if(targetSprites[i].widthScaleType == DynamicWidthType.relativeGap)
					{
						float gap = 1024 - oldX - targetSprites[i].sprite.width;
						targetSprites[i].sprite.width = (int)(1024 * offset - tran.localPosition.x - gap*offset);
					}
						//targetSprites[i].sprite.width = (int)(GameManager.Instance.defaultScreenWidth * offset - (GameManager.Instance.defaultScreenWidth - targetSprites[i].sprite.width));
				}
			}
			else if(targetSprites[i].positionType == DynamicPositioningType.absolute)
			{
				tran.localPosition = new Vector3(-1024 * offset - (-1024-tran.localPosition.x), tran.localPosition.y, tran.localPosition.z);
				if(targetSprites[i].rescaleWidth)
				{
					if(targetSprites[i].widthScaleType == DynamicWidthType.proportional)
						targetSprites[i].sprite.width = (int)(targetSprites[i].sprite.width * offset);
					else if(targetSprites[i].widthScaleType == DynamicWidthType.absolute)
					{
						float oldGap = 1024 - oldX - targetSprites[i].sprite.width;
						targetSprites[i].sprite.width = (int)(1024 * offset - tran.localPosition.x - oldGap);
					}
					else if(targetSprites[i].widthScaleType == DynamicWidthType.relativeGap)
					{
						float gap = 1024 - oldX - targetSprites[i].sprite.width;
						targetSprites[i].sprite.width = (int)(1024 * offset - tran.localPosition.x - gap*offset);
					}
					//targetSprites[i].sprite.width = (int)(GameManager.Instance.defaultScreenWidth * offset - (GameManager.Instance.defaultScreenWidth - targetSprites[i].sprite.width));
				}			
			}
		}
		for (int i = 0; i < targetTransforms.Length; i++) {
			if(targetTransforms[i].positionType == DynamicPositioningType.adjustToWidth)
			{
				float diff = targetTransforms[i].trans.localPosition.x * offset - targetTransforms[i].trans.localPosition.x;
				//targetSprites[i].sprite.gameObject.transform.localPosition.x = targetSprites[i].sprite.gameObject.transform.localPosition.x * offset;
				targetTransforms[i].trans.localPosition = new Vector3(targetTransforms[i].trans.localPosition.x * offset, targetTransforms[i].trans.localPosition.y, targetTransforms[i].trans.localPosition.z);
			}
			if(targetTransforms[i].positionType == DynamicPositioningType.absolute)
			{
				float diff = targetTransforms[i].trans.localPosition.x * offset - targetTransforms[i].trans.localPosition.x;
				//targetSprites[i].sprite.gameObject.transform.localPosition.x = targetSprites[i].sprite.gameObject.transform.localPosition.x * offset;
				targetTransforms[i].trans.localPosition = new Vector3(-1024 * offset - (-1024-targetTransforms[i].trans.localPosition.x), targetTransforms[i].trans.localPosition.y, targetTransforms[i].trans.localPosition.z);
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
	public DynamicWidthType widthScaleType;
}

[Serializable]
public class DynamicTransforms
{
	public Transform trans;
	public DynamicPositioningType positionType;
}

public enum DynamicPositioningType
{
	absolute,
	adjustToWidth,
	noChange
}

public enum DynamicWidthType
{
	proportional,
	absolute,
	relativeGap
}