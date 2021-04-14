using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateActing : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState homingDashState;

	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset characterMoveset;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		//Debug.Log("Action");
	}

	public override void UpdateState(CharacterBase character)
	{
		//character.Action.CanEndAction();
		// Mettre les inputs en dessous
		if (character.Input.CheckAction(0, InputConst.LeftShoulder) && character.MotionSpeed != 0)
		{
			if (character.Action.CharacterHit != null && character.PowerGauge.CurrentPower > 33) // On a touché quelqu'un 
			{
				character.SetState(homingDashState);
				character.Input.inputActions[0].timeValue = 0;
				character.PowerGauge.AddPower(-33);
				return;
			}
		}
		characterMoveset.ActionAttack(character);
	}

	public override void LateUpdateState(CharacterBase character)
	{
		//character.Action.EndActionState();
	}

	public override void EndState(CharacterBase character, CharacterState oldState)
	{
		//Debug.Log("End Action");
	}
}