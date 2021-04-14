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

	float t = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = timeLag;
	}

	public override void UpdateState(CharacterBase character)
	{
		//if((!character.Input.CheckActionUP(0, InputConst.RightShoulder) && !character.Input.CheckActionUP(0, InputConst.RightTrigger)) && character.MotionSpeed != 0)
		if (character.Input.CheckActionUP(0, InputConst.RightShoulder) == false && character.MotionSpeed != 0)
		{
			if (moveset.ActionAttack(character, counterAction) == true)
			{
				character.Input.inputActionsUP[0].timeValue = 0;
				return;
			}
		}




		t -= Time.deltaTime * character.MotionSpeed;
		if (t <= timeCancel)
		{
			if(evasiveMoveset.Dodge(character))
			{
				return;
			}
			else if (character.Input.CheckAction(0, InputConst.Jump))
			{
				character.ResetToIdle();
			}
		}
		if (t <= timeCancelAttack)
		{
			if (moveset.ActionAttack(character))
			{
				return;
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