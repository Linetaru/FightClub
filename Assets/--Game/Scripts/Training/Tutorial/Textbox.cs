﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Textbox : MonoBehaviour, IControllable
{
	[SerializeField]
	TextMeshProUGUI textBox;

	[SerializeField]
	[HideLabel]
	[TextArea(2,3)]
	string text = "";

	[Title("Time")]
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeInterval = 1;

	[SerializeField]
	[SuffixLabel("en frames")]
	float timePause = 12;

	[Title("Feedbacks")]
	[SerializeField]
	Animator textboxAnimator;

	bool show = true;
	float t = 0f;

	public event UnityAction OnTextEnd;


	private void Start()
	{
		timeInterval /= 60f;
		timePause /= 60f;

		textBox.text = text;
		textBox.maxVisibleCharacters = 0;

	}


	public void DrawTextbox(string text)
	{
		this.text = text;
		textBox.text = text;
		textBox.maxVisibleCharacters = 0;
		show = true;
		DrawAnimator();
	}
	//yield return null;

	public void UpdateControl(int id, Input_Info input)
	{
		if (show == false)
			return;

		if(textBox.maxVisibleCharacters >= text.Length)
		{
			if (input.CheckAction(0, InputConst.Attack))
			{
				input.inputActions[0].timeValue = 0;
				EndTextbox();
			}
			return;
		}

		if (input.CheckAction(0, InputConst.Attack))
		{
			input.inputActions[0].timeValue = 0;
			textBox.maxVisibleCharacters = text.Length;
		}

		t += Time.deltaTime;
		if (t > timeInterval)
		{
			textBox.maxVisibleCharacters += 1;
			t = 0;
			if (textBox.maxVisibleCharacters > 0 && textBox.maxVisibleCharacters < text.Length)
			{
				if (text[textBox.maxVisibleCharacters - 1] == ',' && text[textBox.maxVisibleCharacters] == ' ' ||
					text[textBox.maxVisibleCharacters - 1] == '.' && text[textBox.maxVisibleCharacters] == ' ' ||
					text[textBox.maxVisibleCharacters - 1] == '?' && text[textBox.maxVisibleCharacters] == ' ' ||
					text[textBox.maxVisibleCharacters - 1] == '!' && text[textBox.maxVisibleCharacters] == ' ')
					t -= timePause;
			}
		}
	}



	private void EndTextbox()
	{
		show = false;
		DrawAnimator();
		OnTextEnd.Invoke();
	}
	private void DrawAnimator()
	{
		textboxAnimator.gameObject.SetActive(true);
		textboxAnimator.SetBool("Appear", show);
	}
}