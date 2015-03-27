using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardsGUIController : BasicGUIController {
	
	public GameObject rewardsItemsRoot;
	public UIGrid rewardsItemsGrid;
	public GameObject rewardsCurrenciesRoot;
	public UIGrid rewardsCurrencyGrid;
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
	public UIPlayTween fadeOutTween;
	public GameObject badgeRewardObject;
	public UISprite badgeSprite;
	public UIPlayTween countdownTween;
	public GameObject three;
	public GameObject two;
	public GameObject one;
	public GameObject start;

	public GameObject victoryRewardObject;
	public GameObject minigameScoreObject;
	public GameObject failureObject;

	public void DisplayArenaFailure()
	{
		rewardPanelTween.tweenTarget = failureObject;
		rewardPanelTween.Play(true);
	}

	public void OnFailureContinuePressed()
	{
		EndArena();
	}

	public void DisplayArenaRewards(LootItemList loots)
	{
		allLoots = loots;
		Enable();
		DisplayRewards();
		isArena = true;
		//display rewards
		//display badge
		//end
	}

	public void StartCountdown()
	{
		StartCoroutine(StartCountdownSequence());
	}

	public IEnumerator StartCountdownSequence()
	{
		countdownTween.tweenTarget = three;
		countdownTween.Play(true);
		yield return new WaitForSeconds(1f);
		three.SetActive(false);
		countdownTween.tweenTarget = two;
		countdownTween.Play(true);
		yield return new WaitForSeconds(1f);
		two.SetActive(false);
		countdownTween.tweenTarget = one;
		countdownTween.Play(true);
		yield return new WaitForSeconds(1f);
		one.SetActive(false);
		countdownTween.tweenTarget = start;
		countdownTween.Play(true);
		yield return new WaitForSeconds(1f);
		start.SetActive(false);
	}

	public void DisplayMinigameRewards(LootItemList loots, int newScore)
	{
		allLoots = loots;
		score = newScore;
		Enable();
		DisplayMinigameResults();
		isMinigame = true;

		Debug.Log(score);
		//display score first
		//display badge
		//display reward items
		//end
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
		Debug.Log("fade out badege");
		fadeOutTween.tweenTarget = badgeRewardObject;
		fadeOutTween.Play(true);
		//badgeRewardObject.SetActive(false);
	}

	public void DisplayRewards()
	{
		LoadItemTiles(allLoots.items, victoryItemTiles, rewardsItemsRoot, itemTilePrefab, InventoryGUIType.Reward);
		rewardsItemsGrid.Reposition();
		LoadCurrencyTiles(allLoots.currencies, currencyTiles, rewardsCurrenciesRoot, currencyTilePrefab);
		rewardsCurrencyGrid.Reposition();
		rewardPanelTween.tweenTarget = victoryRewardObject;
		rewardPanelTween.Play(true);
	}

	public void HideRewards()
	{
		fadeOutTween.tweenTarget = victoryRewardObject;
		fadeOutTween.Play(true);
		Debug.Log("fade out rewards");
		//victoryRewardObject.SetActive(false);
	}

	public void DisplayMinigameResults()
	{
		Debug.Log(score);
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
		HideBadge();
		if(isArena)
			Invoke("EndArena", 0.15f);
		if(isMinigame)
			DisplayRewards();
	}

	public void OnContinueRewardsPressed()
	{
		HideRewards();
		if(isArena)
		{
			if(DisplayBadge())
				return;
			else
				Invoke("EndArena", 0.15f);
		}
		if(isMinigame)
		{
			Invoke("EndMiniGame", 0.15f);
		}
	}

	public void OnContinueMinigamePressed()
	{
		if(DisplayBadge())
		{
			HideMinigameResults();
		}
		else
		{
			DisplayRewards();
			HideMinigameResults();
		}
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
		failureObject.SetActive(false);
		base.Disable ();
	}
}
