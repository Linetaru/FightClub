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

	[SerializeField]
	Color colorTiltEX;
	[SerializeField]
	Color colorSpecialEX;
	[SerializeField]
	int tiltExCost = 20;
	[SerializeField]
	int specialExCost = 40;

	[Title("Parameter - Actions")]
	[SerializeField]
	AttackManager jab = null;
	[SerializeField]
	AttackManager downTilt = null;
	[SerializeField]
	AttackManager upTilt = null;
	[SerializeField]
	AttackManager forwardTilt = null;
	[SerializeField]
	AttackManager dashAttack = null;

	[Title("Parameter - Actions Aerial")]
	[SerializeField]
	AttackManager neutralAir = null;
	[SerializeField]
	AttackManager forwardAir = null;
	[SerializeField]
	AttackManager upAir = null;
	[SerializeField]
	AttackManager downAir = null;

	[Title("Parameter - Specials")]
	[SerializeField]
	AttackManager upSpecial = null;
	[SerializeField]
	AttackManager downSpecial = null;
	[SerializeField]
	AttackManager forwardSpecial = null;
	[SerializeField]
	AttackManager neutralSpecial = null;


	[Title("Parameter - Specials Ex")]
	[SerializeField]
	AttackManager upSpecialEx = null;
	[SerializeField]
	AttackManager downSpecialEx = null;
	[SerializeField]
	AttackManager forwardSpecialEx = null;
	[SerializeField]
	AttackManager neutralSpecialEx = null;

	[Title("Parameter - Signature Move")]
	[SerializeField]
	AttackManager signatureMove = null;



	[Title("States")]
	[SerializeField]
	CharacterState stateAction;

	[Title("Feedbacks")]
	[SerializeField]
	ParticleSystem particleExTilt = null;
	[SerializeField]
	ParticleSystem particleExSpecial = null;


	private bool exTilt;
	public bool ExTilt
	{
		get { return exTilt; }
		set { exTilt = value; }
	}

	CharacterBase c;


	/// <summary>
	/// Return true if an action has been validated
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public bool ActionAttack(CharacterBase character, bool canSpecial = true)
	{
		if (character.Rigidbody.IsGrounded == true) // Attaque au sol
		{
			if (SignatureMoveManager.Instance != null)
			{
				if (character.Input.CheckAction(0, InputConst.LeftTrigger) && character.PowerGauge.CurrentPower >= character.PowerGauge.maxPower)
				{
					if (signatureMove == null)
						return false;
					if (character.Action.Action(signatureMove) == true)
					{
						//character.PowerGauge.GaugeOn = false;
						character.PowerGauge.CurrentPower = 0;
						character.SetState(stateAction);
						character.Input.inputActions[0].timeValue = 0;
						return true;
					}
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

			if (canSpecial)
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

			if(canSpecial)
				return ActionSpecial(character);
		}

		return false;
	}





	public bool ActionSpecial(CharacterBase character)
	{
		if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			return ActionAttack(character, upSpecial);
		}



		else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			return ActionAttack(character, downSpecial);

		else if (character.Input.CheckAction(0, InputConst.Special) && (character.Input.horizontal < -horizontalDeadZone || character.Input.horizontal > horizontalDeadZone))
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			if (character.Action.Action(forwardSpecial) == true)
			{
				character.SetState(stateAction);
				character.Input.inputActions[0].timeValue = 0;
				return true;
			}
		}
		else if (character.Input.CheckAction(0, InputConst.Special))
			return ActionAttack(character, neutralSpecial);


		return false;
	}




	public bool ActionEx(CharacterBase character)
	{
		// Si on ne laisse pas R1 enfoncé, pas d'actions EX
		if (!character.Input.CheckActionHold(InputConst.RightShoulder))
			return false;

		// Si on a pas la barre pour un tilt EX
		if (character.PowerGauge.CurrentPower < tiltExCost)
			return false;

		character.Rigidbody.CheckGround(character.Movement.Gravity);
		if (ActionAttack(character, false))
		{
			ParticleSystem par = Instantiate(particleExTilt, character.CenterPoint.transform.position, Quaternion.identity, character.CenterPoint.transform);
			Destroy(par.gameObject, 1f);

			character.PowerGauge.ForceAddPower(-tiltExCost);
			character.Model.FlashModel(colorTiltEX, 2f);
			character.Knockback.Parry.IsParry = true;
			character.Knockback.Parry.IsGuard = false;

			exTilt = true;

			if (c == null)
			{
				c = character;
				character.Action.OnAttackActive += CallbackAttackActive;
			}
			c.Knockback.Parry.forceAnimationParry = true;
			return true;
		}
		return false;
	}


	private void CallbackAttackActive()
	{
		exTilt = false;
		c.Knockback.Parry.forceAnimationParry = false;
		c.Knockback.Parry.IsParry = false;
	}

	public bool ActionExSpecial(CharacterBase character)
	{
		// Si on ne laisse pas R1 enfoncé, pas d'actions EX
		if (!character.Input.CheckActionHold(InputConst.RightShoulder))
			return false;

		// Si on a pas la barre pour un special EX
		if (character.PowerGauge.CurrentPower < specialExCost)
			return false;

		if (ActionSpecialEx(character))
		{
			character.PowerGauge.ForceAddPower(-specialExCost);
			character.Model.FlashModel(colorSpecialEX, 2f);

			ParticleSystem par = Instantiate(particleExSpecial, character.CenterPoint.transform.position, Quaternion.identity, character.CenterPoint.transform);
			Destroy(par.gameObject, 1f);
			return true;
		}

		return false;
	}

	/*public bool ActionSpecialEx(CharacterBase character)
	{
		// Si on ne laisse pas R1 enfoncé, pas d'actions EX
		if (!character.Input.CheckActionHold(InputConst.RightShoulder))
			return false;

		// Si on a pas la barre
		if (character.PowerGauge.CurrentPower < specialExCost)
			return false;

		character.PowerGauge.ForceAddPower(-specialExCost);



		if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			return ActionAttack(character, upSpecialEx);
		}


		else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			return ActionAttack(character, downSpecialEx);


		else if (character.Input.CheckAction(0, InputConst.Special) && (character.Input.horizontal < -horizontalDeadZone || character.Input.horizontal > horizontalDeadZone))
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			return ActionAttack(character, forwardSpecialEx);
		}

		else if (character.Input.CheckAction(0, InputConst.Special))
			return ActionAttack(character, neutralSpecialEx);


		return false;
	}*/

	private bool ActionSpecialEx(CharacterBase character)
	{

		if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical > verticalDeadZone)
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			return ActionAttack(character, upSpecialEx);
		}


		else if (character.Input.CheckAction(0, InputConst.Special) && character.Input.vertical < -verticalDeadZone)
			return ActionAttack(character, downSpecialEx);


		else if (character.Input.CheckAction(0, InputConst.Special) && (character.Input.horizontal < -horizontalDeadZone || character.Input.horizontal > horizontalDeadZone))
		{
			if (character.Action.CanAct())
			{
				if (character.Movement.Direction != (int)Mathf.Sign(character.Input.horizontal) && Mathf.Abs(character.Input.horizontal) > horizontalDeadZone)
					character.Movement.Direction = (int)Mathf.Sign(character.Input.horizontal);
			}
			return ActionAttack(character, forwardSpecialEx);
		}

		else if (character.Input.CheckAction(0, InputConst.Special))
			return ActionAttack(character, neutralSpecialEx);


		return false;
	}






	public bool ActionAttack(CharacterBase character, AttackManager attack)
	{
		if (attack == null)
			return false;
		if (character.Action.Action(attack) == true)
		{
			character.SetState(stateAction);
			if (character.Input.inputActions.Count != 0)
				character.Input.inputActions[0].timeValue = 0;
			return true;
		}
		return false;
	}








}
