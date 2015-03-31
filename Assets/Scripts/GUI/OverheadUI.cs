using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class OverheadUI : MonoBehaviour {

	public GameObject damageRoot;
	public GameObject hpMessageObject;
	public UIPlayTween tween;
	public List<GameObject> hpMessageObjects;
	public float timer = 2;
	public GameObject statusRewardObject;
	public UISprite statusRewardSprite;
	public UILabel statusRewardLabel;
	public BillboardConstraint billboard;
	public UISprite hpBar;
	public UILabel nameLabel;
	public GameObject hpBarObject;
	public GameObject speechBubble;
	public GameObject teamIcon;
	public UILabel speechBubbleLabel;
	public UIPlayTween speechBubbleTween;
	public Job hideSpeechBubbleJob = null;
	public CharacterStatus ownerCS;
	// Use this for initialization
	void Start () {
		billboard.enabled = true;
	}
	// Update is called once per frame
	/*void Update () {
		timer -= Time.deltaTime;
		if(timer < 0)
		{
			DisplayHpMessage(-42);
			timer = 2;
		}
	}*/
	public void DisplayStatusReward(RPGCurrency currency, float amount)
	{
		Debug.Log("displaying status rewards");
		statusRewardLabel.text = "+"+amount;
		statusRewardSprite.spriteName = currency.IconPath;
		tween.tweenTarget = statusRewardObject;
		tween.Play(true);
	}

	public void DisplayHpMessage(float amount)
	{
		for (int i = 0; i < hpMessageObjects.Count; i++) {
			if(hpMessageObjects[i].activeSelf == false)
			{
				WriteHpMessage(hpMessageObjects[i].GetComponent<UILabel>(), amount);
				return;
			}
		}
		GameObject newHpMessage = NGUITools.AddChild(damageRoot, hpMessageObject);
		hpMessageObjects.Add(newHpMessage);
		WriteHpMessage(newHpMessage.GetComponent<UILabel>(), amount);

	}

	public void Talk(string text)
	{
		if(hideSpeechBubbleJob != null && hideSpeechBubbleJob.running)
			hideSpeechBubbleJob.kill();
		speechBubbleTween.tweenTarget = speechBubble;
		speechBubbleTween.Play(true);
		speechBubbleLabel.text = text;
		hideSpeechBubbleJob = Job.make(HideSpeechBubble());

	}

	public IEnumerator HideSpeechBubble()
	{
		yield return new WaitForSeconds(3);
		speechBubbleTween.tweenTarget = speechBubble;
		speechBubbleTween.Play(false);
	}

	public void WriteHpMessage(UILabel label, float amount)
	{
		billboard.enabled = true;
		label.text = amount.ToString();
		if(amount < 0)
			label.color = Color.red;
		else
			label.color = Color.green;
		tween.tweenTarget = label.gameObject;
        tween.Play(true);
	}

	public void DisplayName(bool state)
	{
		billboard.enabled = true;
		if(nameLabel != null)
			nameLabel.gameObject.SetActive(state);
	}

	public void UpdateNameTag(string nameString)
	{
		if(nameLabel != null)
			nameLabel.text = nameString;
	}
	
	public void Update()
	{
		if(hpBar != null)
			hpBar.fillAmount = ownerCS.curHealth/ownerCS.maxHealth;
	}

	public void DisplayHpBar(bool state)
	{
		if(hpBarObject != null)
			hpBarObject.SetActive(state);
	}

	public void UpdateHpBar(float amount)
	{
		if(hpBar != null)
			hpBar.fillAmount = amount;
	}

	public void HideInfo()
	{
		DisplayName(false);
		DisplayHpBar(false);
	}
}
