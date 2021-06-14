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
	TextMeshProUGUI textName = null;
	[SerializeField]
	Image characterFace = null;
	[SerializeField]
	Image backgroundColor = null;

	[Space]
	[SerializeField]
	Image healthGaugeColor = null;
	[SerializeField]
	Image healthGauge = null;

	[Space]
	[SerializeField]
	Image[] powerGauge = null;
	[SerializeField]
	Sprite[] powerGaugeOn = null;
	[SerializeField]
	Sprite[] powerGaugeOff = null;


	[Space]
	[SerializeField]
	Image[] livesImage = null;
	[SerializeField]
	TextMeshProUGUI textLivesNumber = null;


	[Space]
	[Title("Dmg")]
	[SerializeField]
	TextMeshProUGUI textHit = null;
	[SerializeField]
	TextMeshProUGUI textDmg = null;

	int comboCount = 0;
	float initialPercent = 0f;
	float finalPercent = 0f;

	[Space]
	[Title("Feedbacks")]
	[SerializeField]
	Vector2 shakePower;
	[SerializeField]
	Vector2 shakeTime;
	[SerializeField]
	Feedbacks.ShakeRectEffect shake = null;
	/*[SerializeField]
	Feedbacks.ShakeRectEffect shakeCombo = null;*/

	[SerializeField]
	Vector2 shakeHUDPower;
	[SerializeField]
	Feedbacks.ShakeRectEffect shakeHUD = null;

	[Space]
	[SerializeField]
	Animator animatorHealthDanger = null;
	[SerializeField]
	Animator animatorCombo = null;
	[SerializeField]
	Animator[] animatorGauge = null;
	[SerializeField]
	Animator animatorParry = null;

	[SerializeField]
	Animator animatorBreak = null;
	[SerializeField]
	Animator animatorFade = null;

	[SerializeField]
	Image redCross = null;

	[Title("Listener")]
	[SerializeField]
	PackageCreator.Event.GameEventListenerUICharacter listener;


	CharacterBase character = null;

	int previousGauge = 0;
	int previousGaugeID = -1;

	public void InitPlayerPanel(CharacterBase user)
	{
		character = user;
		previousGaugeID = -1;
		user.Stats.gameEvent.RegisterListener(listener);
		user.Knockback.Parry.OnParry += CallbackParry;
		user.Knockback.OnKnockback += OnHitCallback;
		character.OnStateChanged += OnStateChangedCallback;
		user.PowerGauge.OnGaugeOn += ShowPowerGauge;
		this.gameObject.SetActive(true);

		//textName.text = user.Stats.data
		DrawPercent(user.Stats.LifePercentage);
		DrawGauge(user.PowerGauge.CurrentPower);
		DrawLives(user.Stats.LifeStocks);
	}

	private void OnDestroy()
	{
		character.Knockback.Parry.OnParry -= CallbackParry;
		character.Knockback.OnKnockback -= OnHitCallback;
		character.OnStateChanged -= OnStateChangedCallback;
		character.PowerGauge.OnGaugeOn -= ShowPowerGauge;
	}

	public void DrawName(string name)
	{
		textName.text = name;
	}

	public void DrawFace(Sprite face)
	{
		characterFace.sprite = face;
	}
	public void DrawLifeFace(Sprite face)
	{
		for (int i = 0; i < livesImage.Length; i++)
		{
			livesImage[i].sprite = face;
		}
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



	public void Fade(bool b)
	{
		animatorFade.SetBool("Fade", b);
		if(b)
		{
			Color fade = new Color(1, 1, 1, 0.02f);
			for (int i = 0; i < livesImage.Length; i++)
			{
				livesImage[i].color = fade;
			}
			for (int i = previousGaugeID; i < powerGauge.Length; i++)
			{
				powerGauge[i].color = fade;
			}
		}
		else
		{
			Color unfade = new Color(1, 1, 1, 1f);
			for (int i = 0; i < livesImage.Length; i++)
			{
				livesImage[i].color = unfade;
			}
			for (int i = previousGaugeID; i < powerGauge.Length; i++)
			{
				powerGauge[i].color = unfade;
			}
		}
	}



	public void CallbackParry(CharacterBase c)
	{
		animatorParry.SetTrigger("Feedback");
	}




	private void OnStateChangedCallback(CharacterState oldState, CharacterState newState) 
	{
		if(!(newState is CharacterStateKnockback))
		{
			if(comboCount >= 2)
				animatorCombo.SetTrigger("Disappear");
			comboCount = 0;
			initialPercent = character.Stats.LifePercentage;

		}
	}

	private void OnHitCallback(AttackSubManager attack)
	{
		comboCount += 1;
		finalPercent = character.Stats.LifePercentage;
		if(comboCount >= 2)
			DrawCombo();
	}


	private void DrawCombo()
	{
		textDmg.text = (finalPercent - initialPercent).ToString();
		textHit.text = comboCount.ToString();
		//shakeCombo.gameObject.SetActive(true);
		//shakeCombo.Shake();

		animatorCombo.SetTrigger("Feedback");
	}

	public void ShowPowerGauge(bool on)
	{
		for (int i = 0; i < powerGauge.Length; i++)
		{
			powerGauge[i].gameObject.SetActive(on);
		}
	}
}
