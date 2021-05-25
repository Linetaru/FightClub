using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;

public class CharacterHUD : MonoBehaviour
{
	[Space]
	[Title("Health")]
	[SerializeField]
	float maxPercent;
	[SerializeField]
	Gradient gradient;



	[Space]
	[Title("Power")]
	[SerializeField]
	int gaugeAmount = 20;




	[Space]
	[Title("UI")]
	[SerializeField]
	Image backgroundColor;

	[Space]
	[SerializeField]
	Image healthGaugeColor;
	[SerializeField]
	Image healthGauge;

	[Space]
	[SerializeField]
	Image[] powerGauge;
	[SerializeField]
	Sprite[] powerGaugeOn;
	[SerializeField]
	Sprite[] powerGaugeOff;


	[Space]
	[SerializeField]
	Image[] livesImage;
	[SerializeField]
	TextMeshProUGUI textLivesNumber;

	[Space]
	[Title("Feedbacks")]
	[SerializeField]
	Vector2 shakePower;
	[SerializeField]
	Vector2 shakeTime;
	[SerializeField]
	Feedbacks.ShakeRectEffect shake;


	[SerializeField]
	Vector2 shakeHUDPower;
	[SerializeField]
	Feedbacks.ShakeRectEffect shakeHUD;

	[Space]
	[SerializeField]
	Animator animatorHealthDanger;
	[SerializeField]
	Animator[] animatorGauge;
	[SerializeField]
	Animator animatorParry;

	[SerializeField]
	Animator animatorBreak;

	[SerializeField]
	Image redCross;

	[Title("Listener")]
	[SerializeField]
	PackageCreator.Event.GameEventListenerUICharacter listener;




	int previousGauge = 0;
	int previousGaugeID = -1;

	public void InitPlayerPanel(CharacterBase user)
	{
		previousGaugeID = -1;
		user.Stats.gameEvent.RegisterListener(listener);
		user.Knockback.Parry.OnParry += CallbackParry;
		this.gameObject.SetActive(true);

		DrawPercent(user.Stats.LifePercentage);
		DrawGauge(user.PowerGauge.CurrentPower);
		DrawLives(user.Stats.LifeStocks);
	}

	public void SetColor(Color c)
	{
		backgroundColor.color = c;
	}

	public void DrawPercent(float percent)
	{
		float coef = percent / maxPercent;
		healthGaugeColor.color = gradient.Evaluate(coef);
		healthGauge.DOFillAmount(1 - coef, 0.5f);
		//healthGauge.fillAmount = 1 - (coef);

		shake.Shake(shakePower.x + ((shakePower.y - shakePower.x ) * coef), shakeTime.x + ((shakeTime.y - shakeTime.x) * coef));
		if (shakeHUDPower.x + ((shakeHUDPower.y - shakeHUDPower.x) * coef) > 0)
			shakeHUD.Shake(shakeHUDPower.x + ((shakeHUDPower.y - shakeHUDPower.x) * coef), 999);
		else
			shakeHUD.Shake(0, 0);
		animatorHealthDanger.SetFloat("Health", coef);

		if (coef > 1)
			animatorBreak.gameObject.SetActive(true);
		else
			animatorBreak.gameObject.SetActive(false);
	}

	public void DrawGauge(int gauge)
	{
		int id = gauge / gaugeAmount;

		for (int i = 0; i < id; i++)
		{
			powerGauge[i].sprite = powerGaugeOn[i];
		}
		for (int i = id; i < powerGauge.Length; i++)
		{
			powerGauge[i].sprite = powerGaugeOff[i];
		}

		if (previousGaugeID < id)
		{
			/*if ((id - 2) > -1)
				animatorGauge[id - 2].SetTrigger("Default");*/
			if (previousGaugeID-1 > -1)
				animatorGauge[previousGaugeID-1].SetTrigger("Default");
			animatorGauge[id - 1].SetFloat("Gain", (float) id / powerGauge.Length);
			animatorGauge[id - 1].SetTrigger("Feedback");
		}
		else if (previousGaugeID > id)
		{
			/*if((id-1) != -1)
				animatorGauge[id-1].SetTrigger("Default2");*/
			if(previousGaugeID-1 > -1)
				animatorGauge[previousGaugeID-1].SetTrigger("Loose");
			if(id-1 > -1)
				animatorGauge[id-1].SetTrigger("Default2");
		}


		previousGaugeID = id;
		previousGauge = gauge;
	}

	public void DrawLives(int nbLives)
	{
		if(nbLives > livesImage.Length)
		{
			textLivesNumber.gameObject.SetActive(true);
			textLivesNumber.text = "x" + nbLives;
		}
		else
		{
			textLivesNumber.gameObject.SetActive(false);
			for (int i = 0; i < nbLives; i++)
			{
				livesImage[i].gameObject.SetActive(true);
			}
			for (int i = nbLives; i < livesImage.Length; i++)
			{
				livesImage[i].gameObject.SetActive(false);
				if (i == 0)
					redCross.gameObject.SetActive(true);
			}
		}
	}


	public void UpdateHUD(CharacterBase user, float percent, int power)
	{
		if(user != null)
			DrawLives(user.Stats.LifeStocks);

		if (percent > -1)
			DrawPercent(percent);

		if(power > -1)
			DrawGauge(power);
	}







	public void CallbackParry(CharacterBase c)
	{
		animatorParry.SetTrigger("Feedback");
	}
}
