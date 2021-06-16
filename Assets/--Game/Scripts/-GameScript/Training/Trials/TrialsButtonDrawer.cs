using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class TrialsButton
{
	[HorizontalGroup(Width = 80)]
	[HideLabel]
	[SerializeField]
	public Vector2 joystick;

	[HorizontalGroup(Width = 20)]
	[HideLabel]
	[SerializeField]
	public bool hasInput1 = false;

	[HorizontalGroup(Width = 50)]
	[HideLabel]
	[ShowIf("hasInput1")]
	[SerializeField]
	public EnumInput input1;

	[HorizontalGroup(Width = 20)]
	[HideLabel]
	[SerializeField]
	public bool hasInput2 = false;

	[HorizontalGroup(Width = 50)]
	[HideLabel]
	[ShowIf("hasInput2")]
	[SerializeField]
	public EnumInput input2;
}

public class TrialsButtonDrawer : MonoBehaviour
{
	[SerializeField]
	EnumInput input;

	[SerializeField]
	List<string> joystickButton = new List<string>();
	[SerializeField]
	List<string> buttons = new List<string>();

	bool add = false;

	public string AddButtonToText(TrialsButton buttonToDisplay, Input_Info info, string text)
	{
		add = false;
		text += "  ";
		if (Mathf.Abs(buttonToDisplay.joystick.x) > 0.2f)
		{
			text += joystickButton[0];
			add = true;
		}
		else if (buttonToDisplay.joystick.y > 0.2f)
		{
			text += joystickButton[1];
			add = true;
		}
		else if (buttonToDisplay.joystick.y < -0.2f)
		{
			text += joystickButton[2];
			add = true;
		}

		if(buttonToDisplay.hasInput1)
		{
			if (add)
				text += " + ";
			text += buttons[(int)info.CheckMapping(buttonToDisplay.input1)];
			add = true;
		}

		if (buttonToDisplay.hasInput2)
		{
			if (add)
				text += " + ";
			text += buttons[(int)info.CheckMapping(buttonToDisplay.input2)];
			add = true;
		}
		return text;
	}

}
