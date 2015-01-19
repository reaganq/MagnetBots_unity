using UnityEngine;
using System.Collections;

public class CategoryButton : MonoBehaviour {

	public BasicGUIController owner;
	public UISprite backgroundSprite;
	public UISprite iconSprite;
	public UILabel name;
	public int index;
	public int categoryLevel;
	public Color selectedColor;
	public Color unselectedColor;
	public Color disabledColor = Color.black;
	public GameObject newItemCountFlag;
	public UILabel newItemCounter;
	//0 is main category, 1 is sub etc

	public void Start()
	{
		backgroundSprite.color = unselectedColor;
	}

	public void LoadCategoryButton(ItemCategoryData category, int i, int level)
	{
		if(iconSprite != null) iconSprite.spriteName = category.iconName;
		if(name != null) name.text = category.name;
		index = i;
		categoryLevel = level;
		if(newItemCountFlag != null) newItemCountFlag.SetActive(false);
	}

	public void LoadSubcategoryButton(ItemCategoryData category, int i, int level, int newItemCount)
	{
		if(iconSprite != null) iconSprite.spriteName = category.iconName;
		if(name != null) name.text = category.name;
		index = i;
		categoryLevel = level;
		if(newItemCountFlag != null)
		{
			if(newItemCount > 0)
			{
				newItemCounter.text = newItemCount.ToString();
				newItemCountFlag.SetActive(true);
			}
			else
				newItemCountFlag.SetActive(false);
		}
	}

	void OnPress(bool isPressed)
	{
		if(!isPressed)
		{
			owner.OnCategoryPressed(index, categoryLevel);
		}
	}

	public void SelectCategory()
	{
		Debug.Log("selected" + index);
		backgroundSprite.color = selectedColor;
	}

	public void DeselectCategory()
	{
		Debug.Log("deselected" + index);
		backgroundSprite.color = unselectedColor;
	}
}