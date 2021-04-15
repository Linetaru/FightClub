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
	[Range(0f, 1f)]
	[SerializeField]
	float fractionOfSpeedMaxToDash = 0.95f;

	[Title("Parameter - Actions")]
	[SerializeField]
	AttackManager jab;
	[SerializeField]
	AttackManager downTilt;
	[SerializeField]
	AttackManager upTilt;
	[SerializeField]
	AttackManager forwardTilt;
	[SerializeField]
	AttackManager dashAttack;

	[Title("Parameter - Actions Aerial")]
	[SerializeField]
	AttackManager neutralAir;
	[SerializeField]
	AttackManager forwardAir;
	[SerializeField]
	AttackManager upAir;
	[SerializeField]
	AttackManager downAir;

	[Title("Parameter - Specials")]
	[SerializeField]
	AttackManager upSpecial;
	[SerializeField]
	AttackManager downSpecial;
	[SerializeField]
	AttackManager forwardSpecial;
	[SerializeField]
	AttackManager simpleSpecial;

	[Title("Parameter - Signature Move")]
	[SerializeField]
	AttackManager signatureMove;

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
			if (character.Input.CheckAction(0, InputConst.LeftTrigger) && character.PowerGauge.CurrentPower >= 99)
			{
				if (character.Action.Action(signatureMove) == true)
				{
					character.PowerGauge.CurrentPower = 0;
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical < -verticalDeadZone)
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
			else if (character.Input.CheckAction(0, InputConst.Attack) 
				&& (character.Movement.SpeedX < -(fractionOfSpeedMaxToDash * character.Movement.SpeedMax) || character.Movement.SpeedX > (fractionOfSpeedMaxToDash * character.Movement.SpeedMax)))
			{
				if (character.Action.Action(dashAttack) == true)
				{
					Debug.Log("Dash attack");
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
			else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
			{
				if (character.Action.Action(upSpecial) == true)
                {
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			{
				if (character.Action.Action(downSpecial) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special) && (character.Input.horizontal < -horizontalDeadZone || character.Input.horizontal > horizontalDeadZone))
			{
				if (character.Action.Action(forwardSpecial) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special))
			{
				if (character.Action.Action(simpleSpecial) == true)
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
			else if (character.Input.CheckAction(0, InputConst.Attack) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
			{
				if (character.Action.Action(forwardAir) == true)
				{
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
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
			else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
			{
				if (character.Action.Action(upSpecial) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			{
				if (character.Action.Action(downSpecial) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special) && (character.Input.horizontal < -horizontalDeadZone || character.Input.horizontal > horizontalDeadZone))
			{
				if (character.Action.Action(forwardSpecial) == true)
				{
					character.SetState(stateAction);
					character.Input.inputActions[0].timeValue = 0;
					return true;
				}
			}
			else if (character.Input.CheckAction(0, InputConst.Special))
			{
				if (character.Action.Action(simpleSpecial) == true)
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
