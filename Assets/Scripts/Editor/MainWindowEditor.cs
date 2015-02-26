using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MainWindowEditor : EditorWindow 
{
	public MainWindowTypeEnum MainWindowType;
	
	private GUISkin Skin;
	private bool loadWindows = true; 
	
	private List<BaseEditorWindow> FirstButtonRow;
	private List<BaseEditorWindow> SecondButtonRow;

	public bool isValidateDisplay;
	
	//windows
	//public ItemCategoryEditor itemCategory;
	//public SkillEditor skillEditor;
	//public AttributeEditor attributeEditor;
	public ArmorEditor armorEditor;
	public ArenaEditor arenaEditor;
	public EnemyEditor enemyEditor;
	public ConversationEditor conversationEditor;
	public ShopEditor shopEditor;
	public CurrencyEditor currencyEditor;
	public ItemEditor itemEditor;
	public NPCEditor npcEditor;
	public MiniGameEditor minigameEditor;
	public ServiceEditor serviceEditor;
	public NPCQuestEditor npcQuestEditor;
	public QuestEditor questEditor;
	public TownEditor townEditor;
	public BadgeEditor badgeEditor;
	public ConstructionEditor constructionEditor;
	
	[MenuItem("Window/RPG")] 	
	static void Init()
	{
		MainWindowEditor window = (MainWindowEditor)EditorWindow.GetWindow(typeof(MainWindowEditor));
		window.autoRepaintOnSceneChange = false;
		window.Show();
		
	}
	
	public void InitWindows()
	{
		Skin = (GUISkin)Resources.Load("GUI/EditorGUI");
		
		//windows init
		npcEditor = new NPCEditor(Skin, this);

		armorEditor = new ArmorEditor(Skin, this);

		arenaEditor = new ArenaEditor(Skin, this);

		enemyEditor = new EnemyEditor(Skin, this);

		currencyEditor = new CurrencyEditor(Skin, this);
		
		conversationEditor = new ConversationEditor(Skin, this);
		
		shopEditor = new ShopEditor(Skin, this);

		questEditor = new QuestEditor(Skin, this);
		
		itemEditor = new  ItemEditor(Skin, this);

		minigameEditor = new MiniGameEditor(Skin, this);

		serviceEditor = new ServiceEditor(Skin, this);

		npcQuestEditor = new NPCQuestEditor(Skin, this);

		townEditor = new TownEditor(Skin, this);

		badgeEditor = new BadgeEditor(Skin, this);
		
		constructionEditor = new ConstructionEditor(Skin, this);

		loadWindows = false;
	}
	
	void OnGUI()
	{
		if (loadWindows)
			InitWindows();
		
		
		GUILayout.BeginArea(new Rect(5,5, position.width - 10, 140), Skin.box);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reload data", GUILayout.Width(200)))
		{
			InitWindows();
		}
		GUILayout.EndHorizontal();
		
		
		GUILayout.BeginHorizontal();
		
		if (armorEditor == null)
		{
			InitWindows();
		}
		
		if (GUILayout.Button(armorEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Armor;
		}

		if (GUILayout.Button(conversationEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Conversation;
		}

		if (GUILayout.Button(itemEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Item;
		}
		
		if (GUILayout.Button(npcEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.NPC;
		}
		
		if (GUILayout.Button(shopEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Shop;
		}

		if (GUILayout.Button(arenaEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Arena;
		}

		if (GUILayout.Button(enemyEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Enemy;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(currencyEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Currency;
		}

		if (GUILayout.Button(minigameEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.MiniGame;
		}

		if (GUILayout.Button(serviceEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Service;
		}

		if (GUILayout.Button(npcQuestEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.NPCQuest;
		}

		if (GUILayout.Button(questEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Quest;
		}

		if (GUILayout.Button(townEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Town;
		}

		if (GUILayout.Button(badgeEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Badge;
		}

		if (GUILayout.Button(constructionEditor.EditorName, GUILayout.Width(100)))
		{
			MainWindowType = MainWindowTypeEnum.Construction;
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		switch(MainWindowType)
		{
			case MainWindowTypeEnum.NPC:	
				npcEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Armor:
				armorEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Conversation:
				conversationEditor.DisplayWindow(position);
				break;

			case MainWindowTypeEnum.Shop:
				shopEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Item:
				itemEditor.DisplayWindow(position);
				break;

			case MainWindowTypeEnum.Arena:
				arenaEditor.DisplayWindow(position);
				break;

			case MainWindowTypeEnum.Enemy:
				enemyEditor.DisplayWindow(position);
				break;

			case MainWindowTypeEnum.Currency:
				currencyEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.MiniGame:
				minigameEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Service:
				serviceEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.NPCQuest:
				npcQuestEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Quest:
				questEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Town:
				townEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Badge:
				badgeEditor.DisplayWindow(position);
				break;
			case MainWindowTypeEnum.Construction:
				constructionEditor.DisplayWindow(position);
				break;
		}
	}
}


public enum MainWindowTypeEnum
{
	None,
	Service,
	Armor,
	Attribute,
	Badge,
	Class,
	Container,
	Conversation,
	Currency,
	Enemy,
	EquipmentSlot,
	Guild,
	Item,
	ItemCategory,
	MiniGame,
	NPC,
	Race,
	Reputation,
	Quest,
    QuestCategory,
	Scene,
	Shop,
	Arena,
	Skill,
	SpawnPoint,
	Spell,
	SpellShop,
	Teleport,
	Town,
	Weapon,
	WorldObject,
	NPCQuest,
	Construction
}