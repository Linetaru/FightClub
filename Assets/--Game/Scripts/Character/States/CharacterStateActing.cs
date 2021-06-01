using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateActing : CharacterState
{
	/*[Title("States")]
	[SerializeField]
	CharacterState homingDashState;*/

	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset characterMoveset;

	//bool homingDashRegister = false;

	/*public override void StartState(CharacterBase character, CharacterState oldState)
	{

	}*/

	public override void UpdateState(CharacterBase character)
	{
		/*if (homingDashRegister == true && character.MotionSpeed != 0)
		{
			if (character.Action.CharacterHit != null && character.PowerGauge.CurrentPower > 20) // On a touché quelqu'un 
			{
				character.SetState(homingDashState);
				character.Input.inputActions[0].timeValue = 0;
				character.PowerGauge.ForceAddPower(-20);
				return;
			}
		}
		else if (character.Input.CheckAction(0, InputConst.LeftShoulder))
		{
			homingDashRegister = true;

		}*/

		if (characterMoveset.ActionExSpecial(character))
		{
			return;
		}
		else if (characterMoveset.ActionEx(character))
		{
			return;
		}
		else if (characterMoveset.ActionAttack(character))
		{
			return;
		}
	}

	/*public override void LateUpdateState(CharacterBase character)
	{
		//character.Action.EndActionState();
	}*/

	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		if (characterMoveset.ExTilt)
			character.Knockback.Parry.IsParry = false;
	}
}