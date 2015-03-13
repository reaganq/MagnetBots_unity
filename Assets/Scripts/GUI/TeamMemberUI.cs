using UnityEngine;
using System.Collections;

public class TeamMemberUI : MonoBehaviour {

	public UISprite portraitIcon;
	public GameObject popupBox;
	public UILabel nameLabel;
	public UIProgressBar healthBar;
	public PlayerCharacter targetCS;
	public bool isDisplayed;

	public void Initialise(int viewID)
	{
		PhotonView view = PhotonView.Find(viewID);
		PlayerCharacter pc = view.GetComponent<PlayerCharacter>();
		if(pc != null)
		{
			Debug.Log("found the son of a bitch");
			targetCS = pc;
			portraitIcon.spriteName = targetCS.headPortraitString;
			nameLabel.text = targetCS.characterName;
			Debug.Log(nameLabel.text);
		}
		isDisplayed = true;
		gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		targetCS = null;
		isDisplayed = true;
		gameObject.SetActive(false);
		HidePopup();
	}

	public void Update()
	{
		if(isDisplayed)
		{
			healthBar.value = targetCS.curHealth/targetCS.maxHealth;
		}
	}

	public void OnPortraitPressed()
	{
		popupBox.SetActive(true);
	}

	public void HidePopup()
	{
		popupBox.SetActive(false);
	}
}
