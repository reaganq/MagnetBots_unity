using UnityEngine;
using System.Collections;

public class TeleporterGUIController : BasicGUIController {

	public GameObject[] townPortraitObjects;
	public UILabel[] townLabels;
	public GameObject detailsBox;
	public UILabel descriptionlabel;

	public void Start()
	{
		for (int i = 0; i < townPortraitObjects.Length; i++) {
			UISprite portrait = townPortraitObjects[i].GetComponent<UISprite>();
			GameObject Atlas = Resources.Load(GeneralData.towns[i].atlasPath) as GameObject;
			portrait.atlas = Atlas.GetComponent<UIAtlas>();
			portrait.spriteName = GeneralData.towns[i].spriteName;
			//portrait.LoadAtlasSprite(GeneralData.towns[i].atlasPath, GeneralData.towns[i].spriteName);
			townLabels[i].text = GeneralData.towns[i].Name;
		}
	}

	public void OnTownButtonPressed(int index)
	{
		DisplayTownDetails(index);
		detailsBox.SetActive(true);
	}

	public void DisplayTownDetails(int index)
	{
		descriptionlabel.text = GeneralData.towns[index].Description;
	}

	public void OnBackButtonPressed()
	{
		Disable();
	}

	public void OnTravelButtonPressed(int index)
	{
		//travel to generaldata.towns[index].sceneID;
	}

}
