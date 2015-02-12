using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

public class ConversationEditor : BaseEditorWindow {

	public ConversationEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Conversations";
		
		Init(guiSkin, data);
		
		LoadData();
	}

	protected override void LoadData()
	{
		List<RPGConversation> list = Storage.Load<RPGConversation>(new RPGConversation());
		items = new List<IItem>();
		foreach(RPGConversation category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGConversation();
	}
	
	public List<RPGConversation> Conversations
	{
		get
		{
			List<RPGConversation> list = new List<RPGConversation>();
			foreach(IItem category in items)
			{
				list.Add((RPGConversation)category);
			}
			return list;
		}
	}

	protected override void SaveCollection()
	{
		List<RPGConversation> conversations = Conversations;
		Storage.Save<RPGConversation>(conversations, new RPGConversation());
	}

	protected override void EditPart()
	{
		RPGConversation s = (RPGConversation)currentItem;
		EditorGUILayout.Separator();


		for (int i = 0; i < s.ConversationParagraphs.Count; i++)
		{
			EditorGUILayout.BeginVertical(skin.box);
			//EditorGUILayout.BeginHorizontal();

			//EditorGUILayout.EndHorizontal();
			AddParagraph(s.ConversationParagraphs[i], i);
			if (GUILayout.Button("Delete Paragraph", GUILayout.Width(400)))
			{
				s.ConversationParagraphs.Remove(s.ConversationParagraphs[i]);
				break;
			}
			EditorGUILayout.EndVertical();
		}

		if (GUILayout.Button("Add paragraph", GUILayout.Width(400)))
		{
			RPGParagraph p = new RPGParagraph();
			s.ConversationParagraphs.Add(p);
		}
		
		currentItem = s;
	}

	public void AddParagraph(RPGParagraph s, int j)
	{
		EditorUtils.Label("paragraph id: "+j);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("NPC text");
		s.ParagraphText = EditorGUILayout.TextArea(s.ParagraphText, GUILayout.Width(700));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Next Paragraph Interaction");
		s.nextParagraphCondition = (NextParagraphInteraction)EditorGUILayout.EnumPopup(s.nextParagraphCondition, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		GUIUtils.ConditionsEvents(s.Conditions, s.Actions, Data);

		for (int i = 0; i < s.LineTexts.Count; i++) {
			EditorGUILayout.BeginVertical(skin.box);
			AddLinetext(s.LineTexts[i], i);
			if (GUILayout.Button("Delete Player Reply" + i, GUILayout.Width(200)))
			{
				s.LineTexts.Remove(s.LineTexts[i]);
				break;
			}
			EditorGUILayout.EndVertical();
		}
		if (GUILayout.Button("Add Player Reply", GUILayout.Width(200)))
		{
			LineText p = new LineText();
			s.LineTexts.Add(p);
		}
	}

	public void AddLinetext(LineText s, int j)
	{
		EditorUtils.Label("Player reply: " + j);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("reply text");
		s.Text = EditorGUILayout.TextArea(s.Text, GUILayout.Width(700));
		EditorGUILayout.EndHorizontal();
		GUIUtils.ConditionsEvents(s.Conditions, s.Events, Data);
	}
}
