using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterMoveset : MonoBehaviour
{
	[Title("Parameter - Actions")]
	[SerializeField]
	float horizontalDeadZone = 0.5f;
	[SerializeField]
	float verticalDeadZone = 0.5f;

	[Title("Parameter - Actions")]
	[SerializeField]
	AttackManager jab;
	[SerializeField]
	AttackManager downTilt;
	[SerializeField]
	AttackManager upTilt;
	[SerializeField]
	AttackManager forwardTilt;

	[Title("Parameter - Actions Aerial")]
	[SerializeField]
	AttackManager neutralAir;
	[SerializeField]
	AttackManager upAir;
	[SerializeField]
	AttackManager downAir;

	[Title("Parameter - Smash")]

	[Title("States")]
	[SerializeField]
	CharacterState stateAction;

	/// <summary>
	/// Return true if an action has been validated
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public bool ActionAttack(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true) // Attaque au sol
		{
			if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical < -verticalDeadZone)
			{
				if (character.Action.Action(downTilt) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical > verticalDeadZone)
			{
				if (character.Action.Action(upTilt) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Attack))
			{
				if (character.Action.Action(jab) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
		}
		else // Attaque dans les airs
		{
			if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical > verticalDeadZone)
			{
				if (character.Action.Action(upAir) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical < -verticalDeadZone)
			{
				if (character.Action.Action(downAir) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Attack))
			{
				if (character.Action.Action(neutralAir) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
		}

		return false;
	}

}
