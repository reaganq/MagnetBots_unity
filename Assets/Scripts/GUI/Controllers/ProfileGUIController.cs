using UnityEngine;
using System.Collections;

public class ProfileGUIController : BasicGUIController {

	public UILabel nameLabel;
	public UISprite guildLogo;
	public UILabel levelLabel;
	public PlayerProfile activeProfile;

	public void Enable(PlayerProfile profile)
	{
		activeProfile = profile;
		Enable();
		nameLabel.text = activeProfile.name;
	}

	public void OnBackButtonPressed()
	{
		Disable();
	}

}
