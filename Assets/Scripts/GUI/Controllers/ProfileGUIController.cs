using UnityEngine;
using System.Collections;

public class ProfileGUIController : BasicGUIController {

	public GameObject loadingIcon;
	public GameObject profileRoot;
	public UILabel nameLabel;
	public UISprite[] badgeSprites;
	public PlayerProfileDisplayData activeProfile;
	public UILabel questPointsLabel;
	public UILabel attackLabel;
	public UILabel citizenPointsLabel;
	public UILabel defenceLabel;
	public UILabel healthLabel;
	public ItemTileButton headTile;
	public ItemTileButton bodyTile;
	public ItemTileButton armLTile;
	public ItemTileButton armRTile;
	public ItemTileButton legsTile;
	public float timer = 5;
	public bool loading = false;

	public void Enable()
	{
		loadingIcon.SetActive(true);
		loading = true;
		profileRoot.SetActive(false);
		base.Enable();
	}

	public void DisplayProfile(PlayerProfileDisplayData profile)
	{
		if(!isDisplayed)
			return;
		loading = false;
		activeProfile = profile;
		loadingIcon.SetActive(false);
		profileRoot.SetActive(true);
		nameLabel.text = activeProfile.name;
		for (int i = 0; i < profile.equips.Count; i++) {
			if(profile.equips[i].slotIndex == 0)
				headTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
			else if(profile.equips[i].slotIndex == 1)
				bodyTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
			else if(profile.equips[i].slotIndex == 2)
				armLTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
			else if(profile.equips[i].slotIndex == 3)
				armRTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
			else if(profile.equips[i].slotIndex == 4)
				legsTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
			else if(profile.equips[i].slotIndex == 5)
				headTile.LoadBasicAmor(profile.equips[i].uniqueItemId, profile.equips[i].level);
		}
		//questPointsLabel.text = profile.questPoints.ToString();
		citizenPointsLabel.text = profile.citizenPoints.ToString();
		healthLabel.text = profile.baseHealth.ToString() + " + " +profile.bonusHealth.ToString();
		attackLabel.text = profile.baseStrength.ToString() + " + " +profile.bonusStrength.ToString();
		defenceLabel.text = profile.baseDefense.ToString() + " + " +profile.bonusDefense.ToString();
		for (int i = 0; i < profile.badgeIDs.Count; i++) {
			
		}
	}

	public override void Disable()
	{
		activeProfile = null;
		profileRoot.SetActive(false);
		timer = 5;
		loading = false;
		base.Disable();
	}

	public void Update()
	{
		if(loading)
		{
			timer -= Time.deltaTime;
			if(timer < 0 && isDisplayed)
				GUIManager.Instance.HideProfile();
		}
	}
}
