using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombobulatorGUIController : BasicGUIController {

	public GameObject lootWindow;
	public GameObject uiWindow;
	public GameObject useButton;
	public Anvil anvil;
	public int useCounter = 0;
	public UIPlayTween lootWindowTween;
	public InventoryItem rewardItem = new InventoryItem();
	public int cost = 50;
	public ItemInfoBox rewardItemInfo;
	public int _time;
	public GameObject timerObject;
	public int rewardTime
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
			if(_time < 0)
				_time = 0;
			timeLabel.text = _time / 3600 + ":" + (_time % 3600) / 60 + ":" + (_time % 3600 ) % 60;
		}
	}
	public UILabel timeLabel;
	public float _timer = 0;
	
	public void Update()
	{
		_timer += Time.deltaTime;
        if(_timer > 1.0f)
        {
            rewardTime --;
            _timer = 0;
        }
    }

	public void Start()
	{
		rewardTime = 0;
	}
    
    public override void Enable ()
	{
		lootWindow.SetActive(false);
		uiWindow.SetActive(true);
		UpdateInfo();
		anvil = GameObject.FindGameObjectWithTag("Anvil").GetComponent<Anvil>();
		base.Enable ();
	}

	public void GenerateRandomReward()
	{
		/*int id = Random.Range(6, 15);
		LootItem loot = new LootItem();
		loot.itemID.Add(id);
		loot.itemType = ItemType.Armor;
		loot.minQuantity = 1;
		loot.maxQuantity = 1;
		loot.itemLevel = 1;
		loot.dropRate = 1;
		return loot;*/
		InventoryItem item = new InventoryItem();
		item.GenerateNewInventoryItem( Storage.LoadById<RPGArmor>(Random.Range(5,16), new RPGArmor()), 1, 1);
		rewardItem = item;
		rewardItemInfo.LoadItemInfo(rewardItem);
	}

	public void UpdateInfo()
	{
		if(rewardTime <= 0)
		{
			useButton.SetActive(true);
			timerObject.SetActive(false);
		}
		else
		{
			useButton.SetActive(false);
			timerObject.SetActive(true);
		}
	}

	public void OnUseButtonPressed()
	{
		if(PlayerManager.Instance.Hero.CitizenPoints > cost)
		{
			PlayerManager.Instance.Hero.RemoveCurrency(cost, BuyCurrencyType.CitizenPoints);
			StartCoroutine(RewardSequence());
			rewardTime = 24*3600;
			UpdateInfo();
		}
	}

	public void OnCollectButtonpressed()
	{
		PlayerManager.Instance.Hero.AddItem(rewardItem);
		lootWindow.SetActive(false);
		GUIManager.Instance.HideNPC();
		Disable();
	}

	public IEnumerator RewardSequence()
	{
		useCounter ++;
		uiWindow.SetActive(false);
		UpdateInfo();
		anvil.Sucess();
		GenerateRandomReward();
		yield return new WaitForSeconds(1f);
		lootWindowTween.Play(true);
	}

	public void OnExitButtonPressed()
	{
		GUIManager.Instance.NPCGUI.HideShop();
		Disable();
	}

}
