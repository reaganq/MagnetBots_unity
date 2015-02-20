using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BankGUIController : BasicGUIController {

	public int _activeAmount;
	public int activeAmount
	{
		get{
			return _activeAmount;
		}
		set
		{
			_activeAmount = value;
			amountLabel.text = _activeAmount.ToString();
		}
	}
	public UILabel amountLabel;
	public UILabel bankCoinsLabel;
	public BankMode bankMode = BankMode.none;
	public int interestAmount;
	public GameObject quantityBox;

	public override void Enable ()
	{
		interestAmount = (int)(GeneralData.interestRate * PlayerManager.Instance.Hero.BankCoins);
		bankMode = BankMode.none;
		quantityBox.SetActive(false);
		bankCoinsLabel.text = PlayerManager.Instance.Hero.BankCoins.ToString();
		base.Enable ();
	}

	public void OnDepositPressed()
	{
		quantityBox.SetActive(true);
	}

	public void Deposit(int amount)
	{
		PlayerManager.Instance.Hero.Deposit(amount);
	}

	public void OnWithdrawPressed()
	{
		quantityBox.SetActive(true);
	}

	public void Withdraw(int amount)
	{
		PlayerManager.Instance.Hero.Withdraw(amount);
	}

	public void OnCollectInterestPressed()
	{
		CollectInterest();

	}

	public void CollectInterest()
	{
		PlayerManager.Instance.Hero.CollectInterest(interestAmount);
		bankCoinsLabel.text = PlayerManager.Instance.Hero.BankCoins.ToString();
	}

	public void OnUpPressed()
	{
		activeAmount ++;
		if(activeAmount > PlayerManager.Instance.Hero.Coins && bankMode == BankMode.deposit)
			activeAmount = PlayerManager.Instance.Hero.Coins;
		if(activeAmount > PlayerManager.Instance.Hero.BankCoins && bankMode == BankMode.withdraw)
			activeAmount = PlayerManager.Instance.Hero.BankCoins;
	}

	public void OnDownpressed()
	{
		activeAmount --;
		if(_activeAmount < 0)
			activeAmount = 0;
	}

	public override void Disable()
	{
		bankMode = BankMode.none;
		base.Disable();
	}
}

public enum BankMode
{
	none,
	withdraw,
	deposit
}
