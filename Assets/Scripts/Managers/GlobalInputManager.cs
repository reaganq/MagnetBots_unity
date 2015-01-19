using UnityEngine;
using System.Collections;

public class GlobalInputManager : MonoBehaviour {

	public InputType inputType;
	public LayerMask layerMask = -1;
	public int terrainMask = 1<<16;
	public int poiMask = 1<<15;
	public int characterLayerMask = 1<<9;
	private Vector2 lastPressDownPos;

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
			if(inputType == InputType.TouchInput)
			{
			//send message: clicked outside of UI
				if(!GUIManager.Instance.IsUIBusy())
				{
					if(Vector2.Distance(UICamera.lastTouchPosition, lastPressDownPos) < 5f)
					{
						Debug.Log("clicking");
						Ray ray = Camera.main.ScreenPointToRay(new Vector3(UICamera.lastTouchPosition.x, UICamera.lastTouchPosition.y, 0 ));
						RaycastHit hit;
						if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
						{
							int layermsk = (1<<hit.collider.gameObject.layer);
							
							if(layermsk == terrainMask)
							{
								if(PlayerManager.Instance.avatarInput != null)
									PlayerManager.Instance.avatarInput.SetWayPoint(hit.point);
							}
							else if(layermsk == poiMask)
							{
								Debug.Log("hit POI");
								hit.collider.gameObject.SendMessage("ActivatePOI");
							}
							else if(layermsk == characterLayerMask && PlayerManager.Instance.ActiveZone.type == ZoneType.town)
							{
								CharacterStatus cs = hit.collider.gameObject.GetComponent<CharacterStatus>();
								Debug.Log("hey");
								if(cs != null)
								{
									GUIManager.Instance.DisplayHoverPopup(cs);
									Debug.Log("wtf");
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
							PlayerManager.Instance.avatarActionManager.LeftAction(InputTrigger.OnPressDown);
						if(UICamera.currentTouchID == -2)
							PlayerManager.Instance.avatarActionManager.RightAction(InputTrigger.OnPressDown);
					}
					else
					{
						if(UICamera.currentTouchID == -1)
							PlayerManager.Instance.avatarActionManager.LeftAction(InputTrigger.OnPressUp);
						if(UICamera.currentTouchID == -2)
							PlayerManager.Instance.avatarActionManager.RightAction(InputTrigger.OnPressUp);
					}
				}
			}
		}
	}
}
