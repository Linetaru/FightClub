using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CharacterUI : MonoBehaviour
{
	public GameObject panelUI;
	//public TextMeshProUGUI percentText;
	//public Image powerGaugeImage;
	[Title("Percent")]
	public Image percentBar;
	public Gradient gradient;

	[Title("Power")]
	public Image[] powerGaugeImages = new Image[4];

	[Title("Life")]
	public Image[] imagesLife = new Image[6];
	public TextMeshProUGUI stocksText;
	public Image redCross;

	public void InitPlayerPanel(CharacterBase user)
	{
		this.gameObject.SetActive(true);
		panelUI.SetActive(true);
		UpdatePercentUI(0);

		//percentText.text = user.Stats.LifePercentage.ToString() + " %";
		if (!imagesLife[0].gameObject.activeSelf)
			imagesLife[0].gameObject.SetActive(true);
		UpdateStocksUI(user);

		for (int i = 0; i < powerGaugeImages.Length; i++)
		{
			powerGaugeImages[i].fillAmount = 0;
		}
	}

	public void UpdatePercentUI(float percent)
    {
		//percentText.text = percent.ToString() + " %";
		//VertexGradient vertexGradient = percentText.colorGradient;
		//vertexGradient.topLeft = gradient.Evaluate(percent / 200);
		//vertexGradient.topRight = gradient.Evaluate(percent / 200);
		//vertexGradient.bottomLeft = gradient.Evaluate(percent / 200);
		//vertexGradient.bottomRight = gradient.Evaluate(percent / 200);
		//percentText.colorGradient = vertexGradient;
		//percentText.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() => percentText.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));


		float percentCalculate = percent / 250;
		//0.90f Bar start fill
		//percentCalculate = percentCalculate + 0.10f;
		//0.20f Bar finish fill
		if (percentBar.fillAmount <= 0.20f)
			percentBar.fillAmount = 0.20f;
		else
			percentBar.DOFillAmount((percentCalculate + 0.10f) + 1 - ((percentCalculate + 0.10f) * 2), 0.5f);

		percentBar.color = gradient.Evaluate(percent / 250);

	}


	public void UpdateStocksUI(CharacterBase cb)
    {
		if (cb.Stats.LifeStocks > 6)
		{
			if (!stocksText.gameObject.activeSelf)
				stocksText.gameObject.SetActive(true);

			for (int i = 1; i < imagesLife.Length; i++)
			{
				if(imagesLife[i].gameObject.activeSelf)
					imagesLife[i].gameObject.SetActive(false);
			}

			if (!imagesLife[0].gameObject.activeSelf)
				imagesLife[0].gameObject.SetActive(true);
			stocksText.text = "x" + cb.Stats.LifeStocks.ToString();
		}
		else
		{
			if (stocksText.gameObject.activeSelf)
				stocksText.gameObject.SetActive(false);

			switch(cb.Stats.LifeStocks)
            {
				case 0:
					if (imagesLife[0].gameObject.activeSelf)
						imagesLife[0].gameObject.SetActive(false);
					if (!redCross.gameObject.activeSelf)
						redCross.gameObject.SetActive(true);
					break;
				case 1:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (i <= 0)
						{
							if (!imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(true);
						}
						else
						{
							if (imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(false);
						}
					}
					break;
				case 2:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (i <= 1)
						{
							if (!imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(true);
						}
						else
						{
							if (imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(false);
						}
					}
					break;
				case 3:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (i <= 2)
						{
							if (!imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(true);
						}
						else
						{
							if (imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(false);
						}
					}
					break;
				case 4:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (i <= 3)
						{
							if (!imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(true);
						}
						else
						{
							if (imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(false);
						}
					}
					break;
				case 5:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (i <= 4)
						{
							if (!imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(true);
						}
						else
                        {
							if (imagesLife[i].gameObject.activeSelf)
								imagesLife[i].gameObject.SetActive(false);
						}
					}
					break;
				case 6:
					for (int i = 0; i < imagesLife.Length; i++)
					{
						if (!imagesLife[i].gameObject.activeSelf)
							imagesLife[i].gameObject.SetActive(true);
					}
					break;
			}

		}
	}

	public void UpdatePowerGaugeUI(int power)
    {
		if(power <= 25)
        {
			powerGaugeImages[0].fillAmount = power / 25;
			powerGaugeImages[1].fillAmount = 0;
			powerGaugeImages[2].fillAmount = 0;
			powerGaugeImages[3].fillAmount = 0;
		}
		else if (power <= 50)
		{
			powerGaugeImages[0].fillAmount = power / 25;
			powerGaugeImages[1].fillAmount = power / 50; 
			powerGaugeImages[2].fillAmount = 0;
			powerGaugeImages[3].fillAmount = 0;
		}
		else if (power <= 75)
		{
			powerGaugeImages[0].fillAmount = power / 25;
			powerGaugeImages[1].fillAmount = power / 50;
			powerGaugeImages[2].fillAmount = power / 75;
			powerGaugeImages[3].fillAmount = 0;
		}
		else
		{
			powerGaugeImages[0].fillAmount = power / 25;
			powerGaugeImages[1].fillAmount = power / 50;
			powerGaugeImages[2].fillAmount = power / 75;
			powerGaugeImages[3].fillAmount = power / 100;
		}
	}

	//Change Fill amount of Power Bar in UI
	public void UpdateUICharacter(CharacterBase user, float percent, int power)
	{
        if (user != null)
        {
            UpdateStocksUI(user);
        }

        if (percent != -1)
        {
			UpdatePercentUI(percent);
        }

		if (power != -1)
		{
			UpdatePowerGaugeUI(power);

			// If power is on max value set fill amount bar to 1
			//if (power == 99)
			//	powerGaugeImage.DOFillAmount(1, 0.2f);
			//else
			// Fill up or Reduce Power Bar to the value on time (gamefeel)
			//powerGaugeImage.DOFillAmount(((float)power) / 100, 0.3f);
		}
	}

}