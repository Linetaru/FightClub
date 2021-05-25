using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class MenuCurrency : MonoBehaviour, IListener<int>
{

	[SerializeField]
	CurrencyData currency;

	[SerializeField]
	bool appearAtStart = true;
	[SerializeField]
	bool disappear = false;

	[Title("UI")]
	[SerializeField]
	TextMeshProUGUI textMoney;
	[SerializeField]
	TextMeshProUGUI textMoneyAdd;

	[Title("Feedbacks")]
	[SerializeField]
	Animator animatorPanel;


	private void Start()
	{
		currency.RegisterListener(this);
		if (appearAtStart)
		{
			animatorPanel.gameObject.SetActive(true);
			animatorPanel.SetBool("Appear", true);
		}

		DrawMoney();
	}


	public void OnEventRaised(int i)
	{
		DrawMoney();
	}




	public void DrawMoney()
	{
		if (currency.MoneyToUpdate != 0)
		{
			StartCoroutine(MoneyGainCoroutine());

			textMoneyAdd.text = "+" + currency.MoneyToUpdate;
			textMoney.text = (currency.Money - currency.MoneyToUpdate).ToString();

			currency.ResetMoneyToUpdate();
		}
		else
		{
			textMoney.text = currency.Money.ToString();
		}
	}


	private IEnumerator MoneyGainCoroutine()
	{
		animatorPanel.gameObject.SetActive(true);
		animatorPanel.SetBool("Appear", true);
		animatorPanel.SetTrigger("Feedback");
		yield return new WaitForSeconds(2.1f);
		textMoney.text = currency.Money.ToString();
		yield return new WaitForSeconds(3f);
		if (disappear)
			animatorPanel.SetBool("Appear", false);
	}



	private void OnDestroy()
	{
		currency.UnregisterListener(this);
	}

}
