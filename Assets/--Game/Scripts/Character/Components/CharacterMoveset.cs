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
	AttackManager neutralSpecial;

	[Title("Parameter - Signature Move")]
	[SerializeField]
	AttackManager signatureMove;


	[Title("Acumods - (à bouger)")]
	[SerializeField]
	StatusData statusData;
	public StatusData StatusData
	{
		get { return statusData; }
		set { statusData = value; }
	}


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

		if (character.Input.CheckAction(0, InputConst.LeftShoulder) && character.PowerGauge.CurrentPower >= 20)
		{
			if(character.Status.AddStatus(new Status("Acumod", statusData)))
			{
				character.PowerGauge.CurrentPower -= 20;
				character.Input.inputActions[0].timeValue = 0;
			}
		}

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

			if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical < -verticalDeadZone)
				return ActionAttack(character, downTilt);


			else if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical > verticalDeadZone)
				return ActionAttack(character, upTilt);


			else if (character.Input.CheckAction(0, InputConst.Attack) 
				&& (character.Movement.SpeedX < -(fractionOfSpeedMaxToDash * character.Movement.SpeedMax) || character.Movement.SpeedX > (fractionOfSpeedMaxToDash * character.Movement.SpeedMax)))
			{
				return ActionAttack(character, dashAttack);
            }
			else if (character.Input.CheckAction(0, InputConst.Attack))
			{
				if (character.Action.CanAct()) // A patcher pour prendre en compte les attack conditions
				{
					if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
						character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
					return ActionAttack(character, jab);
				}
			}


			return ActionSpecial(character);
		}
		else // Attaque dans les airs
		{
			if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical > verticalDeadZone)
				return ActionAttack(character, upAir);


			else if (character.Input.CheckAction(0, InputConst.Attack) && character.Input.vertical < -verticalDeadZone)
				return ActionAttack(character, downAir);


			else if (character.Input.CheckAction(0, InputConst.Attack) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
			{
				if (character.Action.CanAct()) // C'est redondant mais bon 
				{
					if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal))
						character.Movement.SpeedX *= -1;
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
					return ActionAttack(character, forwardAir);
				}
			}


			else if (character.Input.CheckAction(0, InputConst.Attack))
				return ActionAttack(character, neutralAir);

			return ActionSpecial(character);
		}

		return false;
	}


	public bool ActionAttack(CharacterBase character, AttackManager attack)
	{
		if (character.Action.Action(attack) == true)
		{
			character.SetState(stateAction);
			if(character.Input.inputActions.Count != 0)
				character.Input.inputActions[0].timeValue = 0;
			return true;
		}
		return false;
	}




	public bool ActionSpecial(CharacterBase character)
	{
		if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
			return ActionAttack(character, upSpecial);


		else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			return ActionAttack(character, downSpecial);

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
			return ActionAttack(character, neutralSpecial);

		/*else if (character.Input.CheckAction(0, InputConst.Special))
		{
			if (character.Action.Action(simpleSpecial) == true)
			{
				character.SetState(stateAction);
				character.Input.inputActions[0].timeValue = 0;
				return true;
			}
		}*/

		return false;
	}

}
