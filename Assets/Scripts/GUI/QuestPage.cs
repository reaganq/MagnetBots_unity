using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestPage : MonoBehaviour {

	public GameObject questItemsGrid;
	public GameObject itemTilePrefab;
	public UIGrid questItemsPanelGrid;
	public List<ItemTileButton> itemTiles;
	public List<InventoryItem> questItems = new List<InventoryItem>();
	public QuestStep activeQuestStep;
	public UILabel questLogNoteLabel;
	public UILabel questStepNumberLabel;
	public bool isItemTask;

	public void LoadQuestStep(QuestStep step, int totalSteps, int thisStep)
	{
		activeQuestStep = step;
		isItemTask = false;
		for (int i = 0; i < step.Tasks.Count; i++) {
			if(step.Tasks[i].TaskType == TaskTypeEnum.BringItem)
				isItemTask = true;
		}
		questStepNumberLabel.text = thisStep + "/" + totalSteps;
		LoadQuestPage();
	}

	public void LoadQuestPage()
	{
		questLogNoteLabel.text = activeQuestStep.QuestLogNote;
		if(isItemTask)
			DisplayQuestItems();
	}

	public void DisplayQuestItems()
	{
		questItemsGrid.SetActive(true);
		questItems.Clear();
		for (int i = 0; i < activeQuestStep.Tasks.Count; i++) {
			if(activeQuestStep.Tasks[i].TaskType == TaskTypeEnum.BringItem)
			{
				InventoryItem item = new InventoryItem();
				if(activeQuestStep.Tasks[i].PreffixTarget == PreffixType.ARMOR)
				{
					item.GenerateNewInventoryItem(Storage.LoadById<RPGArmor>(activeQuestStep.Tasks[i].TaskTarget, new RPGArmor()), activeQuestStep.Tasks[i].Tasklevel, activeQuestStep.Tasks[i].AmountToReach);
				}
				else if(activeQuestStep.Tasks[i].PreffixTarget == PreffixType.ITEM)
				{
					item.GenerateNewInventoryItem(Storage.LoadById<RPGItem>(activeQuestStep.Tasks[i].TaskTarget, new RPGItem()), activeQuestStep.Tasks[i].Tasklevel, activeQuestStep.Tasks[i].AmountToReach);
				}
				questItems.Add(item);
			}
		}
		int num = questItems.Count - itemTiles.Count;
		if(num>0)
		{
			for (int i = 0; i < num; i++) {
				GameObject itemTile = NGUITools.AddChild(questItemsPanelGrid.gameObject, itemTilePrefab);
				ItemTileButton tileButton = itemTile.GetComponent<ItemTileButton>();
				itemTiles.Add(tileButton);
				tileButton.index = itemTiles.Count-1;
			}
		}
		for (int i = 0; i < itemTiles.Count; i++) {
            if(i>=questItems.Count)
            {
                itemTiles[i].gameObject.SetActive(false);
            }
            else
            {
                itemTiles[i].gameObject.SetActive(true);
                itemTiles[i].LoadQuestDisplayTile(questItems[i], false);
            }
            questItemsPanelGrid.Reposition();
        }
		questItemsPanelGrid.Reposition();
	}

}
