using UnityEngine;
using System.Collections;

public class FriendBox : MonoBehaviour {

	public UILabel Name;
	public UILabel Status;
	public GameObject removeButton;
	public GameObject chatButton;
	public GameObject profileButton;
	public UIPlayTween tween;

	public void LoadFriendBox(string name, bool state)
	{
		Name.text = name;
		if(!state)
			ShowNormal();
		else
			ShowRemove();
	}

	public void OnProfileClick()
	{
	}

	public void OnFriendChatClick()
	{
		GUIManager.Instance.chatGUI.ShowFriendChat(this);
		tween.Play(!tween.tweenTarget.activeSelf);
	}

	public void OnRemoveFriendclick()
	{
		GUIManager.Instance.chatGUI.RemoveFriend(this);
	}

	public void ShowNormal()
	{
		removeButton.SetActive(false);
		chatButton.SetActive(true);
		profileButton.SetActive(true);
	}

	public void ShowRemove()
	{
		removeButton.SetActive(true);
		chatButton.SetActive(false);
		profileButton.SetActive(false);
	}


}
