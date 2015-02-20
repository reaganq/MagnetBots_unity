using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

public class ActivityUtils {

	public static void DisplayActivityComponents(NPCActivity activity, MainWindowEditor Data)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Button Text");
		activity.npcButtonText = EditorGUILayout.TextField(activity.npcButtonText, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Atlas Path");
		activity.npcIconAtlas = EditorGUILayout.TextField(activity.npcIconAtlas, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Sprite Name");
		activity.npcIconSprite = EditorGUILayout.TextField(activity.npcIconSprite, GUILayout.Width(500));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		activity.conversationID = EditorUtils.IntPopup(activity.conversationID, Data.conversationEditor.items, "conversationID: ", FieldTypeEnum.Middle);
		/*for (int i = 0; i < activity.conversationIDs.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			activity.conversationIDs[i] = EditorUtils.IntPopup(activity.conversationIDs[i], Data.conversationEditor.items, "conversationID: " + i, 90, FieldTypeEnum.Middle);
			if(GUILayout.Button("Remove conversation id", GUILayout.Width(400)))
			{
				activity.conversationIDs.Remove(activity.conversationIDs[i]);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}
		
		if(GUILayout.Button("Add Conversation ID", GUILayout.Width(400)))
		{
			activity.conversationIDs.Add(1);
		}*/
		ConditionsUtils.Conditions(activity.Conditions, Data);
		EditorUtils.Separator();
	}
}
