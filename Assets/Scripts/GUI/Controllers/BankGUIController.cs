using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BankGUIController : BasicGUIController {

	public int _activeDepositAmount;
	public int activeDepositAmount
	{
		get{
			return _activeDepositAmount;
		}
		set
		{
			_activeDepositAmount = value;
			depositAmountLabel.text = _activeDepositAmount.ToString();
		}
	}
	public UILabel depositAmountLabel;

	public int _activeWithdrawAmount;
	public int activeWithdrawAmount
	{
		get{
			return _activeWithdrawAmount;
		}
		set
		{
			_activeWithdrawAmount = value;
	
			withdrawAmountLabel.text = _activeWithdrawAmount.ToString();
		}
	}
	public UILabel withdrawAmountLabel;
	public UILabel totalAmountLabel;
	public UILabel interestInfoLabel;
	public GameObject collectButton;
	public GameObject timer;
	public int interestAmount;
	public int _time;
	public int interestTime
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
			interestTime --;
			_timer = 0;
		}
	}

	public void Start()
	{
		interestTime = 3600*24;
	}

	public override void Enable ()
	{
		UpdateInfo();
		base.Enable ();
	}

	public void OnDepositPressed()
	{
		PlayerManager.Instance.Hero.Deposit(activeDepositAmount);
		UpdateInfo();
	}

	public void OnWithdrawPressed()
	{
		PlayerManager.Instance.Hero.Withdraw(activeWithdrawAmount);
		UpdateInfo();
	}
	
	public void OnCollectInterestPressed()
	{
		CollectInterest();
		UpdateInfo();
		interestTime = 24*3600;
	}

	public void UpdateInfo()
	{
		interestAmount = (int)(GeneralData.interestRate * PlayerManager.Instance.Hero.BankCoins);
		totalAmountLabel.text = "You have a total of " + PlayerManager.Instance.Hero.BankCoins.ToString() + " coins in your in your bank account.";
		interestInfoLabel.text = "You will receive " + interestAmount.ToString() + " coins as interest everyday.";
		activeDepositAmount = 0;
		activeWithdrawAmount = 0;
		if(interestTime <= 0)
		{
			timer.SetActive(false);
			collectButton.SetActive(true);
		}
		else
		{
			timer.SetActive(true);
			collectButton.SetActive(false);
		}
	}

	public void CollectInterest()
	{
		if(interestAmount > 0)
			PlayerManager.Instance.Hero.CollectInterest(interestAmount);
	}

	public void IncreaseWithdraw()
	{
		if(activeWithdrawAmount < PlayerManager.Instance.Hero.BankCoins)
			activeWithdrawAmount ++;
	}

	public void DecreaseWithdraw()
	{
		if(activeWithdrawAmount > 0)
			activeWithdrawAmount --;
	}

	public void IncreaseDeposit()
	{
		if(activeDepositAmount < PlayerManager.Instance.Hero.Coins)
			activeDepositAmount ++;
	}
	
	public void DecreaseDeposit()
	{
		if(activeDepositAmount > 0)
			activeDepositAmount --;
	}

	public override void Disable()
	{
		base.Disable();
	}

	public void OnExitButtonPressed()
	{
		GUIManager.Instance.NPCGUI.HideShop();
	}
}
