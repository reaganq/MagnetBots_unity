using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System;

public class ItemUtils {
	
	public static void DisplayItemPart(RPGItem item, MainWindowEditor Data)
	{
		if (string.IsNullOrEmpty(item.IconPath))
			item.IconPath = string.Empty;
		
        if (string.IsNullOrEmpty(item.AtlasName))
            item.AtlasName = string.Empty;
		//if (string.IsNullOrEmpty(item.PrefabName))
		//	item.PrefabName = string.Empty;
		
		//if (item.LevelItem == 0)
		//	item.LevelItem = 1;
		
		//List<RPGItemCategory> itemCategories = Storage.Load<RPGItemCategory>(new RPGItemCategory());
		
		EditorUtils.Separator();
		
		EditorUtils.Label("General item info");
		
		item.IconPath = EditorUtils.TextField(item.IconPath, "Icon name");
        
        item.AtlasName = EditorUtils.TextField(item.AtlasName, "Atlas name");
		
		//item.PrefabName = EditorUtils.TextField(item.PrefabName, "Prefab name");
		
		
		/*item.Stackable = EditorUtils.Toggle(item.Stackable, "Stack", 100,FieldTypeEnum.BeginningOnly);
		if (item.Stackable)
		{
			item.MaximumStack = EditorUtils.IntField(item.MaximumStack, "Stack amount", 100,FieldTypeEnum.Middle);
		}*/
		//EditorGUILayout.EndHorizontal();
		
		//item.Destroyable = EditorUtils.Toggle(item.Destroyable, "Destroyable");
		
		//item.Droppable = EditorUtils.Toggle(item.Droppable, "Droppable");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		//item.LevelItem = EditorGUILayout.IntSlider(item.LevelItem, -1, GlobalSettings.MaxLevel,GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();
		
		item.BuyValue = EditorUtils.IntField(item.BuyValue, "buy Value");
        item.SellValue = EditorUtils.IntField(item.SellValue, "sell value");
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Rarity");
		
		item.Rarity = (RarityType)EditorGUILayout.EnumPopup(item.Rarity ,GUILayout.Width(300));
        
        EditorGUILayout.PrefixLabel("Item Type");
        
        item.ItemCategory = (ItemType)EditorGUILayout.EnumPopup(item.ItemCategory , GUILayout.Width(300));
        
		EditorGUILayout.EndHorizontal();
		
		EditorUtils.Separator();
		
		/*if (GUILayout.Button("Add item category", GUILayout.Width(350)))
		{
			RPGItemCategory itemCateogry = new RPGItemCategory();
			item.Categories.Add(itemCateogry);
		}
		foreach(RPGItemCategory itemCategory in item.Categories)
		{
			AddItemCategory(itemCategory, itemCategories);
			
			if (GUILayout.Button("Remove", GUILayout.Width(100)))
			{
				item.Categories.Remove(itemCategory);
				break;
			}
			
			EditorGUILayout.EndHorizontal();
		}*/
		
		//EditorUtils.Separator();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Stackable");
		item.Stackable = EditorGUILayout.Toggle(item.Stackable ,GUILayout.Width(50));
		EditorGUILayout.PrefixLabel("Upgradeable");
		item.IsUpgradeable = EditorGUILayout.Toggle(item.IsUpgradeable ,GUILayout.Width(50));
		EditorGUILayout.PrefixLabel("Usable");
		item.IsUsable = EditorGUILayout.Toggle(item.IsUsable ,GUILayout.Width(50));
		EditorGUILayout.PrefixLabel("Equippable");
		item.IsEquippable = EditorGUILayout.Toggle(item.IsEquippable ,GUILayout.Width(50));
		EditorGUILayout.EndHorizontal();
		if (item.IsUsable)
		{
			AddUsableItem(item, Data);
		}
		if(item.IsEquippable)
		{
			EditorUtils.Label("Equiped slot");
			item.EquipmentSlotIndex = (EquipmentSlots)EditorGUILayout.EnumPopup(item.EquipmentSlotIndex ,GUILayout.Width(300));
		}
	}
	
