using UnityEngine;
using UnityEditor;
using System.Collections;

public class EventUtils 
{
	public static void DisplayEvent(ActionEvent action, MainWindowEditor data)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Action type");
		action.ActionType = (ActionEventType)EditorGUILayout.EnumPopup(action.ActionType, GUILayout.Width(150));
		switch(action.ActionType)
		{
			//end conversation
		case ActionEventType.EndConversation : 
			break;
			//take item
		case ActionEventType.TakeItem:
			action.PreffixItem = (PreffixType)EditorGUILayout.EnumPopup(action.PreffixItem, GUILayout.Width(200));
			
			switch (action.PreffixItem)
			{
			case PreffixType.ARMOR:
				action.Item = EditorUtils.IntPopup(action.Item, data.armorEditor.items, "Armor", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.ITEM:
				action.Item = EditorUtils.IntPopup(action.Item, data.itemEditor.items, "Item", 90, FieldTypeEnum.Middle);
				break;
				
			/*case PreffixType.QUEST:
				action.Item = EditorUtils.IntPopup(action.Item, data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.WEAPON:
				action.Item = EditorUtils.IntPopup(action.Item, data.weaponEditor.items, "Weapon", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.SKILL:
				action.Item = EditorUtils.IntPopup(action.Item, data.skillEditor.items, "Skill", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.SPELL:
				action.Item = EditorUtils.IntPopup(action.Item, data.skillEditor.items, "Spell", 90, FieldTypeEnum.Middle);
				break;
				
			case PreffixType.REPUTATION:
				action.Item = EditorUtils.IntPopup(action.Item, data.reputationEditor.items, "Reputation", 90, FieldTypeEnum.Middle); 
				break;*/
			}
			action.Amount = EditorUtils.IntField(action.Amount, "Amount: ", 90, FieldTypeEnum.Middle);
			break;
		case ActionEventType.TakeQuestStepItemsTask:

			//quest start
		case ActionEventType.QuestStart:
		case ActionEventType.QuestFailed:
		case ActionEventType.DisplayQuestDetails:
		case ActionEventType.DisplayQuestInfo:
		case ActionEventType.DisplayQuestStatus:
		case ActionEventType.GiveQuestRewards:
			//quest end
		case ActionEventType.QuestEnd:
			action.Item = EditorUtils.IntPopup(action.Item, data.questEditor.items, "Quest", 90, FieldTypeEnum.Middle);
			break;

		case ActionEventType.Continueconversation:
			break;
		case ActionEventType.GoToParagraph:
			action.Item = EditorGUILayout.IntField( action.Item, GUILayout.Width(100));
			break;
		}
	}
}
