using UnityEngine;
using System.Collections;

public class GlobalInputManager : MonoBehaviour {

	public InputType inputType;
	public LayerMask layerMask = -1;
	public int terrainMask = 1<<16;
	public int poiMask = 1<<15;
	public int characterLayerMask = 1<<9;
	private Vector2 lastPressDownPos;
	public GameObject indicatorObjectPrefab;
	public float indicatorTimer;

	void Start () {
		inputType = GameManager.Instance.inputType;
		UICamera.fallThrough = this.gameObject;
	}

	void OnPress(bool isDown)
	{
		if(isDown)
		{
			lastPressDownPos = UICamera.lastTouchPosition;
		}
		else
		{
			if(inputType == InputType.TouchInput || inputType == InputType.WASDInput)
			{
			//send message: clicked outside of UI
				if(!GUIManager.Instance.IsUIBusy())
				{
					if(Vector2.Distance(UICamera.lastTouchPosition, lastPressDownPos) < 20f)
					{
						Ray ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y, 0 ));
						RaycastHit hit;
						if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
						{
							int layermsk = (1<<hit.collider.gameObject.layer);
							
							if(layermsk == terrainMask)
							{
								if(PlayerManager.Instance.avatarInput != null)
								{
									PlayerManager.Instance.avatarInput.SetWayPoint(hit.point);
									GameObject indicator = GameObject.Instantiate(indicatorObjectPrefab, hit.point, Quaternion.identity) as GameObject;
								}

							}
							else if(layermsk == poiMask)
							{
								hit.collider.gameObject.SendMessage("ActivatePOI");
							}
							else if(layermsk == characterLayerMask && PlayerManager.Instance.ActiveZone.zoneType == ZoneType.town)
							{
								PlayerCharacter cs = hit.collider.gameObject.GetComponent<PlayerCharacter>();
								if(cs != null)
								{
									GUIManager.Instance.DisplayHoverPopup(cs);
								}
							}
						}
					}
				}
			}
			else
			{
				if(!GUIManager.Instance.IsUIBusy())
				{
					if(isDown)
					{	                   
						if(UICamera.currentTouchID == -1)
							PlayerManager.Instance.avatarActionManager.UseSkill(InputTrigger.OnPressDown, 0);
						if(UICamera.currentTouchID == -2)
							PlayerManager.Instance.avatarActionManager.UseSkill(InputTrigger.OnPressDown, 1);
					}
					else
					{
						if(UICamera.currentTouchID == -1)
							PlayerManager.Instance.avatarActionManager.UseSkill(InputTrigger.OnPressUp, 0);
						if(UICamera.currentTouchID == -2)
							PlayerManager.Instance.avatarActionManager.UseSkill(InputTrigger.OnPressUp, 1);
					}
				}
			}
		}
	}
}