	public static void AddUsableItem(UsableItem usableItem, MainWindowEditor Data)
	{
		
		//usableItem.Recharge = EditorUtils.FloatField(usableItem.Recharge ,"Cooldown");
		
		EditorGUILayout.Separator();
		EditorUtils.Label("Usable effects");
		
		//EffectUtils.EffectsEditor(usableItem.Effects, Data);
		
		//ConditionsUtils.Conditions(usableItem.UseConditions, Data);
	}
	
	/*public static void AddItemCategory(RPGItemCategory itemCategoryID, List<RPGItemCategory> categories)
	{
		string[] names = new string[categories.Count];
		int[] ID = new int[categories.Count];
		int index = 0;
		foreach(RPGItemCategory item in categories)
		{
			names[index] = item.Name;
			ID[index] = item.ID;
			index++;
		}
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Item category");
		itemCategoryID.ID = EditorGUILayout.IntPopup(itemCategoryID.ID, names, ID ,GUILayout.Width(300));
	}*/
	
	public static void AddEquiped(Equiped equiped, MainWindowEditor Data)
	{
		EditorUtils.Separator();

		for (int i = 0; i < equiped.FBXName.Count; i++) {

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" FBX location ");
			equiped.FBXName[i] = EditorUtils.TextField(equiped.FBXName[i], "fbxname");
			//DisplayShopItem(item);
			if (GUILayout.Button("Delete", GUILayout.Width(200)))
			{
				equiped.FBXName.Remove(equiped.FBXName[i]);
			}
			EditorGUILayout.EndHorizontal();

		}
		
		if (GUILayout.Button("Add fbx location", GUILayout.Width(200)))
		{
			equiped.FBXName.Add("Armor/");
		}

		EditorUtils.Separator();
		//EffectUtils.EffectsEditor(equiped.WornEffects, Data, EffectTypeUsage.Equiped);
		
		//ConditionsUtils.Conditions(equiped.Conditions, Data);
		
		//equiped.Durability = EditorUtils.IntField(equiped.Durability, "Durability", 300, FieldTypeEnum.WholeLine);
		
		//EditorGUILayout.Separator();
		//EditorGUILayout.BeginHorizontal();
		        
		/*if (GUILayout.Button("Add slot", GUILayout.Width(150)))
		{
			equiped.EquipmentSlots.Add(new RPGEquipmentSlot());
		}
		EditorGUILayout.EndHorizontal();*/
		
