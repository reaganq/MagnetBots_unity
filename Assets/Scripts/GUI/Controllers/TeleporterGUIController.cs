using UnityEngine;
using System.Collections;

public class TeleporterGUIController : BasicGUIController {

	public UISprite[] townPortraitObjects;
	public GameObject scrollView;
	//public UILabel[] townLabels;
	public GameObject detailsBox;
	public GameObject detailPortraitBox;
	public UILabel descriptionlabel;
	public string naSpriteName;
	public int selectedIndex;
	public UIPlayTween portraitTweener;
	public TweenPosition portraitPosTween;

	public void Start()
	{
		for (int i = 0; i < townPortraitObjects.Length; i++) {
			if(i + 1 > GeneralData.towns.Count)
			{
				townPortraitObjects[i].spriteName = naSpriteName;
				townPortraitObjects[i].collider.enabled = false;
			}
			else
			{
				GameObject Atlas = Resources.Load(GeneralData.towns[i].atlasPath) as GameObject;
				townPortraitObjects[i].atlas = Atlas.GetComponent<UIAtlas>();
				townPortraitObjects[i].spriteName = GeneralData.towns[i].spriteName;
				townPortraitObjects[i].collider.enabled = true;
			}
		}
	}

	public void OnTownButtonPressed(int index)
	{
		selectedIndex = index;
		//DisplayTownDetails();
		//portraitPosTween.from = townPortraitObjects[selectedIndex].transform.position;
	}

	public void DisplayTownDetails()
	{
		detailsBox.SetActive(true);
		descriptionlabel.text = GeneralData.towns[selectedIndex].Description;
	}

	public void OnBackButtonPressed()
	{

	}

	public void OnExitButtonPressed()
	{
		Disable();
	}

	public void OnTravelButtonPressed()
	{
		//travel to generaldata.towns[selectedIndex].sceneID;
	}

}
