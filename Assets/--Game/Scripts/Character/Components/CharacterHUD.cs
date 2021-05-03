﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

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

	[SerializeField]
	Image[] powerGauge;
	[SerializeField]
	Sprite[] powerGaugeOn;
	[SerializeField]
	Sprite[] powerGaugeOff;

	[Space]
	[Title("Feedbacks")]
	[SerializeField]
	Vector2 shakePower;
	[SerializeField]
	Vector2 shakeTime;
	[SerializeField]
	Feedbacks.ShakeRectEffect shake;

	[Space]
	[SerializeField]
	Animator animatorHealthDanger;
	[SerializeField]
	Animator[] animatorGauge;
	[SerializeField]
	Animator animatorParry;

	[SerializeField]
	Animator animatorBreak;



	[Title("Listener")]
	[SerializeField]
	PackageCreator.Event.GameEventListenerUICharacter listener;




	int previousGauge = 0;
	int previousGaugeID = 0;

	public void InitPlayerPanel(CharacterBase user)
	{
		user.Stats.gameEvent.RegisterListener(listener);
		user.Knockback.Parry.OnParry += CallbackParry;
		this.gameObject.SetActive(true);

		/*panelUI.SetActive(true);
		UpdatePercentUI(0);

		//percentText.text = user.Stats.LifePercentage.ToString() + " %";
		if (!imagesLife[0].gameObject.activeSelf)
			imagesLife[0].gameObject.SetActive(true);
		UpdateStocksUI(user);

		for (int i = 0; i < powerGaugeImages.Length; i++)
		{
			powerGaugeImages[i].fillAmount = 0;
		}*/
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
			animatorGauge[id-1].SetTrigger("Feedback");


		previousGaugeID = id;
		previousGauge = gauge;
	}


	public void UpdateHUD(CharacterBase user, float percent, int power)
	{
		if(percent > 0)
			DrawPercent(percent);

		if(power > 0)
			DrawGauge(power);
	}

	public void CallbackParry(CharacterBase c)
	{
		animatorParry.SetTrigger("Feedback");
	}
}
