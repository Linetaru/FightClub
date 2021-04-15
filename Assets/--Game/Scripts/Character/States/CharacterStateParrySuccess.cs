using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateParrySuccess : CharacterState
{
	[SerializeField]
	float timeLag = 0.2f;
	[SerializeField]
	float timeCancel = 0.2f;
	[SerializeField]
	float timeCancelAttack = 0.2f;

	[SerializeField]
	CharacterMoveset moveset;
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;

	[SerializeField]
	AttackManager counterAction;

	[SerializeField]
	CharacterState homingDashState;

	float t = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = timeLag;
		character.Movement.CurrentNumberOfJump += 1;
		evasiveMoveset.ResetDodge();
	}

	public override void UpdateState(CharacterBase character)
	{
		//if((!character.Input.CheckActionUP(0, InputConst.RightShoulder) && !character.Input.CheckActionUP(0, InputConst.RightTrigger)) && character.MotionSpeed != 0)
		/*if (character.Input.CheckActionHold(InputConst.RightShoulder) == true && character.MotionSpeed != 0)
		{
			if (moveset.ActionAttack(character, counterAction) == true)
			{
				return;
			}
		}*/

		/*if (character.Input.CheckAction(0, InputConst.LeftShoulder) && character.MotionSpeed != 0)
		{
			if (character.Action.CharacterHit != null) // On a touché quelqu'un 
			{
				character.SetState(homingDashState);
				character.Input.inputActions[0].timeValue = 0;
				return;
			}
		}*/



		t -= Time.deltaTime * character.MotionSpeed;
		if (t <= timeCancel)
		{
			if (evasiveMoveset.Parry(character))
			{
				return;
			}

		}
		if (t <= timeCancelAttack)
		{
			if (moveset.ActionAttack(character))
			{
				return;
			}
			else if (evasiveMoveset.Dodge(character))
			{
				return;
			}
			else if (character.Input.CheckAction(0, InputConst.Jump))
			{
				character.ResetToIdle();
			}
		}

		if (t <= 0)
		{
			character.ResetToIdle();
		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		if (character.Rigidbody.IsGrounded == true && character.MotionSpeed != 0)
			character.ResetToIdle();
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}