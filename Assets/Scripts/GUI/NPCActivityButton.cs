using UnityEngine;
using System.Collections;

public class NPCActivityButton : MonoBehaviour {

	public UILabel textLabel;
	public UISprite icon;
	public int index;

	public void LoadActivityButton(NPCActivity activity, int i)
	{
		switch (activity.activityType)
		{
		case NPCActivityType.Arena:
			break;
		case NPCActivityType.Minigame:
			break;
		case NPCActivityType.Default:
			break;
		case NPCActivityType.Shop:
			break;
		case NPCActivityType.Service:
			break;
		case NPCActivityType.Quest:
			break;
		case NPCActivityType.Teleporter:
			//display teleporter UI
			break;
		}
		GameObject atlas = Resources.Load(activity.npcIconAtlas) as GameObject;
		icon.atlas = atlas.GetComponent<UIAtlas>();
		icon.spriteName = activity.npcIconSprite;
		textLabel.text = activity.npcButtonText;
		index = i;
	}

	void OnPress(bool pressed)
	{
		if(!pressed)
		{
			GUIManager.Instance.NPCGUI.OnActivityButtonPressed(index);
		}
	}

}
