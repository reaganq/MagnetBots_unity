using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ArmorEditor : BaseEditorWindow 
{
	//ArmorGenerator generator = new ArmorGenerator();
	
	public ArmorEditor(GUISkin guiSkin, MainWindowEditor data)
	{
		EditorName = "Armor";
		
		Init(guiSkin, data);
		LoadData();
	}
	
	protected override void LoadData()
	{
		List<RPGArmor> list = Storage.Load<RPGArmor>(new RPGArmor());
		items = new List<IItem>();
		foreach(RPGArmor category in list)
		{
			items.Add((IItem)category);
		}
	}
	
	/*protected override void AditionalSwitch()
	{
		if (MenuMode ==  MenuModeEnum.ThirdWindow)
		{
			GenerateStrongerItem();
		}
	}*/
	
	protected override void StartNewIItem()
	{
		currentItem = new RPGArmor();
	}
	
	public List<RPGArmor> Armors
	{
		get
		{
			List<RPGArmor> list = new List<RPGArmor>();
			foreach(IItem category in items)
			{
				list.Add((RPGArmor)category);
			}
			return list;
		}
	}
	
	protected override void SaveCollection()
	{
		Storage.Save<RPGArmor>(Armors, new RPGArmor());
	}
	
	protected override void EditPart()
	{
		RPGArmor s = (RPGArmor)currentItem;
		        
        EditorGUILayout.PrefixLabel("has ability?");
        
        s.HasAbility = EditorGUILayout.Toggle(s.HasAbility, GUILayout.Width(300));
        
		if(s.HasAbility)
		{
        //s.AbilityIconPath = EditorUtils.TextField(s.AbilityIconPath, "ability icon path");
        	s.AbilityIconPath = EditorGUILayout.TextField(s.AbilityIconPath, GUILayout.Width(500));
			s.AbilityAtlasPath = EditorGUILayout.TextField(s.AbilityAtlasPath, GUILayout.Width(500));
        	s.AbilityString = EditorGUILayout.TextField(s.AbilityString, GUILayout.Width(1000));
		}

		if(s.EquipmentSlotIndex == EquipmentSlots.Head || s.EquipmentSlotIndex == EquipmentSlots.Face)
		{
			EditorGUILayout.PrefixLabel("portrait: ");
			s.headPortraitPath = EditorGUILayout.TextField(s.headPortraitPath, GUILayout.Width(500));
		}

		EditorGUILayout.PrefixLabel("max level: ");
		s.maxLevel = EditorGUILayout.IntField(s.maxLevel, GUILayout.Width(100));

		if(s.ItemCategory == ItemType.Armor)
		{
			foreach(ArmorStatsSet set in s.armorStatsSets)
			{
				EditorGUILayout.BeginVertical(skin.box);
				EditorUtils.Label("armor stats: ");
				foreach(ArmorStat stat in set.armorStats)
				{
					EditorGUILayout.BeginHorizontal();
					stat.armorStatsValue = EditorGUILayout.IntField(stat.armorStatsValue, GUILayout.Width(100));
					stat.armorStatsType = (ArmorStatsType)EditorGUILayout.EnumPopup(stat.armorStatsType, GUILayout.Width(300));
					EditorGUILayout.EndHorizontal();

					if (GUILayout.Button("Delete armor stat set", GUILayout.Width(150)))
					{
						set.armorStats.Remove(stat);
						break;
					}
					EditorGUILayout.Separator();
				}
				if(GUILayout.Button("add armor stat", GUILayout.Width(150)))
				{
					set.armorStats.Add(new ArmorStat());
				}
				EditorGUILayout.Separator();
				//DisplayShopItem(item);
				if (GUILayout.Button("Delete armor stat set", GUILayout.Width(200)))
				{
					s.armorStatsSets.Remove(set);
					break;
				}
				EditorGUILayout.EndVertical();
			}

			
			if (GUILayout.Button("Add armor stat set", GUILayout.Width(200)))
			{
				s.armorStatsSets.Add(new ArmorStatsSet());
			}
		}

		ItemUtils.DisplayItemPart(s, Data);
		
		ItemUtils.AddEquiped(s, Data);
		
		//EditorUtils.Label("Effects on hit");
		
		//EffectUtils.EffectsEditor(s.EffectsOnHit, Data, EffectTypeUsage.ArmorTakeHit);
		
		//EditorUtils.Separator();
		
		currentItem = s;
	}
	
	private List<RPGArmor> generatorCollection;
}
