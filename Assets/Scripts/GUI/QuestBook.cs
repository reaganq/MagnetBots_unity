using UnityEngine;
using System.Collections;

public class QuestBook : MonoBehaviour {

	public int index;
	public UILabel questNameLabel;
	public QuestGUIController questGUI;

	public void OnQuestBookPressed()
	{
		questGUI.QuestBookPressed(index);
	}

	public void LoadQuest(RPGQuest quest, int id)
	{
		index = id;
		questNameLabel.text = quest.Name;
	}
}
