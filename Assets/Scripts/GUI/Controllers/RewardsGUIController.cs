using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardsGUIController : BasicGUIController {
	
	public GameObject rewardsItemsRoot;
	public GameObject rewardsCurrenciesRoot;
	public float offset;
	public List<ItemTileButton> victoryItemTiles;
	public List<CurrencyTilebutton> currencyTiles;
	public LootItemList allLoots;
	public bool isArena;
	public bool isMinigame;
	public float score;
	public UILabel scoreLabel;
	public GameObject itemTilePrefab;
	public GameObject currencyTilePrefab;
	public UIPlayTween rewardPanelTween;
	public GameObject badgeRewardObject;
	public UISprite badgeSprite;

	public GameObject victoryRewardObject;
	public GameObject minigameScoreObject;

	public void DisplayArenaRewards(LootItemList loots)
	{
		allLoots = loots;
		Enable();
		DisplayRewards();
		isArena = true;
	}

	public void DisplayMinigameRewards(LootItemList loots, float newScore)
	{
		allLoots = loots;
		Enable();
		DisplayRewards();
		isMinigame = true;
		score = newScore;
	}

	public bool DisplayBadge()
	{
		if(allLoots.badges.Count <= 0)
			return false;
		GameObject atlas = Resources.Load(allLoots.badges[0].AtlasName) as GameObject;
		badgeSprite.atlas = atlas.GetComponent<UIAtlas>();
		badgeSprite.spriteName = allLoots.badges[0].IconPath;
		rewardPanelTween.tweenTarget = badgeRewardObject;
		rewardPanelTween.Play(true);
		return true;
	}

	public void HideBadge()
	{
		badgeRewardObject.SetActive(false);
	}

	public void DisplayRewards()
	{
		LoadItemTiles(allLoots.items, victoryItemTiles, rewardsItemsRoot, itemTilePrefab, InventoryGUIType.Reward);
		LoadCurrencyTiles(allLoots.currencies, currencyTiles, rewardsCurrenciesRoot, currencyTilePrefab);
		rewardPanelTween.tweenTarget = victoryRewardObject;
		rewardPanelTween.Play(true);
	}

	public void HideRewards()
	{
		victoryRewardObject.SetActive(false);
	}

	public void DisplayMinigameResults()
	{
		scoreLabel.text = score.ToString();
		rewardPanelTween.tweenTarget = minigameScoreObject;
		rewardPanelTween.Play(true);
	}

	public void HideMinigameResults()
	{
		minigameScoreObject.SetActive(false);
	}

	public void OnContinueBadgePressed()
	{
		if(isArena)
			EndArena();
	}

	public void OnContinueRewardsPressed()
	{
		if(DisplayBadge())
		{
			HideRewards();
		}
		else
		{
			if(isArena)
			{
				EndArena();
				return;
			}
			if(isMinigame)
			{
				DisplayMinigameResults();
				HideRewards();
			}
		}
	}

	public void OnContinueMinigamePressed()
	{
		EndMiniGame();
	}

	public void EndArena()
	{
		PlayerManager.Instance.LeaveArena();
	}

	public void EndMiniGame()
	{
		PlayerManager.Instance.EndMiniGame();
	}

	public void PopulateRewards()
	{

		/*int index = 0;
		Debug.Log(items.Count);
		for (int i = 0; i < itemTiles.Length; i++) 
		{
			if(i < items.Count)
			{
				itemTiles[i].gameObject.SetActive(true);
				//itemTiles[i].LoadItemTile(items[i]);
				itemTiles[i].transform.localPosition = new Vector3(index*offset, 0 ,0);
				index ++;
				Debug.Log(i);
			}
			else
				itemTiles[i].gameObject.SetActive(false);
			//rewardQuantityLabels
		}
		root.localPosition = new Vector3(((index -1)*offset*-0.5f), -50 ,0);*/
	}

	public void Continue()
	{
		/*GUIManager.Instance.HideRewards();
		for (int i = 0; i < items.Count; i++) {
			PlayerManager.Instance.Hero.AddItem(items[i]);
		}
		if(PlayerManager.Instance.activityState == PlayerActivityState.arena)
		{
			PlayerManager.Instance.LeaveArena(PlayerManager.Instance.ActiveWorld.DefaultZone);
		}

		else if(PlayerManager.Instance.activityState == PlayerActivityState.minigame)
		{
			PlayerManager.Instance.EndMiniGame();
		}*/
	}

	public override void Disable ()
	{
		isArena = false;
		score = 0;
		isMinigame = false;
		victoryRewardObject.SetActive(false);
		minigameScoreObject.SetActive(false);
		badgeRewardObject.SetActive(false);
		base.Disable ();
	}
}
