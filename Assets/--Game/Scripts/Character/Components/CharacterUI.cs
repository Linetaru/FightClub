using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterUI : MonoBehaviour
{
	public GameObject panelUI;
	public TextMeshProUGUI percentText;
	public TextMeshProUGUI stocksText;
	public Image powerGaugeImage;

    public void InitPlayerPanel(CharacterBase user)
	{
		this.gameObject.SetActive(true);
		panelUI.SetActive(true);
		percentText.text = user.Stats.LifePercentage.ToString() + " %";
		stocksText.text = "Stocks : " + user.Stats.LifeStocks.ToString();
	}

	public void UpdateUI(float percent)
    {
		percentText.text = percent.ToString() + " %";
	}

	public void UpdateStocksUI(float lifeStocks)
    {
		stocksText.text = "Stocks : " + lifeStocks.ToString();
    }

	public void UpdateStocksUI(CharacterBase cb)
    {
		UpdateStocksUI(cb.Stats.LifeStocks);
	}

	//Change Fill amount of Power Bar in UI
	public void UpdateUICharacter(CharacterBase user, float percent, int power)
	{
        if (user != null)
        {
            UpdateStocksUI(user.Stats.LifeStocks);
        }

        if (percent != -1)
        {
            UpdateUI(percent);
        }

		if (power != -1)
		{

			// If power is on max value set fill amount bar to 1
			if (power == 99)
				powerGaugeImage.DOFillAmount(1, 0.2f);
			else
				// Fill up or Reduce Power Bar to the value on time (gamefeel)
				powerGaugeImage.DOFillAmount(((float)power) / 100, 0.3f);

		}
	}

}