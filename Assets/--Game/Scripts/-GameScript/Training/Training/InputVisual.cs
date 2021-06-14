using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputVisual : MonoBehaviour
{
	[SerializeField]
	CharacterBase character;

	[Space]
	[SerializeField]
	RectTransform joystick;
	[SerializeField]
	Animator[] buttons;


	public void SetCharacter(CharacterBase c)
	{
		character = c;
	}

	// Update is called once per frame
	void Update()
	{
		if (character == null)
			return;

		joystick.anchoredPosition = new Vector2(character.Input.horizontal * 30f, character.Input.vertical * 30f);

		if(character.Input.CheckActionAbsolute(0, InputConst.Attack))
		{
			buttons[0].SetTrigger("Feedback");
		}
		else if (character.Input.CheckActionAbsolute(0, InputConst.Smash))
		{
			buttons[1].SetTrigger("Feedback");
		}
		else if (character.Input.CheckActionAbsolute(0, InputConst.Special))
		{
			buttons[2].SetTrigger("Feedback");
		}
		else if (character.Input.CheckActionAbsolute(0, InputConst.Jump))
		{
			buttons[3].SetTrigger("Feedback");
		}
		else if (character.Input.CheckActionAbsolute(0, InputConst.RightShoulder))
		{
			buttons[4].SetTrigger("Feedback");
		}
		else if (character.Input.CheckActionAbsolute(0, InputConst.RightTrigger))
		{
			buttons[5].SetTrigger("Feedback");
		}
	}

}
