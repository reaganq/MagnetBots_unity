using UnityEngine;
using System.Collections;

public class TeleporterGUIController : BasicGUIController {

	public UISprite[] townPortraitObjects;
	public GameObject scrollView;
	public GameObject scrollPanelRoot;
	//public UILabel[] townLabels;
	public GameObject detailsBox;
	public GameObject detailPortraitBox;
	public UISprite detailPortraitSprite;
	public UILabel descriptionlabel;
	public string naSpriteName;
	public int selectedIndex;
	public UIPlayTween portraitTweener;
	public TweenPosition portraitPosTween;
	public Transform portraitPosRef;

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
				LoadTownAtlasSprite(townPortraitObjects[i], i);
			}
		}
	}

	public void LoadTownAtlasSprite(UISprite sprite, int index)
	{
		GameObject Atlas = Resources.Load(GeneralData.towns[index].AtlasName) as GameObject;
		sprite.atlas = Atlas.GetComponent<UIAtlas>();
		sprite.spriteName = GeneralData.towns[index].IconPath;
		sprite.collider.enabled = true;
	}

	public void OnTownButtonPressed(int index)
	{
		selectedIndex = index;
		LoadTownAtlasSprite(detailPortraitSprite, selectedIndex);

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
