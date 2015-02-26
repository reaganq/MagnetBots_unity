using UnityEngine;
using System.Collections;

public class PreffixSolver  {

 	public static void GiveItem(PreffixType preffix, int ID, int level, int amount)
 	{
     	switch(preffix)
     	{
         	case PreffixType.ARMOR:
				RPGArmor armor = Storage.LoadById<RPGArmor>(ID, new RPGArmor());
             //armor.CurrentDurability = armor.Durability;
				PlayerManager.Instance.Hero.AddItem(armor, level, amount);
            	break;
         	case PreffixType.ITEM:
            	RPGItem item = Storage.LoadById<RPGItem>(ID, new RPGItem());
            	PlayerManager.Instance.Hero.AddItem(item, level, amount);
			break;
		case PreffixType.NAKEDARMOR:
			RPGArmor nakedArmor = Storage.LoadById<RPGArmor>(ID, new RPGArmor());
			PlayerManager.Instance.Hero.AddItem(nakedArmor, level, amount);
			break;
     	}
 	}

	public static void GiveNakedArmor(int ID)
	{
	}
}

public enum PreffixType
{
	QUEST = 0,
	PARAGRAPH = 1,
	LINETEXT = 2,
	ITEM = 3,
	SKILLPOINT = 4,
	SKILL = 5,
	ATTRIBUTEPOINT = 6,
	ATTRIBUTE = 7,
	NPC = 8,
	WEAPON = 9,
	ARMOR = 10,
	XP = 11,
	XPPERCENT = 12,
	ENEMY = 13,
	SPELL = 14,
	WORLDOBJECT = 15,
	REPUTATION = 16,
	NAKEDARMOR = 17
}
