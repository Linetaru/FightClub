using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionModePanel : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI textMain;


	[SerializeField]
	Animator animator;

	public void DrawItem(string text)
	{
		textMain.text = text;
	}

	public void ValidateButton()
	{
		animator.SetTrigger("Validate");
	}

	public void ResetButton()
	{
		animator.SetTrigger("Reset");
	}
}
