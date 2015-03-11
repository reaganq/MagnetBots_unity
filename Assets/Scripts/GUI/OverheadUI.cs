using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OverheadUI : MonoBehaviour {

	public GameObject damageRoot;
	public GameObject hpMessageObject;
	public UIPlayTween tween;
	public List<GameObject> hpMessageObjects;
	public float timer = 2;
	public GameObject statusRewardObject;
	public UISprite statusRewardSprite;
	public UILabel statusRewardLabel;
	// Use this for initialization
	void Start () {
	
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

	public void WriteHpMessage(UILabel label, float amount)
	{
		label.text = amount.ToString();
		if(amount < 0)
			label.color = Color.red;
		else
			label.color = Color.green;
		tween.tweenTarget = label.gameObject;
        tween.Play(true);
	}
}
