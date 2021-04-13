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

	float t = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = timeLag;
	}

	public override void UpdateState(CharacterBase character)
	{
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
		if (character.Rigidbody.IsGrounded == true)
			character.ResetToIdle();
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}