		/*foreach(RPGEquipmentSlot slot in equiped.EquipmentSlots)
		{
			AddEquipmentSlot(slot, Data.equipmentSlotEditor.Slots);
			
			if (GUILayout.Button("Delete", GUILayout.Width(150)))
			{
				equiped.EquipmentSlots.Remove(slot);
				break;
			}
			EditorGUILayout.EndHorizontal();
		}*/
	}
	
	/*public static void AddEquipmentSlot(RPGEquipmentSlot slot, List<RPGEquipmentSlot> slots)
	{
		List<IItem> items = new List<IItem>();
		
		foreach(IItem item in slots)
			items.Add((IItem)item);
		
		slot.ID = EditorUtils.IntPopup(slot.ID, items, "Eq slot ID", 200, FieldTypeEnum.BeginningOnly);
	}*/
	
	/*public static void AddWeapon(RPGWeapon weapon)
	{
		EditorUtils.Separator();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Melee / range");
		weapon.Attack = (WeaponCombatSkillType)EditorGUILayout.EnumPopup(weapon.Attack ,GUILayout.Width(100));
		
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Minimum dmg");
		weapon.MinimumDmg = EditorGUILayout.IntField(weapon.MinimumDmg ,GUILayout.Width(100));
		
		EditorGUILayout.PrefixLabel("Maximum dmg");
		weapon.MaximumDmg = EditorGUILayout.IntField(weapon.MaximumDmg ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Attack speed");
		weapon.AttackSpeed = EditorGUILayout.FloatField(weapon.AttackSpeed ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Minimum range");
		weapon.MinimumRange = EditorGUILayout.IntField(weapon.MinimumRange ,GUILayout.Width(100));
		
		EditorGUILayout.PrefixLabel("Maximum range");
		weapon.MaximumRange = EditorGUILayout.IntField(weapon.MaximumRange ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Is ammo");
		weapon.IsAmmo = EditorGUILayout.Toggle(weapon.IsAmmo ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Need ammo");
		weapon.NeedAmmo = EditorGUILayout.Toggle(weapon.NeedAmmo ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Weapon type");
		weapon.WeaponType = (WeaponCategoryType)EditorGUILayout.EnumPopup(weapon.WeaponType ,GUILayout.Width(100));
		EditorGUILayout.EndHorizontal();
	}*/
	
	public static void ItemGenerator(RPGItem item, ItemGenerator generator)
	{
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Lvl increase");
		
		generator.Frequency = EditorGUILayout.IntSlider(generator.Frequency,1,20,  GUILayout.Width(100));
		if (GUILayout.Button("Generate", GUILayout.Width(200)))
		{
			generator.Calculate(item);
		}
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Use adjective");
		generator.UseNames = EditorGUILayout.Toggle(generator.UseNames, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();	
		
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
			
		EditorGUILayout.LabelField("Names", generator.GetNames);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Increasing price");
		generator.Price = EditorGUILayout.Toggle(generator.Price, GUILayout.Width(100));
		if (generator.Price)
		{
			generator.PriceType = (IncresingTypeEnum)EditorGUILayout.EnumPopup(generator.PriceType, GUILayout.Width(100));
		
			EditorGUILayout.PrefixLabel("amount");
			generator.IncreasingPrice = EditorGUILayout.FloatField(generator.IncreasingPrice, GUILayout.Width(100));
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Prices", generator.GetPrices);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Increasing effect");
		generator.Effects = EditorGUILayout.Toggle(generator.Effects, GUILayout.Width(100));
		if (generator.Effects)
		{
			generator.EffectType = (IncresingTypeEnum)EditorGUILayout.EnumPopup(generator.EffectType, GUILayout.Width(100));
		
			EditorGUILayout.PrefixLabel("amount");
			generator.IncreasingEffect = EditorGUILayout.FloatField(generator.IncreasingEffect, GUILayout.Width(100));
		}
		EditorGUILayout.EndHorizontal();
	}
	
	/*public static void WeaponGenerator(WeaponGenerator generator)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Increasing dmg");
		generator.WeaponDamage = EditorGUILayout.Toggle(generator.WeaponDamage, GUILayout.Width(100));
		if (generator.WeaponDamage)
		{
			generator.WeaponIncreasingDamageType = (IncresingTypeEnum)EditorGUILayout.EnumPopup(generator.WeaponIncreasingDamageType, GUILayout.Width(100));
		
			EditorGUILayout.PrefixLabel("amount");
			generator.WeaponAmount = EditorGUILayout.IntField(generator.WeaponAmount, GUILayout.Width(100));
		}
		EditorGUILayout.Separator();
		EditorGUILayout.EndHorizontal();
		if (generator.WeaponDamage)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Damage", generator.GetDamage);
			EditorGUILayout.EndHorizontal();
		
		}
	}*/
	
	public static void ArmorGenerator(ArmorGenerator generator)
	{
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.PrefixLabel("Increasing armor");
		generator.ArmorClassValue = EditorGUILayout.Toggle(generator.ArmorClassValue, GUILayout.Width(100));
		if (generator.ArmorClassValue)
		{
			generator.ArmorIncreasingDamageType = (IncresingTypeEnum)EditorGUILayout.EnumPopup(generator.ArmorIncreasingDamageType, GUILayout.Width(100));
		
			EditorGUILayout.PrefixLabel("amount");
			generator.ArmorAmount = EditorGUILayout.IntField(generator.ArmorAmount, GUILayout.Width(100));
		}
		EditorGUILayout.EndHorizontal();
		if (generator.ArmorClassValue)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			//EditorGUILayout.LabelField("Armor", generator.GetArmor);
			EditorGUILayout.EndHorizontal();
		}
	}
}
